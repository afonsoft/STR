using System.Threading.Tasks;
using Eaf.Domain.Repositories;
using System.Linq;
using Eaf.UI;
using System;
using Eaf.Middleware.Chat;
using Hangfire.Console;
using Hangfire.Server;

namespace Eaf.Str.Airplanes
{
    public class AirplaneManager : StrDomainServiceBase, IAirplaneManager
    {
        private readonly IRepository<Airplane> _repositoryAirplane;
        private readonly IChatCommunicator _chatCommunicator;

        public virtual IQueryable<Airplane> Airplanes
        {
            get
            {
                return _repositoryAirplane.GetAll();
            }
        }

        public AirplaneManager(
            IRepository<Airplane> repositoryAirplane,
            IChatCommunicator chatCommunicator
        )
        {
            _repositoryAirplane = repositoryAirplane;
            _chatCommunicator = chatCommunicator;
        }

        public async Task<Airplane> GetByIdAsync(int id)
        {
            return await _repositoryAirplane.FirstOrDefaultAsync(id);
        }

        public async Task<Airplane> CreateAsync(Airplane airplane)
        {
            if (_repositoryAirplane.GetAll().Any(e => e.Number.ToLower() == airplane.Number))
                throw new UserFriendlyException(L("AirplaneValidate"));

            return await _repositoryAirplane.InsertAsync(airplane);
        }

        public async Task DeleteAsync(int id)
        {
            await _repositoryAirplane.DeleteAsync(id);
        }

        public async Task<Airplane> UpdateAsync(Airplane airplane)
        {
            return await _repositoryAirplane.UpdateAsync(airplane);
        }

        public async Task DateUpdate(PerformContext context)
        {
            context.WriteLine("Job Start!");
            await _chatCommunicator.SendMessageToAll(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            context.WriteLine("Job End!");
        }
    }
}