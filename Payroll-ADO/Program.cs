using System;

namespace Payroll_ADO
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            EmployeeRepo employeeRepo = new EmployeeRepo();
            employeeRepo.GetAllEmployee();
        }
    }
}
