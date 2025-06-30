using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SW_Dev_SHMS.Data;
using SW_Dev_SHMS.Data.Entities;
using SW_Dev_SHMS.Services.DataServices.Interfaces;

namespace SW_Dev_SHMS.Services
{
    public class RoomDataService : IDataService<Room>
    {
        private readonly ApplicationDbContext _context;

        public RoomDataService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Room?> FirstOrDefaultAsync(Expression<Func<Room, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<Room> CreateAsync(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(room.Id);
        }

        Task<Room> IDataService<Room>.UpdateAsync(Room entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Room entity)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            return await _context.Rooms
                .ToListAsync();
        }

        public async Task<Room?> GetByIdAsync(int id)
        {
            return await _context.Rooms
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public Task<Room?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Room>> FindAsync(Expression<Func<Room, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }
        

        public async Task<Room?> GetByStudentIdAsync(string studentId)
        {
            return await _context.Rooms
                .FirstOrDefaultAsync(r => r.Students != null && r.Students.Any(s => s.Id == studentId));
        }
    }
}