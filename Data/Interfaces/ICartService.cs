namespace Data.Interfaces
{
    public interface ICartService
    {
        void AddGroceryToCart(int userId, int groceryId);
        void RemoveCartItem(int userId, int groceryId);
        void DeleteOneCartItem(int userId, int groceryId);
        int GetCartItemCount(int userId);
        void MergeCart(int tempUserId, int permUserId);
        int ClearCart(int userId);
        string GetCartId(int userId);
    }
}
