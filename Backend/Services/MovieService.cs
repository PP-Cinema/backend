using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.DTO;
using Backend.Entities;
using Backend.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Backend.Services
{
    public class MovieService : IMovieService
    {
        private readonly DataContext context;
        private readonly IMovieRepository movieRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public MovieService(DataContext context, IMovieRepository movieRepository, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.movieRepository = movieRepository;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> CreateAsync(string title, int length, string movieAbstract, string description, IFormFile posterFile,
            HttpRequest request)
        {
            if (posterFile == null) return new JsonResult(new ExceptionDto {Message = "File not found"}) {StatusCode = 422};
            
            var extension = Path.GetExtension(posterFile.FileName);
            if (string.IsNullOrEmpty(extension) || !(extension == ".png" || extension == ".jpg"))
                return new JsonResult(new ExceptionDto {Message = "Wrong file extension"}) {StatusCode = 422};

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

            var fileName = Path.GetRandomFileName();
            var path = Path.Combine(webHostEnvironment.WebRootPath, "posters", fileName + extension);
            
            await using var fileStream = new FileStream(path, FileMode.Create);
            await posterFile.CopyToAsync(fileStream);
            
            var createdMovie = await movieRepository.AddAsync(new Movie
            {
                Title = title,
                Length = length,
                Abstract = movieAbstract,
                Description = description,
                PosterFilePath = request.Scheme + "://" + request.Host + "/posters/" + fileName +
                                 extension
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

        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await movieRepository.GetAllAsync();
            return new JsonResult(movies) {StatusCode = 200};
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
            var fileName = existingMovie.PosterFilePath.Split('/').Last();
            var filePath = Path.Combine(webHostEnvironment.WebRootPath, "posters", fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            context.Database?.CommitTransactionAsync();

            return new JsonResult(result) {StatusCode = 200};
        }
    }
}