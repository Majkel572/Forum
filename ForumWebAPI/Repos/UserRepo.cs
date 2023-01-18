using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ForumWebAPI.UserDTOs;

namespace ForumWebAPI.UserRepos;

public class UserRepo : IUserRepo
{   
    private readonly DataContext dataContext;
    private readonly IPasswordHasher<User> passwordHasher;
    public UserRepo(DataContext dataContext)
    {
        this.dataContext = dataContext;
        passwordHasher = new PasswordHasher<User>();
    }

    public async Task<string> AddUser(RegisterUserDTO p){
        User dbUser = RegUsrDTO_To_User(p);
        dataContext.Users.Add(dbUser);
        await dataContext.SaveChangesAsync();
        string secretCode = dbUser.validationCode.ToString();
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

    #region swappers
    private User RegUsrDTO_To_User(RegisterUserDTO registerUserDTO){
        User u = new User();
        // u.Id = registerUserDTO.Id;
        u.Name = registerUserDTO.Name;
        u.Surname = registerUserDTO.Surname;
        u.Country = registerUserDTO.Country;
        u.BirthDate = registerUserDTO.BirthDate;
        u.Email = registerUserDTO.Email;
        u.Username = registerUserDTO.Username;
        u.HashedPassword = passwordHasher.HashPassword(u, registerUserDTO.Password);//EncodePassword(registerUserDTO.Password);
        u.Role = Roles.DEFAULT;
        u.isValidated = false;
        var rnd = new Random();
        u.validationCode = rnd.Next(0, 100000);
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