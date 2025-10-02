using Farmacia_Singleton.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;

namespace Farmacia_Singleton.Pages
{
    public class UsersCreateModel : PageModel
    {
        [BindProperty, Required, StringLength(55)]
        public string Name { get; set; } = "";

        [BindProperty, Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = "";

        // Matches column 'password' (VARCHAR(45)) in your schema.
        [BindProperty, Required, StringLength(45)]
        public string Password { get; set; } = "";

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            const string query = @"
                INSERT INTO users
                    (name, email, password,
                     created_at, updated_at, status, created_by, updated_by)
                VALUES
                    (@name, @email, @password,
                     CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 1, 'web', 'web');";

            using var cn = DbConnectionSingleton.Instance.GetConnection();
            using var cmd = new MySqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@name", Name.Trim());
            cmd.Parameters.AddWithValue("@email", Email.Trim());
            cmd.Parameters.AddWithValue("@password", Password);

            cn.Open();
            cmd.ExecuteNonQuery();

            return RedirectToPage("Users");
        }
    }
}
