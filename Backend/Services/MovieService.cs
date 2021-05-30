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
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository movieRepository;
        private readonly DataContext context;

        public MovieService(IMovieRepository movieRepository, DataContext context)
        {
            this.movieRepository = movieRepository;
            this.context = context;
        }

        public async Task<IActionResult> CreateAsync(string title, int length, string description)
        {
            var existingMovie = await movieRepository.GetAsync(title);
            if (existingMovie != null)
                return new JsonResult(new ExceptionDto {Message = "Movie with given title already exists"})
                {
                    StatusCode = 422
                };
            
            context.Database?.BeginTransactionAsync();

            var createdMovie = await movieRepository.AddAsync(new Movie
            {
                Title = title,
                Length = length,
                Description = description,
            });

            context.Database?.CommitTransactionAsync();

            return new JsonResult(createdMovie) {StatusCode = 201};
        }

        public async Task<IActionResult> GetAsync(string title)
        {
            return new JsonResult(await movieRepository.GetContainsAsync(title)) {StatusCode = 201};
        }

        public async Task<IActionResult> UpdateAsync(int id, string newTitle, int newLength, string newDescription)
        {
            var existingMovie = await movieRepository.GetAsync(id);
            if (existingMovie == null)
                return new JsonResult(new ExceptionDto {Message = "Movie with given id don't exists"})
                {
                    StatusCode = 422
                };

            existingMovie.Title = newTitle;
            existingMovie.Length = newLength;
            existingMovie.Description = newDescription;
            
            context.Database?.BeginTransactionAsync();
            
            var updatedMovie = await movieRepository.UpdateAsync(existingMovie);
            
            context.Database?.CommitTransactionAsync();
            
            return new JsonResult(updatedMovie) {StatusCode = 200};
        }

        public async Task<IActionResult> DeleteAsync(string title)
        {
            var existingMovie = await movieRepository.GetAsync(title);
            if (existingMovie == null)
                return new JsonResult(new ExceptionDto {Message = "Movie with given id don't exists"})
                {
                    StatusCode = 422
                };
            
            context.Database?.BeginTransactionAsync();
            
            var result = await movieRepository.DeleteAsync(existingMovie.Id);
            
            context.Database?.CommitTransactionAsync();

            return new JsonResult(result) {StatusCode = 200};
        }
    }
}