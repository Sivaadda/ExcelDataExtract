using Microsoft.EntityFrameworkCore;

namespace ExcelDataExtract.Models.Context
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ExcelSheetData> ExcelSheetData { get; set; }
    }
}
