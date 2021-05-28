using Backend.Data;
using Backend.Repositories;

namespace Backend.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly DataContext context;
        private readonly IAdminRepository adminRepository;

        public EmployeeService(DataContext context, IAdminRepository adminRepository)
        {
            this.context = context;
            this.adminRepository = adminRepository;
        }
    }
}