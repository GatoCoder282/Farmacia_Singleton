using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Farmacia.Core.Models.Entities
{
    public class Medicine
    {
        #region Atributes

        private int _id { get; set; }
        private string _name { get; set; } = string.Empty;
        private string _skuCode { get; set; } = string.Empty;
        private int _concentrationAmount { get; set; }
        private bool _isRestricted { get; set; } = false;
        private bool _isActive { get; set; } = true;
        #endregion

        #region Constructor
        public Medicine()
        {

        }

        public Medicine(int id, string name, string skuCode, int concentrationAmount)
        {
            _id = id;
            _name = name;
            _skuCode = skuCode;
            _concentrationAmount = concentrationAmount;
            _isRestricted = false;
            _isActive = true;

        }
        #endregion
    }
}
