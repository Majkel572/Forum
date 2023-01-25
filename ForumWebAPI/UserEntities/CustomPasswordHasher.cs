using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

public class CustomPasswordHasher
{
    private const string pepper = "pizzapepperoni";
    public string HashPassword(string password)
    {
        // Generate a new salt
        byte[] salt;
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

        // Combine the password, salt, and pepper
        var saltedPepperedPassword = password + Convert.ToBase64String(salt) + pepper;

        // Use SHA256 to hash the salted and peppered password
        var saltedPepperedPasswordBytes = System.Text.Encoding.UTF8.GetBytes(saltedPepperedPassword);
        using (var sha256 = SHA256.Create())
        {
            var saltedPepperedHashedPasswordBytes = sha256.ComputeHash(saltedPepperedPasswordBytes);
            return Convert.ToBase64String(saltedPepperedHashedPasswordBytes);
        }
    }

    public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        // Extract the salt and pepper from the hashed password
        var salt = Convert.FromBase64String(hashedPassword.Substring(0, 22));

        // Combine the provided password, salt, and pepper
        var saltedPepperedProvidedPassword = providedPassword + Convert.ToBase64String(salt) + pepper;

        // Use SHA256 to hash the salted and peppered provided password
        var saltedPepperedProvidedPasswordBytes = System.Text.Encoding.UTF8.GetBytes(saltedPepperedProvidedPassword);
        using (var sha256 = SHA256.Create())
        {
            var saltedPepperedHashedProvidedPasswordBytes = sha256.ComputeHash(saltedPepperedProvidedPasswordBytes);

            // Compare the provided password hash to the stored password hash
            return hashedPassword == Convert.ToBase64String(saltedPepperedHashedProvidedPasswordBytes)
                ? PasswordVerificationResult.Success
                : PasswordVerificationResult.Failed;
        }
    }
}