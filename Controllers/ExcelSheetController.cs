using ExcelDataExtract.Models;
using ExcelDataExtract.Models.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace ExcelDataExtract.Controllers
{
    public class ExcelSheetController:Controller
    {
        private readonly ApplicationDbContext _context;

        public ExcelSheetController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length <= 0)
                return BadRequest("Invalid file.");

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets[0];
            var rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                var data = new ExcelSheetData
                {
                    //id = int.Parse(worksheet.Cells[row, 1].Value.ToString()),
                    employeName = worksheet.Cells[row, 2].Value.ToString(),
                    salary = decimal.Parse(worksheet.Cells[row, 3].Value.ToString()),
                    // Map more columns as necessary
                };
                _context.ExcelSheetData.Add(data);
            }
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("getdata")]
        public async Task<IActionResult> Get()
        {
            var data = await _context.ExcelSheetData.ToListAsync();
            return Ok(data);
        }
    }
}
