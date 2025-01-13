using Producer.Data.Entities;
using Producer.Dtos;

namespace Producer.Extensions
{
    public static class CommonExtensions
    {
        public static Order ToOrderEntity(this OrderDto orderDto)
        {
            return new Order
            {
                ProductName = orderDto.ProductName,
                Quantity = orderDto.Quantity,
                Price = orderDto.Price
            };
        }

        public static OrderDto ToOrderDto(this Order order)
        {
            return new OrderDto
            {
                ProductName = order.ProductName,
                Quantity = order.Quantity,
                Price = order.Price
            };
        }
    }
}
