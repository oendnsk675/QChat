using Microsoft.AspNetCore.SignalR;
using System.Net.WebSockets;
using System;
using Microsoft.Data.SqlClient;

namespace WebChatApplication.Controllers.Hubs
{
    public class ChatHub : Hub
    {
        private readonly string _connectionString;

        public ChatHub()
        {
            _connectionString = "Server=(localdb)\\mssqllocaldb;Database=aspnet-WebChatQ-08f89d38-acc3-49dd-99c6-1c16cfc2c887;Trusted_Connection=True;MultipleActiveResultSets=true";
        }

        public async Task SendMessage(string userId, string message)
        {
            try
            {
                //call method store message on model
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string currentUser = Context.UserIdentifier;
                    conn.Open();

                    string sql = "INSERT INTO UserChat (Message, FromId, ToId, ReadStatus,Waktu) VALUES (@Message, @FromId, @ToId, @ReadStatus, @Waktu)";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Message", message);
                    cmd.Parameters.AddWithValue("@FromId", currentUser);
                    cmd.Parameters.AddWithValue("@ToId", userId);
                    cmd.Parameters.AddWithValue("@ReadStatus", 0);
                    cmd.Parameters.AddWithValue("@Waktu", DateTime.Now);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    _ = rowsAffected;
                    if (rowsAffected > 0)
                    {
                        await Clients.User(userId).SendAsync("ReceiveMessage", userId, message);

                    }


                    conn.Close();
                }
            }
            catch (Exception e)
            {
                _ = e;
            }
            

                
        }

    }
}
