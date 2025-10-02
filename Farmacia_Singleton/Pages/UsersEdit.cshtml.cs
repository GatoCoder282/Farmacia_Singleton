using Farmacia_Singleton.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;

namespace Farmacia_Singleton.Pages
{
    public class UsersEditModel : PageModel
    {
        [BindProperty]
        public int Id { get; set; }

        [BindProperty, Required, StringLength(55)]
        public string Name { get; set; } = "";

        [BindProperty, Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = "";

        // Optional on edit: if empty, password won't change.
        [BindProperty, StringLength(45)]
        public string? Password { get; set; }

        public void OnGet(int id)
        {
            const string query = @"
                SELECT id, name, email
                FROM users
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
                Email = rd["email"].ToString()!;
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            var updateWithPwd = @"
                UPDATE users
                SET name = @name,
                    email = @email,
                    password = @password,
                    updated_at = CURRENT_TIMESTAMP,
                    updated_by = 'web'
                WHERE id = @id AND status = 1;";

            var updateWithoutPwd = @"
                UPDATE users
                SET name = @name,
                    email = @email,
                    updated_at = CURRENT_TIMESTAMP,
                    updated_by = 'web'
                WHERE id = @id AND status = 1;";

            using var cn = DbConnectionSingleton.Instance.GetConnection();
            using var cmd = new MySqlCommand(
                string.IsNullOrWhiteSpace(Password) ? updateWithoutPwd : updateWithPwd,
                cn
            );

            cmd.Parameters.AddWithValue("@id", Id);
            cmd.Parameters.AddWithValue("@name", Name.Trim());
            cmd.Parameters.AddWithValue("@email", Email.Trim());
            if (!string.IsNullOrWhiteSpace(Password))
                cmd.Parameters.AddWithValue("@password", Password);

            cn.Open();
            cmd.ExecuteNonQuery();

            return RedirectToPage("Users");
        }
    }
}
