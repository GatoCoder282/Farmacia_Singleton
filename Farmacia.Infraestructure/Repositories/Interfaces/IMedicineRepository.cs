using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Farmacia.Core.Models.Entities;

namespace Farmacia.Infraestructure.Repositories.Interfaces
{
    public interface IMedicineRepository
    {
        void Create(Medicine medicine);
        Medicine? GetById(int id);
        IEnumerable<Medicine> GetAll();
        void Update(Medicine medicine);
        void Delete(int id);
    }
}
