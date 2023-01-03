using System.Security.Cryptography;


namespace ForumWebAPI.PasswordEncoder;

public class PasswordEncoder{
     #region hash
    private string EncodePassword(string password){
        byte[] salt;
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
        byte[] hash = pbkdf2.GetBytes(20);
        byte[] hashBytes = new byte[36];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);
        string savedPasswordHash = Convert.ToBase64String(hashBytes);
        return savedPasswordHash;
    }
    #endregion
}