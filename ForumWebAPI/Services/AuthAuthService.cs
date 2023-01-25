using System.Text.RegularExpressions;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using ForumWebAPI.Externals;
using ForumWebAPI.UserRepos;
using ForumWebAPI.UserDTOs;
using ForumWebAPI.RegexCheckers;

namespace ForumWebAPI.Services;

public class AuthAuthService
{
    private readonly DataContext dataContext;
    private UserRepo ur;
    private IConfiguration config;
    private readonly IPasswordHasher<User> passwordHasher;
    public AuthAuthService(DataContext dataContext, IConfiguration _config)
    {
        this.dataContext = dataContext;
        ur = new UserRepo(dataContext);
        this.config = _config;
        passwordHasher = new PasswordHasher<User>();
    }

    // public AuthAuthService(DataContext dataContext)
    // {
    //     this.dataContext = dataContext;
    //     ur = new UserRepo(dataContext);
    //     passwordHasher = new PasswordHasher<User>();
    // }
    public async Task<string> LoginUser(UserLoginDTO userLogin)
    {
        RegexChecker regexChecker = new RegexChecker();
        if (!regexChecker.CheckUserLogin(userLogin))
        {
            throw new ArgumentException();
        }

        List<User> UserList = await GetUsersList();
        var user = Authenticate(userLogin, UserList);

        if (user != null)
        {
            var token = Generate(user);
            return token;
        }
        throw new ArgumentException();
    }

    public async Task<string> ForgottenUser(UserLoginDTO userLogin)
    {
        RegexChecker regexChecker = new RegexChecker();
        // if (!regexChecker.CheckUserLogin(userLogin))
        // {
        //     // throw new ArgumentException();
        // }

        Console.WriteLine("DUPA");
        List<User> UserList = await GetUsersList();
        var user = AuthenticateForgotten(userLogin, UserList);

        if (user != null)
        {
            var token = Generate(user);
            return token;
        }
        throw new ArgumentException();
    }

    private string Generate(AlreadyRegisteredUserDTO user)
    {
        var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, user.Username),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Surname, user.Surname),
            new Claim(ClaimTypes.Role, user.RoleReader()),
            new Claim(ClaimTypes.Country, user.Country),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.DateOfBirth, user.lastLogin)
        };

        var token = new JwtSecurityToken(config["Jwt:Issuer"],
            config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private AlreadyRegisteredUserDTO Authenticate(UserLoginDTO userLogin, List<User> userList)
    {
        CustomPasswordHasher cph = new CustomPasswordHasher();
        var currentUser = userList.FirstOrDefault(o => o.Username.ToLower() ==
            userLogin.Username.ToLower() && PasswordEncodere.PasswordEncoder.IsMatchPassword(userLogin.Password, o.HashedPassword) != PasswordVerificationResult.Failed); // na koniec zobacycz czy jak tutaj dodam || to samo z mailem zamiast username to się nie wykrzaczy
        if (currentUser != null)
        {
            if (currentUser.loginCounter >= 10)
            {
                throw new Exception("User account blocked.");
            }
            UserRepo ur = UserRepo.Instance;
            currentUser.lastLogin = DateTime.Now.ToString();
            var currentUserARUDTO = ur.User_To_AlrRegUsrDTO(currentUser);
            ur.UpdateDateTimeLastLogin(currentUser);
            return currentUserARUDTO;
        }
        else
        {
            UserRepo ur = UserRepo.Instance;
            ur.UpdateCounter(userLogin.Username);
            return null;
        }
    }

    private AlreadyRegisteredUserDTO AuthenticateForgotten(UserLoginDTO userLogin, List<User> userList)
    {
        var currentUser = userList.FirstOrDefault(o => o.Email ==
            userLogin.Username && PasswordEncodere.PasswordEncoder.IsMatchPassword(userLogin.Password, o.validationCode) != PasswordVerificationResult.Failed);
        if (currentUser != null)
        {
            var currentUserARUDTO = ur.User_To_AlrRegUsrDTO(currentUser);
            return currentUserARUDTO;
        }
        return null;
    }


    private async Task<List<User>> GetUsersList()
    {
        var UserList = await ur.GetRawUsers();
        if (UserList == null || UserList.Count <= 0)
        {
            throw new ArgumentException();
        }
        return UserList;
    }

}
