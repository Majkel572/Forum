using Microsoft.EntityFrameworkCore;
using ForumWebAPI.UserDTOs;
namespace ForumWebAPI.UserRepos;

public interface IUserRepo{   
    public Task<bool> AddUser(RegisterUserDTO p);

    public Task<bool> UpdateUser(RegisterUserDTO u);

    public Task<AlreadyRegisteredUserDTO> GetUser(string email);

    public Task<List<AlreadyRegisteredUserDTO>> DeleteUser(string email);

    public Task<List<AlreadyRegisteredUserDTO>> GetUsers(); // dodac pobranie listy administratorow do serwisu
    // dodac test na pelnoletnich
    // uzytkownikow tylko z polski.
    // uzytkownikow tylko z takim samym haslem xd
    // uzytkownicy bez posta
    // uzytkownicy o takiej samej ilosci postow
    // przepisac sobie obrobke danych do repo!
}