using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Hierarchy_DLL
{
    public class Employee
    {
        ArrayList employees;
        public Employee(string csv_file) {

            employees = processCSVFile(csv_file);
            validate_entries(employees);

            
        }
        public ArrayList processCSVFile(string csv_file)
        {
            ArrayList result = new ArrayList();
            try
            {               
            //check if provided csv file is empty
            if (string.IsNullOrEmpty(csv_file))
            {
                throw new Exception("The csv file cannot be empty");

            }

            //get the rows in the csv files

            string[] rows = csv_file.Split(new[] { Environment.NewLine },StringSplitOptions.None);


            //iterate through the rows to capture data in the cells.
             

            foreach (string row in rows)
            {
                string[] arr = row.Split(',');
                ArrayList employee_record = new ArrayList();

                if (arr.Length != 3) {
                    throw new Exception("Error! all the rows of the csv file must have 3 columns");
                }

                foreach (string cell in arr)
                {
                    employee_record.Add(cell);
                }
               
                result.Add(employee_record);
            }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public void validate_entries(ArrayList employees) {

            //this function validates the data against the set rules
            ArrayList employeeNameList = new ArrayList();
            ArrayList managers = new ArrayList();
            ArrayList ceos = new ArrayList();
            ArrayList entryLevelEmployees = new ArrayList();
            

            foreach (ArrayList employee in employees) 
            {

                // Employee||manager||salary
                string employee_name = employee[0].ToString();
                string employee_manager = employee[1].ToString();
                string employee_salary = employee[2].ToString();

                //check if salaries are valid integers
                int salary;
                if (!int.TryParse(employee_salary, out salary)) 
                {
                    throw new Exception("CSV contains an non integer value in the salary column");
                }


                //check if employee has more than one manager. This is only possible if there is more than one record of an employee
                //to validate this, we will check for a duplicate entry for an employee
                if (employeeNameList.Contains(employee_name))
                {
                    // duplicate employee record which could lead to a more than one manager for an employee
                    throw new Exception("Employee cannot have more than one manager");
                }

                 //keep track of all valid employees
                 employeeNameList.Add(employee_name);               

                //keep track of the ceos. one is ceo if the manager field is null or empty
                if (String.IsNullOrEmpty(employee_manager))
                {
                    if (ceos.Count == 0)
                    {
                        ceos.Add(employee_name);
                    }
                    else 
                    {
                        throw new Exception("There can only be one CEO in the organization");
                    }
                    
                }
                else 
                {
                 //keep track of all the managers
                    managers.Add(employee_manager);
                }


            }


           /* if (ceos.Count != 1) 
            {
                throw new Exception("There can only be one CEO in the organization");
            }*/

            /*check if all managers are employees.
             * Loop through the managers list and check if manager name exists in the employee list
            */

            foreach (string manager in managers) 
            {
                if (!employeeNameList.Contains(manager)) 
                {
                    throw new Exception("All managers must be employees");
                }
            }

            /*
             * circular reference check
             */

            //first build a list of entry level employees that no one reports to them           
            foreach (string employee in employeeNameList)
            {
                if (!managers.Contains(employee) && !ceos.Contains(employee))
                {
                    //if an employee is not a manager or a ceo, then he/she is an entry level employee
                    entryLevelEmployees.Add(employee.Trim());
                }
            }

            //use the employee lists to check circular reference
            for (var i = 0; i < employees.Count; i++)
            {
                var employee_object = employees[i] as ArrayList;
                var employee_manager = employee_object[1].ToString();
                int index = employeeNameList.IndexOf(employee_manager);

                if (index != -1)
                {
                    var manager_object = employees[index] as ArrayList;
                    var highest_manager = manager_object[1].ToString();

                    if ((managers.Contains(highest_manager) && !ceos.Contains(highest_manager)) || entryLevelEmployees.Contains(highest_manager))
                    {
                        throw new Exception("A circular reference error exists!!");
                    }
                }
            }



        }

        public int getManagerSalaryBudget(string managerName)
        {
            int total_salary = 0;
            foreach (ArrayList employee_obj in employees)
            { 
                string employee_name = employee_obj[0].ToString();
                string employee_manager = employee_obj[1].ToString();
                string employee_salary = employee_obj[2].ToString();               

                if (employee_name == managerName) {
                    //add manager salary
                    total_salary += int.Parse(employee_salary);
                }
                if (employee_manager.Trim() == managerName.Trim())
                {
                    //add salaries of employees under him/her                    
                    //recursive call to  get the salaries of the employees under the manager
                    return total_salary += int.Parse(employee_salary) + getManagerSalaryBudget(employee_manager);

                }
            }
            return total_salary;
        }

        
    }
}
