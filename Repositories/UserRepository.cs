using CRUD_JWT_AUTH.Models;

namespace CRUD_JWT_AUTH.Repositories
{
    public class UserRepository
    {
        public static User Get(string username, string password)
        {
            var users = new List<User>
            {
                new () {Id = 1, Username = "GerenteDaLoja", Password = "Gerente", Role = "manager"},
                new () {Id = 2, Username = "Caixa", Password = "Caixa", Role = "employee"}
            };

            return users.
            FirstOrDefault(x => x.Username == username 
            && x.Password == password);
        }
    }
}