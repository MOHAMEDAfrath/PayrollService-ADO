using System;

namespace Payroll_ADO
{
    class Program
    {
        static void Main(string[] args)
        {
            TransactionQuery transaction = new TransactionQuery();
            transaction.InsertIntoTables();
        }
    }
}
