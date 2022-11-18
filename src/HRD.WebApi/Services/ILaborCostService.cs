using HRD.WebApi.Data.Entities;
using HRD.WebApi.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRD.WebApi.Services
{
    public interface ILaborCostService
    {
        Task<PagedResponse<List<LaborCostViewModel>>> GetAll(PaginationFilter filter);
        Task<LaborCostViewModel> GetLaborCost(string year);
        Task CreateLaborCost(LaborCostViewModel model);
        Task UpdateLaborCost(LaborCostViewModel model);
        Task DeleteLaborCost(string year);
        Task<bool> IsLaborCostExists(string id);
        Task<bool> IsLaborCostYearUsed(string year);
    }
}
