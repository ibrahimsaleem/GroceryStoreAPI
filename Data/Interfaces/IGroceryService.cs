using Data.Dto;
using Data.Models;
using System.Collections.Generic;

namespace Data.Interfaces
{
    public interface IGroceryService
    {
        List<Grocery> GetAllGrocerys();
        int AddGrocery(Grocery grocery);
        int UpdateGrocery(Grocery grocery);
        Grocery GetGroceryData(int groceryId);
        string DeleteGrocery(int groceryId);
        List<Categories> GetCategories();
        List<CartItemDto> GetGrocerysAvailableInCart(string cartId);
    }
}
