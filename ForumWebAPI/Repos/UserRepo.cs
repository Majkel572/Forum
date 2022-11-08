using Microsoft.EntityFrameworkCore;

namespace ForumWebAPI;

public class UserRepo
{   
    private readonly DataContext dataContext;

    public UserRepo(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public async Task<List<User>> AddUser(User p){
        dataContext.Users.Add(p);
        await dataContext.SaveChangesAsync();
        return await dataContext.Users.ToListAsync();
    }

    public async Task<List<User>> UpdateUser(){
        await dataContext.SaveChangesAsync();
        return await dataContext.Users.ToListAsync();
    }

    public async Task<User> GetUser(int id){
        var User = await dataContext.Users.FindAsync(id);
        return User;
    }

    public async Task<List<User>> DeleteUser(User u){
        dataContext.Users.Remove(u);
        await dataContext.SaveChangesAsync();
        return await dataContext.Users.ToListAsync();
    }

    public async Task<List<User>> GetUsers(){
        var UserList = await dataContext.Users.ToListAsync();
        return UserList;
    }
}