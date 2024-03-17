using Data.Dto;
using System.Collections.Generic;

namespace Data.Interfaces
{
    public interface IOrderService
    {
        void CreateOrder(int userId, OrdersDto orderDetails);
        List<OrdersDto> GetOrderList(int userId);
        List<OrdersDto> GetAllOrders();

    }
}
