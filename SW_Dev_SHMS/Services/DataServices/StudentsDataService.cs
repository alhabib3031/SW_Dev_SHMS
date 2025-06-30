using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SW_Dev_SHMS.Data;
using SW_Dev_SHMS.Data.Entities;
using SW_Dev_SHMS.Models;
using SW_Dev_SHMS.Services.DataServices.Interfaces;

namespace SW_Dev_SHMS.Services.DataServices
{
    public class StudentsDataService : IStudentDataService
    {
        private readonly ApplicationDbContext _context;

        public StudentsDataService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            throw new NotSupportedException("Student entity uses string ID from Identity.");
        }

        public async Task<Student?> GetByIdAsync(string id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<IEnumerable<Student>> FindAsync(Expression<Func<Student, bool>> predicate)
        {
            return await _context.Students.Where(predicate).ToListAsync();
        }

        public async Task<Student?> FirstOrDefaultAsync(Expression<Func<Student, bool>> predicate)
        {
            return await _context.Students.FirstOrDefaultAsync(predicate);
        }

        public async Task<Student> CreateAsync(Student entity)
        {
            await _context.Students.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Student> UpdateAsync(Student entity)
        {
            _context.Students.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Student entity)
        {
            _context.Students.Remove(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}