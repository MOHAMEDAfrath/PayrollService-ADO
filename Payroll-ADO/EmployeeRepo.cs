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
        public static string connectionString = @"Server=.;Database=payroll_service;Trusted_Connection=True;";
        SqlConnection sqlConnection = new SqlConnection(connectionString);
        public void GetAllEmployee()
        {

            try
            {
                //Employee Model object 
               
                using (sqlConnection)
                {
                    //query execution
                    string query = @"Select * from employee_payroll";
                    SqlCommand command = new SqlCommand(query, this.sqlConnection);
                    //open sql connection
                    sqlConnection.Open();
                    //sql reader to read data from db
                    SqlDataReader sqlDataReader = command.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            GetDetail(sqlDataReader);
                        }

                    }
                    //close reader
                    sqlDataReader.Close();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

                sqlConnection.Close();
            }
        }
        public string UpdateEmployee(EmployeeModel employeeModel)
        {
            string change = "Not success";
            try
            {
                using (sqlConnection)
                {
                    //Open command with spUpdateEmployeeDetails 
                    string query = @"update employee_payroll set BasicPay = '300000' where EmployeeId = '3' and EmployeeName = 'Priya'";
                    SqlCommand command = new SqlCommand(query, this.sqlConnection);
                    //open connection
                    sqlConnection.Open();
                    //executes the query and returns the no of rows the changes are reflected
                    int result = command.ExecuteNonQuery();
                    if (result != 0)
                        change = "success";
                   
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                //closes the connection
                sqlConnection.Close();
            
            }
            return change;


        }
        public EmployeeModel GetDetail(SqlDataReader sqlDataReader)
        {
            EmployeeModel employeeModel = new EmployeeModel();
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
            Console.WriteLine("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11}", employeeModel.EmployeeId, employeeModel.EmployeeName, employeeModel.BasePay, employeeModel.startDate, employeeModel.Gender, employeeModel.PhoneNumber, employeeModel.EmployeeDepartment, employeeModel.Address, employeeModel.Deductions, employeeModel.TaxablePay, employeeModel.Tax, employeeModel.NetPay);
            return employeeModel;
        }

        public string UpdateEmployeeUsingStoredProcedure(EmployeeModel employeeModel)
        {
            string change = "Unsuccessful";
            try
            {
                using (sqlConnection)
                {
                    //spUdpateEmployeeDetails is stored procedure
                    SqlCommand sqlCommand = new SqlCommand("spUdpateEmployeeDetails", this.sqlConnection);
                    //setting command type as stored procedure
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    //sending params 
                    sqlCommand.Parameters.AddWithValue("@name", employeeModel.EmployeeName);
                    sqlCommand.Parameters.AddWithValue("@Basic_Pay", employeeModel.BasePay);
                    sqlCommand.Parameters.AddWithValue("@id", employeeModel.EmployeeId);
                    sqlConnection.Open();
                    //returns the number of rows updated
                    int result = sqlCommand.ExecuteNonQuery();
                    if (result != 0)
                        change = "Updated";

                    //close reader
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                //closes the connection
                sqlConnection.Close();

            }
            return change;
        }
        public EmployeeModel RetrieveDataBasedOnName(EmployeeModel employeeModel)
        {
            try
            {
                using (this.sqlConnection)
                {
                    //spRetrieveDataBasedOnName is the stored procedure
                    SqlCommand sqlCommand = new SqlCommand("spRetrieveDataBasedOnName",this.sqlConnection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    //Sends params to procedure
                    sqlCommand.Parameters.AddWithValue("@name", employeeModel.EmployeeName);
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            employeeModel = GetDetail(sqlDataReader);
                        }
                    }
                    else
                    {
                        //if no result present
                        return null;
                    }
                    
                }
            }
            //catch 
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlConnection.Close();
            }

            return employeeModel;
        }
        //finds the employees in a given range from start date to current
        public int DataBasedOnDateRange()
        {
            List<EmployeeModel> employees = new List<EmployeeModel>();
            try
            {
                //Employee Model object 
                EmployeeModel employeeModel = new EmployeeModel();
                
                using (sqlConnection)
                {
                    //query execution
                    string query = @"select * from employee_payroll where StartDate BETWEEN Cast('2019-01-01' as Date) and GetDate();";
                    SqlCommand command = new SqlCommand(query, this.sqlConnection);
                    //open sql connection
                    sqlConnection.Open();
                    //sql reader to read data from db
                    SqlDataReader sqlDataReader = command.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            employeeModel = GetDetail(sqlDataReader);
                            employees.Add(employeeModel);

                        }

                    }
                    //close reader
                    sqlDataReader.Close();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

                sqlConnection.Close();
            }
            //returns the count of employee in the list between the given range
            return employees.Count;

        }
    }
}
