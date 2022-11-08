using System.Text.RegularExpressions;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace ForumWebAPI;

public class UserService
{   
    private readonly DataContext dataContext;
    private UserRepo ur;
    private IConfiguration config;
    public UserService(DataContext dataContext, IConfiguration _config)
    {
        this.dataContext = dataContext;
        ur = new UserRepo(dataContext);
        this.config = _config;
    }

    #region CRUD
    public async Task<List<AlreadyRegisteredUserDTO>> RegisterUser(RegisterUserDTO u){
        User newUser = RegUsrDTO_To_User(u);
        if(!CheckUser(newUser)){
            throw new ArgumentException();
        }
        var Users = await ur.AddUser(newUser);
        List<AlreadyRegisteredUserDTO> AUsersList = UserList_To_AlreadyRegisteredUserDTOList(Users);
        return AUsersList;
    }

    public async Task<List<AlreadyRegisteredUserDTO>> UpdateUser(User u){
        if(!CheckUser(u)){
            throw new ArgumentException();
        }
        var dbUser = await ur.GetUser(u.Id);
        if(dbUser == null){
            throw new ArgumentException();
        }
        dbUser.Name = u.Name;
        dbUser.Surname = u.Surname;
        dbUser.Country = u.Country;
        dbUser.BirthDate = u.BirthDate;
        dbUser.Email = u.Email;
        //dodać jeśli nie chce updatować jednego lub wielu z pól żeby przypadkiem nulla nie przypisać
        var UserList = await ur.UpdateUser();
        List<AlreadyRegisteredUserDTO> AUsersList = UserList_To_AlreadyRegisteredUserDTOList(UserList);
        return AUsersList;
    }

    public async Task<AlreadyRegisteredUserDTO> GetUser(int id){
        var User = await ur.GetUser(id);
        if(User == null){
            throw new ArgumentException();
        }
        AlreadyRegisteredUserDTO AUser = User_To_AlrRegUsrDTO(User);
        return AUser;
    }

    public async Task<List<AlreadyRegisteredUserDTO>> DeleteUser(int id){
        var dbUser = await ur.GetUser(id);
        if(dbUser == null){
            throw new ArgumentException();
        }
        var UserList = await ur.DeleteUser(dbUser);
        List<AlreadyRegisteredUserDTO> AUsersList = UserList_To_AlreadyRegisteredUserDTOList(UserList);
        return AUsersList;
    }

    public async Task<List<AlreadyRegisteredUserDTO>> GetUsers(){
        var UserList = await ur.GetUsers();
        if(UserList == null || UserList.Count <= 0){
            throw new ArgumentException();
        }
        var AUserList = UserList_To_AlreadyRegisteredUserDTOList(UserList);
        return AUserList;
    }
    #endregion

    #region login
    public async Task<string> LoginUser(UserLoginDTO userLogin){
        List<User> UserList = await GetUsersList();
        var user = Authenticate(userLogin, UserList);

        if(user != null){
            var token = Generate(user);
            return token;
        }
        throw new ArgumentException();
    }
    private string Generate(AlreadyRegisteredUserDTO user){
        var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, user.Username),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Surname, user.Surname),
            new Claim(ClaimTypes.Role, user.RoleReader()),
            new Claim(ClaimTypes.Country, user.Country),
        };

        var token = new JwtSecurityToken(config["Jwt:Issuer"],
            config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private AlreadyRegisteredUserDTO Authenticate(UserLoginDTO userLogin, List<User> userList){
        var currentUser = userList.FirstOrDefault(o => o.Username.ToLower() ==
            userLogin.Username.ToLower() && o.HashedPassword == userLogin.HashedPassword);
        if(currentUser != null){
            var currentUserARUDTO = User_To_AlrRegUsrDTO(currentUser);
            return currentUserARUDTO;
        }
        return null;
    }
    private async Task<List<User>> GetUsersList(){
        var UserList = await ur.GetUsers();
        if(UserList == null || UserList.Count <= 0){
            throw new ArgumentException();
        }
        return UserList;
    }
    #endregion

    #region Checkers
    private bool CheckUser(User p){
        bool IsValid = true;
        if(p == null){
            IsValid = false;
            return IsValid;
        } else if(!p.Email.Contains("@") || !p.Email.Contains(".")){
            IsValid = false;
            return IsValid;
        } else if (CheckRegex(p.Name) || CheckRegex(p.Surname) || CheckRegex(p.Country)){
            IsValid = false;
            return IsValid;
        }
        return IsValid;
    }

    private bool CheckRegex(string s){
        bool IsBadString = true;
        if(string.IsNullOrEmpty(s)){
            return IsBadString;
        } else if (!Regex.IsMatch(s, @"(^[a-zA-Z]+$)|(^[a-zA-Z]-[a-zA-Z]$)")){
            return IsBadString;
        }
        foreach (char c in s){
            if(!Char.IsLetter(c)){
                if(c == '-'){
                    continue;
                }
                return IsBadString;
            }
        }
        IsBadString = false;
        return IsBadString;
    }
    #endregion

    #region swappers
    private User RegUsrDTO_To_User(RegisterUserDTO registerUserDTO){
        User u = new User();
        u.Id = registerUserDTO.Id;
        u.Name = registerUserDTO.Name;
        u.Surname = registerUserDTO.Surname;
        u.Country = registerUserDTO.Country;
        u.BirthDate = registerUserDTO.BirthDate;
        u.Email = registerUserDTO.Email;
        u.Username = registerUserDTO.Username;
        u.HashedPassword = registerUserDTO.Password;//EncodePassword(registerUserDTO.Password);
        u.Role = Roles.DEFAULT;
        return u;
    }

    private AlreadyRegisteredUserDTO User_To_AlrRegUsrDTO(User u){
        AlreadyRegisteredUserDTO user = new AlreadyRegisteredUserDTO();
        user.Id = u.Id;
        user.Name = u.Name;
        user.Surname = u.Surname;
        user.Country = u.Country;
        user.Username = u.Username;
        return user;
    }

    private List<AlreadyRegisteredUserDTO> UserList_To_AlreadyRegisteredUserDTOList(List<User> userList){
        var AUserList = new List<AlreadyRegisteredUserDTO>();
        for(int i = 0; i < userList.Count; i++){
            AUserList.Add(User_To_AlrRegUsrDTO(userList[i]));
        }
        return AUserList;
    }
    #endregion

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
