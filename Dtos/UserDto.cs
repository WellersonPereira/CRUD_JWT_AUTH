namespace CRUD_JWT_AUTH.Dtos
{
    public class UserDto
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        
        public string Email { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; }
    }
}