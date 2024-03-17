using Data.Dto;
using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.DataAccess
{
    public class OrderDataAccessLayer : IOrderService
    {
        readonly GroceryDBContext _dbContext;
        public OrderDataAccessLayer(GroceryDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void CreateOrder(int userId, OrdersDto orderDetails)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    StringBuilder orderId = new StringBuilder();
                    orderId.Append(CreateRandomNumber(3));
                    orderId.Append('-');
                    orderId.Append(CreateRandomNumber(6));

                    CustomerOrders customerOrder = new CustomerOrders
                    {
                        OrderId = orderId.ToString(),
                        DateCreated = DateTime.Now.Date,
                        CartTotal = orderDetails.CartTotal
                    };

                    _dbContext.CustomerOrders.Add(customerOrder);
                    _dbContext.SaveChanges();

                    foreach (CartItemDto order in orderDetails.OrderDetails)
                    {
                        CustomerOrderDetails productDetails = new CustomerOrderDetails
                        {
                            OrderId = orderId.ToString(),
                            ProductId = order.Grocery.GroceryId,
                            Quantity = order.Quantity,
                            Price = order.Grocery.Price
                        };

                        // Update the stock in the Grocery table
                        Grocery grocery = _dbContext.Grocery.Find(order.Grocery.GroceryId);
                        if (grocery != null)
                        {
                            if (grocery.Stock < order.Quantity)
                            {
                                // Provide a message instead of throwing an exception
                                throw new Exception("Insufficient stock for " + grocery.Title);
                                // Alternatively, you can set a message property and return it in the response.
                                // productDetails.Message = "Insufficient stock for " + grocery.Name;
                            }
                            else
                            {
                                grocery.Stock -= order.Quantity;
                                _dbContext.Entry(grocery).State = EntityState.Modified;
                            }
                        }

                        _dbContext.CustomerOrderDetails.Add(productDetails);
                    }

                    _dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public List<OrdersDto> GetOrderList(int userId)
        {
            List<OrdersDto> userOrders = new List<OrdersDto>();
            List<string> userOrderId = new List<string>();

            userOrderId = _dbContext.CustomerOrders.Where(x => x.UserId == userId)
                .Select(x => x.OrderId).ToList();

            foreach (string orderid in userOrderId)
            {
                OrdersDto order = new OrdersDto
                {
                    OrderId = orderid,
                    CartTotal = _dbContext.CustomerOrders.FirstOrDefault(x => x.OrderId == orderid).CartTotal,
                    OrderDate = _dbContext.CustomerOrders.FirstOrDefault(x => x.OrderId == orderid).DateCreated
                };

                List<CustomerOrderDetails> orderDetail = _dbContext.CustomerOrderDetails.Where(x => x.OrderId == orderid).ToList();

                order.OrderDetails = new List<CartItemDto>();

                foreach (CustomerOrderDetails customerOrder in orderDetail)
                {
                    CartItemDto item = new CartItemDto();

                    Grocery grocery = new Grocery
                    {
                        GroceryId = customerOrder.ProductId,
                        Title = _dbContext.Grocery.FirstOrDefault(x => x.GroceryId == customerOrder.ProductId && customerOrder.OrderId == orderid).Title,
                        Price = _dbContext.CustomerOrderDetails.FirstOrDefault(x => x.ProductId == customerOrder.ProductId && customerOrder.OrderId == orderid).Price
                    };

                    item.Grocery = grocery;
                    item.Quantity = _dbContext.CustomerOrderDetails.FirstOrDefault(x => x.ProductId == customerOrder.ProductId && x.OrderId == orderid).Quantity;

                    order.OrderDetails.Add(item);
                }
                userOrders.Add(order);
            }
            return userOrders.OrderByDescending(x => x.OrderDate).ToList();
        }
       
        // Method to fetch all orders details
        public List<OrdersDto> GetAllOrders()
        {
            List<OrdersDto> allOrders = new List<OrdersDto>();

            var orders = _dbContext.CustomerOrders.ToList(); // Retrieve all orders

            foreach (var order in orders)
            {
                OrdersDto orderDto = new OrdersDto
                {
                    OrderId = order.OrderId,
                    CartTotal = order.CartTotal,
                    OrderDate = order.DateCreated,
                    // Initialize the OrderDetails list
                    OrderDetails = new List<CartItemDto>()
                };

                // Retrieve order details for each order
                var orderDetails = _dbContext.CustomerOrderDetails.Where(x => x.OrderId == order.OrderId).ToList();

                foreach (var orderDetail in orderDetails)
                {
                    // Create a CartItemDto object for each order detail
                    CartItemDto item = new CartItemDto
                    {
                        Quantity = orderDetail.Quantity,
                        Grocery = new Grocery
                        {
                            GroceryId = orderDetail.ProductId,
                            Title = _dbContext.Grocery.FirstOrDefault(g => g.GroceryId == orderDetail.ProductId)?.Title,
                            Price = orderDetail.Price
                        }
                    };

                    // Add the CartItemDto object to the OrderDetails list
                    orderDto.OrderDetails.Add(item);
                }

                // Add the order to the list of all orders
                allOrders.Add(orderDto);
            }

            return allOrders;
        }
        
        int CreateRandomNumber(int length)
        {
            Random rnd = new Random();
            return rnd.Next(Convert.ToInt32(Math.Pow(10, length - 1)), Convert.ToInt32(Math.Pow(10, length)));
        }
    }
}
