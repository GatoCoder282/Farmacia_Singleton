
using Farmacia_Singleton.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Farmacia_Singleton.Data;
using System.ComponentModel.DataAnnotations;
namespace Farmacia_Singleton.Pages
{
    public class FamiliesCreateModel : PageModel
    {
        [BindProperty, Required, StringLength(120)]
        public string Name { get; set; } = "";

        [BindProperty, Required, StringLength(120)]
        public string TherapeuticArea { get; set; } = "";

        [BindProperty, Required, StringLength(255)]
        public string Description { get; set; } = "";

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            const string query = @"
                INSERT INTO family (name, therapeutic_area, description, created_by)
                VALUES (@name, @therapeutic_area, @description, 'web');";

            using var cn = DbConnectionSingleton.Instance.GetConnection();
            using var cmd = new MySqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@name", Name);
            cmd.Parameters.AddWithValue("@therapeutic_area", TherapeuticArea);
            cmd.Parameters.AddWithValue("@description", Description);

            cn.Open();
            cmd.ExecuteNonQuery();

            return RedirectToPage("Families");
        }
    }
}
