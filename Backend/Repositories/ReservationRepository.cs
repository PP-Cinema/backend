﻿using System.Threading.Tasks;
using Backend.Data;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class ReservationRepository: IReservationRepository
    {
        private readonly DataContext context;

        public ReservationRepository(DataContext context) => this.context = context;
        
        public async Task<Reservation> AddAsync(Reservation reservation)
        {
            var result = await context.Reservations.AddAsync(reservation);
            await context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Reservation> UpdateAsync(Reservation reservation)
        {
            var result = context.Reservations.Update(reservation);
            await context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var reservationToDelete = await context.Reservations.FindAsync(id);
            if (reservationToDelete == null)
                return false;
            var result = context.Reservations.Remove(reservationToDelete);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Reservation> GetAsync(int id)
        {
            return await context.Reservations.Include(reservation => reservation.Id == id).FirstOrDefaultAsync();
        }
    }
}