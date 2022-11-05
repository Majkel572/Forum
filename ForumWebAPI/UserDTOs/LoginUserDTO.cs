namespace ForumWebAPI
{
    public class LoginUserDTO{
        private string username { set{ username = value; }}
        private string hashedPassword { set{ hashedPassword = value; } }
    }
}