using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Data;
using Backend.DTO;
using Backend.Entities;
using Backend.Repositories;
using Backend.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IAdminRepository adminRepository;
        private readonly DataContext context;
        private readonly IEmployeeRepository employeeRepository;
        private readonly IJwtManager jwtManager;

        public EmployeeService(DataContext context,
            IAdminRepository adminRepository,
            IEmployeeRepository employeeRepository,
            IJwtManager jwtManager)
        {
            this.context = context;
            this.jwtManager = jwtManager;
            this.adminRepository = adminRepository;
            this.employeeRepository = employeeRepository;
        }

        public async Task<IActionResult> AuthenticateAsync(string login, string password)
        {
            var employee = await employeeRepository.GetAsync(login);
            
            if (employee == null && !BCrypt.Net.BCrypt.Verify(password,employee.PasswordHash))
            {
                return new JsonResult(new ExceptionDto { Message = "Invalid Credentials"}) { StatusCode = 422};
            }
            
            return new JsonResult(jwtManager.GenerateTokens(login,await GetRolesAsync(login),DateTime.Now)) {StatusCode = 200};
        }

        public async Task<IActionResult> RefreshAsync(string accessToken, string refreshToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var accessData = handler.ReadJwtToken(accessToken);
            var refreshData = handler.ReadJwtToken(refreshToken);
            var date = refreshData.ValidTo;

            if (DateTime.Now > date)
                return new JsonResult(new ExceptionDto {Message = "Refresh token is invalid"}) {StatusCode = 422};

            var login = accessData.Claims.Where(c => c.Type == ClaimTypes.Name).ToArray()[0].Value;
            var employee = await employeeRepository.GetAsync(login);

            if (employee == null || !jwtManager.ContainsRefreshToken(refreshToken))
                return new JsonResult(new ExceptionDto
                    {Message = "Employee with given login does not exist or refresh token is invalid"})
                {
                    StatusCode = 422
                };
            
            return new JsonResult(jwtManager.GenerateTokens(login,await GetRolesAsync(login),DateTime.Now)) {StatusCode = 200};
        }

        public async Task<string> GetRolesAsync(string login)
        {
            if (string.IsNullOrEmpty(login)) throw new ArgumentException("Login cannot be empty");

            var employee = employeeRepository.GetAsync(login);
            if (employee == null) throw new ArgumentException("Employee with given login does not exist");

            var roles = "";
            roles += await adminRepository.GetAsync(login) != null ? "Admin" : "";
            roles += "Employee";

            return roles;
        }

        public async Task<IActionResult> CreateAsync(string login, string password, string role)
        {
            var existingEmployee = await employeeRepository.GetAsync(login);
            if (existingEmployee != null)
                return new JsonResult(new ExceptionDto {Message = "User with given login already exists"})
                {
                    StatusCode = 422
                };

            context.Database?.BeginTransactionAsync();

            var createdEmployee = await employeeRepository.AddAsync(new Employee
            {
                Login = login,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            });

            if (role == "Admin") await adminRepository.AddAsync(new Admin {Employee = createdEmployee});

            context.Database?.CommitTransactionAsync();

            return new JsonResult(createdEmployee) {StatusCode = 201};
        }
    }
}