using Eaf.Domain.Services;

namespace Eaf.Str
{
    public abstract class StrDomainServiceBase : DomainService
    {
        /* ADD YOUR COMMON MEMBERS FOR ALL YOUR DOMAIN SERVICES. */

        protected StrDomainServiceBase()
        {
            LocalizationSourceName = StrConsts.LocalizationSourceName;
        }
    }
}
