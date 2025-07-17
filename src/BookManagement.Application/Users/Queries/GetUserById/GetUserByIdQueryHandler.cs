using BookManagement.Application.Common.Messaging;
using BookManagement.Application.Users.Queries.Common.Factories;
using BookManagement.Application.Users.Queries.Common.Responses;
using BookManagement.Application.Users.Queries.GetUserById;
using BookManagement.Domain.Errors;
using BookManagement.Domain.Repositories.Users;
using BookManagement.Domain.Shared;

internal sealed class GetUserByIdQueryHandler(
    IUserRepository userRepository
) : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        Console.WriteLine("\n--- GetUserByIdQueryHandler ishga tushdi ---");
        Console.WriteLine($"So'rov parametrlari: UserId={request.UserId}");

        // --- 1. Foydalanuvchini Repository'dan olish ---
        Console.WriteLine("\n--- Bosqich 1: Foydalanuvchini Repository'dan olish ---");
        Console.WriteLine($"1.1. UserRepository.GetByIdAsync({request.UserId}) ni chaqiryapmiz.");
        Console.WriteLine("     Bu joyda Entity Framework Core ma'lumotlar bazasiga SQL so'rovini yuboradi.");
        Console.WriteLine("     SQL so'rovi faqat bitta foydalanuvchini ID bo'yicha olishga qaratilgan.");
        Console.WriteLine("     Bu bosqichda, 'UserConfiguration.cs' dagi 'HasConversion' sozlamalari ishga tushadi.");
        Console.WriteLine("     Ma'lumotlar bazasidan kelgan 'string' qiymatlari (Email, FirstName, LastName) 'User' entity'siga 'Value Object'lar sifatida konvertatsiya qilinadi.");

        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        Console.WriteLine("1.2. UserRepository.GetByIdAsync() dan natija olindi.");

        // --- 2. Foydalanuvchi topilmaganligini tekshirish ---
        Console.WriteLine("\n--- Bosqich 2: Foydalanuvchi topilmaganligini tekshirish ---");
        if (user is null)
        {
            Console.WriteLine($"2.1. Foydalanuvchi ID: {request.UserId} bo'yicha topilmadi. Natija 'Failure' qaytariladi.");
            return Result.Failure<UserResponse>(DomainErrors.User.NotFound(request.UserId));
        }

        Console.WriteLine($"2.1. Foydalanuvchi ID: {request.UserId} bo'yicha topildi.");
        Console.WriteLine($"     Yuklangan 'User' entity holati (Value Objectlar allaqachon konvertatsiya qilingan):");
        Console.WriteLine($"     User.Id:         {user.Id}");
        Console.WriteLine($"     User.Email:      '{user.Email.Value}' (Bu allaqachon 'Email' Value Objectining ichidagi string qiymati)");
        Console.WriteLine($"     User.FirstName:  '{user.FirstName.Value}' (Bu allaqachon 'FirstName' Value Objectining ichidagi string qiymati)");
        Console.WriteLine($"     User.LastName:   '{user.LastName.Value}' (Bu allaqachon 'LastName' Value Objectining ichidagi string qiymati)");

        // --- 3. Javobni tayyorlash (UserResponseFactory orqali) ---
        Console.WriteLine("\n--- Bosqich 3: Javobni tayyorlash (UserResponseFactory orqali) ---");
        Console.WriteLine("3.1. UserResponseFactory.Create(user) metodini chaqiryapmiz.");
        Console.WriteLine("     Bu metod 'User' entity'sidan 'UserResponse' DTO'siga xotira ichida (in-memory) mapping qiladi.");
        Console.WriteLine("     Bu yerda hech qanday SQL so'rovi yoki EF Core LINQ provider tarjimasi kerak emas.");
        Console.WriteLine("     U to'g'ridan-to'g'i 'user.Email.Value' kabi C# property'lariga murojaat qiladi.");

        var response = UserResponseFactory.Create(user);

        Console.WriteLine("3.2. UserResponseFactory.Create() dan natija olindi.");
        Console.WriteLine("     Tayyorlangan 'UserResponse' DTO holati:");
        Console.WriteLine($"     Response.Id:        {response.Id}");
        Console.WriteLine($"     Response.Email:     '{response.Email}'");
        Console.WriteLine($"     Response.FirstName: '{response.FirstName}'");
        Console.WriteLine($"     Response.LastName:  '{response.LastName}'");

        Console.WriteLine("\n--- GetUserByIdQueryHandler tugadi ---");
        return Result.Success(response);
    }
}
