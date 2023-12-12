using System.Collections.Generic;
using System.Linq;
using Eaf.Localization;
using Microsoft.EntityFrameworkCore;
using Eaf.Str.EntityFrameworkCore;

namespace Eaf.Str.Migrations.Seed.Host
{
    public class DefaultLanguagesCreator
    {
        public static List<ApplicationLanguage> InitialLanguages => GetInitialLanguages();
        private readonly StrDbContext _context;

        private static List<ApplicationLanguage> GetInitialLanguages()
        {
            var tenantId = StrConsts.MultiTenancyEnabled ? null : (int?)1;
            return new List<ApplicationLanguage>
            {
                new ApplicationLanguage(tenantId, "pt-BR", "Português (Brasil)", "famfamfam-flags br"),
                new ApplicationLanguage(tenantId, "en", "English", "famfamfam-flags us"),
                new ApplicationLanguage(tenantId, "es", "Español", "famfamfam-flags es")
            };
        }

        public DefaultLanguagesCreator(
            StrDbContext context
        )
        {
            _context = context;
        }

        public void Create()
        {
            foreach (var language in InitialLanguages)
            {
                if (_context.Languages.IgnoreQueryFilters().Any(l => l.TenantId == language.TenantId && l.Name == language.Name))
                    continue;

                _context.Languages.Add(language);
                _context.SaveChanges();
            }
        }
    }
}