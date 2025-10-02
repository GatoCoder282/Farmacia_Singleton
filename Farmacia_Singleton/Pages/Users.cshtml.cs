using Farmacia_Singleton.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;

namespace Farmacia_Singleton.Pages
{
    public class UsersModel : PageModel
    {
        public DataTable UsersTable { get; private set; } = new();

        public void OnGet() => LoadUsers();

        private void LoadUsers()
        {
            const string query = @"
                SELECT id, name, email, created_at
                FROM users
                WHERE status = 1
                ORDER BY name;";

            using var cn = DbConnectionSingleton.Instance.GetConnection();
            using var cmd = new MySqlCommand(query, cn);
            using var da = new MySqlDataAdapter(cmd);
            cn.Open();
            da.Fill(UsersTable);
        }

        public IActionResult OnPostDelete(int id)
        {
            const string query = @"
                UPDATE users
                SET status = 0,
                    updated_at = CURRENT_TIMESTAMP,
                    updated_by = 'web'
                WHERE id = @id;";


            using var cn = DbConnectionSingleton.Instance.GetConnection();
            using var cmd = new MySqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@id", id);
            cn.Open();
            cmd.ExecuteNonQuery();

            return RedirectToPage();
        }
    }
}
