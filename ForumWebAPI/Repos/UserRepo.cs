using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ForumWebAPI.UserDTOs;

namespace ForumWebAPI.UserRepos;

public class UserRepo : IUserRepo
{   
    private readonly DataContext dataContext;
    private readonly IPasswordHasher<User> passwordHasher;
    private static UserRepo instance = null;
    public static UserRepo Instance {
        get{
            return instance;
        }
    } 
    public UserRepo(DataContext dataContext)
    {
        instance = this;
        this.dataContext = dataContext;
        passwordHasher = new PasswordHasher<User>();
    }

    public async Task<string> AddUser(RegisterUserDTO p){
        string secretCode = Guid.NewGuid().ToString();
        User dbUser = RegUsrDTO_To_User(p, secretCode);
        dataContext.Users.Add(dbUser);
        await dataContext.SaveChangesAsync();
        return secretCode;
    }

    public async Task<bool> UpdateUser(RegisterUserDTO u){
        var dbUser = await dataContext.Users.FindAsync(u.Id);
        if(dbUser == null){
            throw new ArgumentException();
        }
        dbUser.Name = u.Name;
        dbUser.Surname = u.Surname;
        dbUser.Country = u.Country;
        dbUser.BirthDate = u.BirthDate;
        dbUser.Email = u.Email;
        dataContext.Users.Update(dbUser);
        await dataContext.SaveChangesAsync();
        bool updated = true;
        return updated;
    }
    public async Task<bool> UpdatePassword(AlreadyRegisteredUserDTO currentUser, string oldP, string newP){
        var dbUser = await dataContext.Users.FindAsync(currentUser.Email);
        if(dbUser == null){
            throw new ArgumentException();
        }
        if(passwordHasher.VerifyHashedPassword(dbUser, dbUser.HashedPassword, oldP) == PasswordVerificationResult.Success){
            dbUser.HashedPassword = passwordHasher.HashPassword(dbUser, newP);
            dataContext.Users.Update(dbUser);
            await dataContext.SaveChangesAsync();
            return true;
        }
        throw new ArgumentException();
    }
    public async Task<AlreadyRegisteredUserDTO> GetUser(string email){
        var user = await dataContext.Users.FindAsync(email);
        if(user == null){
            throw new ArgumentException();
        }
        AlreadyRegisteredUserDTO userDTO = User_To_AlrRegUsrDTO(user);
        return userDTO;
    }

    public async Task<List<AlreadyRegisteredUserDTO>> DeleteUser(string email){
        var dbUser = await dataContext.Users.FindAsync(email);
        if(dbUser == null){
            throw new ArgumentException();
        }
        dataContext.Users.Remove(dbUser);
        await dataContext.SaveChangesAsync();
        var userlist = await dataContext.Users.ToListAsync();
        var alreguserlist = UserList_To_AlreadyRegisteredUserDTOList(userlist);
        return alreguserlist;
    }

    public async Task<List<AlreadyRegisteredUserDTO>> GetUsers(){
        var UserList = await dataContext.Users.ToListAsync();
        if(UserList == null || UserList.Count <= 0){
            throw new ArgumentException();
        }
        var AUserList = UserList_To_AlreadyRegisteredUserDTOList(UserList);
        return AUserList;
    }
    
    public async Task<List<User>> GetRawUsers(){
        var UserList = await dataContext.Users.ToListAsync();
        if(UserList == null || UserList.Count <= 0){
            throw new ArgumentException();
        }
        return UserList;
    }

    public async Task<bool> UpdateCounter(string username){
        var userList = await GetRawUsers();
        var dbUser = userList.FirstOrDefault(o => o.Username.ToLower() == username.ToLower());
        if(dbUser is null){
            throw new Exception();
        }
        dbUser.loginCounter++;
        dataContext.Users.Update(dbUser);
        await dataContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateDateTimeLastLogin(User user){
        dataContext.Users.Update(user);
        await dataContext.SaveChangesAsync();
        return true;
    }

    #region swappers
    private User RegUsrDTO_To_User(RegisterUserDTO registerUserDTO, string secretCode){
        User u = new User();
        // u.Id = registerUserDTO.Id;
        CustomPasswordHasher cph = new CustomPasswordHasher();
        u.Name = registerUserDTO.Name;
        u.Surname = registerUserDTO.Surname;
        u.Country = registerUserDTO.Country;
        u.BirthDate = registerUserDTO.BirthDate;
        u.Email = registerUserDTO.Email;
        u.Username = registerUserDTO.Username;
        u.HashedPassword = /*cph.HashPassword(registerUserDTO.Password);//passwordHasher.HashPassword(u, registerUserDTO.Password);*/PasswordEncodere.PasswordEncoder.EncodePassword(registerUserDTO.Password);
        u.Role = Roles.DEFAULT;
        u.isValidated = false;
        u.validationCode = PasswordEncodere.PasswordEncoder.EncodePassword(secretCode);
        u.lastLogin = DateTime.Now.ToString();
        return u;
    }

    public AlreadyRegisteredUserDTO User_To_AlrRegUsrDTO(User u){
        AlreadyRegisteredUserDTO user = new AlreadyRegisteredUserDTO();
        // user.Id = u.Id;
        user.Name = u.Name;
        user.Surname = u.Surname;
        user.Country = u.Country;
        user.Username = u.Username;
        user.Role = u.Role;
        user.Email = u.Email;
        user.BirthDate = u.BirthDate;
        user.validationCode = u.validationCode;
        user.lastLogin = u.lastLogin;
        user.loginCounter = u.loginCounter;
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
}