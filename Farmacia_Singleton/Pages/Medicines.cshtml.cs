
using Farmacia_Singleton.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Farmacia_Singleton.Data;
using System.Data;

namespace Farmacia_Singleton.Pages
{
    public class MedicinesModel : PageModel
    {
        public DataTable MedicinesTable { get; set; } = new();

        public void OnGet() => LoadMedicines();

        private void LoadMedicines()
        {
            const string query = @"
                SELECT m.id, m.name, m.cost_price, m.sale_price, m.created_at,
                       f.name AS family_name
                FROM medicine m
                INNER JOIN family f ON f.id = m.family_id
                WHERE m.status = 1 AND f.status = 1
                ORDER BY m.name;";

            using var cn = DbConnectionSingleton.Instance.GetConnection();
            using var cmd = new MySqlCommand(query, cn);
            using var da = new MySqlDataAdapter(cmd);
            cn.Open();
            da.Fill(MedicinesTable);
        }

        public IActionResult OnPostDelete(int id)
        {
            const string query = @"
                UPDATE medicine
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
