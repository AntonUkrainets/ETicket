﻿using ETicket.DataAccess.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ETicket.DataAccess.Domain.Repositories
{
    public class PriceListRepository
    {
        #region 

        private readonly ETicketDataContext context;

        #endregion

        public PriceListRepository(ETicketDataContext eTicketDataContext)
        {
            context = eTicketDataContext;
        }

        public void Create(PriceList item)
        {
            context.PriceList.Add(item);
        }

        public PriceList Get(int id)
        {
            return context.PriceList.Include(p => p.Area)
                .FirstOrDefault(p => p.Id == id);
        }

        public IQueryable<PriceList> GetAll()
        {
            return context.PriceList;
        }
    }
}
