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
        private readonly ISeatRepository seatRepository;

        public ReservationService(DataContext context, IReservationRepository reservationRepository,
            IPerformanceRepository performanceRepository, ISeatRepository seatRepository)
        {
            this.context = context;
            this.reservationRepository = reservationRepository;
            this.performanceRepository = performanceRepository;
            this.seatRepository = seatRepository;
        }


        public async Task<IActionResult> CreateAsync(string email, int normalTickets, int discountedTickets, string firstName, string lastName,
            string remarks, int performanceId, string seats)
        {
            var existingPerformance = await performanceRepository.GetAsync(performanceId);
            if (existingPerformance == null)
                return new JsonResult(new ExceptionDto {Message = "Performance with given id does not exist"})
                {
                    StatusCode = 422
                };
            
            string[] seatsString = seats.Split(",");

            if (seatsString.Length < 2 || seatsString.Length % 2 != 0 )
            {
                return new JsonResult(new ExceptionDto {Message = "Given seats list is not correct."})
                {
                    StatusCode = 422
                };
            }

            List<Seat> seatsForReservation = new List<Seat>();

            for (int i = 0; i < seatsString.Length; i=i+2)
            {
                var seat = new Seat()
                {
                    Row = int.Parse(seatsString[i]),
                    SeatNumber = int.Parse(seatsString[i + 1])
                };
                await seatRepository.AddAsync(seat);
                seatsForReservation.Add(seat);
            }
            
            
            var reservation = new Reservation()
            {
                Email = email,
                NormalTickets = normalTickets,
                DiscountedTickets = discountedTickets,
                FirstName = firstName,
                LastName = lastName,
                Remarks = remarks,
                Performance = existingPerformance,
                Seats = seatsForReservation
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
            return new JsonResult(result) {StatusCode = 200};
        }
    }
}
