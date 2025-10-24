using System.Linq.Expressions;
using ECommerce.ShareLibrary.Interface;
using OrderApi.Domain.Entities;

namespace OrderApi.Application.Interfaces
{
    public interface IOrder : IGenericInterface<Order>
    {
        Task<IEnumerable<Order>> GetOrdersAsync(Expression<Func<Order, bool>> predicate);
    }
}
