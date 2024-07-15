using MySql.Data.MySqlClient;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class UserRepository
    {
        //Dependencies Injection
        private string connStr = string.Empty;


        public UserRepository(IConfiguration configuration)
        {
            // dijalankan pertama kali ketika object dibuat 
            connStr = configuration.GetConnectionString("Default");

        }

        public List<User> GetAll()
        {
            List<User> users = new List<User>();


            MySqlConnection conn = new MySqlConnection(connStr);
            // get connecttion to database
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                // Perform database operations
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM pengguna ", conn);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    string name = reader.GetString("nama");
                    string email = reader.GetString("email");
                    string password = reader.GetString("password");
                    string role = reader.GetString("role");
                    int status = reader.GetInt32("status");


                    users.Add(new User
                    {
                        Id = id,
                        Nama = name,
                        Email = email,
                        Password = password,
                        Role = role,
                        Status = status,

                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");


            return users;
        }

        public void CreateUser(string name, string email, string hashedPassword)
        {

            MySqlConnection conn = new MySqlConnection(connStr);
            // get connecttion to database
            try
            {

                conn.Open();
                // Perform database operations
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `pengguna` (`id`, `nama`, `email`, `password`,`role`) VALUES (DEFAULT, @Nama, @Email, @Password,'user')", conn);

                cmd.Parameters.AddWithValue("@Nama", name);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", hashedPassword);
                //cmd.Parameters.AddWithValue("@Role", role);
                //cmd.Parameters.AddWithValue("@Status", status);



                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
        }

        public void CreateAdmin(string name, string email, string hashedPassword)
        {
            //get connection to database
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `pengguna` (`id`, `nama`, `email`, `password`,`role`) VALUES (DEFAULT, @Nama, @Email, @Password,'admin')", conn);


                cmd.Parameters.AddWithValue("@Nama", name);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", hashedPassword);
                //cmd.Parameters.AddWithValue("@Role", role);
                //cmd.Parameters.AddWithValue("@Status", status);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();
        }
        public void Update(int id, string name, string email, string password)
        {

            MySqlConnection conn = new MySqlConnection(connStr);
            // get connecttion to database
            try
            {

                conn.Open();
                // Perform database operations
                MySqlCommand cmd = new MySqlCommand("UPDATE pengguna SET nama=@Nama, email=@Email, password=@Password WHERE id=@Id", conn);

                cmd.Parameters.AddWithValue("@Nama", name);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Id", id);
                //cmd.Parameters.AddWithValue("@Status", status);


                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
        }

        public void Delete(int id)
        {
            {

                MySqlConnection conn = new MySqlConnection(connStr);
                // get connecttion to database
                try
                {

                    conn.Open();
                    // Perform database operations
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM pengguna WHERE id=@id", conn);


                    cmd.Parameters.AddWithValue("@Id", id);

                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                conn.Close();
            }
        }
        public void SetActive(int id)
        {
            //get connection to database
            MySqlConnection conn = new MySqlConnection(connStr);
            int isStatus = 0;
            try
            {
                conn.Open();

                string selectQuery = "SELECT status FROM pengguna WHERE id = @Id";
                MySqlCommand selectCmd = new MySqlCommand(selectQuery, conn);
                selectCmd.Parameters.AddWithValue("@Id", id);


                using (MySqlDataReader reader = selectCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isStatus = reader.GetInt32("status") == 1 ? 0 : 1;
                    }
                }

                string updateQuery = "UPDATE pengguna SET status = @Status WHERE id = @Id";
                MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                updateCmd.Parameters.AddWithValue("@Status", isStatus);
                updateCmd.Parameters.AddWithValue("@Id", id);

                updateCmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                // Tangani kesalahan saat eksekusi perintah SQL
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                conn.Close(); // Pastikan koneksi ditutup, baik terjadi kesalahan maupun tidak
            }
        }

        public User? GetByEmailAndPassword(string email, string password)
        {
            User? user = null;

            //get connection to database
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT id, email, role FROM pengguna WHERE email=@Email and password=@Password", conn);

                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    user = new User
                    {
                        Id = reader.GetInt32("id"),
                        Email = reader.GetString("email"),
                        Role = reader.GetString("role"),
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();

            return user;
        }

        public User? GetByEmail(string email)
        {
            User? user = null;

            //get connection to database
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT id, email, role FROM pengguna WHERE email=@Email", conn);

                cmd.Parameters.AddWithValue("@Email", email);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    user = new User
                    {
                        Id = reader.GetInt32("id"),
                        Email = reader.GetString("email"),
                        Role = reader.GetString("role"),
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();

            return user;
        }

        public User? GetByEmailAndResetToken(string email, string resetToken)
        {
            User? user = null;

            //get connection to database
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT id, email, role FROM pengguna WHERE email=@Email AND reset_password_token=@ResetToken", conn);

                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@ResetToken", resetToken);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    user = new User
                    {
                        Id = reader.GetInt32("id"),
                        Email = reader.GetString("email"),
                        Role = reader.GetString("role"),
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();

            return user;
        }


        public bool InsertResetPasswordToken(int userId, string token)
        {
            bool isSuccess = false;

            //get connection to database
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE pengguna SET reset_password_token=@Token WHERE id=@Id", conn);

                cmd.Parameters.AddWithValue("@Id", userId);
                cmd.Parameters.AddWithValue("@Token", token);

                cmd.ExecuteNonQuery();

                isSuccess = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();

            return isSuccess;
        }

        public bool UpdatePassword(int userId, string newPassword)
        {
            bool isSuccess = false;

            //get connection to database
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE pengguna SET password=@NewPassword WHERE id=@Id", conn);

                cmd.Parameters.AddWithValue("@Id", userId);
                cmd.Parameters.AddWithValue("@NewPassword", newPassword);

                cmd.ExecuteNonQuery();

                isSuccess = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();

            return isSuccess;
        }

        public bool Checkemail(string email)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            bool isEmailExists = false;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM pengguna WHERE email=@Email", conn);
                cmd.Parameters.AddWithValue("@Email", email);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                isEmailExists = count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                conn.Close();
            }

            return isEmailExists;
        }
    }

}

