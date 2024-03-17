using Data.Dto;
using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.DataAccess
{
    public class GroceryDataAccessLayer : IGroceryService
    {
        readonly GroceryDBContext _dbContext;

        public GroceryDataAccessLayer(GroceryDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Grocery> GetAllGrocerys()
        {
            try
            {
                return _dbContext.Grocery.AsNoTracking().ToList();
            }
            catch
            {
                throw;
            }
        }

        public int AddGrocery(Grocery grocery)
        {
            try
            {
                _dbContext.Grocery.Add(grocery);
                _dbContext.SaveChanges();

                return 1;
            }
            catch
            {
                throw;
            }
        }

        public int UpdateGrocery(Grocery grocery)
        {
            try
            {
                Grocery oldGroceryData = GetGroceryData(grocery.GroceryId);

                if (oldGroceryData.CoverFileName != null)
                {
                    if (grocery.CoverFileName == null)
                    {
                        grocery.CoverFileName = oldGroceryData.CoverFileName;
                    }
                }

                _dbContext.Entry(grocery).State = EntityState.Modified;
                _dbContext.SaveChanges();

                return 1;
            }
            catch
            {
                throw;
            }
        }

        public Grocery GetGroceryData(int groceryId)
        {
            try
            {
                Grocery grocery = _dbContext.Grocery.FirstOrDefault(x => x.GroceryId == groceryId);
                if (grocery != null)
                {
                    _dbContext.Entry(grocery).State = EntityState.Detached;
                    return grocery;
                }
                return null;
            }
            catch
            {
                throw;
            }
        }

        public string DeleteGrocery(int groceryId)
        {
            try
            {
                Grocery grocery = _dbContext.Grocery.Find(groceryId);
                _dbContext.Grocery.Remove(grocery);
                _dbContext.SaveChanges();

                return (grocery.CoverFileName);
            }
            catch
            {
                throw;
            }
        }

        public List<Categories> GetCategories()
        {
            List<Categories> lstCategories = new List<Categories>();
            lstCategories = (from CategoriesList in _dbContext.Categories select CategoriesList).ToList();

            return lstCategories;
        }

     
        public List<CartItemDto> GetGrocerysAvailableInCart(string cartID)
        {
            try
            {
                List<CartItemDto> cartItemList = new List<CartItemDto>();
                List<CartItems> cartItems = _dbContext.CartItems.Where(x => x.CartId == cartID).ToList();

                foreach (CartItems item in cartItems)
                {
                    Grocery grocery = GetGroceryData(item.ProductId);
                    CartItemDto objCartItem = new CartItemDto
                    {
                        Grocery = grocery,
                        Quantity = item.Quantity
                    };

                    cartItemList.Add(objCartItem);
                }
                return cartItemList;
            }
            catch
            {
                throw;
            }
        }

    }
}
