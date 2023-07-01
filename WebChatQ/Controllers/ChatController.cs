using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebChatQ.Models;

namespace WebChatQ.Controllers
{
    public class ChatController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly string _connectionString;

        public ChatController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _connectionString = "Server=(localdb)\\mssqllocaldb;Database=aspnet-WebChatQ-08f89d38-acc3-49dd-99c6-1c16cfc2c887;Trusted_Connection=True;MultipleActiveResultSets=true";
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        public async Task<ActionResult<IEnumerable<UserChat>>> GetAll(string ToId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    _ = currentUserId;
                    string sql = "SELECT * FROM UserChat WHERE FromId = @Id AND ToId = @ToId  OR  FromId = @ToId AND ToId = @Id";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Id", currentUserId);
                    cmd.Parameters.AddWithValue("@ToId", ToId);
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        List<UserChat> messages = new List<UserChat>();

                        while (await reader.ReadAsync())
                        {
                            UserChat message = new UserChat
                            {
                                Id = (int)reader["Id"],
                                Message = (string)reader["Message"],
                                FromId = (string)reader["FromId"],
                                ToId = (string)reader["ToId"],
                                ReadStatus = (bool)reader["ReadStatus"],
                                Waktu = (DateTime)reader["Waktu"]
                            };

                            messages.Add(message);
                        }
                        _ = messages;
                        conn.Close();
                        return messages;
                    }
                }
            } catch (Exception e)
            {
                _ = e;
                return null;
            }
        }
    }
}
