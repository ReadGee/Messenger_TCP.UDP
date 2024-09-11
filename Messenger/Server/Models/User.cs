
namespace Server.Models
{
    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string key { get; set; }

        public User(int id, string username, string password, string key)
        {
            this.id = id;
            this.username = username;
            this.password = password;
            this.key = key;
        }

        public string GetInfo()
        {
            return $"{id} {username} {password} {key}";
        }
    }
}
