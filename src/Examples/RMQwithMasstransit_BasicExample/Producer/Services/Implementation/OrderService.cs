using Core.Commons.Shared;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Producer.Data;
using Producer.Data.Entities;
using Producer.Dtos;
using Producer.Extensions;
using Producer.Services.Contracts;

namespace Producer.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly ApiDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderService(ApiDbContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<int> SaveOrder(OrderDto orderDto)
        {
            try
            {
                var order = orderDto.ToOrderEntity();
                _context.Orders.Add(order);
                var changeCount = await _context.SaveChangesAsync();

                if (changeCount > 0)
                {
                    await _publishEndpoint.Publish<IOrderCreated>(new
                    {
                        order.Id,
                        order.ProductName,
                        order.Quantity,
                        order.Price,
                    });

                    return StatusCodes.Status201Created;
                }

                return StatusCodes.Status204NoContent;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<OrderDto>> GetAllOrders()
        {
            var orders = await _context.Orders.ToListAsync();
            var ordersList = new List<OrderDto>();

            foreach (var order in orders)
            {
                ordersList.Add(order.ToOrderDto());
            }
            
            return ordersList;
        }
    }
}
