using Microsoft.EntityFrameworkCore;
using QPET.Data;
using QPET.Data.Entities;
using QPET.Services.Interfaces;

namespace QPET.Services
{
    public class BranchService : IBranchService
    {
        private readonly QpetDbContext _context;

        public BranchService(QpetDbContext context)
        {
            _context = context;
        }

        public async Task<List<Branch>> GetBranchesAsync()
        {
            return await _context.Branches
                .AsNoTracking()
                .OrderBy(branch => branch.BranchName)
                .ToListAsync();
        }

        public async Task<Branch?> GetBranchByIdAsync(
            int branchId)
        {
            if (branchId <= 0)
            {
                return null;
            }

            return await _context.Branches
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    branch =>
                        branch.BranchId == branchId
                );
        }

        public async Task<bool> ExistsAsync(
            int branchId)
        {
            if (branchId <= 0)
            {
                return false;
            }

            return await _context.Branches
                .AnyAsync(
                    branch =>
                        branch.BranchId == branchId
                );
        }
    }
}
