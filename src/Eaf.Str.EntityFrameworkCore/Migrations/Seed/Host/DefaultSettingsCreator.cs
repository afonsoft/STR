using System.Linq;
using Eaf.Configuration;
using Eaf.Localization;
using Eaf.Net.Mail;
using Microsoft.EntityFrameworkCore;
using Eaf.Str.EntityFrameworkCore;
using Eaf.Middleware.Core.Authentication;
using Eaf.Middleware.Configuration;
using Eaf.Json;

namespace Eaf.Str.Migrations.Seed.Host
{
    public class DefaultSettingsCreator
    {
        private readonly StrDbContext _context;
        private readonly MicrosoftExternalLoginProviderSettings microsoftExternalLogin;
        private readonly AuthZeroExternalLoginProviderSettings authZeroExternalLogin;

        public DefaultSettingsCreator(StrDbContext context)
        {
            _context = context;
            microsoftExternalLogin = new MicrosoftExternalLoginProviderSettings
            {
                ClientId = "",
                TenantId = "",
                ClientSecret = ""
            };

            authZeroExternalLogin = new AuthZeroExternalLoginProviderSettings
            {
                ClientId = "",
                ClientSecret = "",
                Endpoint = ""
            };
        }

        public void Create()
        {
            //Emailing
            AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "admin@eaf.com.br");
            AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "eaf.com.br mailer");
            AddSettingIfNotExists(EmailSettingNames.Smtp.Host, "localhost");
            AddSettingIfNotExists(EmailSettingNames.Smtp.Port, "25");

            //Languages
            AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, "pt-BR");

            //MFA Microsift
            if (microsoftExternalLogin != null && microsoftExternalLogin.IsValid())
            {
                AddSettingIfNotExists(AppSettings.ExternalLoginProvider.Tenant.Microsoft_IsEnabled, "true");
                AddSettingIfNotExists(AppSettings.ExternalLoginProvider.Host.Microsoft, microsoftExternalLogin.ToJsonString());
            }

            //MFA AuthZero
            if (authZeroExternalLogin != null && authZeroExternalLogin.IsValid())
            {
                AddSettingIfNotExists(AppSettings.ExternalLoginProvider.Tenant.AuthZero_IsEnabled, "true");
                AddSettingIfNotExists(AppSettings.ExternalLoginProvider.Host.AuthZero, authZeroExternalLogin.ToJsonString());
            }
        }

        private void AddSettingIfNotExists(string name, string value, int? tenantId = null)
        {
            if (_context.Settings.IgnoreQueryFilters().Any(s => s.Name == name && s.TenantId == tenantId && s.UserId == null))
            {
                return;
            }

            _context.Settings.Add(new Setting(tenantId, null, name, value));
            _context.SaveChanges();
        }
    }
}