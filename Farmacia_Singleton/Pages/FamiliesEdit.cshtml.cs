
using Farmacia_Singleton.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Farmacia_Singleton.Data;
using System.ComponentModel.DataAnnotations;

namespace Farmacia_Singleton.Pages
{
    public class FamiliesEditModel : PageModel
    {
        [BindProperty]
        public int Id { get; set; }

        [BindProperty, Required, StringLength(120)]
        public string Name { get; set; } = "";

        [BindProperty, Required, StringLength(120)]
        public string TherapeuticArea { get; set; } = "";

        [BindProperty, Required, StringLength(255)]
        public string Description { get; set; } = "";

        public void OnGet(int id)
        {
            const string query = @"
                SELECT id, name, therapeutic_area, description
                FROM family
                WHERE id = @id AND status = 1;";

            using var cn = DbConnectionSingleton.Instance.GetConnection();
            using var cmd = new MySqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@id", id);
            cn.Open();

            using var rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                Id = rd.GetInt32("id");
                Name = rd["name"].ToString()!;
                TherapeuticArea = rd["therapeutic_area"].ToString()!;
                Description = rd["description"].ToString()!;
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            const string query = @"
                UPDATE family
                SET name = @name,
                    therapeutic_area = @therapeutic_area,
                    description = @description,
                    updated_at = CURRENT_TIMESTAMP,
                    updated_by = 'web'
                WHERE id = @id AND status = 1;";

            using var cn = DbConnectionSingleton.Instance.GetConnection();
            using var cmd = new MySqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@id", Id);
            cmd.Parameters.AddWithValue("@name", Name);
            cmd.Parameters.AddWithValue("@therapeutic_area", TherapeuticArea);
            cmd.Parameters.AddWithValue("@description", Description);

            cn.Open();
            cmd.ExecuteNonQuery();

            return RedirectToPage("Families");
        }
    }
}
