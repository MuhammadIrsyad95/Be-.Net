using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using WebApi.Models;
using WebApi.Repositories.Interface;

namespace WebApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string connStr = string.Empty;

        public ProductRepository(IConfiguration configuration)
        {
            connStr = configuration.GetConnectionString("Default");
        }

        public Product GetById(int id)
        {
            Product product = null;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string sql = "SELECT * FROM produk WHERE id = @Id";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            product = new Product()
                            {
                                Id = reader.GetInt32("id"),
                                Nama_produk = reader.GetString("nama_produk"),
                                Deskripsi_produk = reader.GetString("deskripsi_produk"),
                                Harga = reader.GetInt32("harga"),
                                ImageUrl = reader.GetString("imageUrl"),
                                Kategori_id = reader.GetInt32("kategori_id")
                            };
                        }
                    }
                }
            }

            return product;
        }

        public List<Product> GetAll()
        {
            List<Product> products = new List<Product>();

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string sql = "SELECT produk.*, kategori_produk.nama_kategori FROM produk join kategori_produk on produk.kategori_id = kategori_produk.id ";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32("id");
                            string name = reader.GetString("nama_produk");
                            string description = reader.GetString("deskripsi_produk");
                            int price = reader.GetInt32("harga");
                            string imageUrl = reader.GetString("imageUrl");
                            int status = reader.GetInt32("status");
                            int kategori_id = reader.GetInt32("kategori_id");
                            string nama_kategori = reader.GetString("nama_kategori");

                            products.Add(new Product
                            {
                                Id = id,
                                Nama_produk = name,
                                Deskripsi_produk = description,
                                Harga = price,
                                ImageUrl = imageUrl,
                                Status = status,
                                Kategori_id = kategori_id,
                                NamaKategori = nama_kategori
                            });
                        }
                    }
                }
            }

            return products;
        }

        public void Create(string name, string description, int price, string imageUrl, int kategori_id)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string sql = "INSERT INTO `produk` (`nama_produk`, `deskripsi_produk`, `harga`, `imageUrl`, `kategori_id`) VALUES (@Nama_produk, @Deskripsi_produk, @Harga, @ImageUrl, @Kategori_id)";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Nama_produk", name);
                    cmd.Parameters.AddWithValue("@Deskripsi_produk", description);
                    cmd.Parameters.AddWithValue("@Harga", price);
                    cmd.Parameters.AddWithValue("@ImageUrl", imageUrl);
                    cmd.Parameters.AddWithValue("@Kategori_id", kategori_id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool Update(int id, string name, string description, int price, string imageUrl, int kategori_id)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string sql = "UPDATE produk SET nama_produk = @Nama_produk, deskripsi_produk = @Deskripsi_produk, harga = @Harga, imageUrl = @ImageUrl, kategori_id = @Kategori_id WHERE id = @Id";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Nama_produk", name);
                    cmd.Parameters.AddWithValue("@Deskripsi_produk", description);
                    cmd.Parameters.AddWithValue("@Harga", price);
                    cmd.Parameters.AddWithValue("@ImageUrl", imageUrl);
                    cmd.Parameters.AddWithValue("@Kategori_id", kategori_id);
                    cmd.Parameters.AddWithValue("@Id", id);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0; // Assuming rowsAffected > 0 indicates successful update
                }
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string sql = "DELETE FROM produk WHERE id = @Id";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    cmd.ExecuteNonQuery();
                }
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

                string selectQuery = "SELECT status FROM produk WHERE id = @Id";
                MySqlCommand selectCmd = new MySqlCommand(selectQuery, conn);
                selectCmd.Parameters.AddWithValue("@Id", id);


                using (MySqlDataReader reader = selectCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isStatus = reader.GetInt32("status") == 1 ? 0 : 1;
                    }
                }

                string updateQuery = "UPDATE produk SET status = @Status WHERE id = @Id";
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

        //====================================== BEGIN Landing Page Course Limit ======================================
        public List<ProductLanding> GetCourseLimit(int userId)
        {
            List<ProductLanding> productLandings = new List<ProductLanding>();

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = "SELECT produk.*, produk.kategori_id, kategori_produk.nama_kategori FROM produk JOIN kategori_produk on produk.kategori_id = kategori_produk.id WHERE produk.id NOT IN ( SELECT keranjang_item.produk_id FROM keranjang_item WHERE keranjang_item.pengguna_id = @userId ) AND produk.status = 1 limit 6";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    productLandings.Add(new ProductLanding()
                    {
                        Id = reader.GetInt32("id"), //get data dari database
                        NamaCourse = reader.GetString("nama_produk"),
                        Deskripsi = reader.GetString("deskripsi_produk"),
                        Harga = reader.GetInt32("harga"),
                        imageUrl = reader.GetString("imageUrl"),
                        IdKategori = reader.GetInt32("kategori_id"),
                        NamaKategori = reader.GetString("nama_kategori"),
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();

            return productLandings;
        }
        //====================================== END Landing Page Course Limit ======================================

        //====================================== BEGIN Landing Page Course Limits (Unglogin) ======================================
        public List<ProductLanding> GetCourseLimits()
        {
            List<ProductLanding> productLandings = new List<ProductLanding>();

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = "SELECT produk.*, produk.kategori_id, kategori_produk.nama_kategori FROM produk JOIN kategori_produk on produk.kategori_id = kategori_produk.id WHERE produk.status = 1 limit 6";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                //cmd.Parameters.AddWithValue("@userId", userId);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    productLandings.Add(new ProductLanding()
                    {
                        Id = reader.GetInt32("id"), //get data dari database
                        NamaCourse = reader.GetString("nama_produk"),
                        Deskripsi = reader.GetString("deskripsi_produk"),
                        Harga = reader.GetInt32("harga"),
                        imageUrl = reader.GetString("imageUrl"),
                        IdKategori = reader.GetInt32("kategori_id"),
                        NamaKategori = reader.GetString("nama_kategori"),
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();

            return productLandings;
        }
        //====================================== END Landing Page Course Limits (Unglogin) ======================================

        //====================================== BEGIN Course Detail Course By Id ======================================
        public ProductLanding GetCourseById(int CourseId)
        {
            ProductLanding productLandings = new ProductLanding();

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = "SELECT produk.*, kategori_produk.id as idKategori, kategori_produk.nama_kategori FROM produk JOIN kategori_produk on produk.kategori_id = kategori_produk.id WHERE produk.status = 1 AND produk.id = @CourseId;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                //cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@CourseId", CourseId);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    productLandings = new ProductLanding()
                    {
                        Id = reader.GetInt32("id"), //get data dari database
                        NamaCourse = reader.GetString("nama_produk"),
                        Deskripsi = reader.GetString("deskripsi_produk"),
                        Harga = reader.GetInt32("harga"),
                        imageUrl = reader.GetString("imageUrl"),
                        IdKategori = reader.GetInt32("idKategori"),
                        NamaKategori = reader.GetString("nama_kategori"),
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();

            return productLandings;
        }
        //====================================== END Course Detail Course By Id ======================================

        //====================================== BEGIN Course By Category Id ======================================
        public List<ProductLanding> GetCourseByCategoryId(int categoryId)
        {
            List<ProductLanding> productLandings = new List<ProductLanding>();

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = "SELECT produk.*, kategori_produk.id as idKategori, kategori_produk.nama_kategori FROM produk JOIN kategori_produk on produk.kategori_id = kategori_produk.id WHERE produk.status = 1 AND produk.kategori_id = @categoryId";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                //cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@categoryId", categoryId);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    productLandings.Add(new ProductLanding()
                    {
                        Id = reader.GetInt32("id"), //get data dari database
                        NamaCourse = reader.GetString("nama_produk"),
                        Deskripsi = reader.GetString("deskripsi_produk"),
                        Harga = reader.GetInt32("harga"),
                        imageUrl = reader.GetString("imageUrl"),
                        IdKategori = reader.GetInt32("idKategori"),
                        NamaKategori = reader.GetString("nama_kategori"),
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();

            return productLandings;
        }
        //====================================== END Course By Category Id ======================================
    }
}
