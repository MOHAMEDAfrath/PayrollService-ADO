using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll_ADO
{
    public class EmployeeRepo
    {
        //connection string contains the database URI
        public static string connectionString = @"Server=.;Database=payroll_service;Trusted_Connection=True";
        SqlConnection sqlConnection = new SqlConnection(connectionString);
        public void GetAllEmployee()
        {
           
            try
            {
                //Employee Model object 
                EmployeeModel employeeModel = new EmployeeModel();
                using (sqlConnection)
                {
                    //query execution
                    string query = @"Select * from employee_payroll";
                    SqlCommand command = new SqlCommand(query,this.sqlConnection);
                    //open sql connection
                    this.sqlConnection.Open();
                    //sql reader to read data from db
                    SqlDataReader sqlDataReader = command.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            employeeModel.EmployeeId = sqlDataReader.GetInt32(0);
                            employeeModel.EmployeeName = sqlDataReader.GetString(1);
                            employeeModel.BasePay = sqlDataReader.GetDouble(2);
                            employeeModel.startDate = sqlDataReader.GetDateTime(3);
                            employeeModel.Gender = sqlDataReader.GetString(4);
                            employeeModel.PhoneNumber = Convert.ToString(sqlDataReader.GetInt64(5));
                            employeeModel.EmployeeDepartment = sqlDataReader.GetString(6);
                            employeeModel.Address = sqlDataReader.GetString(7);
                            employeeModel.Deductions = sqlDataReader.GetDouble(8);
                            employeeModel.TaxablePay = sqlDataReader.GetDouble(9);
                            employeeModel.Tax = sqlDataReader.GetDouble(10);
                            employeeModel.NetPay = sqlDataReader.GetDouble(11);
                            Console.WriteLine("{0}      {1}       {2}       {3}       {4}       {5}       {6}       {7}     {8}     {9}    {10}    {11}", employeeModel.EmployeeId, employeeModel.EmployeeName, employeeModel.BasePay, employeeModel.startDate,employeeModel.Gender,employeeModel.PhoneNumber,employeeModel.EmployeeDepartment,employeeModel.Address,employeeModel.Deductions,employeeModel.TaxablePay,employeeModel.Tax,employeeModel.NetPay);
                        }
                      
                    }
                    //close reader
                    sqlDataReader.Close();
                }
                
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                //closes the connection
                this.sqlConnection.Close();
            }
        }

    }
}
