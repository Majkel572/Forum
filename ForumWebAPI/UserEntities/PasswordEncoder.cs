using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

namespace ForumWebAPI.PasswordEncodere;

public class PasswordEncoder
{
    #region hash
    // public string EncodePassword(string password){
    //     byte[] salt;
    //     string pepper = "qwerty";
    //     new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
    //     var pbkdf2 = new Rfc2898DeriveBytes(password+pepper, salt, 100000);
    //     byte[] hash = pbkdf2.GetBytes(20);
    //     byte[] hashBytes = new byte[36];
    //     Array.Copy(salt, 0, hashBytes, 0, 16);
    //     Array.Copy(hash, 0, hashBytes, 16, 20);
    //     string savedPasswordHash = Convert.ToBase64String(hashBytes);
    //     return savedPasswordHash;
    // }
    public static string EncodePassword(string password)
    {
        byte[] salt;
        string pepper = "qwerty";
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
        byte[] passwordAndPepper = System.Text.Encoding.UTF8.GetBytes(pepper + password);
        byte[] hash = new byte[1];
        using (SHA256 sha256 = SHA256.Create())
        {
            for (int i = 0; i < 10000; i++)
            {
                hash = sha256.ComputeHash(passwordAndPepper);
                passwordAndPepper = hash;
            }
        }
        Console.WriteLine(hash.Length);
        byte[] merge = new byte[salt.Length + hash.Length];
        Buffer.BlockCopy(salt, 0, merge, 0, salt.Length);
        Buffer.BlockCopy(hash, 0, merge, salt.Length, hash.Length);
        return Convert.ToBase64String(merge);
    }

    public static PasswordVerificationResult IsMatchPassword(string providedPassword, string dbHash)
    {
        string pepper = "qwerty";
        byte[] hashBytes = Convert.FromBase64String(dbHash);
        byte[] salt = new byte[16];
        Buffer.BlockCopy(hashBytes, 0, salt, 0, salt.Length);

        byte[] passwordAndPepper = System.Text.Encoding.UTF8.GetBytes(pepper + providedPassword);
        byte[] hash = new byte[1];
        using (SHA256 sha256 = SHA256.Create())
        {
            for (int i = 0; i < 10000; i++)
            {
                hash = sha256.ComputeHash(passwordAndPepper);
                passwordAndPepper = hash;
            }
        }
        Console.WriteLine(hash.Length);
        byte[] merge = new byte[salt.Length + hash.Length];
        Buffer.BlockCopy(salt, 0, merge, 0, salt.Length);
        Buffer.BlockCopy(hash, 0, merge, salt.Length, hash.Length);
        var haselko = Convert.ToBase64String(merge);
        if(haselko.Equals(dbHash)){
            return PasswordVerificationResult.Success;
        }

        return PasswordVerificationResult.Failed;
    }

    // public bool IsMatchPassword(string providedPassword, string dbHash)
    // {
    //     byte[] hashBytes = Convert.FromBase64String(dbHash);
    //     string pepper = "qwerty";
    //     byte[] salt = new byte[16];
    //     Array.Copy(hashBytes, 0, salt, 0, 16);
    //     var pbkdf2 = new Rfc2898DeriveBytes(providedPassword + pepper, salt, 100000);
    //     byte[] hash = pbkdf2.GetBytes(20);
    //     for (int i = 0; i < 20; i++)
    //     {
    //         if (hashBytes[i + 16] != hash[i])
    //         {
    //             return false;
    //         }
    //     }

    //     return true;
    // }
    #endregion
}