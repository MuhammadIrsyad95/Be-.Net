using MySql.Data.MySqlClient;
using WebApi.Controllers;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class CartRepository
    {
        private string connStr = string.Empty;
        public CartRepository(IConfiguration configuration)
        {
            // dijalankan pertama kali ketika object dibuat 
            connStr = configuration.GetConnectionString("Default");

        }
        //====================================== BEGIN List Course in Cart ======================================
        public List<Cart> GetCheckoutByUserId(int userId)
        {
            List<Cart> carts = new List<Cart>();

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = "SELECT keranjang_item.id as idKeranjang, produk.*, produk.imageUrl as img, kategori_produk.nama_kategori, CONCAT( CASE WHEN DAYOFWEEK(keranjang_item.schedule) = 1 THEN 'Minggu' WHEN DAYOFWEEK(keranjang_item.schedule) = 2 THEN 'Senin' WHEN DAYOFWEEK(keranjang_item.schedule) = 3 THEN 'Selasa' WHEN DAYOFWEEK(keranjang_item.schedule) = 4 THEN 'Rabu' WHEN DAYOFWEEK(keranjang_item.schedule) = 5 THEN 'Kamis' WHEN DAYOFWEEK(keranjang_item.schedule) = 6 THEN 'Jumat' WHEN DAYOFWEEK(keranjang_item.schedule) = 7 THEN 'Sabtu' END, ', ', DAY(keranjang_item.schedule), ' ', CASE WHEN MONTH(keranjang_item.schedule) = 1 THEN 'Januari' WHEN MONTH(keranjang_item.schedule) = 2 THEN 'Februari' WHEN MONTH(keranjang_item.schedule) = 3 THEN 'Maret' WHEN MONTH(keranjang_item.schedule) = 4 THEN 'April' WHEN MONTH(keranjang_item.schedule) = 5 THEN 'Mei' WHEN MONTH(keranjang_item.schedule) = 6 THEN 'Juni' WHEN MONTH(keranjang_item.schedule) = 7 THEN 'Juli' WHEN MONTH(keranjang_item.schedule) = 8 THEN 'Agustus' WHEN MONTH(keranjang_item.schedule) = 9 THEN 'September' WHEN MONTH(keranjang_item.schedule) = 10 THEN 'Oktober' WHEN MONTH(keranjang_item.schedule) = 11 THEN 'November' WHEN MONTH(keranjang_item.schedule) = 12 THEN 'Desember' END, ' ', YEAR(keranjang_item.schedule) ) AS schedule, pengguna.nama as nama_pembeli, pengguna.id, keranjang_item.status as status_co from produk JOIN kategori_produk on produk.kategori_id = kategori_produk.id JOIN keranjang_item on produk.id = keranjang_item.produk_id JOIN pengguna on keranjang_item.pengguna_id = pengguna.id WHERE keranjang_item.status = 1 AND pengguna.id = @userId;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    carts.Add(new Cart()
                    {
                        Id = reader.GetInt32("idKeranjang"),
                        IdCourse = reader.GetInt32("id"), //get data dari database
                        NamaCourse = reader.GetString("nama_produk"),
                        Deskripsi = reader.GetString("deskripsi_produk"),
                        Harga = reader.GetInt32("harga"),
                        imageUrl = reader.GetString("img"),
                        NamaKategori = reader.GetString("nama_kategori"),
                        NamaPembeli = reader.GetString("nama_pembeli"),
                        Schedule = reader.GetString("schedule")
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();

            return carts;
        }

        public SpecificCart Get1CartByUserId(int userId, int idCourse, string schedule)
        {
            SpecificCart carts = new SpecificCart();

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = "SELECT keranjang_item.id as idKeranjang, produk.*, produk.imageUrl as img, kategori_produk.nama_kategori, left(keranjang_item.schedule, 10) schedule, pengguna.nama as nama_pembeli, pengguna.id, keranjang_item.status as status_co from produk JOIN kategori_produk on produk.kategori_id = kategori_produk.id JOIN keranjang_item on produk.id = keranjang_item.produk_id JOIN pengguna on keranjang_item.pengguna_id = pengguna.id WHERE pengguna.id = @userId and keranjang_item.produk_id = @idCourse and schedule = @schedule;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@idCourse", idCourse);
                cmd.Parameters.AddWithValue("@schedule", schedule);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    carts = new SpecificCart()
                    {
                        Id = reader.GetInt32("idKeranjang"),
                        IdCourse = reader.GetInt32("id"),
                        Schedule = reader.GetString("schedule")
                        // Add other properties as needed
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();

            return carts;
        }

        public static implicit operator CartRepository(CartController v)
        {
            throw new NotImplementedException();
        }

        public static implicit operator CartRepository(OrderController v)
        {
            throw new NotImplementedException();
        }

        //====================================== END List Course in Cart ======================================
        public void CreateCart(int userId, List<int> productIds, string schedule)
        {
            //get connection to database
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            MySqlTransaction transaction = conn.BeginTransaction();
            try
            {
                //insert new cart
                foreach (int productId in productIds)
                {
                    using (var cmd = new MySqlCommand("INSERT INTO keranjang_item(pengguna_id, produk_id, schedule) VALUES (@userId, @ProductId, @schedule)", conn))
                    {
                        //cmd.Parameters.AddWithValue("@OrderId", orderId);
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@ProductId", productId);
                        cmd.Parameters.AddWithValue("@schedule", schedule);
                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();
                    }

                    //using (var cmdInactive = new MySqlCommand("UPDATE produk SET status=0 where id = @ProductId", conn))
                    //{
                    //cmd.Parameters.AddWithValue("@OrderId", orderId);
                    //cmd.Parameters.AddWithValue("@userId", userId);
                    //cmdInactive.Parameters.AddWithValue("@ProductId", productId);
                    //cmdInactive.Transaction = transaction;
                    //cmdInactive.ExecuteNonQuery();
                    //}
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();
        }

        public bool Delete(int cartId)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            MySqlTransaction transaction = conn.BeginTransaction();

            try
            {
                //string sql = "SELECT produk_id FROM keranjang_item where id = @Id";
                //MySqlCommand cmdselect = new MySqlCommand(sql, conn);
                //cmdselect.Parameters.AddWithValue("@Id", cartId);

                //MySqlDataReader reader = cmdselect.ExecuteReader();

                //int produkId = 0;
                //if (reader.Read())
                //{
                //produkId = reader.GetInt32("produk_id");
                //}
                //reader.Close();

                using (var cmd = new MySqlCommand("DELETE FROM keranjang_item WHERE id = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", cartId);
                    cmd.Transaction = transaction;
                    cmd.ExecuteNonQuery();
                }

                //using (var cmd = new MySqlCommand("UPDATE produk SET status = 1 WHERE id = @produkId", conn))
                //{
                //cmd.Parameters.AddWithValue("@produkId", produkId);
                //cmd.Transaction = transaction;
                //cmd.ExecuteNonQuery();
                //}

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine(ex.ToString());
                return false; // Jika terjadi kesalahan, kembalikan false
            }
            finally
            {
                conn.Close();
            }

            return true; // Jika sukses, kembalikan true
        }
    }
}