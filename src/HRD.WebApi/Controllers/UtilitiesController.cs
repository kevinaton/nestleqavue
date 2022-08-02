using HRD.WebApi.Data;
using HRD.WebApi.ViewModels;
using HRD.WebApi.ViewModels.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using CsvHelper;
using System;
using System.Net.Http;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using HRD.WebApi.Authorization;
using Microsoft.EntityFrameworkCore;

namespace HRD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilitiesController : ControllerBase
    {

        private readonly HRDContext _context;

        public UtilitiesController(HRDContext context)
        {
            _context = context;
        }

        private byte[] ExportToExcel<T>(IEnumerable<T> dataList, string sheetName)
        {
            var stream = new MemoryStream();
            //required using OfficeOpenXml;
            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add(sheetName);
                workSheet.Cells.LoadFromCollection(dataList, true, TableStyles.Light8);
                package.Save();
            }
            stream.Position = 0;

            return stream.ToArray();
        }
        private byte[] ExportToCsv(IEnumerable<object> dataList)
        {
            //Convert results to csv format and send to a memory stream
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);
            CsvWriter csvWriter = new(streamWriter, CultureInfo.CurrentCulture);
            
            using (memoryStream)
            using (streamWriter)
            using (csvWriter)
            {
                csvWriter.WriteRecords(dataList);
                streamWriter.Flush();
                memoryStream.Seek(0, SeekOrigin.Begin);
            }

            return memoryStream.ToArray();
        }

        // GET: api/Export/HrdQa
        [HttpGet("Export/HrdQa")]
        [Authorize(Policy = PolicyNames.ViewHRDs)]
        public IActionResult ExportHrdQa([FromQuery] ExportFilter filter)
        {
            var query = _context.Hrds
                .Select(s => new QAListViewModel
                {
                    Id = s.Id,
                    DayCode = s.DayCode,
                    IsHRD = s.IsHrd,
                    IsPest = s.IsPest,
                    IsSMI = s.IsSmi,
                    IsNR = s.IsNr,
                    IsFM = s.IsFm,
                    IsMicro = s.IsMicro,
                    Fert = s.Globenum,
                    ProductDescription = _context.Products.Where(x => x.Gpn == s.Globenum).Select(s => s.Description).First(),
                    Line = s.Line,
                    Shift = s.Shift,
                    HourCode = s.HourCode,
                    Cases = s.Cases,
                    ShortDescription = s.ShortDescription,
                    Originator = s.Originator,
                });

            //Sorting
            switch (filter.SortColumn)
            {
                case "id":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Id)
                        : query.OrderBy(o => o.Id);
                    break;
                case "daycode":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.DayCode)
                        : query.OrderBy(o => o.DayCode);
                    break;
                case "fert":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Fert)
                        : query.OrderBy(o => o.Fert);
                    break;
                case "productdescription":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.ProductDescription)
                        : query.OrderBy(o => o.ProductDescription);
                    break;
                case "line":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Line)
                        : query.OrderBy(o => o.Line);
                    break;
                case "shift":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Shift)
                        : query.OrderBy(o => o.Shift);
                    break;
                case "hourcode":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.HourCode)
                        : query.OrderBy(o => o.HourCode);
                    break;
                case "cases":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Cases)
                        : query.OrderBy(o => o.Cases);
                    break;
                case "shortdescription":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.ShortDescription)
                        : query.OrderBy(o => o.ShortDescription);
                    break;
                case "originator":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Originator)
                        : query.OrderBy(o => o.Originator);
                    break;
            }

            //Search
            if (!string.IsNullOrWhiteSpace(filter.SearchString))
            {
                query = query.Where(f => f.DayCode.Contains(filter.SearchString)
                                        || f.ProductDescription.Contains(filter.SearchString)
                                        || f.ShortDescription.Contains(filter.SearchString)
                                        || f.Fert.Contains(filter.SearchString)
                                        || f.Originator.Contains(filter.SearchString));
            }

            byte[] exportBytes = Array.Empty<byte>();
            var contentType = string.Empty;
            var exportFormat = string.Empty;

            switch (filter.ExportFormat)
            {
                case EnumExportFormat.Csv:
                    exportBytes = ExportToCsv(query);
                    contentType = "text/csv";
                    exportFormat = ".csv";
                    break;
                case EnumExportFormat.Excel:
                    exportBytes = ExportToExcel(query.ToList(), "HrdQa");
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    exportFormat = ".xlsx";
                    break;
            }

            return File(exportBytes, contentType, $"HrdQa_{DateTime.Now:yyyyMMddHHmmssfff}" + exportFormat,true);
        }

        // GET: api/Export/Products
        [HttpGet("Export/Products")]
        [Authorize(Policy = PolicyNames.ViewHRDs)]
        public IActionResult ExportProducts([FromQuery] ExportFilter filter)
        {
            var query = _context.Products
                .Select(s => new ProductViewModel
                {
                    Id = s.Id,
                    Year = s.Year,
                    Fert = s.Gpn,
                    Description = s.Description,
                    CostPerCase = s.CostPerCase,
                    Country = s.Country,
                    NoBbdate = s.NoBbdate,
                    Holiday = s.Holiday
                });

            //Sorting
            switch (filter.SortColumn)
            {
                case "id":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Id)
                        : query.OrderBy(o => o.Id);
                    break;
                case "year":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Year)
                        : query.OrderBy(o => o.Year);
                    break;
                case "fert":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Fert)
                        : query.OrderBy(o => o.Fert);
                    break;
                case "description":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Description)
                        : query.OrderBy(o => o.Description);
                    break;
                case "costpercase":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.CostPerCase)
                        : query.OrderBy(o => o.CostPerCase);
                    break;
                case "country":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Country)
                        : query.OrderBy(o => o.Country);
                    break;
                case "nobestbeforedate":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.NoBbdate)
                        : query.OrderBy(o => o.NoBbdate);
                    break;
            }

            //Search
            if (!string.IsNullOrWhiteSpace(filter.SearchString))
            {
                query = query.Where(f => f.Year.Contains(filter.SearchString)
                                        || f.Fert.Contains(filter.SearchString)
                                        || f.Description.Contains(filter.SearchString)
                                        || f.Country.Contains(filter.SearchString));
            }

            byte[] exportBytes = new byte[] { };
            var contentType = string.Empty;
            var exportFormat = string.Empty;

            switch (filter.ExportFormat)
            {
                case EnumExportFormat.Csv:
                    exportBytes = ExportToCsv(query);
                    contentType = "text/csv";
                    exportFormat = ".csv";
                    break;
                case EnumExportFormat.Excel:
                    exportBytes = ExportToExcel(query.ToList(), "Products");
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    exportFormat = ".xlsx";
                    break;
            }

            return File(exportBytes, contentType, $"Products_{DateTime.Now:yyyyMMddHHmmssfff}" + exportFormat);
        }

        // GET: api/Export/LaborCosts
        [HttpGet("Export/LaborCosts")]
        [Authorize(Policy = PolicyNames.ViewHRDs)]
        public IActionResult ExportLaborCosts([FromQuery] ExportFilter filter)
        {
            var query = _context.LaborCosts
                .Select(s => new LaborCostViewModel
                {
                    Year = s.Year,
                    LaborCost = s.LaborCostValue
                });

            //Sorting
            switch (filter.SortColumn)
            {
                case "year":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Year)
                        : query.OrderBy(o => o.Year);
                    break;
                case "laborcost":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.LaborCost)
                        : query.OrderBy(o => o.LaborCost);
                    break;
            }

            //Search
            if (!string.IsNullOrWhiteSpace(filter.SearchString))
            {
                query = query.Where(f => f.Year.Contains(filter.SearchString));
            }

            byte[] exportBytes = new byte[] { };
            var contentType = string.Empty;
            var exportFormat = string.Empty;

            switch (filter.ExportFormat)
            {
                case EnumExportFormat.Csv:
                    exportBytes = ExportToCsv(query);
                    contentType = "text/csv";
                    exportFormat = ".csv";
                    break;
                case EnumExportFormat.Excel:
                    exportBytes = ExportToExcel(query.ToList(), "LaborCosts");
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";//"application/vnd.ms-excel";
                    exportFormat = ".xlsx";
                    break;
            }

            return File(exportBytes, contentType, $"LaborCosts_{DateTime.Now:yyyyMMddHHmmssfff}" + exportFormat);
        }

        // GET: api/Export/TestCosts
        [HttpGet("Export/TestCosts")]
        [Authorize(Policy = PolicyNames.ViewHRDs)]
        public IActionResult ExportTestCosts([FromQuery] ExportFilter filter)
        {
            var query = _context.TestCosts
                .Select(s => new TestCostViewModel
                {
                    Id = s.Id,
                    Year = s.Year,
                    TestCost = s.TestCostValue,
                    TestName = s.TestName
                });

            //Sorting
            switch (filter.SortColumn)
            {
                case "id":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Id)
                        : query.OrderBy(o => o.Id);
                    break;
                case "year":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Year)
                        : query.OrderBy(o => o.Year);
                    break;
                case "testname":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.TestName)
                        : query.OrderBy(o => o.TestName);
                    break;
                case "testcost":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.TestCost)
                        : query.OrderBy(o => o.TestCost);
                    break;
            }

            //Search
            if (!string.IsNullOrWhiteSpace(filter.SearchString))
            {
                query = query.Where(f => f.Year.Contains(filter.SearchString) || f.TestName.Contains(filter.SearchString));
            }

            byte[] exportBytes = new byte[] { };
            var contentType = string.Empty;
            var exportFormat = string.Empty;

            switch (filter.ExportFormat)
            {
                case EnumExportFormat.Csv:
                    exportBytes = ExportToCsv(query);
                    contentType = "text/csv";
                    exportFormat = ".csv";
                    break;
                case EnumExportFormat.Excel:
                    exportBytes = ExportToExcel(query.ToList(), "TestCosts");
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    exportFormat = ".xlsx";
                    break;
            }

            return File(exportBytes, contentType, $"TestCosts_{DateTime.Now:yyyyMMddHHmmssfff}" + exportFormat);
        }

        // GET: api/Export/Users
        [HttpGet("Export/Users")]
        [Authorize(Policy = PolicyNames.ViewHRDs)]
        public IActionResult ExportUsers([FromQuery] ExportFilter filter)
        {
            var query = _context.Users.Select(s => new UserViewModel
            {
                Id = s.Id,
                Name = s.Name,
                UserId = s.UserId
            });

            //Sorting
            switch (filter.SortColumn)
            {
                case "id":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Id)
                        : query.OrderBy(o => o.Id);
                    break;
                case "name":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Name)
                        : query.OrderBy(o => o.Name);
                    break;
                case "userid":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.UserId)
                        : query.OrderBy(o => o.UserId);
                    break;
            }

            if (!string.IsNullOrEmpty(filter.SearchString))
            {
                query = query.Where(f => f.Name.Contains(filter.SearchString)
                                        || f.UserId.Contains(filter.SearchString));
            }

            byte[] exportBytes = new byte[] { };
            var contentType = string.Empty;
            var exportFormat = string.Empty;

            switch (filter.ExportFormat)
            {
                case EnumExportFormat.Csv:
                    exportBytes = ExportToCsv(query);
                    contentType = "text/csv";
                    exportFormat = ".csv";
                    break;
                case EnumExportFormat.Excel:
                    exportBytes = ExportToExcel(query.ToList(), "Users");
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    exportFormat = ".xlsx";
                    break;
            }

            return File(exportBytes, contentType, $"Users_{DateTime.Now:yyyyMMddHHmmssfff}" + exportFormat);
        }

        // GET: api/Export/Lookup
        [HttpGet("Export/Lookup")]
        [Authorize(Policy = PolicyNames.ViewHRDs)]
        public IActionResult ExportLookup([FromQuery] ExportFilter filter)
        {
            var query = _context.DropDownItems
                .Include(i => i.DropDownType)
                .Select(s => new DropDownItemViewModel
                {
                    Id = s.Id,
                    DropDownTypeId = s.DropDownTypeId,
                    IsActive = s.IsActive,
                    SortOrder = s.SortOrder,
                    Value = s.Value,
                    TypeName = s.DropDownType.Name
                });

            //Sorting
            switch (filter.SortColumn)
            {
                case "id":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Id)
                        : query.OrderBy(o => o.Id);
                    break;
                case "typeName":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.TypeName)
                        : query.OrderBy(o => o.TypeName);
                    break;
                case "value":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Value)
                        : query.OrderBy(o => o.Value);
                    break;
            }

            if (!string.IsNullOrEmpty(filter.SearchString))
            {
                query = query.Where(f => f.Value.Contains(filter.SearchString)
                                        || f.TypeName.Contains(filter.SearchString));
            }

            byte[] exportBytes = new byte[] { };
            var contentType = string.Empty;
            var exportFormat = string.Empty;

            switch (filter.ExportFormat)
            {
                case EnumExportFormat.Csv:
                    exportBytes = ExportToCsv(query);
                    contentType = "text/csv";
                    exportFormat = ".csv";
                    break;
                case EnumExportFormat.Excel:
                    exportBytes = ExportToExcel(query.ToList(), "Lookup");
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    exportFormat = ".xlsx";
                    break;
            }

            return File(exportBytes, contentType, $"Lookup_{DateTime.Now:yyyyMMddHHmmssfff}" + exportFormat);
        }
    }
}
