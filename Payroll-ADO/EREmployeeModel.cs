using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll_ADO
{
    public class EREmployeeModel
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime startDate { get; set; }
        public string Gender { get; set; }
        public double BasePay { get; set; }
        public double Deductions { get; set; }
        public double TaxablePay { get; set; }
        public double Tax { get; set; }
        public double NetPay { get; set; }
        public string DepartmentName { get; set; }
        public int IsActive { get; set; }

    }
}
