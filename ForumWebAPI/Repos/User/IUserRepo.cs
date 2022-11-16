using Microsoft.EntityFrameworkCore;

namespace ForumWebAPI;

public interface IUserRepo{   
    public Task<List<User>> AddUser(User p);

    public Task<List<User>> UpdateUser();

    public Task<User> GetUser(int id);

    public Task<List<User>> DeleteUser(User u);

    public Task<List<User>> GetUsers();
}