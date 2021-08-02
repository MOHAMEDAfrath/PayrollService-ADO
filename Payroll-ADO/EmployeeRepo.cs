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
                            Console.WriteLine("{0} {1}", employeeModel.EmployeeId, employeeModel.EmployeeName);
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
                this.sqlConnection.Close();
            }
        }

    }
}
