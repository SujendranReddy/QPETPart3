using QPET.Data.Entities;

namespace QPET.Services.Interfaces
{
    public interface IBranchService
    {
        Task<List<Branch>> GetBranchesAsync();

        Task<Branch?> GetBranchByIdAsync(int branchId);

        Task<bool> ExistsAsync(int branchId);
    }
}
