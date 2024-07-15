using MySql.Data.MySqlClient;
using WebApi.Controllers;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class OrderRepository
    {
        private readonly string connStr = string.Empty;
        public OrderRepository(IConfiguration configuration)
        {
            connStr = configuration.GetConnectionString("Default");
        }

        //====================================== BEGIN Invoice User ======================================
        public List<Invoice> GetInvoiceByUserId(int userId)
        {
            List<Invoice> invoices = new List<Invoice>();

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                //sql before : SELECT pesanan.id as order_id, DATE_FORMAT(pesanan.created_at, '%d %M %Y') as tanggal_beli, COUNT(detail_pesanan.id) as jumlah_kursus, SUM(produk.harga) as total_harga FROM produk JOIN kategori_produk on produk.kategori_id = kategori_produk.id JOIN keranjang_item on produk.id = keranjang_item.produk_id JOIN detail_pesanan on keranjang_item.id = detail_pesanan.kItem_id JOIN pesanan on detail_pesanan.pesanan_id = pesanan.id JOIN pengguna on keranjang_item.pengguna_id = pengguna.id WHERE pengguna.id = @UserId;
                string sql = "select pesanan.id as order_id, CONCAT( DAY(pesanan.created_at), ' ', CASE WHEN MONTH(pesanan.created_at) = 1 THEN 'Januari' WHEN MONTH(pesanan.created_at) = 2 THEN 'Februari' WHEN MONTH(pesanan.created_at) = 3 THEN 'Maret' WHEN MONTH(pesanan.created_at) = 4 THEN 'April' WHEN MONTH(pesanan.created_at) = 5 THEN 'Mei' WHEN MONTH(pesanan.created_at) = 6 THEN 'Juni' WHEN MONTH(pesanan.created_at) = 7 THEN 'Juli' WHEN MONTH(pesanan.created_at) = 8 THEN 'Agustus' WHEN MONTH(pesanan.created_at) = 9 THEN 'September' WHEN MONTH(pesanan.created_at) = 10 THEN 'Oktober' WHEN MONTH(pesanan.created_at) = 11 THEN 'November' WHEN MONTH(pesanan.created_at) = 12 THEN 'Desember' END, ' ', YEAR(pesanan.created_at) ) AS tanggal_beli, COUNT(detail_pesanan.id) as jumlah_kursus, SUM(produk.harga) as total_harga from pesanan join detail_pesanan on pesanan.id = detail_pesanan.pesanan_id JOIN keranjang_item on detail_pesanan.kItem_id = keranjang_item.id JOIN produk on keranjang_item.produk_id = produk.id where keranjang_item.pengguna_id = @userId GROUP BY pesanan.id;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    invoices.Add(new Invoice()
                    {
                        Id = reader.GetInt32("order_id"), //get data dari database
                        NoInvoice = "APM-" + reader.GetString("order_id"),
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

        public static implicit operator OrderRepository(OrderController v)
        {
            throw new NotImplementedException();
        }
        //====================================== END Invoice User ======================================

        //====================================== BEGIN Detail Invoice User ======================================
        public List<Invoice> GetDetailInvoiceByUserId(int orderId)
        {
            List<Invoice> invoices = new List<Invoice>();

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = "select pesanan.id as order_id, CONCAT( DAY(pesanan.created_at), ' ', CASE WHEN MONTH(pesanan.created_at) = 1 THEN 'Januari' WHEN MONTH(pesanan.created_at) = 2 THEN 'Februari' WHEN MONTH(pesanan.created_at) = 3 THEN 'Maret' WHEN MONTH(pesanan.created_at) = 4 THEN 'April' WHEN MONTH(pesanan.created_at) = 5 THEN 'Mei' WHEN MONTH(pesanan.created_at) = 6 THEN 'Juni' WHEN MONTH(pesanan.created_at) = 7 THEN 'Juli' WHEN MONTH(pesanan.created_at) = 8 THEN 'Agustus' WHEN MONTH(pesanan.created_at) = 9 THEN 'September' WHEN MONTH(pesanan.created_at) = 10 THEN 'Oktober' WHEN MONTH(pesanan.created_at) = 11 THEN 'November' WHEN MONTH(pesanan.created_at) = 12 THEN 'Desember' END, ' ', YEAR(pesanan.created_at) ) AS tanggal_beli, COUNT(detail_pesanan.id) as jumlah_kursus, SUM(produk.harga) as total_harga from pesanan join detail_pesanan on pesanan.id = detail_pesanan.pesanan_id JOIN keranjang_item on detail_pesanan.kItem_id = keranjang_item.id JOIN produk on keranjang_item.produk_id = produk.id where pesanan.id = @orderId;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@orderId", orderId);
                //cmd.Parameters.AddWithValue("@userId", userId);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    invoices.Add(new Invoice()
                    {
                        Id = reader.GetInt32("order_id"), //get data dari database
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
        public List<DetInvoice> GetDetailInvoiceItemeByUserId(int orderId)
        {
            List<DetInvoice> detInvoices = new List<DetInvoice>();

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = "SELECT produk.nama_produk, kategori_produk.nama_kategori, CONCAT( CASE WHEN DAYOFWEEK(keranjang_item.schedule) = 1 THEN 'Minggu' WHEN DAYOFWEEK(keranjang_item.schedule) = 2 THEN 'Senin' WHEN DAYOFWEEK(keranjang_item.schedule) = 3 THEN 'Selasa' WHEN DAYOFWEEK(keranjang_item.schedule) = 4 THEN 'Rabu' WHEN DAYOFWEEK(keranjang_item.schedule) = 5 THEN 'Kamis' WHEN DAYOFWEEK(keranjang_item.schedule) = 6 THEN 'Jumat' WHEN DAYOFWEEK(keranjang_item.schedule) = 7 THEN 'Sabtu' END, ', ', DAY(keranjang_item.schedule), ' ', CASE WHEN MONTH(keranjang_item.schedule) = 1 THEN 'Januari' WHEN MONTH(keranjang_item.schedule) = 2 THEN 'Februari' WHEN MONTH(keranjang_item.schedule) = 3 THEN 'Maret' WHEN MONTH(keranjang_item.schedule) = 4 THEN 'April' WHEN MONTH(keranjang_item.schedule) = 5 THEN 'Mei' WHEN MONTH(keranjang_item.schedule) = 6 THEN 'Juni' WHEN MONTH(keranjang_item.schedule) = 7 THEN 'Juli' WHEN MONTH(keranjang_item.schedule) = 8 THEN 'Agustus' WHEN MONTH(keranjang_item.schedule) = 9 THEN 'September' WHEN MONTH(keranjang_item.schedule) = 10 THEN 'Oktober' WHEN MONTH(keranjang_item.schedule) = 11 THEN 'November' WHEN MONTH(keranjang_item.schedule) = 12 THEN 'Desember' END, ' ', YEAR(keranjang_item.schedule) ) AS schedule, produk.harga FROM produk JOIN kategori_produk on produk.kategori_id = kategori_produk.id JOIN keranjang_item on produk.id = keranjang_item.produk_id JOIN detail_pesanan on keranjang_item.id = detail_pesanan.kItem_id JOIN pesanan on detail_pesanan.pesanan_id = pesanan.id JOIN pengguna on keranjang_item.pengguna_id = pengguna.id WHERE pesanan.id = @orderId;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@orderId", orderId);
                //cmd.Parameters.AddWithValue("@userId", userId);
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

        //====================================== BEGIN Myclass User ======================================
        public List<Myclass> GetDetailMyclass(int userId)
        {
            List<Myclass> myclasses = new List<Myclass>();

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                //sql before : SELECT pesanan.id as order_id, DATE_FORMAT(pesanan.created_at, '%d %M %Y') as tanggal_beli, COUNT(detail_pesanan.id) as jumlah_kursus, SUM(produk.harga) as total_harga FROM produk JOIN kategori_produk on produk.kategori_id = kategori_produk.id JOIN keranjang_item on produk.id = keranjang_item.produk_id JOIN detail_pesanan on keranjang_item.id = detail_pesanan.kItem_id JOIN pesanan on detail_pesanan.pesanan_id = pesanan.id JOIN pengguna on keranjang_item.pengguna_id = pengguna.id WHERE pengguna.id = @UserId;
                string sql = "SELECT produk.id as idProduk, produk.nama_produk, produk.imageUrl, kategori_produk.nama_kategori, CONCAT( CASE WHEN DAYOFWEEK(keranjang_item.schedule) = 1 THEN 'Minggu' WHEN DAYOFWEEK(keranjang_item.schedule) = 2 THEN 'Senin' WHEN DAYOFWEEK(keranjang_item.schedule) = 3 THEN 'Selasa' WHEN DAYOFWEEK(keranjang_item.schedule) = 4 THEN 'Rabu' WHEN DAYOFWEEK(keranjang_item.schedule) = 5 THEN 'Kamis' WHEN DAYOFWEEK(keranjang_item.schedule) = 6 THEN 'Jumat' WHEN DAYOFWEEK(keranjang_item.schedule) = 7 THEN 'Sabtu' END, ', ', DAY(keranjang_item.schedule), ' ', CASE WHEN MONTH(keranjang_item.schedule) = 1 THEN 'Januari' WHEN MONTH(keranjang_item.schedule) = 2 THEN 'Februari' WHEN MONTH(keranjang_item.schedule) = 3 THEN 'Maret' WHEN MONTH(keranjang_item.schedule) = 4 THEN 'April' WHEN MONTH(keranjang_item.schedule) = 5 THEN 'Mei' WHEN MONTH(keranjang_item.schedule) = 6 THEN 'Juni' WHEN MONTH(keranjang_item.schedule) = 7 THEN 'Juli' WHEN MONTH(keranjang_item.schedule) = 8 THEN 'Agustus' WHEN MONTH(keranjang_item.schedule) = 9 THEN 'September' WHEN MONTH(keranjang_item.schedule) = 10 THEN 'Oktober' WHEN MONTH(keranjang_item.schedule) = 11 THEN 'November' WHEN MONTH(keranjang_item.schedule) = 12 THEN 'Desember' END, ' ', YEAR(keranjang_item.schedule) ) AS schedule, produk.harga FROM produk JOIN kategori_produk on produk.kategori_id = kategori_produk.id JOIN keranjang_item on produk.id = keranjang_item.produk_id JOIN detail_pesanan on keranjang_item.id = detail_pesanan.kItem_id JOIN pesanan on detail_pesanan.pesanan_id = pesanan.id JOIN pengguna on keranjang_item.pengguna_id = pengguna.id WHERE pengguna.id = @userId;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    myclasses.Add(new Myclass()
                    {
                        IdProduk = reader.GetInt32("idProduk"), //get data dari database
                        NamaCourse = reader.GetString("nama_produk"),
                        imageUrl = reader.GetString("imageUrl"),
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

            return myclasses;
        }
        //====================================== END Myclass User ======================================

        //====================================== BEGIN Invoice All User ======================================
        public List<InvoiceAdm> GetInvoiceAll()
        {
            List<InvoiceAdm> invoiceAdms = new List<InvoiceAdm>();

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                //sql before : SELECT pesanan.id as order_id, DATE_FORMAT(pesanan.created_at, '%d %M %Y') as tanggal_beli, COUNT(detail_pesanan.id) as jumlah_kursus, SUM(produk.harga) as total_harga FROM produk JOIN kategori_produk on produk.kategori_id = kategori_produk.id JOIN keranjang_item on produk.id = keranjang_item.produk_id JOIN detail_pesanan on keranjang_item.id = detail_pesanan.kItem_id JOIN pesanan on detail_pesanan.pesanan_id = pesanan.id JOIN pengguna on keranjang_item.pengguna_id = pengguna.id WHERE pengguna.id = @UserId;
                string sql = "select pesanan.id as order_id, pengguna.id as idPemesan, pengguna.nama as namaPemesan, CONCAT( DAY(pesanan.created_at), ' ', CASE WHEN MONTH(pesanan.created_at) = 1 THEN 'Januari' WHEN MONTH(pesanan.created_at) = 2 THEN 'Februari' WHEN MONTH(pesanan.created_at) = 3 THEN 'Maret' WHEN MONTH(pesanan.created_at) = 4 THEN 'April' WHEN MONTH(pesanan.created_at) = 5 THEN 'Mei' WHEN MONTH(pesanan.created_at) = 6 THEN 'Juni' WHEN MONTH(pesanan.created_at) = 7 THEN 'Juli' WHEN MONTH(pesanan.created_at) = 8 THEN 'Agustus' WHEN MONTH(pesanan.created_at) = 9 THEN 'September' WHEN MONTH(pesanan.created_at) = 10 THEN 'Oktober' WHEN MONTH(pesanan.created_at) = 11 THEN 'November' WHEN MONTH(pesanan.created_at) = 12 THEN 'Desember' END, ' ', YEAR(pesanan.created_at) ) AS tanggal_beli, COUNT(detail_pesanan.id) as jumlah_kursus, SUM(produk.harga) as total_harga from pesanan join detail_pesanan on pesanan.id = detail_pesanan.pesanan_id JOIN keranjang_item on detail_pesanan.kItem_id = keranjang_item.id JOIN produk on keranjang_item.produk_id = produk.id JOIN pengguna on keranjang_item.pengguna_id = pengguna.id GROUP BY pesanan.id;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                //cmd.Parameters.AddWithValue("@userId", userId);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    invoiceAdms.Add(new InvoiceAdm()
                    {
                        Id = reader.GetInt32("order_id"), //get data dari database
                        IdPemesan = reader.GetInt32("idPemesan"),
                        NamaPemesan = reader.GetString("namaPemesan"),
                        NoInvoice = "APM-" + reader.GetString("order_id"),
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

            return invoiceAdms;
        }
        //====================================== END Invoice All User ======================================

        public void CreateOrderAndOrderDetail(int metodeId, List<int> KeranjangIds)
        {
            //get connection to database
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            MySqlTransaction transaction = conn.BeginTransaction();
            try
            {
                //insert new Order
                int orderId;
                //List<int> detOrderIds = new List<int>();
                //int i = 0;

                using (var cmd = new MySqlCommand("INSERT INTO pesanan(metode_id, created_at) VALUES (@metodeId, current_timestamp())", conn))
                {
                    cmd.Parameters.AddWithValue("@metodeId", metodeId);
                    cmd.Transaction = transaction;
                    cmd.ExecuteNonQuery();

                    orderId = (int)cmd.LastInsertedId;
                }

                foreach (int KeranjangId in KeranjangIds)
                {
                    using (var cmdDetailPesanan = new MySqlCommand("INSERT INTO detail_pesanan(pesanan_id, kItem_id) VALUES (@OrderId, @KeranjangId)", conn))
                    {
                        cmdDetailPesanan.Parameters.AddWithValue("@OrderId", orderId);
                        cmdDetailPesanan.Parameters.AddWithValue("@KeranjangId", KeranjangId);
                        cmdDetailPesanan.Transaction = transaction;
                        cmdDetailPesanan.ExecuteNonQuery();
                    }

                    using (var cmdUpdateKeranjangItem = new MySqlCommand("UPDATE keranjang_item SET status = 0 WHERE id = @KeranjangId", conn))
                    {
                        cmdUpdateKeranjangItem.Parameters.AddWithValue("@KeranjangId", KeranjangId);
                        cmdUpdateKeranjangItem.Transaction = transaction;
                        cmdUpdateKeranjangItem.ExecuteNonQuery();
                    }
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

    }
}