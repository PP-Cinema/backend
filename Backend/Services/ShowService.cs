using System;
using System.Threading.Tasks;
using Backend.Data;
using Backend.DTO;
using Backend.Entities;
using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services
{
    public class ShowService: IShowService
    {
        private readonly DataContext context;
        private readonly IShowRepository showRepository;
        private readonly IHallRepository hallRepository;
        private readonly IMovieRepository movieRepository;

        public ShowService(DataContext context, IShowRepository showRepository, IHallRepository hallRepository, IMovieRepository movieRepository)
        {
            this.context = context;
            this.showRepository = showRepository;
            this.hallRepository = hallRepository;
            this.movieRepository = movieRepository;
        }
        
        public async Task<IActionResult> CreateAsync(DateTime time, decimal price, string hall, string movie)
        {
            var existingShow = await showRepository.GetAsync(movie);
            if (existingShow != null)
                return new JsonResult(new ExceptionDto {Message = "Show with given movie title already exists"})
                {
                    StatusCode = 422
                };
            
            var existingHall = await hallRepository.GetAsync(hall);
            if (existingHall == null)
                return new JsonResult(new ExceptionDto {Message = "Hall with given letter does not exist"})
                {
                    StatusCode = 422
                };
            
            var existingMovie = await movieRepository.GetAsync(movie);
            if (existingMovie == null)
                return new JsonResult(new ExceptionDto {Message = "Movie with given title does not exist"})
                {
                    StatusCode = 422
                };
            
            var show = new Show()
            {
                Time = time,
                Price = price,
                Hall = existingHall,
                Movie = existingMovie
            };
            
            return new JsonResult(await showRepository.AddAsync(show)) {StatusCode = 201};
        }

        public async Task<IActionResult> GetAsync(int id)
        {
            var existingShow = await showRepository.GetAsync(id);
            return existingShow == null ?
                new JsonResult(new ExceptionDto{ Message = "No show was found"}){ StatusCode = 422} : 
                new JsonResult(existingShow){StatusCode = 200};
        }

        public async Task<IActionResult> DeleteAsync(int id)
        {
            var existingShow = await showRepository.GetAsync(id);
            if (existingShow == null)
                return new JsonResult(new ExceptionDto {Message = "Show with given movie title does not exist"})
                {
                    StatusCode = 422
                };
            
            context.Database?.BeginTransactionAsync();
            var result = await showRepository.DeleteAsync(existingShow.Id);
            context.Database?.CommitTransactionAsync();
            
            return new JsonResult(result) {StatusCode = 200};
        }
    }
}