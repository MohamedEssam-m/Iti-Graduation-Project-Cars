using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.Service.Abstraction
{
    public interface IGeminiService
    {
        Task<string> SendMessageAsync(string userMessage);
        Task<List<string>> GetAvailableModelsAsync();
    }
}
