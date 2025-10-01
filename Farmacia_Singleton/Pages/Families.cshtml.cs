using Farmacia_Singleton.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Farmacia_Singleton.Data;
using System.Data;

namespace Farmacia_Singleton.Pages
{
    public class FamiliesModel : PageModel
    {
        public DataTable FamiliesTable { get; set; } = new();

        public void OnGet() => LoadFamilies();

        private void LoadFamilies()
        {
            const string query = @"
                SELECT id, name, therapeutic_area, description, created_at
                FROM family
                WHERE status = 1
                ORDER BY name;";

            using var cn = DbConnectionSingleton.Instance.GetConnection();
            using var cmd = new MySqlCommand(query, cn);
            using var da = new MySqlDataAdapter(cmd);
            cn.Open();
            da.Fill(FamiliesTable);
        }

        public IActionResult OnPostDelete(int id)
        {
            const string query = @"
                UPDATE family
                SET status = 0, updated_at = CURRENT_TIMESTAMP, updated_by = 'web'
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
