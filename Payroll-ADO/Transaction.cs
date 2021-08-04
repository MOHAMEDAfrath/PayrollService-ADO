using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll_ADO
{
    public class TransactionQuery
    {
        public static string connectionString = @"Server=.;Database=payroll_service;Trusted_Connection=True;";
        SqlConnection sqlConnection = new SqlConnection(connectionString);
        //Transaction Query
        public void InsertIntoTables()
        {
                using (sqlConnection)
                {
                     sqlConnection.Open();
                //begins sql transaction
                    SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.Transaction = sqlTransaction;
                try { 
                    //performs the query
                    sqlCommand.CommandText = "Insert into Employee values ('Mohan', 'M', '7812447898', 'Chennai', '2020-12-07', '2')";
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
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //if any error roll backs to the last point
                    sqlTransaction.Rollback();
                }
            
            }

        }
    }
}
