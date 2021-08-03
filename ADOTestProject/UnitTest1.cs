using Microsoft.VisualStudio.TestTools.UnitTesting;
using Payroll_ADO;
using System;

namespace ADOTestProject
{
    [TestClass]
    public class UnitTest1
    {
        EmployeeRepo employeeRepo;
        [TestInitialize]
        public void SetUp() 
        {
            employeeRepo = new EmployeeRepo();   
        }
        //TC for updation using query
        [TestMethod]
        public void TestMethodForUpdateUsingQuery()
        {
            EmployeeModel model = new EmployeeModel();
            model.EmployeeName = "Priya";
            model.EmployeeId = 3;
            model.BasePay = 300000;
           string actual = employeeRepo.UpdateEmployee(model);
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
    }
}
