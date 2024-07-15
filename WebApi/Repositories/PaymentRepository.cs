// PaymentRepository.cs
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using WebApi.Models;
using WebApi.Repositories.Interface;

namespace WebApi.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly string connStr = string.Empty;
        public PaymentRepository(IConfiguration configuration)
        {
            connStr = configuration.GetConnectionString("Default");
        }

        public Payment GetById(int id)
        {
            Payment payment = null;

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = "SELECT * FROM metode_pembayaran WHERE id = @Id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                using MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    payment = new Payment()
                    {
                        Id = reader.GetInt32("id"),
                        Nama_metode = reader.GetString("nama_metode"),
                        //Rekening = reader.GetString("no_rekening"),
                        ImageUrl = reader.GetString("imageUrl"),
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                conn.Close();
            }

            return payment;
        }

        public List<Payment> GetAll()
        {
            List<Payment> payments = new List<Payment>();

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT * FROM metode_pembayaran";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        payments.Add(new Payment()
                        {
                            Id = reader.GetInt32("id"),
                            Nama_metode = reader.GetString("nama_metode"),
                            //Rekening = reader.GetString("no_rekening"),
                            ImageUrl = reader.GetString("imageUrl"),
                            Status = reader.GetInt32("status")
                        });
                    }
                }
            }

            return payments;
        }

        public List<Payment> GetUserPayment()
        {
            List<Payment> payments = new List<Payment>();

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT * FROM metode_pembayaran where status = 1";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        payments.Add(new Payment()
                        {
                            Id = reader.GetInt32("id"),
                            Nama_metode = reader.GetString("nama_metode"),
                            //Rekening = reader.GetString("no_rekening"),
                            ImageUrl = reader.GetString("imageUrl"),
                            Status = reader.GetInt32("status")
                        });
                    }
                }
            }

            return payments;
        }

        public void Create(string name, string image)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO metode_pembayaran(nama_metode, imageUrl) VALUES (@Name, @Image)", conn);
                cmd.Parameters.AddWithValue("@Name", name);
                //cmd.Parameters.AddWithValue("@Rekening", rekening);
                cmd.Parameters.AddWithValue("@Image", image);
                cmd.ExecuteNonQuery();
            }
        }

        public bool Update(int id, string name, string image)
        {
            int rowsAffected;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE metode_pembayaran SET nama_metode=@Name, imageUrl=@Image WHERE id=@Id", conn);
                cmd.Parameters.AddWithValue("@Name", name);
                //cmd.Parameters.AddWithValue("@Rekening", rekening);
                cmd.Parameters.AddWithValue("@Image", image);
                cmd.Parameters.AddWithValue("@Id", id);
                rowsAffected = cmd.ExecuteNonQuery();
            }

            return rowsAffected > 0;
        }




        public void SetActive(int metodeId)
        {
            //get connection to database
            MySqlConnection conn = new MySqlConnection(connStr);
            int isStatus = 0;
            try
            {
                conn.Open();

                string selectQuery = "SELECT status FROM metode_pembayaran WHERE id = @Id";
                MySqlCommand selectCmd = new MySqlCommand(selectQuery, conn);
                selectCmd.Parameters.AddWithValue("@Id", metodeId);


                using (MySqlDataReader reader = selectCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isStatus = reader.GetInt32("status") == 1 ? 0 : 1;
                    }
                }

                string updateQuery = "UPDATE metode_pembayaran SET status = @Status WHERE id = @Id";
                MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                updateCmd.Parameters.AddWithValue("@Status", isStatus);
                updateCmd.Parameters.AddWithValue("@Id", metodeId);

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

    }
}