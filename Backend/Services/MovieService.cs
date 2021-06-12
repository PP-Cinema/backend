using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.DTO;
using Backend.Entities;
using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Backend.Services
{
    public class MovieService : IMovieService
    {
        private readonly DataContext context;
        private readonly IMovieRepository movieRepository;

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

            if (length <= 0)
                return new JsonResult(new ExceptionDto {Message = "Length parameter cannot be negative or null"})
                {
                    StatusCode = 422
                };

            context.Database?.BeginTransactionAsync();

            var createdMovie = await movieRepository.AddAsync(new Movie
            {
                Title = title,
                Length = length,
                Description = description
            });

            context.Database?.CommitTransactionAsync();

            return new JsonResult(createdMovie) {StatusCode = 201};
        }

        public async Task<IActionResult> GetAsync(int id)
        {
            var existingMovie = await movieRepository.GetAsync(id);
            if (existingMovie == null)
                return new JsonResult(new ExceptionDto {Message = "Movie with given id does not exist"})
                {
                    StatusCode = 422
                };
            return new JsonResult(existingMovie) {StatusCode = 200};
        }

        public async Task<IActionResult> GetPageAsync(int page, int itemsPerPage, string searchString)
        {
            var movies = (await movieRepository.GetContainsAsync(searchString)).OrderByDescending(m => m.Title).ToList();
            var moviesCount = movies.Count();
            var startIndex = page * itemsPerPage;
            var itemsOnPage = itemsPerPage;
            if (moviesCount - startIndex < itemsPerPage)
            {
                itemsOnPage = moviesCount - startIndex;
            }

            if (startIndex < 0 || itemsOnPage <= 0)
                return new JsonResult(new ExceptionDto {Message = "Page don't exist"}) {StatusCode = 422};
            
            var moviesOnPage = movies.GetRange(startIndex, itemsOnPage);
            
            return new JsonResult(moviesOnPage) {StatusCode = 200};
        }

        public async Task<IActionResult> GetPageCountAsync(int itemsPerPage, string searchString)
        {
            if (itemsPerPage <= 0)
            {
                return new JsonResult(new ExceptionDto {Message = "Incorrect page count"}) {StatusCode = 422};
            }
            var moviesCount = (await movieRepository.GetContainsAsync(searchString)).Count();
            var pagesCount = moviesCount / itemsPerPage + (moviesCount%itemsPerPage == 0 ? 0 : 1);
            
            return new JsonResult(pagesCount) {StatusCode = 200};
        }

        public async Task<IActionResult> UpdateAsync(int id, string newTitle, int newLength, string newDescription)
        {
            var existingMovie = await movieRepository.GetAsync(id);
            if (existingMovie == null)
                return new JsonResult(new ExceptionDto {Message = "Movie with given id does not exist"})
                {
                    StatusCode = 422
                };

            if (!string.IsNullOrEmpty(newTitle))
                existingMovie.Title = newTitle;
            if (!string.IsNullOrEmpty(newDescription))
                existingMovie.Description = newDescription;

            var updatedLength = newLength > 0 ? newLength : existingMovie.Length;
            existingMovie.Length = updatedLength;

            context.Database?.BeginTransactionAsync();

            var updatedMovie = await movieRepository.UpdateAsync(existingMovie);

            context.Database?.CommitTransactionAsync();

            return new JsonResult(updatedMovie) {StatusCode = 200};
        }

        public async Task<IActionResult> DeleteAsync(int id)
        {
            var existingMovie = await movieRepository.GetAsync(id);
            if (existingMovie == null)
                return new JsonResult(new ExceptionDto {Message = "Movie with given id does not exist"})
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