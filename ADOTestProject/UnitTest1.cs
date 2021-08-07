using Microsoft.VisualStudio.TestTools.UnitTesting;
using Payroll_ADO;
using System;
using System.Diagnostics;

namespace ADOTestProject
{
    [TestClass]
    public class UnitTest1
    {
        EmployeeRepo employeeRepo;
        EntityRelationshipTable entityRelationshipTable;
        TransactionQuery transactionQuery;
        [TestInitialize]
        public void SetUp() 
        {
            employeeRepo = new EmployeeRepo();
            entityRelationshipTable = new EntityRelationshipTable();
            transactionQuery = new TransactionQuery();
        }
        //TC for updation using query
        [TestMethod]
        public void TestMethodForUpdateUsingQuery()
        {
           string actual = employeeRepo.UpdateEmployee();
            string expected = "success";
            Assert.AreEqual(expected, actual);
        }
        //Test case for updation using StoredProcedure
        [TestMethod]
        public void TestMethodForUpdateUsingProcedure()
        {
            EmployeeModel model = new EmployeeModel();
            model.EmployeeName = "Priya";
            model.EmployeeId = 3;
            model.BasePay = 300000;
            string actual = employeeRepo.UpdateEmployeeUsingStoredProcedure(model);
            string expected = "Updated";
            Assert.AreEqual(expected, actual);
        }
        //TC for valid result for retrieved data
        [TestMethod]
        public void TestMethodForSelectDataBasedOnName()
        {
            EmployeeModel model = new EmployeeModel();
            model.EmployeeName = "Priya";
            EmployeeModel actual = employeeRepo.RetrieveDataBasedOnName(model);
            Assert.AreEqual(model.EmployeeName, actual.EmployeeName);
        }
        //Tc for invalid name
        [TestMethod]
        public void TestMethodForSelectDataForNullRecords()
        {
            try
            {
                EmployeeModel model = new EmployeeModel();
                model.EmployeeName = "Priy";
                employeeRepo.RetrieveDataBasedOnName(model);
            }
            catch(Exception ex)
            {
                Assert.AreEqual("Object reference not set to an instance of an object.", ex.Message);
            }
        }
        //TC to check employee between range from startdate to current
        [TestMethod]
        public void TestMethodToFindEmployeesBetweenRange()
        {
            int actual = employeeRepo.DataBasedOnDateRange();
            int expected =3 ;
            Assert.AreEqual(expected, actual);
        }
        //Aggregate method for gender male
        [TestMethod]
        public void TestMethodForAggregate()
        {
            string expected = "M 2300500 650000 950500 766833 3";
            EmployeeModel model = new EmployeeModel();
            model.Gender = "M";
            string actual = employeeRepo.PerformAggregateFunctions(model);
            Assert.AreEqual(expected, actual);
        }
        //the below test cases checks the table after normalization
        //checks the retrieval using join
        [TestMethod]
        public void TestMethodForCheckingPayrollList()
        {
            int expected = 6;
            int actual = entityRelationshipTable.GetEmployeeDetails();
            Assert.AreEqual(expected, actual);
        }
        //updates using join query
        [TestMethod]
        public void TestMethodToCheckUpdateFunctions_AfterER()
        {
            int actual = entityRelationshipTable.UpdateUsingQuery();
            int expected = 1;
            Assert.AreEqual(expected, actual);

        }
        //updates using join query in stored procedure
        [TestMethod]
        public void TestMethodToCheckUpdateFunctionsUsingProcedure_AfterER()
        {
            EREmployeeModel model = new EREmployeeModel();
            model.EmployeeName = "Priya";
            model.EmployeeId = 3;
            model.BasePay = 300000;
            int actual = entityRelationshipTable.UpdateUsingProcedure(model);
            int expected = 1;
            Assert.AreEqual(expected, actual);
        }
        //retrieve data betwwen joining date and current 
        [TestMethod]
        public void TestMethodToRetrieveDataBetweenRange_AfterER()
        {
            int actual = entityRelationshipTable.DataBasedOnDateRange();
            int expected = 4;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestMethodAggregateFunctionsAfterER()
        {
            string expected = "M 2800500 500000 950500 700125 4";
            EREmployeeModel model = new EREmployeeModel();
            model.Gender = "M";
            string actual = entityRelationshipTable.PerformAggregateFunctions(model);
            Assert.AreEqual(expected, actual);

        }
        ///Transaction Query////
        ///checks insert
        [TestMethod]
        public void TestMethodTransactionInsert()
        {
            //without thread
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            string actual = transactionQuery.InsertIntoTables();
            stopWatch.Stop();
            Console.WriteLine("Duration Without thread "+stopWatch.Elapsed.TotalSeconds);
            string expected = "All transaction are updated";
            Assert.AreEqual(expected,actual);
        }
        //Transaction query for cascading delete
        [TestMethod]
        public void TestMethodTransactionDelete()
        {
            string actual = transactionQuery.CascadingDelete();
            string expected = "All transactions are updated";
            Assert.AreEqual(expected, actual);
        }
        //checks the retrieval using join
        [TestMethod]
        public void TestMethodForCheckingPayrollListAfterER_IsActive()
        {
            TransactionQuery query = new TransactionQuery();
            query.AuditPurpose(1);
            int expected = 5;
            int actual = transactionQuery.GetEmployeeDetails();
            Assert.AreEqual(expected, actual);
        }
        //retrieve data betwwen joining date and current 
        [TestMethod]
        public void TestMethodToRetrieveDataBetweenRange_AfterER_IsActive()
        {
            int actual = transactionQuery.DataBasedOnDateRange();
            int expected = 3;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestMethodAggregateFunctionsAfterER_IsActive()
        {
            string expected = "M 2150500 500000 950500 716833 3";
            EREmployeeModel model = new EREmployeeModel();
            model.Gender = "M";
            string actual = transactionQuery.PerformAggregateFunctions(model);
            Assert.AreEqual(expected, actual);

        }
        [TestMethod]
        public void WithoutThread()
        {
            Stopwatch start = new Stopwatch();
            start.Start();
            transactionQuery.GetEmployeeDetails();
            start.Stop();
            Console.WriteLine("Without thread " + start.Elapsed.TotalSeconds);

        }
        //with thread along with lock
       [TestMethod]
        public void WithThread()
        {
            TransactionQuery tq = new TransactionQuery();
            Stopwatch start = new Stopwatch();
            start.Start();
            tq.GetEmployeeDetailsWithThread();
            start.Stop();
            Console.WriteLine("With thread " + start.Elapsed.TotalSeconds);

        }


    }
}
