using BookManagement.Domain.Errors;
using BookManagement.Domain.Primitives;
using BookManagement.Domain.Shared;

namespace BookManagement.Domain.ValueObjects.Users;

public sealed class Email : ValueObject
{
    #region Constants
    
    public const int MaxLength = 50; // Maximum length for an email
    
    #endregion
    
    #region Constructors
    
    private Email(string value)
    {
        Value = value;
    }
    
    private Email()
    {
    }
    
    #endregion
    
    #region Properties
    
    public string Value { get; }
    
    #endregion

    #region Factory Methods
    
    /// <summary> 
    /// Creates an Email instance after validating the input. 
    /// </summary> 
    /// <param name="email">The email string to create the Email value object from.</param> 
    /// <returns>A Result object containing the Email value object or an error.</returns>
    public static Result<Email> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Result.Failure<Email>(DomainErrors.Email.Empty);
        }
        
        if (email.Split('@').Length != 2)
        {
            return Result.Failure<Email>(DomainErrors.Email.InvalidFormat);
        }
        
        return Result.Success(new Email(email));
    }

    #endregion

    #region Implicit Operators

    /// <summary>
    /// Email Value Objectini to'g'ridan-to'g'i stringga (o'zining ichki qiymatiga) o'tkazish uchun implicit operator.
    /// EF Core'ning LINQ provideri va Mapster uchun yordam beradi,
    /// chunki u Value Objectdan string qiymatini to'g'ridan-to'g'i olish yo'lini ko'rsatadi.
    /// </summary>
    public static implicit operator string(Email email)
    {
        // Null tekshiruvini qo'shish tavsiya etiladi
        // Agar email null bo'lsa va uni stringga aylantirish kerak bo'lsa.
        return email?.Value;
    }

    #endregion

    #region Explicit Operators

    /// <summary>
    /// String qiymatini Email Value Objectiga aniq o'tkazish uchun explicit operator.
    /// Bu operator domen qoidalarini (validatsiyani) qo'llash uchun Create metodidan foydalanadi.
    /// Aniqlik uchun explicit operator ishlatiladi, chunki konvertatsiya muvaffaqiyatsiz bo'lishi mumkin.
    /// </summary>
    public static explicit operator Email(string emailString)
    {
        // Create metodimiz Result<T> qaytargani uchun,
        // bu yerda uni xatolarni boshqarish kerak.
        // Agar konvertatsiya muvaffaqiyatsiz bo'lsa, InvalidCastException otilishi mumkin.
        var result = Create(emailString);
        if (result.IsFailure)
        {
            // Bu istisno faqat konvertatsiya Explicit operator orqali chaqirilganda otiladi.
            // EF Core HasConversion da bu ishlamasligi mumkinligini yodda tuting.
            throw new InvalidCastException($"Cannot convert '{emailString}' to Email. Error: {result.Error.Message}");
        }
        return result.Value;
    }

    #endregion

    #region Overrides

    /// <summary>
    /// Email Value Objectining string ko'rinishini (uning ichki qiymatini) qaytaradi.
    /// Bu EF Core ProjectToType ichida Value Object to'g'ri tarjima qilinmaganda
    /// oxirgi chora sifatida chaqirilishi mumkin edi.
    /// Endi implicit operator mavjudligi sababli, bu kamroq chaqirilishi kerak.
    /// </summary>
    public override string ToString()
    {
        return Value;
    }

    #endregion

    #region Overrides

    /// <summary> 
    /// Returns the atomic values of the Email object for equality checks. 
    /// </summary>
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
    
    #endregion
}