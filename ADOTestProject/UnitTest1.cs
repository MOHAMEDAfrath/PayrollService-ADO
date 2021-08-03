using Microsoft.VisualStudio.TestTools.UnitTesting;
using Payroll_ADO;
using System;

namespace ADOTestProject
{
    [TestClass]
    public class UnitTest1
    {
        EmployeeRepo employeeRepo;
        EntityRelationshipTable entityRelationshipTable;
        [TestInitialize]
        public void SetUp() 
        {
            employeeRepo = new EmployeeRepo();
            entityRelationshipTable = new EntityRelationshipTable();
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
            int expected = 3;
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
            int expected = 5;
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
            int expected = 3;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestMethodAggregateFunctionsAfterER()
        {
            string expected = "M 2300500 650000 950500 766833 3";
            EREmployeeModel model = new EREmployeeModel();
            model.Gender = "M";
            string actual = entityRelationshipTable.PerformAggregateFunctions(model);
            Assert.AreEqual(expected, actual);

        }


    }
}
