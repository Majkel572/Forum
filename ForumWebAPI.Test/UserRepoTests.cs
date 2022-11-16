using Moq;

namespace ForumWebAPI.Test;

public class UserRepoTests{
   
    [Fact]
    public async Task AddPlayer_ShouldExecute_WhenPlayerIsValid(){
        var dataContext = new Mock<DataContext>();
        var userRepo = new Mock<IUserRepo>();
    }

    #region helpers
    private User CreateCorrectUser(){
        User u = new User();
        u.Id = 0;
        u.Name = "Mikolaj";
        u.Surname = "Guryn";
        u.Country = "Poland";
        u.BirthDate = new DateTime(2001, 03, 25);
        u.Email = "mikulio@gmail.com";
        u.Username = "Miki";
        u.HashedPassword = "Miki01";
        u.Role = Roles.ADMINISTRATOR;
        u.Posts = new List<Post>();
        return u;
    }
    #endregion
}