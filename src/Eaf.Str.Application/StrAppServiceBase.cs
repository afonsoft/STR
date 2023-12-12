using Eaf.Middleware;

namespace Eaf.Str
{
    public abstract class StrAppServiceBase : MiddlewareAppServiceBase
    {
        /* ADD YOUR COMMON MEMBERS FOR ALL YOUR APP SERVICES. */

        protected StrAppServiceBase()
        {
            LocalizationSourceName = StrConsts.LocalizationSourceName;
        }
    }
}