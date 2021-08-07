using System;

namespace Payroll_ADO
{
    class Program
    {
        static void Main(string[] args)
        {

            TransactionQuery transaction = new TransactionQuery();
            transaction.GetEmployeeDetailsWithThread();
            //transaction.InsertIntoTables();
            //TransactionQuery transaction1 = new TransactionQuery();
            //transaction1.CascadingDelete();
            //transaction.AuditPurpose(3);
        }
    }
}
