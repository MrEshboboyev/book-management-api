using BookManagement.Application.Common.Data;
using BookManagement.Application.Common.Mappings;
using BookManagement.Application.Common.Messaging;
using BookManagement.Application.Common.Pagination;
using BookManagement.Application.Users.Queries.Common.Responses;
using BookManagement.Application.Users.Queries.GetAllUsers;
using BookManagement.Domain.Shared;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

internal sealed class GetAllUsersQueryHandler(
    IApplicationDbContext context,
    IMapper mapper
) : IQueryHandler<GetAllUsersQuery, PaginatedList<UserResponse>>
{
    public async Task<Result<PaginatedList<UserResponse>>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken)
    {
        Console.WriteLine("\n--- GetAllUsersQueryHandler ishga tushdi ---");
        Console.WriteLine($"So'rov parametrlari: PageNumber={request.PageNumber}, PageSize={request.PageSize}");

        // --- 1. IQueryable ni olish: Query qurishning boshlanishi ---
        Console.WriteLine("\n--- Bosqich 1: IQueryable<User> ni olish ---");
        Console.WriteLine("1.1. DbContext.Users ga murojaat qilyapmiz. Bu hali ma'lumotlar bazasiga murojaat qilmaydi.");
        Console.WriteLine("     Bu bosqichda faqat ma'lumotlar bazasidan User turidagi ob'ektlarni olish uchun boshlang'ich ifoda yaratiladi.");
        var query = context.Users.AsNoTracking(); // AsNoTracking: faqat o'qish uchun, EF Core keshiga qo'shilmaydi.

        Console.WriteLine($"1.2. Dastlabki IQueryable holati: {query.GetType().Name}");
        Console.WriteLine($"     IQueryable elementi turi: {query.ElementType.Name} (Bu bizning 'User' entity'miz).");
        Console.WriteLine("     Hozircha hech qanday SQL yaratilmadi yoki ma'lumot olinmadi.");

        // --- 2. Tartiblashni qo'llash: Queryga qo'shimcha shartlar ---
        Console.WriteLine("\n--- Bosqich 2: Tartiblashni (OrderBy) qo'llash ---");
        Console.WriteLine("2.1. Natijani 'CreatedOnUtc' bo'yicha tartiblashni qo'shyapmiz.");
        Console.WriteLine("     Bu ham hali ma'lumotlar bazasiga murojaat qilmaydi, faqat mavjud IQueryable ifodasiga qo'shiladi.");
        query = query.OrderBy(x => x.CreatedOnUtc);
        Console.WriteLine($"2.2. Tartiblangan IQueryable holati: {query.GetType().Name}");
        Console.WriteLine("     SQL so'roviga ORDER BY qismi qo'shilishi kutilmoqda.");

        // --- 3. Mapster ning ProjectToType() metodini qo'llash: SQL tarjimasi urinishi ---
        Console.WriteLine("\n--- Bosqich 3: Mapster ProjectToType<UserResponse>() ni qo'llash ---");
        Console.WriteLine("3.1. Mapsterga 'User' entity'sini 'UserResponse' DTO'siga proyeksiyalashni buyuryapmiz.");
        Console.WriteLine("     Mapster sizning 'UserMappingRegister.cs' dagi konfiguratsiyangizni ishlatadi.");
        Console.WriteLine("     Mapster mapping ifodalarini (masalan, 'src.Email.Value') EF Core LINQ provideri orqali SQL SELECT qismiga tarjima qilishga urinadi.");
        Console.WriteLine("     Bu qismda, agar EF Core (va uning LINQ provideri) sizning Value Objectlaringizni to'g'ri tushunmasa (ayniqsa 'Result<T>' qaytaruvchi 'Create' metodlari sababli), muammo yuzaga keladi.");

        // Bu joyda `ProjectToType` ning ichki mexanizmi ishga tushadi:
        // Mapster: "User" dan "UserResponse" ga qanday o'tkazishni bilish uchun mappingni ko'rib chiqadi.
        // EF Core LINQ provideri: Mapster bergan ifodalarni olib, ularni SQL SELECT ustunlariga aylantirishga harakat qiladi.
        // Aynan mana shu yerda `src.Email.Value` (va `FirstName.Value`, `LastName.Value`)
        // Value Object bo'lgani va uning ichidagi `Result<T>` qaytaruvchi `Create` metodi SQL ga tarjima qilinmagani uchun
        // EF Core chalkashadi va ob'ektning `ToString()` qiymatini (ya'ni tur nomini) oladi.
        var projectedQuery = query.ProjectToType<UserResponse>(mapper.Config);

        Console.WriteLine($"3.2. ProjectToType dan keyingi IQueryable holati: {projectedQuery.GetType().Name}");
        Console.WriteLine($"     IQueryable elementi turi: {projectedQuery.ElementType.Name} (Endi bu bizning 'UserResponse' DTO'miz).");

        Console.WriteLine("\n--- SQL so'rovini oldindan ko'rishga urinish (Faoliyatni tushunish uchun) ---");
        //Eslatma: Bu kodni ishga tushirishda real SQLni ko'rish qiyin bo'lishi mumkin.
        //     Lekin agar siz Debuggerda `projectedQuery` ustiga sichqonchani olib borsangiz,
        //     "DebugView" ni yoki "Raw Query" ni tekshirishingiz mumkin.
        try
        {
            // Bu qism real proyektda ishlatilmasligi kerak, chunki u ma'lumotlar bazasiga murojaat qilishi mumkin.
            // Faqat diagnostika uchun. Bu faqat ba'zi EF Core versiyalarida ishlaydi.
            var debugSql = projectedQuery.ToQueryString(); // EF Core 5+ uchun
            Console.WriteLine($"Generated SQL Preview (might not be exact): \n{debugSql}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to get SQL preview: {ex.Message}");
        }
        Console.WriteLine("     SQL so'rovining qanday shakllanayotganini tushunish uchun, Visual Studioda 'projectedQuery' ustida 'DebugView' ni tekshiring.");
        Console.WriteLine("     Agar SQL da 'Email.Value' o'rniga 't.Email' (db ustuni) yoki 'CAST(N'...')' kabi narsalar ko'rmasangiz, muammo shu yerda.");


        // --- 4. PaginatedListAsync(): Ma'lumotlar bazasiga murojaat va natijalarni olish ---
        Console.WriteLine("\n--- Bosqich 4: PaginatedListAsync() metodini chaqirish ---");
        Console.WriteLine("4.1. Bu joyda, qurilgan IQueryable<UserResponse> so'rovi ma'lumotlar bazasiga yuboriladi.");
        Console.WriteLine("     EF Core ichki COUNT va OFFSET/LIMIT (yoki SKIP/TAKE) operatsiyalarini bajarish uchun ikkita SQL so'rovini yuboradi:");
        Console.WriteLine("       a) Umumiy elementlar sonini hisoblash uchun bitta so'rov (COUNT).");
        Console.WriteLine("       b) Joriy sahifaning elementlarini olish uchun ikkinchi so'rov (SELECT, ORDER BY, OFFSET, LIMIT).");

        var paginatedUsers = await projectedQuery
            .PaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);

        Console.WriteLine("\n--- Bosqich 5: PaginatedListAsync() dan natija olindi ---");
        Console.WriteLine($"5.1. Umumiy foydalanuvchilar soni: {paginatedUsers.TotalCount}");
        Console.WriteLine($"5.2. Joriy sahifa: {paginatedUsers.PageNumber}");
        Console.WriteLine($"5.3. Jami sahifalar: {paginatedUsers.TotalPages}");
        Console.WriteLine($"5.4. Sahifadagi elementlar soni: {paginatedUsers.Items.Count}");

        Console.WriteLine("\n--- Ma'lumotlar bazasidan kelgan va DTO ga map qilingan natijalar (har bir ustun uchun): ---");
        if (paginatedUsers.Items.Any())
        {
            foreach (var userResponse in paginatedUsers.Items)
            {
                Console.WriteLine($"  --------------------------------------------------");
                Console.WriteLine($"  Id:         {userResponse.Id}");
                Console.WriteLine($"  Email:      '{userResponse.Email}' (DBdan kelgan 'Email' ustuni qiymati)");
                Console.WriteLine($"  FirstName:  '{userResponse.FirstName}' (DBdan kelgan 'FirstName' ustuni qiymati)");
                Console.WriteLine($"  LastName:   '{userResponse.LastName}' (DBdan kelgan 'LastName' ustuni qiymati)");
            }
            Console.WriteLine($"  --------------------------------------------------");
        }
        else
        {
            Console.WriteLine("  Hech qanday foydalanuvchi topilmadi.");
        }

        Console.WriteLine("\n--- GetAllUsersQueryHandler tugadi ---");
        return Result.Success(paginatedUsers);
    }
}
