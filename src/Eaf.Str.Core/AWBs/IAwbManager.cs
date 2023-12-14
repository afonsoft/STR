using Eaf.Domain.Services;
using System.Linq;

namespace Eaf.Str.AWBs
{
    public interface IAwbManager : IDomainService
    {
        IQueryable<AwbAddress> AwbAddress { get; }
        IQueryable<AwbItem> AwbItems { get; }
        IQueryable<Awb> Awbs { get; }
    }
}