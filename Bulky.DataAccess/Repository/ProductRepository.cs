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
    public class ProductRepository : Repository<Product>, IProductRepository        
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
                _db= db;
        }

        public void Update(Product product)
        {
            var objFromDb = _db.Products.FirstOrDefault(u => u.Id == product.Id);

            if (objFromDb != null)
            {

                objFromDb.Title = product.Title;
                objFromDb.Description = product.Description;
                objFromDb.Price = product.Price;
                objFromDb.Price50 = product.Price50; 
                objFromDb.Price100 = product.Price100;
                objFromDb.ListPrice = product.ListPrice;
                objFromDb.CategoryId = product.CategoryId;
                objFromDb.Author = product.Author;
                objFromDb.ISBN = product.ISBN;
                if(product.ImageUrl != null)
                {

                    objFromDb.ImageUrl = product.ImageUrl;
                }

            }
        }

    }
}
