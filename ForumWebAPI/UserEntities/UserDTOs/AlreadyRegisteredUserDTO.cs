namespace ForumWebAPI
{
    public class AlreadyRegisteredUserDTO{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Country { get; set; }
        public string Username { get; set; }
        public Roles Role { get; set; }

        public string RoleReader(){
            switch (this.Role){
                case Roles.DEFAULT:
                    return "default";
                case Roles.MODERATOR:
                    return "moderator";
                case Roles.ADMINISTRATOR:
                    return "administrator";
                case Roles.OWNER:
                    return "owner";
            }
            return "";
        }

        public Roles RoleWriter(string role){
            switch (role){
                case "default":
                    return Roles.DEFAULT;
                case "moderator":
                    return Roles.MODERATOR;
                case "administrator":
                    return Roles.ADMINISTRATOR;
                case "owner":
                    return Roles.OWNER;
            }
            return Roles.DEFAULT;
        }
    }
}