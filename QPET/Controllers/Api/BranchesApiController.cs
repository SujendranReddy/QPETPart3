using Microsoft.AspNetCore.Mvc;
using QPET.DTOs;
using QPET.Services.Interfaces;

namespace QPET.Controllers.Api
{
    [ApiController]
    [Route("api/branches")]
    public class BranchesApiController
        : ControllerBase
    {
        private readonly IBranchService
            _branchService;

        public BranchesApiController(
            IBranchService branchService)
        {
            _branchService =
                branchService;
        }

        [HttpGet]
        public async Task<
            ActionResult<IEnumerable<BranchResponse>>>
            GetBranches()
        {
            var branches =
                await _branchService
                    .GetBranchesAsync();

            var response =
                branches
                    .Select(
                        branch =>
                            new BranchResponse
                            {
                                BranchId =
                                    branch.BranchId,

                                BranchName =
                                    branch.BranchName,

                                PhoneNumber =
                                    branch.PhoneNumber,

                                EmailAddress =
                                    branch.EmailAddress,

                                City =
                                    branch.City,

                                Province =
                                    branch.Province
                            }
                    )
                    .ToList();

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BranchResponse>>
            GetBranch(
                int id)
        {
            var branch =
                await _branchService
                    .GetBranchByIdAsync(id);

            if (branch == null)
            {
                return NotFound(
                    new
                    {
                        message =
                            "Branch not found."
                    }
                );
            }

            var response =
                new BranchResponse
                {
                    BranchId =
                        branch.BranchId,

                    BranchName =
                        branch.BranchName,

                    PhoneNumber =
                        branch.PhoneNumber,

                    EmailAddress =
                        branch.EmailAddress,

                    City =
                        branch.City,

                    Province =
                        branch.Province
                };

            return Ok(response);
        }
    }
}