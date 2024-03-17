using Data.Models;

namespace Data.Dto
{
    public class CartItemDto
    {
        public Grocery Grocery { get; set; }
        public int Quantity { get; set; }
    }
}
