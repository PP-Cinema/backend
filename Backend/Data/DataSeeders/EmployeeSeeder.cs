using System.Collections.Generic;
using System.Linq;
using Backend.Entities;

namespace Backend.Data.DataSeeders
{
    public class EmployeeSeeder
    {
        private readonly DataContext context;

        public EmployeeSeeder(DataContext context)
        {
            this.context = context;
        }

        public void SeedData()
        {
            if (context.Employees.Any()) return;

            context.Employees.AddRange(GetEmployees());
            context.SaveChanges();
            context.Admins.AddRange(GetAdmins());
            context.SaveChanges();
        }

        private IEnumerable<Employee> GetEmployees()
        {
            var employees = new List<Employee>
            {
                new Employee
                {
                    Login = "ClintEastwood",
                    PasswordHash = "$2b$10$wmiLD9v2yDpt0wYVkcNecuDbL92XjO4DffGTmHSQGKzwSStOegEgW"
                },
                new Employee
                    {Login = "AlPacino", PasswordHash = "$2b$10$1zC/JHOplojmejJVDwAorOufIpRaM8AX.x0tCoYW8RHfKGjVMxOhe"},
                new Employee
                    {Login = "Admin", PasswordHash = "$2b$10$rjeyx6iEh9crpksZumgt6OLFCqq/nnOPKucbayQlsaaD9nj861OQK"}
            };

            return employees;
        }

        private IEnumerable<Admin> GetAdmins()
        {
            var adminEmployee = context.Employees.FirstOrDefault(e => e.Login == "Admin");
            if (adminEmployee == null) return null;

            var admins = new List<Admin>
            {
                new Admin {Employee = adminEmployee}
            };

            return admins;
        }
    }
}