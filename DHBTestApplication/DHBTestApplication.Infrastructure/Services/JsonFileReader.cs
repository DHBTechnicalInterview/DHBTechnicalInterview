namespace DHBTestApplication.Infrastructure.Services
{
    public interface IJsonFileReader
    {
        Task<string> ReadJsonFileAsync(string filePath);
    }

    public class JsonFileReader : IJsonFileReader
    {
        public async Task<string> ReadJsonFileAsync(string filePath)
        {
            using var jsonFile = new StreamReader(filePath);
            return await jsonFile.ReadToEndAsync();
        }
    }
}