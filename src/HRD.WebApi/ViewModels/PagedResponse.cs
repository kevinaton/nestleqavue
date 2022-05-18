﻿using System;
using System.Collections.Generic;

namespace HRD.WebApi.ViewModels
{
    public class PagedResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public T Data { get; set; }

        public PagedResponse(T Data, int pageNumber, int pageSize, int totalRecords, int totalPages)
        {
            this.Data = Data;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.TotalPages = totalPages;
            this.TotalRecords = totalRecords;
        }
    }
}