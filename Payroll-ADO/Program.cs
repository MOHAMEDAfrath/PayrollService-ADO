using System;

namespace Payroll_ADO
{
    class Program
    {
        static void Main(string[] args)
        {
            EmployeeRepo employeeRepo = new EmployeeRepo();
            //EmployeeRepo employeeRepo1 = new EmployeeRepo();
            employeeRepo.GetAllEmployee();
            //EmployeeModel employeeModel = new EmployeeModel();
            //employeeRepo.DataBasedOnDateRange();
        }
    }
}
