using Farmacia.Infraestructure.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Farmacia.Infraestructure.Repositories.Interfaces;
using Farmacia.Core.Models.Entities;

namespace Farmacia.Infraestructure.Repositories.Implementations
{
    public class MedicineRepository : IMedicineRepository
    {
        private readonly IDatabaseConnection _db;

        public MedicineRepository(IDatabaseConnection db)
        {
            _db = db;
        }

        public void Create(Medicine medicine)
        {
            using var connection = _db.GetConnection();
            connection.Open();
            string query = @"INSERT INTO medicine 
                            (name, skuCode, concentrationAmount, isRestricted, isActive)
                            VALUES (@name, @skucode, @concentrationamount, @isrestricted, @isactive)";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@name", medicine.name);
            cmd.Parameters.AddWithValue("@skucode", medicine.);
            cmd.Parameters.AddWithValue("@concentrationamount", medicine.)
            cmd.Parameters.AddWithValue("@activo", Medicine.Activo);
            cmd.ExecuteNonQuery();
        }

        public Medicine? GetById(int id)
        {
            using var connection = _db.GetConnection();
            connection.Open();
            string query = "SELECT * FROM Medicine WHERE Id = @id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Medicine
                {
                    id = reader.GetInt32("Id"),
                    name = reader.GetString("Nombre"),
                    skuCode = reader.GetString("CodigoSKU"),

                    isActive = reader.GetBoolean("Activo")
                };
            }
            return null;
        }

        public IEnumerable<Medicine> GetAll()
        {
            var lista = new List<Medicine>();
            using var connection = _db.GetConnection();
            connection.Open();
            string query = "SELECT * FROM Medicine";
            using var cmd = new MySqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Medicine
                {
                    id = reader.GetInt32("Id"),
                    Nombre = reader.GetString("Nombre"),
                    CodigoSKU = reader.GetString("CodigoSKU"),
                    ClasificacionId = reader.GetInt32("ClasificacionId"),
                    PresentacionId = reader.GetInt32("PresentacionId"),
                    Activo = reader.GetBoolean("Activo")
                });
            }
            return lista;
        }

        public void Update(Medicine Medicine)
        {
            using var connection = _db.GetConnection();
            connection.Open();
            string query = @"UPDATE Medicine 
                             SET Nombre=@nombre, CodigoSKU=@sku, 
                                 ClasificacionId=@clasificacion, 
                                 PresentacionId=@presentacion, 
                                 Activo=@activo
                             WHERE Id=@id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", Medicine.Id);
            cmd.Parameters.AddWithValue("@nombre", Medicine.Nombre);
            cmd.Parameters.AddWithValue("@sku", Medicine.CodigoSKU);
            cmd.Parameters.AddWithValue("@clasificacion", Medicine.ClasificacionId);
            cmd.Parameters.AddWithValue("@presentacion", Medicine.PresentacionId);
            cmd.Parameters.AddWithValue("@activo", Medicine.Activo);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var connection = _db.GetConnection();
            connection.Open();
            string query = "DELETE FROM Medicine WHERE Id=@id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
