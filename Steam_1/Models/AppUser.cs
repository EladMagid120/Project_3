using Steam_1.DAL;
using System.Text.Json.Serialization;

namespace Steam_1.Models
{
    public class AppUser
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Email")]
        public string Email { get; set; }

        [JsonPropertyName("Password")]
        public string Password { get; set; }


        public AppUser() { }

        public AppUser(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }

        static public List<AppUser> Read()
        {
            DBServices dbs = new DBServices();

            return dbs.Read();
        }

        public int Insert()
        {
            List<AppUser> users = Read();

            foreach (AppUser user in users)
            {
                if (this.Email == user.Email)
                {
                    return 0;
                }
            }

            DBServices dbs = new DBServices();
            return dbs.Insert(this);
        }

         public bool Login(string Email, string Password)
         {
            
            DBServices dbs = new DBServices();

            foreach (AppUser user in dbs.Read())
            {
                if (user.Email == Email && user.Password == Password)
                {
                    return true;

                }
            }
            return false;
         }

        public (int UserId, string Name) GetUserIdByEmail(string email)
        {
            // שימוש ב-DBServices לשליפת ה-ID לפי אימייל
            DBServices dbs = new DBServices();
            return dbs.GetUserIdByEmail(email); // קריאה למתודה הנכונה ב-DBServices
        }

        public AppUser Update()
        {
            DBServices dbs = new DBServices();
            List<AppUser> usersList = dbs.Read(); // קבלת רשימת המשתמשים

            // בדיקת אם המשתמש קיים
            AppUser existingUser = usersList.FirstOrDefault(u => u.Id == this.Id);
            if (existingUser == null)
            {
                return null; // המשתמש לא נמצא
            }

            // בדיקת אם האימייל בשימוש על ידי משתמש אחר
            bool isEmailUsedByAnother = usersList.Any(u => u.Email == this.Email && u.Id != this.Id);
            if (isEmailUsedByAnother)
            {
                return null; // האימייל תפוס
            }

            // עדכון המידע של המשתמש
            return dbs.UpdateUser(this);
        }
    }
}
