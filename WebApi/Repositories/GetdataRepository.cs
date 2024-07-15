using MySql.Data.MySqlClient;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class GetdataRepository
    {
        private readonly string connStr = string.Empty;
        public GetdataRepository(IConfiguration configuration)
        {
            connStr = configuration.GetConnectionString("Default");
        }
        
        //====================================== BEGIN Landing Page Course Limit ======================================
        public List<Getdata> GetCourseLimit()
        {
            List<Getdata> checkouts = new List<Getdata>();

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = "SELECT produk.*, kategori_produk.nama_kategori FROM produk JOIN kategori_produk on produk.kategori_id = kategori_produk.id WHERE produk.id NOT IN ( SELECT keranjang_item.produk_id FROM keranjang_item WHERE keranjang_item.pengguna_id = 1 ) AND produk.status = 1 limit 6";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                //cmd.Parameters.AddWithValue("@userId", userId);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    checkouts.Add(new Getdata()
                    {
                        Id = reader.GetInt32("id"), //get data dari database
                        NamaCourse = reader.GetString("nama_produk"),
                        Deskripsi = reader.GetString("deskripsi_produk"),
                        Harga = reader.GetInt32("harga"),
                        imageUrl = reader.GetString("imageUrl"),
                        NamaKategori = reader.GetString("nama_kategori"),
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();

            return checkouts;
        }
        //====================================== END Landing Page Course Limit ======================================

        //====================================== BEGIN Course Detail Course By Id ======================================
        public List<Getdata> GetCourseById(int CourseId)
        {
            List<Getdata> checkouts = new List<Getdata>();

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = "SELECT produk.*, kategori_produk.nama_kategori FROM produk JOIN kategori_produk on produk.kategori_id = kategori_produk.id WHERE produk.id NOT IN ( SELECT keranjang_item.produk_id FROM keranjang_item WHERE keranjang_item.pengguna_id = 1 ) AND produk.status = 1 AND produk.id = @CourseId;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@CourseId", CourseId);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    checkouts.Add(new Getdata()
                    {
                        Id = reader.GetInt32("id"), //get data dari database
                        NamaCourse = reader.GetString("nama_produk"),
                        Deskripsi = reader.GetString("deskripsi_produk"),
                        Harga = reader.GetInt32("harga"),
                        imageUrl = reader.GetString("imageUrl"),
                        NamaKategori = reader.GetString("nama_kategori"),
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();

            return checkouts;
        }
        //====================================== END Course Detail Course By Id ======================================

        //====================================== BEGIN Course By Category Id ======================================
        public List<Getdata> GetCourseByCategoryId(int categoryId)
        {
            List<Getdata> checkouts = new List<Getdata>();

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = "SELECT produk.*, kategori_produk.nama_kategori FROM produk JOIN kategori_produk on produk.kategori_id = kategori_produk.id WHERE produk.id NOT IN ( SELECT keranjang_item.produk_id FROM keranjang_item WHERE keranjang_item.pengguna_id = 1 ) AND produk.status = 1 AND produk.kategori_id = @categoryId";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@categoryId", categoryId);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    checkouts.Add(new Getdata()
                    {
                        Id = reader.GetInt32("id"), //get data dari database
                        NamaCourse = reader.GetString("nama_produk"),
                        Deskripsi = reader.GetString("deskripsi_produk"),
                        Harga = reader.GetInt32("harga"),
                        imageUrl = reader.GetString("imageUrl"),
                        NamaKategori = reader.GetString("nama_kategori"),
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();

            return checkouts;
        }
        //====================================== END Course By Category Id ======================================

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

        //====================================== BEGIN List Course in Cart ======================================
        public List<Getdata> GetCheckoutByUserId(int userId)
        {
            List<Getdata> checkouts = new List<Getdata>();

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = "SELECT produk.*, kategori_produk.nama_kategori, keranjang_item.schedule, " +
                    "pengguna.nama as nama_pembeli, pengguna.id, keranjang_item.status as status_co " +
                    "from produk " +
                    "JOIN kategori_produk on produk.kategori_id = kategori_produk.id " +
                    "JOIN keranjang_item on produk.id = keranjang_item.produk_id " +
                    "JOIN pengguna on keranjang_item.pengguna_id = pengguna.id " +
                    "WHERE keranjang_item.status = 1 AND pengguna.id = @userId;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    checkouts.Add(new Getdata()
                    {
                        Id = reader.GetInt32("id"), //get data dari database
                        NamaCourse = reader.GetString("nama_produk"),
                        Deskripsi = reader.GetString("deskripsi_produk"),
                        Harga = reader.GetInt32("harga"),
                        imageUrl = reader.GetString("imageUrl"),
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

            return checkouts;
        }
        //====================================== END List Course in Cart ======================================

        //====================================== BEGIN Invoice User ======================================
        public List<Invoice> GetInvoiceByUserId(int userId)
        {
            List<Invoice> invoices = new List<Invoice>();

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = "SELECT pesanan.id as order_id, DATE_FORMAT(pesanan.created_at, '%d %M %Y') as tanggal_beli, COUNT(detail_pesanan.id) as jumlah_kursus, SUM(produk.harga) as total_harga FROM produk JOIN kategori_produk on produk.kategori_id = kategori_produk.id JOIN keranjang_item on produk.id = keranjang_item.produk_id JOIN detail_pesanan on keranjang_item.id = detail_pesanan.kItem_id JOIN pesanan on detail_pesanan.pesanan_id = pesanan.id JOIN pengguna on keranjang_item.pengguna_id = pengguna.id WHERE pengguna.id = @UserId;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    invoices.Add(new Invoice()
                    {
                        //Id = reader.GetInt32("id"), //get data dari database
                        NoInvoice = "APM-"+reader.GetString("order_id"),
                        JumlahKursus = reader.GetInt32("jumlah_kursus"),
                        TanggalBeli = reader.GetString("tanggal_beli"),
                        TotalHarga = reader.GetInt32("total_harga")
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();

            return invoices;
        }
        //====================================== END Invoice User ======================================

        //====================================== BEGIN Detail Invoice User ======================================
        public List<Invoice> GetDetailInvoiceByUserId(int orderId,int userId)
        {
            List<Invoice> invoices = new List<Invoice>();

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = "SELECT pesanan.id as order_id, DATE_FORMAT(pesanan.created_at, '%d %M %Y') as tanggal_beli, COUNT(detail_pesanan.id) as jumlah_kursus, SUM(produk.harga) as total_harga FROM produk JOIN kategori_produk on produk.kategori_id = kategori_produk.id JOIN keranjang_item on produk.id = keranjang_item.produk_id JOIN detail_pesanan on keranjang_item.id = detail_pesanan.kItem_id JOIN pesanan on detail_pesanan.pesanan_id = pesanan.id JOIN pengguna on keranjang_item.pengguna_id = pengguna.id WHERE pengguna.id = @UserId and pesanan.id = @orderId;;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@orderId", orderId);
                cmd.Parameters.AddWithValue("@userId", userId);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    invoices.Add(new Invoice()
                    {
                        //Id = reader.GetInt32("id"), //get data dari database
                        NoInvoice = "APM-" + reader.GetString("order_id"),
                        //JumlahKursus = reader.GetInt32("jumlah_kursus"),
                        TanggalBeli = reader.GetString("tanggal_beli"),
                        TotalHarga = reader.GetInt32("total_harga")
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();

            return invoices;
        }
        //====================================== END Detail Invoice User ======================================

        //====================================== BEGIN Detail Invoice Item User ======================================
        public List<DetInvoice> GetDetailInvoiceItemeByUserId(int orderId, int userId)
        {
            List<DetInvoice> detInvoices = new List<DetInvoice>();

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = "SELECT produk.nama_produk, kategori_produk.nama_kategori, DATE_FORMAT(keranjang_item.schedule, '%W, %d %M %Y') AS schedule, produk.harga FROM produk JOIN kategori_produk on produk.kategori_id = kategori_produk.id JOIN keranjang_item on produk.id = keranjang_item.produk_id JOIN detail_pesanan on keranjang_item.id = detail_pesanan.kItem_id JOIN pesanan on detail_pesanan.pesanan_id = pesanan.id JOIN pengguna on keranjang_item.pengguna_id = pengguna.id WHERE pengguna.id = @UserId and pesanan.id = @orderId;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@orderId", orderId);
                cmd.Parameters.AddWithValue("@userId", userId);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    detInvoices.Add(new DetInvoice()
                    {
                        //Id = reader.GetInt32("id"), //get data dari database
                        NamaCourse = reader.GetString("nama_produk"),
                        NamaKategori = reader.GetString("nama_kategori"),
                        Schedule = reader.GetString("schedule"),
                        Harga = reader.GetInt32("harga")
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();

            return detInvoices;
        }
        //====================================== END Detail Invoice Item User ======================================

    }
}
