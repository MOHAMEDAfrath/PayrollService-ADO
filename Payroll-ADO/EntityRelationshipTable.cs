using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll_ADO
{
    public class EntityRelationshipTable
    {
        public static string connectionString = @"Server=.;Database=payroll_service;Trusted_Connection=True;";
        SqlConnection sqlConnection = new SqlConnection(connectionString);
        public int GetEmployeeDetails()
        {
            List<EREmployeeModel> employeepayroll = new List<EREmployeeModel>();
            EREmployeeModel eREmployeeModel = new EREmployeeModel();

            try
            {
                //Employee Model object 
                using (sqlConnection)
                {
                    //query execution
                    string query = @"select company.company_Id ,company.company_name,EmployeeId,EmployeeName,Gender,EmployeePhoneNumber,EmployeeAddress,StartDate,payroll.BasicPay,TaxablePay,IncomeTax,Deductions,NetPay,department_table.DeptName from Employee inner join company on company.company_Id = Employee.CompanyId inner join payroll on payroll.EmpId = Employee.EmployeeId inner join emp_Dept on Employee.EmployeeId = emp_Dept.EmpId inner join department_table on department_table.DeptId = emp_Dept.DeptId";
                    SqlCommand command = new SqlCommand(query, this.sqlConnection);
                    //open sql connection
                    sqlConnection.Open();
                    //sql reader to read data from db
                    SqlDataReader sqlDataReader = command.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            eREmployeeModel = GetDetail(sqlDataReader);
                            employeepayroll.Add(eREmployeeModel);
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
            return employeepayroll.Count;
        }
        public EREmployeeModel GetDetail(SqlDataReader sqlDataReader)
        {

            EREmployeeModel eREmployeeModel = new EREmployeeModel();
            eREmployeeModel.CompanyId = Convert.ToInt32(sqlDataReader["company_Id"]);
            eREmployeeModel.CompanyName = Convert.ToString(sqlDataReader["company_name"]);
            eREmployeeModel.EmployeeId = Convert.ToInt32(sqlDataReader["EmployeeId"]);
            eREmployeeModel.EmployeeName = Convert.ToString(sqlDataReader["EmployeeName"]);
            eREmployeeModel.Gender = Convert.ToString(sqlDataReader["Gender"]);
            eREmployeeModel.PhoneNumber = Convert.ToString(sqlDataReader["EmployeePhoneNumber"]);
            eREmployeeModel.Address = Convert.ToString(sqlDataReader["EmployeeAddress"]);
            eREmployeeModel.startDate = sqlDataReader.GetDateTime(7);
            eREmployeeModel.BasePay = Convert.ToDouble(sqlDataReader["BasicPay"]);
            eREmployeeModel.TaxablePay = Convert.ToDouble(sqlDataReader["TaxablePay"]);
            eREmployeeModel.Tax = Convert.ToDouble(sqlDataReader["IncomeTax"]);
            eREmployeeModel.Deductions = Convert.ToDouble(sqlDataReader["Deductions"]);
            eREmployeeModel.NetPay = Convert.ToDouble(sqlDataReader["NetPay"]);
            eREmployeeModel.DepartmentName = Convert.ToString(sqlDataReader["DeptName"]);
            Console.WriteLine("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12} {13}",eREmployeeModel.CompanyId,eREmployeeModel.CompanyName,eREmployeeModel.EmployeeId,eREmployeeModel.EmployeeName,eREmployeeModel.Gender,eREmployeeModel.PhoneNumber,eREmployeeModel.Address,eREmployeeModel.startDate,eREmployeeModel.BasePay,eREmployeeModel.TaxablePay,eREmployeeModel.Tax,eREmployeeModel.Deductions,eREmployeeModel.NetPay,eREmployeeModel.DepartmentName);

            return eREmployeeModel;
            
        }
        public int UpdateUsingQuery()
        { 
            int change = 0;
            try
            {
                using (sqlConnection)
                {
                    //Open command with spUpdateEmployeeDetails 
                    string query = @"update payroll set BasicPay = '300000' from payroll inner join Employee on payroll.EmpId = Employee.EmployeeId where Employee.EmployeeName = 'Priya'";
                    SqlCommand command = new SqlCommand(query, this.sqlConnection);
                    //open connection
                    sqlConnection.Open();
                    //executes the query and returns the no of rows the changes are reflected
                    int result = command.ExecuteNonQuery();
                    if (result != 0)
                        change = 1;

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
        public int UpdateUsingProcedure(EREmployeeModel employeeModel)
        {
            int change = 0;
            try
            {
                using (sqlConnection)
                {
                    //spUdpateEmployeeDetails is stored procedure
                    SqlCommand sqlCommand = new SqlCommand("spUdpateEmployeeDetailsAfterER", this.sqlConnection);
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
                        change = 1;

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
        public int DataBasedOnDateRange()
        {
            List<EREmployeeModel> employees = new List<EREmployeeModel>();
            try
            {
                //Employee Model object 
                EREmployeeModel employeeModel = new EREmployeeModel();

                using (sqlConnection)
                {
                    //query execution
                    string query = @"select Employee.EmployeeId,Employee.EmployeeName,payroll.BasicPay from payroll inner join Employee on Employee.EmployeeId = payroll.EmpId where Employee.StartDate between Cast('2019-01-01' as Date) and GETDATE();";
                    SqlCommand command = new SqlCommand(query, this.sqlConnection);
                    //open sql connection
                    sqlConnection.Open();
                    //sql reader to read data from db
                    SqlDataReader sqlDataReader = command.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            employeeModel.EmployeeId = Convert.ToInt32(sqlDataReader["EmployeeId"]);
                            employeeModel.EmployeeName = Convert.ToString(sqlDataReader["EmployeeName"]);
                            employeeModel.BasePay = Convert.ToDouble(sqlDataReader["BasicPay"]);
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
        public string PerformAggregateFunctions(EREmployeeModel employeeModel)
        {
            string result = "";
            try
            {
                SqlCommand sqlCommand = new SqlCommand("spAggregateFunctionsAfterER", this.sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                //Sends params to procedure
                sqlCommand.Parameters.AddWithValue("@Gender", employeeModel.Gender);
                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {

                    while (sqlDataReader.Read())
                    {
                        Console.WriteLine("Grouped By Gender {0}", sqlDataReader[4]);
                        Console.WriteLine("Total Salary {0}", sqlDataReader[0]);
                        Console.WriteLine("Min Salary {0}", sqlDataReader[1]);
                        Console.WriteLine("Max Salary {0}", sqlDataReader[2]);
                        Console.WriteLine("Average Salary {0}", sqlDataReader[3]);
                        Console.WriteLine("No of employess {0}", sqlDataReader[5]);
                        result += sqlDataReader[4] + " " + sqlDataReader[0] + " " + sqlDataReader[1] + " " + sqlDataReader[2] + " " + sqlDataReader[3] + " " + sqlDataReader[5];
                    }
                }
                else
                {
                    result = "empty";
                }
                sqlDataReader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
            return result;

        }
    }
}
