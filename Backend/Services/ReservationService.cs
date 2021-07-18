using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.DTO;
using Backend.Entities;
using Backend.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services
{
    public class ReservationService : IReservationService
    {
        private readonly DataContext context;
        private readonly IReservationRepository reservationRepository;
        private readonly IPerformanceRepository performanceRepository;

        public ReservationService(DataContext context, IReservationRepository reservationRepository,
            IPerformanceRepository performanceRepository)
        {
            this.context = context;
            this.reservationRepository = reservationRepository;
            this.performanceRepository = performanceRepository;
        }


        public async Task<IActionResult> CreateAsync(string email, int normalTickets, int discountedTickets, string firstName, string lastName,
            string remarks, int performanceId)
        {
            var existingPerformance = await performanceRepository.GetAsync(performanceId);
            if (existingPerformance == null)
                return new JsonResult(new ExceptionDto {Message = "Performance with given id does not exist"})
                {
                    StatusCode = 422
                };

            var reservation = new Reservation()
            {
                Email = email,
                NormalTickets = normalTickets,
                DiscountedTickets = discountedTickets,
                FirstName = firstName,
                LastName = lastName,
                Remarks = remarks,
                Performance = existingPerformance
            };

            if (existingPerformance.Reservations == null)
            {
                existingPerformance.Reservations = new List<Reservation>();
            }
            existingPerformance.Reservations.Add(reservation);

            await performanceRepository.UpdateAsync(existingPerformance);

            return new JsonResult(reservation) { StatusCode = 201 };
        }

        public async Task<IActionResult> GetAsync(int id)
        {
            var reservation = await reservationRepository.GetAsync(id);
            if (reservation == null)
                return new JsonResult(new ExceptionDto() {Message = "Reservation does not exist"})
                {
                    StatusCode = 422
                };
            return new JsonResult(reservation) {StatusCode = 200};
        }

        public async Task<IActionResult> DeleteAsync(int id)
        {
            var reservation = await reservationRepository.GetAsync(id);
            if (reservation == null)
                return new JsonResult(new ExceptionDto() {Message = "Reservation does not exist"})
                {
                    StatusCode = 422
                };
            context.Database?.BeginTransactionAsync();
            var result = await reservationRepository.DeleteAsync(id);
            context.Database?.CommitTransactionAsync();

            return new JsonResult(result) {StatusCode = 200};
        }

        public async Task<IActionResult> GetAllUsersReservations(string email, string lastName)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(lastName))
                return new JsonResult(new ExceptionDto() {Message = "Missing value/s!"}) {StatusCode = 422};

            var result = await reservationRepository.GetAllUsersReservationsAsync(email, lastName);
            return !result.Any() ? new JsonResult(new ExceptionDto() {Message = "No reservations found!"}) {StatusCode = 422} : new JsonResult(result) {StatusCode = 200};
        }
    }
}
