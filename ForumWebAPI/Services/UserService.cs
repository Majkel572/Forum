using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using ForumWebAPI.Externals;
using ForumWebAPI.UserRepos;
using ForumWebAPI.UserDTOs;

namespace ForumWebAPI.Services;

public class UserService
{   
    private readonly DataContext dataContext;
    private UserRepo ur;
    private IConfiguration config;
    private readonly IPasswordHasher<User> passwordHasher;
    public UserService(DataContext dataContext, IConfiguration _config)
    {
        this.dataContext = dataContext;
        ur = new UserRepo(dataContext);
        this.config = _config;
        passwordHasher = new PasswordHasher<User>();
    }

    public UserService(DataContext dataContext)
    {
        this.dataContext = dataContext;
        ur = new UserRepo(dataContext);
        passwordHasher = new PasswordHasher<User>();
    }
    
    #region CRUD
    public async Task<bool> RegisterUser(RegisterUserDTO u){
        await ur.AddUser(u);
        bool userSuccessfullyAdded = true;
        EmailSender.SendEmail(u.Email);
        return userSuccessfullyAdded;
    }

    public async Task<bool> UpdateUser(RegisterUserDTO u){
        var dbUser = await ur.UpdateUser(u);
        //dodać jeśli nie chce updatować jednego lub wielu z pól żeby przypadkiem nulla nie przypisać
        bool userSuccessfullyUpdated = true;
        return userSuccessfullyUpdated;
    }

    public async Task<AlreadyRegisteredUserDTO> GetUser(int id){
        AlreadyRegisteredUserDTO user = await ur.GetUser(id);
        if(user == null){
            throw new ArgumentException();
        }
        return user;
    }

    public async Task<List<AlreadyRegisteredUserDTO>> DeleteUser(int id){
        var UserList = await ur.DeleteUser(id);
        return UserList;
    }

    public async Task<List<AlreadyRegisteredUserDTO>> GetUsers(){
        var UserList = await ur.GetUsers();
        return UserList;
    }

    public async Task<List<AlreadyRegisteredUserDTO>> PageUsers(int itemsToShow, int pageNumber){
        List<AlreadyRegisteredUserDTO> uList = await ur.GetUsers();
        return uList.Skip(itemsToShow * pageNumber).Take(itemsToShow).ToList();
    }
    #endregion

    #region sorter
    public async Task<List<AlreadyRegisteredUserDTO>> SortAlreadyRegisteredUserDTOs(ListSortDirection dir = ListSortDirection.Descending, string? userSortTypes = null){
        var AlreadyRegisteredUserDTOValueList = await ur.GetUsers();
        if(userSortTypes is null){
            throw new ArgumentException();
        }
        var UserSortTypesList = splitter(userSortTypes);
        UserSortTypesList ??= new List<UserSortTypes>();
        if (UserSortTypesList.Count == 0){
            return AlreadyRegisteredUserDTOValueList;
        }
        IOrderedEnumerable<AlreadyRegisteredUserDTO> sortedAlreadyRegisteredUserDTOs;
        if(dir == ListSortDirection.Descending){
            sortedAlreadyRegisteredUserDTOs = AlreadyRegisteredUserDTOValueList.OrderByDescending(keySelector(UserSortTypesList[0]));
        } else {
            sortedAlreadyRegisteredUserDTOs = AlreadyRegisteredUserDTOValueList.OrderBy(keySelector(UserSortTypesList[0]));
        }
        for(int i = 1; i < UserSortTypesList.Count; i++){
            if(dir == ListSortDirection.Descending){
                sortedAlreadyRegisteredUserDTOs = sortedAlreadyRegisteredUserDTOs.ThenByDescending(keySelector(UserSortTypesList[i]));
            } else {
                sortedAlreadyRegisteredUserDTOs = sortedAlreadyRegisteredUserDTOs.ThenBy(keySelector(UserSortTypesList[i]));
            }
        }
        return sortedAlreadyRegisteredUserDTOs.ToList();
    }

    private List<UserSortTypes> splitter(string toSplit){
        var list = new List<UserSortTypes>();
        for(int i = 0; i < toSplit.Count(); i++){
            int a = toSplit[i] - 48;
            list.Add((UserSortTypes)a);
        }
        return list;
    }

    private Func<AlreadyRegisteredUserDTO, object> keySelector(UserSortTypes ust){
        if(ust == UserSortTypes.NAME){
            return AlreadyRegisteredUserDTO => AlreadyRegisteredUserDTO.Name;
        } else if(ust == UserSortTypes.SURNAME){
            return AlreadyRegisteredUserDTO => AlreadyRegisteredUserDTO.Surname;
        } else if(ust == UserSortTypes.BIRTH_DATE){
            return AlreadyRegisteredUserDTO => AlreadyRegisteredUserDTO.BirthDate;
        } else if(ust == UserSortTypes.COUNTRY){
            return AlreadyRegisteredUserDTO => AlreadyRegisteredUserDTO.Country;
        } else if(ust == UserSortTypes.ROLE){
            return AlreadyRegisteredUserDTO => AlreadyRegisteredUserDTO.Role;
        }
        return AlreadyRegisteredUserDTO => AlreadyRegisteredUserDTO.Name;
    }
    #endregion

    #region listings
    public async Task<List<AlreadyRegisteredUserDTO>> ListAdministrators(){
        List<AlreadyRegisteredUserDTO> userList = await ur.GetUsers();
        var adminList = new List<AlreadyRegisteredUserDTO>();
        foreach(AlreadyRegisteredUserDTO o in userList){
            if(o.Role == Roles.ADMINISTRATOR){
                adminList.Add(o);
            }
        }
        return adminList;
    }
    #endregion
}
