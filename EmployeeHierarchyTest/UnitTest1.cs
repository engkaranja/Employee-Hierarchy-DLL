using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Employee_Hierarchy_DLL;

namespace EmployeeHierarchyTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestThrowsExceptionWhenAnInvalidIntegerSalaryIsProvided()
        {
			Assert.ThrowsException<Exception>(() => new Employee("employee1,manager1,200" +
				"\nemployee2,manager2,340" +
				"\nCeo,,hello" + //expecting error since hello is not an int
				"\nemployee4,manager3,780" +
				"\nemployee5,manager4,1000"));
        }

		[TestMethod]
		public void TestThrowsExceptionWhenEmployeeHasMoreThanOneManager()
		{
			Assert.ThrowsException<Exception>(() => new Employee("employee1,manager1,200" +
				"\nemployee2,manager2,340" +
				"\nemployee1,manager2,560" + //expecting error since employee1 has a more than one manager
				"\nemployee4,manager3,780" +
				"\nemployee5,manager4,1000"));
		}

		[TestMethod]
		public void TestThrowsExceptionWhenThereIsMoreThanOneCEO()
		{
			Assert.ThrowsException<Exception>(() => new Employee("employee1,manager1,200" +
				"\nemployee2,manager2,340" +
				"\nCEO,,560" + 
				"\nemployee4,manager3,780" +
				"\nCEO2,,1000"));
		}

		[TestMethod]
		public void TestThrowsExceptionIfThereIsACircularReference()
		{
			Assert.ThrowsException<Exception>(() => new Employee("employee1,manager1,200" +
				"\nmanager1,employee1,340" +
				"\nCEO,,560" +
				"\nemployee4,manager3,780" +
				"\nmanager3,CEO,1000"));
		}
		[TestMethod]
		public void TestThrowsExceptionIfThereIsAManagerWhoIsNotAnEmployee()
		{
			Assert.ThrowsException<Exception>(() => new Employee("employee1,manager1,200" +
				"\nmanager1,manager4,340" +
				"\nCEO,,560" +
				"\nemployee4,manager3,780" +
				"\nmanager3,CEO,1000"));
		}

		[TestMethod]
		public void TestThrowsExceptionIfCSVStringIsEmpty()
		{
			Assert.ThrowsException<Exception>(() => new Employee(""));
		}

		[TestMethod]
		public void TestIfManagerSalaryBudgetIsCorrect()
		{
			Employee obj = new Employee("employee1,manager1,200" +
				"\nmanager1,manager4,340" +
				"\nCEO,,560" +
				"\nmanager4,manager3,780" +
				"\nmanager3,CEO,1000");

			Assert.AreEqual(2880, obj.getManagerSalaryBudget("CEO"));

		}
	}
}
