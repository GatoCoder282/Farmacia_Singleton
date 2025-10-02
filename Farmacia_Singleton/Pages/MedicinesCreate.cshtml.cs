
using Farmacia_Singleton.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySql.Data.MySqlClient;
using Farmacia_Singleton.Data;
using System.ComponentModel.DataAnnotations;

namespace Farmacia_Singleton.Pages
{
    public class MedicinesCreateModel : PageModel
    {
        [BindProperty, Required, StringLength(150)]
        public string Name { get; set; } = "";

        [BindProperty, Required]
        public int? FamilyId { get; set; }

        [BindProperty, Required, Range(0, 9999999.99)]
        public decimal CostPrice { get; set; }

        [BindProperty, Required, Range(0, 9999999.99)]
        public decimal SalePrice { get; set; }

        public List<SelectListItem> FamiliesOptions { get; set; } = new();

        public void OnGet()
        {
            LoadFamiliesOptions();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                LoadFamiliesOptions();
                return Page();
            }

            const string insert = @"
                INSERT INTO medicine (name, cost_price, sale_price, family_id, created_by)
                VALUES (@name, @cost_price, @sale_price, @family_id, 'web');";

            using var cn = DbConnectionSingleton.Instance.GetConnection();
            using var cmd = new MySqlCommand(insert, cn);
            cmd.Parameters.AddWithValue("@name", Name);
            cmd.Parameters.AddWithValue("@cost_price", CostPrice);
            cmd.Parameters.AddWithValue("@sale_price", SalePrice);
            cmd.Parameters.AddWithValue("@family_id", FamilyId!.Value);

            cn.Open();
            cmd.ExecuteNonQuery();

            return RedirectToPage("Medicines");
        }

        private void LoadFamiliesOptions()
        {
            const string q = @"SELECT id, name FROM family WHERE status = 1 ORDER BY name;";

            using var cn = DbConnectionSingleton.Instance.GetConnection();
            using var cmd = new MySqlCommand(q, cn);
            cn.Open();
            using var rd = cmd.ExecuteReader();
            FamiliesOptions = new List<SelectListItem>();
            while (rd.Read())
            {
                FamiliesOptions.Add(new SelectListItem
                {
                    Value = rd["id"].ToString(),
                    Text = rd["name"].ToString()
                });
            }
        }
    }
}
