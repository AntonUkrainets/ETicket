﻿using DBContextLibrary.Domain.Entities;
using DBContextLibrary.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBContextLibrary.Domain.Repositories
{
    public class UserRepository : IRepository<User, Guid>
    {
        private readonly ETicketDataContext context;

        public UserRepository(ETicketDataContext context)
        {
            this.context = context;
        }

        public IQueryable<User> GetAll()
        {
            return context.Users.Include(u => u.Document).Include(u => u.Privilege).Include(u => u.Role);
        }

        public User Get(Guid id)
        {
            var user = context.Users
                .Include(u => u.Document)
                .Include(u => u.Privilege)
                .Include(u => u.Role)
                .FirstOrDefault(m => m.Id == id);
            return user;
        }

        public void Create(User user)
        {
            context.Users.Add(user);
        }

        public void Update(User user)
        {
            context.Update(user);
        }

        public void Delete(Guid id)
        {
            var user = context.Users.Find(id);

            if (user != null)
            {
                context.Users.Remove(user);
            }
        }
        public bool UserExists(Guid id)
        {
            return context.Users.Any(e => e.Id == id);
        }
    }
}
