using System;
using System.Threading.Tasks;
using Backend.Data;
using Backend.DTO;
using Backend.Entities;
using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services
{
    public class PerformanceService: IPerformanceService
    {
        private readonly DataContext context;
        private readonly IPerformanceRepository performanceRepository;
        private readonly IHallRepository hallRepository;
        private readonly IMovieRepository movieRepository;

        public PerformanceService(DataContext context, IPerformanceRepository performanceRepository, IHallRepository hallRepository, IMovieRepository movieRepository)
        {
            this.context = context;
            this.performanceRepository = performanceRepository;
            this.hallRepository = hallRepository;
            this.movieRepository = movieRepository;
        }
        
        public async Task<IActionResult> CreateAsync(
            DateTime time, float normalPrice, float discountedPrice, int length, string hall, string movie)
        {
            var existingPerformance = await performanceRepository.GetAsync(movie);
            if (existingPerformance != null)
                return new JsonResult(new ExceptionDto {Message = "Performance with given movie title already exists"})
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
            
            var performance = new Performance()
            {
                Date = time,
                NormalPrice = normalPrice,
                DiscountedPrice = discountedPrice,
                Length = length,
                Hall = existingHall,
                Movie = existingMovie
            };
            
            return new JsonResult(await performanceRepository.AddAsync(performance)) {StatusCode = 201};
        }

        public async Task<IActionResult> GetAsync(int id)
        {
            var existingPerformance = await performanceRepository.GetAsync(id);
            return existingPerformance == null ?
                new JsonResult(new ExceptionDto{ Message = "No performance was found"}){ StatusCode = 422} : 
                new JsonResult(existingPerformance){StatusCode = 200};
        }

        public async Task<IActionResult> DeleteAsync(int id)
        {
            var existingPerformance = await performanceRepository.GetAsync(id);
            if (existingPerformance == null)
                return new JsonResult(new ExceptionDto {Message = "Performance with given movie title does not exist"})
                {
                    StatusCode = 422
                };
            
            context.Database?.BeginTransactionAsync();
            var result = await performanceRepository.DeleteAsync(existingPerformance.Id);
            context.Database?.CommitTransactionAsync();
            
            return new JsonResult(result) {StatusCode = 200};
        }
    }
}