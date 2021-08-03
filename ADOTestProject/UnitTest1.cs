using Microsoft.VisualStudio.TestTools.UnitTesting;
using Payroll_ADO;

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

    }
}
