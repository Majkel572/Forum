namespace ForumWebAPI
{
    public class RegisteredUserDTO{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Country { get; set; }
        public DateTime BirthDate { get; set; }
        public string email { get; set; }
    }
}