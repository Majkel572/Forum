// using Moq;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;
// using Microsoft.AspNetCore.Identity;

// namespace ForumWebAPI.Test;

// public class UserRepoTests{
   
//     [Fact]
//     public async Task AddPlayer_ShouldExecute_WhenPlayerIsValid(){
//         var mockRepo = new Mock<IUserRepo>();
//         var mockContext = new Mock<DataContext>();
//         // var mockSet = new Mock<DbSet<User>>();
        
//         List<AlreadyRegisteredUserDTO> user = CreateCorrectUsers();
        
//         mockRepo.Setup(m => m.GetUsers()).ReturnsAsync(CreateCorrectUsers());
//         // mockContext.Setup(m => m.)
//         var userService = new UserService(mockContext.Object);

//         var adminList = await userService.ListAdministrators();
        
//         int adminListExpected = 1;
//         int adminListOutcome = adminList.Count;
//         string adminNameExpected = "Mikolaj";
//         string adminNameOutcome = adminList[0].Name;

//         Assert.Equal(adminListExpected, adminListOutcome);
//         Assert.Equal(adminNameExpected, adminNameOutcome);
//     }

//     #region helpers
//     private List<AlreadyRegisteredUserDTO> CreateCorrectUsers(){
//         var userList = new List<AlreadyRegisteredUserDTO>();
//         userList.Add(new AlreadyRegisteredUserDTO(0, "Mikolaj", "Guryn", "Poland",
//                                                 "Miki", new DateTime(2001,03,25), Roles.ADMINISTRATOR));
//         userList.Add(new AlreadyRegisteredUserDTO(0, "Michal", "Tamburyn", "Romania",
//                                                 "Michi", new DateTime(2006,04,30), Roles.DEFAULT));
//         return userList;
//     }
//     #endregion
// }