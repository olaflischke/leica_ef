using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindDal
{
    partial class Customer : IDataErrorInfo
    {
        public string this[string columnName]
        {
            get
            {
                string result = "";

                if (columnName == "CustomerID")
                {
                    if (string.IsNullOrWhiteSpace(this.CustomerID) || this.CustomerID.Length > 5)
                    {
                        result = "CustomerID ist zu lang!";
                    }
                }

                return result;
            }
        }

        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string ToString()
        {
            return this.CompanyName;
        }
    }
}
