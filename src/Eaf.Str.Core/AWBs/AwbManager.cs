﻿using Eaf.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Eaf.Str.AWBs
{
    public class AwbManager : StrDomainServiceBase, IAwbManager
    {
        private readonly IRepository<AwbAddress> _awbAddressRepository;
        private readonly IRepository<AwbItem> _awbItemRepository;
        private readonly IRepository<Awb> _awbRepository;

        public AwbManager(IRepository<AwbAddress> awbAddressRepository,
            IRepository<AwbItem> awbItemRepository,
            IRepository<Awb> awbRepository)
        {
            _awbAddressRepository = awbAddressRepository;
            _awbItemRepository = awbItemRepository;
            _awbRepository = awbRepository;
        }

        public IQueryable<AwbAddress> AwbAddress => _awbAddressRepository.GetAll();

        public IQueryable<AwbItem> AwbItems => _awbItemRepository.GetAll();

        public IQueryable<Awb> Awbs => _awbRepository.GetAll()
                                    .Include(x => x.Sender)
                                    .Include(x => x.Recipient)
                                    .Include(x => x.Itens);
    }
}