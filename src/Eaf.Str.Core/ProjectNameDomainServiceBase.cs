using Eaf.Domain.Services;

namespace Eaf.Str
{
    public abstract class ProjectNameDomainServiceBase : DomainService
    {
        /* ADD YOUR COMMON MEMBERS FOR ALL YOUR DOMAIN SERVICES. */

        protected ProjectNameDomainServiceBase()
        {
            LocalizationSourceName = ProjectNameConsts.LocalizationSourceName;
        }
    }
}
