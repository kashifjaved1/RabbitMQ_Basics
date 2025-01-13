using Producer.Data;
using Producer.Dtos;

namespace Producer.Services.Contracts
{
    public interface IOrderService
    {
        Task<int> SaveOrder(OrderDto orderDto);

        Task<List<OrderDto>> GetAllOrders();
    }
}
