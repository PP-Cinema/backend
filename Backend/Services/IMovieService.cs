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
    public interface IMovieService
    {
        public Task<IActionResult> CreateAsync(string title, int length, string description);
        public Task<IActionResult> GetAsync(string title);
        public Task<IActionResult> UpdateAsync(int id, string newTitle, int newLength, string newDescription);
        public Task<IActionResult> DeleteAsync(string title);
    }
}