using BCrypt.Net;

namespace Practika2_OPAM_Ubohyi_Stanislav.Services;

public class PasswordHasher
{
    // Хешує пароль з використанням BCrypt
    public static string HashPassword(string password)
    {
        // Використовуємо BCrypt для створення хешу пароля
        // WorkFactor 12 забезпечує гарний баланс між безпекою і швидкодією
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
    }
    
    // Перевіряє, чи відповідає пароль хешу
    public static bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}