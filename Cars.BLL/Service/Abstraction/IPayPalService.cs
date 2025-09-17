using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.Service.Abstraction
{
    public interface IPayPalService
    {
        Task<string> GetAccessTokenAsync();
        Task<string> CreateOrderAsync(string totalAmount);
        Task<bool> CompleteOrderAsync(string orderId);

    }
}
