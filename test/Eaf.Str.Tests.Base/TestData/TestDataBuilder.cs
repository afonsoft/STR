using Eaf.Str.EntityFrameworkCore;

namespace Eaf.Str.Test.Base.TestData
{
    public class TestDataBuilder
    {
        private readonly ProjectNameDbContext _context;
        private readonly int _tenantId;

        public TestDataBuilder(ProjectNameDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            new TestOrganizationUnitsBuilder(_context, _tenantId).Create();

            _context.SaveChanges();
        }
    }
}