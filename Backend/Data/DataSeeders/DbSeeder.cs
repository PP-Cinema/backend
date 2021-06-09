namespace Backend.Data.DataSeeders
{
    public class DbSeeder
    {
        private readonly EmployeeSeeder employeeSeeder;
        private readonly HallSeeder hallSeeder;


        public DbSeeder(HallSeeder hallSeeder, EmployeeSeeder employeeSeeder)
        {
            this.hallSeeder = hallSeeder;
            this.employeeSeeder = employeeSeeder;
        }

        public void SeedData()
        {
            hallSeeder.SeedData();
            employeeSeeder.SeedData();
        }
    }
}