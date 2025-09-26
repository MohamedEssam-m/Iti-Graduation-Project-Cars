namespace Cars.BLL.Service.Abstraction
{
    public interface IOllamaService
    {
        Task<string> AskModelAsync(string prompt, bool useStream = false);
    }

}
