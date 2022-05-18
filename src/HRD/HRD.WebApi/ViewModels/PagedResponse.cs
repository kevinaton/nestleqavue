using System;
using System.Collections.Generic;

namespace HRD.WebApi.ViewModels
{
    public class PagedResponse<T>
    {
        private List<LaborCostViewModel> laborCostList;

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public List<QAListViewModel> HrdList { get; }
        public List<ProductsViewModel> ProductList { get; }
        public List<LaborCostViewModel> LaborCostList { get; }

        public PagedResponse(List<QAListViewModel> hrdList, int pageNumber, int pageSize, int totalRecords, int totalPages)
        {
            HrdList = hrdList;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = totalPages;
            TotalRecords = totalRecords;
        }

        public PagedResponse(List<ProductsViewModel> productList, int pageNumber, int pageSize, int totalRecords, int totalPages)
        {
            ProductList = productList;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = totalPages;
        }

        public PagedResponse(List<LaborCostViewModel> laborCostList, int pageNumber, int pageSize, int totalRecords, int totalPages)
        {
            LaborCostList = laborCostList;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = totalPages;
        }
    }
}