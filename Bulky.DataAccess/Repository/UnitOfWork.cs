﻿using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }

        public ICompanyRepository Company { get; private set; }

        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailsRepository OrderDetails { get; private set; }

        public IApplicationUserRepository ApplicationUser { get; private set; }
        public UnitOfWork(ApplicationDbContext db) 
        { 
            _db = db;
            Category=new CategoryRepository(_db);
            Product = new ProductRepository(_db);
            Company=new CompanyRepository(_db);
            OrderHeader=new OrderHeaderRepository(_db);
            OrderDetails=new OrderDetailsRepository(_db);

            ShoppingCart = new ShoppingCartRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
        }
        public ICategoryRepository CategoryRepository { get; private set; }
        public ICategoryRepository ProductRepository { get; private set; }

        public ICompanyRepository CompanyRepository { get; private set; }

        public IOrderHeaderRepository OrderHeaderRepository { get; private set; }

        public IOrderDetailsRepository OrderDetailsRepository { get; private set; }

        public IApplicationUserRepository AppUserRepository { get; private set; }

        public IShoppingCartRepository ShoppingCartRepository { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
