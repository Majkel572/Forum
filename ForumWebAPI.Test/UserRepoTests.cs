using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ForumWebAPI.Test;

public class UserRepoTests{
   
    [Fact]
    public async Task AddPlayer_ShouldExecute_WhenPlayerIsValid(){
        var mockContext = new Mock<DataContext>();
        var mockSet = new Mock<DbSet<User>>();
        
        RegisterUserDTO user = CreateCorrectUser();

        mockContext.Setup(m => m.Users).Returns(mockSet.Object);

        var userService = new UserService(mockContext.Object);

        await userService.RegisterUser(user);
        AlreadyRegisteredUserDTO dbUser = await userService.GetUser(user.Id);

        Assert.Equal(dbUser.Name, user.Name);
    }

    #region helpers
    private RegisterUserDTO CreateCorrectUser(){
        RegisterUserDTO u = new RegisterUserDTO();
        u.Id = 0;
        u.Name = "Mikolaj";
        u.Surname = "Guryn";
        u.Country = "Poland";
        u.BirthDate = new DateTime(2001, 03, 25);
        u.Email = "mikulio@gmail.com";
        u.Username = "Miki";
        u.Password = "Miki01";
        u.Role = Roles.ADMINISTRATOR;
        return u;
    }
    #endregion
}