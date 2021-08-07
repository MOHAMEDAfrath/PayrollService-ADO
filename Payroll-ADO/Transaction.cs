using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll_ADO
{
    public class TransactionQuery : EntityRelationshipTable
    {
        List<EREmployeeModel> list = new List<EREmployeeModel>();
        public static string connectionString = @"Server=.;Database=payroll_service;Trusted_Connection=True;";
        SqlConnection sqlConnection = new SqlConnection(connectionString);
        //Transaction Query
        public string InsertIntoTables()
        {
            string update = "Not Successful";
            using (sqlConnection)
            {
                sqlConnection.Open();
                //begins sql transaction
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;
                try
                {
                    //performs the query
                    sqlCommand.CommandText = "Insert into Employee values ('Mohan', 'M', '7812447898', 'Chennai', '2020-12-07', '2','1')";
                    sqlCommand.ExecuteScalar();
                    Console.WriteLine("Inserted into Employee");
                    sqlCommand.CommandText = "insert into payroll(EmpId,BasicPay) values('5','500000')";
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.CommandText = "update payroll set Deductions = (BasicPay *20)/100 where EmpId = '5'";
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.CommandText = "update payroll set TaxablePay = (BasicPay - Deductions) where EmpId = '5'";
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.CommandText = "update payroll set IncomeTax = (TaxablePay * 10) / 100 where EmpId = '5'";
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.CommandText = "update payroll set NetPay = (BasicPay - IncomeTax) where EmpId = '5'";
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.CommandText = "insert into emp_Dept values('3','5')";
                    sqlCommand.ExecuteNonQuery();
                    //commits if all the above transactions are executed
                    sqlTransaction.Commit();
                    Console.WriteLine("All transaction are updated");
                    update = "All transaction are updated";
                    GetEmployeeDetails();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //if any error roll backs to the last point
                    sqlTransaction.Rollback();
                }
            }
            sqlConnection.Close();

            return update;

        }
        public string CascadingDelete()
        {
            string update = "Not Successful";
            using (sqlConnection)
            {
                sqlConnection.Open();
                //begins sql transaction
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;
                try
                {
                    //performs the query
                    sqlCommand.CommandText = "DBCC CHECKIDENT ('Employee', RESEED, 4)";
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.CommandText = "delete from employee where EmployeeId='5'";
                    sqlCommand.ExecuteNonQuery();
                    //commits if all the above transactions are executed
                    sqlTransaction.Commit();
                    Console.WriteLine("All transaction are updated");
                    update = "All transactions are updated";
                        GetEmployeeDetails();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //if any error roll backs to the last point
                    sqlTransaction.Rollback();
                }

            }
            sqlConnection.Close();
            return update;
        }
        //Executed once
        public void AlterTable()
        {
            using (sqlConnection)
            {
                sqlConnection.Open();
                //begins sql transaction
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;
                try
                {
                    //performs the query
                    sqlCommand.CommandText = "Alter table Employee add IsActive bit NOT NULL default 1";
                    sqlCommand.ExecuteNonQuery();
                    //commits if all the above transactions are executed
                    sqlTransaction.Commit();
                    Console.WriteLine("All transaction are updated");
                    GetEmployeeDetails();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //if any error roll backs to the last point
                    sqlTransaction.Rollback();
                }
            }
            sqlConnection.Close();

        }
        public void AuditPurpose(int identity)
        {
            using (sqlConnection)
            {
                sqlConnection.Open();
                //begins sql transaction
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;
                try
                {
                    sqlCommand.CommandText = @"update Employee set IsActive = 0 where EmployeeId = '"+identity+"'";
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                    Console.WriteLine("Updated");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //if any error roll backs to the last point
                    sqlTransaction.Rollback();
                }
            }
            sqlConnection.Close();
        }
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
                    string query = @"select company.company_Id ,company.company_name,EmployeeId,EmployeeName,Gender,EmployeePhoneNumber,EmployeeAddress,StartDate,IsActive,payroll.BasicPay,TaxablePay,IncomeTax,Deductions,NetPay,department_table.DeptName from Employee inner join company on company.company_Id = Employee.CompanyId inner join payroll on payroll.EmpId = Employee.EmployeeId inner join emp_Dept on Employee.EmployeeId = emp_Dept.EmpId inner join department_table on department_table.DeptId = emp_Dept.DeptId where Employee.IsActive = 1";
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
                            Console.WriteLine("Adding "+eREmployeeModel.EmployeeName);
                            employeepayroll.Add(eREmployeeModel);
                            Console.WriteLine("Added " +eREmployeeModel.EmployeeName);

                        }

                    }
                    sqlDataReader.Close();

                    //close reader

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
        public int GetEmployeeDetailsWithThread()
        {
            List<EREmployeeModel> employeepayroll = new List<EREmployeeModel>();
            EREmployeeModel eREmployeeModel = new EREmployeeModel();
            try
            {
                //Employee Model object 
                using (sqlConnection)
                {
                    //query execution
                    string query = @"select company.company_Id ,company.company_name,EmployeeId,EmployeeName,Gender,EmployeePhoneNumber,EmployeeAddress,StartDate,IsActive,payroll.BasicPay,TaxablePay,IncomeTax,Deductions,NetPay,department_table.DeptName from Employee inner join company on company.company_Id = Employee.CompanyId inner join payroll on payroll.EmpId = Employee.EmployeeId inner join emp_Dept on Employee.EmployeeId = emp_Dept.EmpId inner join department_table on department_table.DeptId = emp_Dept.DeptId where Employee.IsActive = 1";
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
                            //creating thread
                            Task thread = new Task(() =>
                            {
                                Console.WriteLine("Adding"+eREmployeeModel.EmployeeName);
                                employeepayroll.Add(eREmployeeModel);
                                Console.WriteLine("Added "+eREmployeeModel.EmployeeName);
                               
                            });
                            //starting thread
                            thread.Start();
                         }

                        }
                    sqlDataReader.Close();

                    //close reader

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
                    string query = @"select Employee.EmployeeId,Employee.EmployeeName,payroll.BasicPay from payroll inner join Employee on Employee.EmployeeId = payroll.EmpId where Employee.StartDate between Cast('2019-01-01' as Date) and GETDATE() and IsActive = 1;";
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
                SqlCommand sqlCommand = new SqlCommand("spAggregateFunctionsAfterER_ISActive", this.sqlConnection);
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
