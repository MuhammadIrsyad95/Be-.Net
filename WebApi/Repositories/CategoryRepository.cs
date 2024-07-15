using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration; // Anda perlu menambahkan namespace ini untuk IConfiguration

using WebApi.Models;
using WebApi.Repositories.Interface;
using static System.Net.Mime.MediaTypeNames;

namespace WebApi.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        //Dependencies Injection
        private string connStr = string.Empty;


        public CategoryRepository(IConfiguration configuration)
        {
            // dijalankan pertama kali ketika object dibuat 
            connStr = configuration.GetConnectionString("Default");

        }

        public Category GetById(int id)
        {
            Category category = null;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string sql = "SELECT * FROM kategori_produk WHERE id = @Id";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            category = new Category()
                            {
                                Id = reader.GetInt32("id"),
                                Nama_kategori = reader.GetString("nama_kategori"),
                                Deskripsi = reader.GetString("deskripsi"),
                                //Harga = reader.GetInt32("harga"),
                                ImageUrl = reader.GetString("imageUrl"),
                                //Kategori_id = reader.GetInt32("kategori_id")
                            };
                        }
                    }
                }
            }

            return category;
        }

        public List<Category> GetAll()
        {
            List<Category> categories = new List<Category>();


            MySqlConnection conn = new MySqlConnection(connStr);
            // get connecttion to database
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                // Perform database operations
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM kategori_produk ", conn);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    string name = reader.GetString("nama_kategori");
                    string description = reader.GetString("deskripsi");
                    
                    string imageUrl = reader.GetString("imageUrl");
                    int status = reader.GetInt32("status");
                  

                    categories.Add (new Category
                    {
                        Id = id,
                        Nama_kategori = name,
                        Deskripsi = description,
                        ImageUrl = imageUrl,
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


            return categories;
        }

        

        public Category GetCategoryById(int Id)
        {
            Category categories = new Category();

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = "SELECT * FROM kategori_produk WHERE id=@Id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                //cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@Id", Id);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    categories = new Category()
                    {
                        Id = reader.GetInt32("id"), //get data dari database
                        Nama_kategori = reader.GetString("nama_kategori"),
                        Deskripsi = reader.GetString("deskripsi"),
                        //Harga = reader.GetInt32("harga"),
                        ImageUrl = reader.GetString("imageUrl"),
                        //IdKategori = reader.GetInt32("idKategori"),
                        //NamaKategori = reader.GetString("nama_kategori"),
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();

            return categories;
        }
        public void Create(string name, string description, string imageUrl)
        {

            MySqlConnection conn = new MySqlConnection(connStr);
            // get connecttion to database
            try
            {

                conn.Open();
                // Perform database operations
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `kategori_produk` (`id`, `nama_kategori`, `deskripsi`, `imageUrl`) VALUES (DEFAULT, @Nama_kategori, @Deskripsi, @ImageUrl)", conn);

                cmd.Parameters.AddWithValue("@Nama_kategori", name);
                cmd.Parameters.AddWithValue("@Deskripsi", description);
                
                cmd.Parameters.AddWithValue("@ImageUrl", imageUrl);
                //cmd.Parameters.AddWithValue("@Status", status);
                


                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
        }

        public bool Update(int id, string name, string description, string imageUrl)
        {
            int rowsAffected = 0;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                using (MySqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        MySqlCommand cmd = new MySqlCommand("UPDATE kategori_produk SET nama_kategori=@Nama_kategori, deskripsi=@Deskripsi, imageUrl=@ImageUrl WHERE id=@Id", conn, transaction);

                        cmd.Parameters.AddWithValue("@Nama_kategori", name);
                        cmd.Parameters.AddWithValue("@Deskripsi", description);
                        cmd.Parameters.AddWithValue("@ImageUrl", imageUrl);
                        cmd.Parameters.AddWithValue("@Id", id);

                        rowsAffected = cmd.ExecuteNonQuery();

                        // Commit the transaction if the update is successful
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Log the exception

                        // Rollback the transaction if an exception occurs
                        transaction.Rollback();

                        // Rethrow the exception to propagate it up
                        throw;
                    }
                }
            }

            return rowsAffected > 0;


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
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM kategori_produk WHERE id=@id", conn);


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

                string selectQuery = "SELECT status FROM kategori_produk WHERE id = @Id";
                MySqlCommand selectCmd = new MySqlCommand(selectQuery, conn);
                selectCmd.Parameters.AddWithValue("@Id", id);

                
                using (MySqlDataReader reader = selectCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isStatus = reader.GetInt32("status") == 1 ? 0 : 1;
                    }
                }

                string updateQuery = "UPDATE kategori_produk SET status = @Status WHERE id = @Id";
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

        //====================================== BEGIN Landing Page List Category ======================================
        public List<Category> GetAllCategory()
        {
            List<Category> categories = new List<Category>();

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = "SELECT * FROM kategori_produk";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                //cmd.Parameters.AddWithValue("@userId", userId);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    categories.Add(new Category()
                    {
                        Id = reader.GetInt32("id"), //get data dari database
                        Nama_kategori = reader.GetString("nama_kategori"),
                        Deskripsi = reader.GetString("deskripsi"),
                        //Harga = reader.GetInt32("harga"),
                        ImageUrl = reader.GetString("imageUrl"),
                        //Status = reader.GetBoolean("status"),

                });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();

            return categories;
        }
        //====================================== END Landing Page List Category ======================================

        
    }
}
