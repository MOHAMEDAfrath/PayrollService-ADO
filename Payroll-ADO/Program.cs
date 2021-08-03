using System;

namespace Payroll_ADO
{
    class Program
    {
        static void Main(string[] args)
        {
            EmployeeRepo employeeRepo = new EmployeeRepo();
            EmployeeRepo employeeRepo1 = new EmployeeRepo();
            employeeRepo.GetAllEmployee();
            EmployeeModel model = new EmployeeModel();
            model.EmployeeName = "Priya";
            model.EmployeeId = 3;
            model.BasePay = 300000;
            employeeRepo1.UpdateEmployee(model);
        }
    }
}
