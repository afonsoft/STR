using Eaf.Str.EntityFrameworkCore;

namespace Eaf.Str.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly StrDbContext _context;

        public InitialHostDbBuilder(
            StrDbContext context
        )
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}