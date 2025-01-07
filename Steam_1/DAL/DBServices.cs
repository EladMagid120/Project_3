using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using Steam_1.Models;

namespace Steam_1.DAL
{
    public class DBServices
    {

        public SqlConnection connect(String conString)
        {

            // read the connection string from the configuration file
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            string cStr = configuration.GetConnectionString("igroup7_test2");
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }

        private SqlCommand CreateCommandWithStoredGeneral(String spName, SqlConnection con, Dictionary<string, object> paramDic)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            if (paramDic != null)
                foreach (KeyValuePair<string, object> param in paramDic)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
                }
            return cmd;
        }

        public int Insert(AppUser user)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup7_test2"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Name", user.Name);
            paramDic.Add("@Email", user.Email);
            paramDic.Add("@Password", user.Password);

            cmd = CreateCommandWithStoredGeneral("sp_UserRegister", con, paramDic);          // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        public List<AppUser> Read()
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup7_test2"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            List<AppUser> users = new List<AppUser>();

            cmd = CreateCommandWithStoredGeneral("ReadUsers", con, null);

            try
            {

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    AppUser s = new AppUser();
                    s.Id = Convert.ToInt32(dataReader["UserId"]);
                    s.Name = dataReader["Name"].ToString();
                    s.Email = dataReader["Email"].ToString();
                    s.Password = dataReader["Password"].ToString();
                    users.Add(s);
                }
                return users;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        public (int UserId, string Name) GetUserIdByEmail(string email)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup7_test2");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            try
            {
                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@Email", email);

                cmd = CreateCommandWithStoredGeneral("GetUserIdByEmail", con, paramDic);

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (dataReader.Read())
                {
                    int userId = Convert.ToInt32(dataReader["UserId"]);
                    string name = dataReader["Name"].ToString();

                    return (userId, name); // החזר Tuple עם UserId ו-Name
                }

                return (-1, null); // מזהה לא נמצא
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        public List<Game> ReadGame()
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup7_test2"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            List<Game> games = new List<Game>();

            cmd = CreateCommandWithStoredGeneral("SP_ReadGame", con, null);

            try
            {

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Game g = new Game();
                    g.AppID = Convert.ToInt32(dataReader["AppID"]);
                    g.Name = dataReader["Name"].ToString();
                    g.Release_date = dataReader["Release_date"].ToString();
                    g.Price = Convert.ToDouble(dataReader["Price"]);
                    g.Description = dataReader["description"].ToString();
                    g.Full_audio_languages = dataReader["Full_audio_languages"].ToString();
                    g.Header_image = dataReader["Header_image"].ToString();
                    g.Website = dataReader["Website"].ToString();
                    g.Windows = dataReader["Windows"].ToString();
                    g.Mac = dataReader["Mac"].ToString();
                    g.Linux = dataReader["Linux"].ToString();
                    g.Score_rank = Convert.ToInt32(dataReader["Score_rank"]);
                    g.Recommendations = dataReader["Recommendations"].ToString();
                    g.Developers = dataReader["Developers"].ToString();
                    g.Categories = dataReader["Categories"].ToString();
                    g.Genres = dataReader["Genres"].ToString();
                    g.Tags = dataReader["Tags"].ToString();
                    g.Screenshots = dataReader["Screenshots"].ToString();
                    g.numberOfPurchases = Convert.ToInt32(dataReader["numberOfPurchases"]);
                    games.Add(g);
                }
                return games;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        public List<Game> ReadMyGamesList(int id)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup7_test2"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            List<Game> games = new List<Game>();

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@UserId", id);

            cmd = CreateCommandWithStoredGeneral("GetMyListGame", con, paramDic);

            try
            {

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Game g = new Game();
                    g.AppID = Convert.ToInt32(dataReader["AppID"]);
                    g.Name = dataReader["Name"].ToString();
                    g.Release_date = dataReader["Release_date"].ToString();
                    g.Price = Convert.ToDouble(dataReader["Price"]);
                    g.Description = dataReader["description"].ToString();
                    g.Full_audio_languages = dataReader["Full_audio_languages"].ToString();
                    g.Header_image = dataReader["Header_image"].ToString();
                    g.Website = dataReader["Website"].ToString();
                    g.Windows = dataReader["Windows"].ToString();
                    g.Mac = dataReader["Mac"].ToString();
                    g.Linux = dataReader["Linux"].ToString();
                    g.Score_rank = Convert.ToInt32(dataReader["Score_rank"]);
                    g.Recommendations = dataReader["Recommendations"].ToString();
                    g.Developers = dataReader["Developers"].ToString();
                    g.Categories = dataReader["Categories"].ToString();
                    g.Genres = dataReader["Genres"].ToString();
                    g.Tags = dataReader["Tags"].ToString();
                    g.Screenshots = dataReader["Screenshots"].ToString();
                    g.numberOfPurchases = Convert.ToInt32(dataReader["numberOfPurchases"]);
                    games.Add(g);
                }
                return games;
            }

            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        public List<Game> GetMyListGameByPrice(int id, double Price)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup7_test2"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            List<Game> games = new List<Game>();

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@UserId", id);
            paramDic.Add("@Price", Price);

            cmd = CreateCommandWithStoredGeneral("[GetMyListGameByPrice]", con, paramDic);

            try
            {

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Game g = new Game();
                    g.AppID = Convert.ToInt32(dataReader["AppID"]);
                    g.Name = dataReader["Name"].ToString();
                    g.Release_date = dataReader["Release_date"].ToString();
                    g.Price = Convert.ToDouble(dataReader["Price"]);
                    g.Description = dataReader["description"].ToString();
                    g.Full_audio_languages = dataReader["Full_audio_languages"].ToString();
                    g.Header_image = dataReader["Header_image"].ToString();
                    g.Website = dataReader["Website"].ToString();
                    g.Windows = dataReader["Windows"].ToString();
                    g.Mac = dataReader["Mac"].ToString();
                    g.Linux = dataReader["Linux"].ToString();
                    g.Score_rank = Convert.ToInt32(dataReader["Score_rank"]);
                    g.Recommendations = dataReader["Recommendations"].ToString();
                    g.Developers = dataReader["Developers"].ToString();
                    g.Categories = dataReader["Categories"].ToString();
                    g.Genres = dataReader["Genres"].ToString();
                    g.Tags = dataReader["Tags"].ToString();
                    g.Screenshots = dataReader["Screenshots"].ToString();
                    g.numberOfPurchases = Convert.ToInt32(dataReader["numberOfPurchases"]);
                    games.Add(g);
                }
                return games;
            }

            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        public List<Game> GetMyListGameByRank(int id, int Score_Rank)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup7_test2"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            List<Game> games = new List<Game>();

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@UserId", id);
            paramDic.Add("@rank", Score_Rank);

            cmd = CreateCommandWithStoredGeneral("[GetMyListGameByRank]", con, paramDic);

            try
            {

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Game g = new Game();
                    g.AppID = Convert.ToInt32(dataReader["AppID"]);
                    g.Name = dataReader["Name"].ToString();
                    g.Release_date = dataReader["Release_date"].ToString();
                    g.Price = Convert.ToDouble(dataReader["Price"]);
                    g.Description = dataReader["description"].ToString();
                    g.Full_audio_languages = dataReader["Full_audio_languages"].ToString();
                    g.Header_image = dataReader["Header_image"].ToString();
                    g.Website = dataReader["Website"].ToString();
                    g.Windows = dataReader["Windows"].ToString();
                    g.Mac = dataReader["Mac"].ToString();
                    g.Linux = dataReader["Linux"].ToString();
                    g.Score_rank = Convert.ToInt32(dataReader["Score_rank"]);
                    g.Recommendations = dataReader["Recommendations"].ToString();
                    g.Developers = dataReader["Developers"].ToString();
                    g.Categories = dataReader["Categories"].ToString();
                    g.Genres = dataReader["Genres"].ToString();
                    g.Tags = dataReader["Tags"].ToString();
                    g.Screenshots = dataReader["Screenshots"].ToString();
                    g.numberOfPurchases = Convert.ToInt32(dataReader["numberOfPurchases"]);
                    games.Add(g);
                }
                return games;
            }

            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        public int SaveGameToMyList(int UserId, long GameId)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup7_test2"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@UserId", UserId);
            paramDic.Add("@GameId", GameId);
            

            cmd = CreateCommandWithStoredGeneral("SP_BuyGame", con, paramDic);          // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        public bool DeleteFromMyList(int userid, long GameId)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("igroup7_test2"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@userId", userid);
            paramDic.Add("@gameId", GameId);

            cmd = CreateCommandWithStoredGeneral("DeleteGame", con, paramDic);          // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return (numEffected > 0);

            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        public AppUser UpdateUser(AppUser user)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                // יצירת חיבור למסד הנתונים
                con = connect("igroup7_test2");
            }
            catch (Exception ex)
            {
                throw (ex); // כתיבת שגיאה ללוג
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@UserId", user.Id); // שם הפרמטר לפי הפורמט שלך
            paramDic.Add("@Name", user.Name);
            paramDic.Add("@Email", user.Email);
            paramDic.Add("@Password", user.Password);

            cmd = CreateCommandWithStoredGeneral("UpdateUser", con, paramDic);

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dataReader.Read())
                {
                    // מילוי אובייקט המשתמש עם הנתונים המעודכנים
                    user.Id = Convert.ToInt32(dataReader["UserId"]);
                    user.Name = dataReader["Name"].ToString();
                    user.Email = dataReader["Email"].ToString();
                    user.Password = dataReader["Password"].ToString();
                    return user;
                }
                else
                {
                    return null; // אם לא נמצאו תוצאות
                }
            }
            catch (Exception ex)
            {
                throw (ex); // כתיבת שגיאה ללוג
            }
            finally
            {
                if (con != null)
                {
                    con.Close(); // סגירת החיבור למסד הנתונים
                }
            }
        }

    }
}