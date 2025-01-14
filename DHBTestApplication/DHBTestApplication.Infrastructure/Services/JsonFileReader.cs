namespace DHBTestApplication.Infrastructure.Services
{//fix bug:before the data access logic is in the controller, it will be mess, so extract it here,
    //and also move AllCountry.json file to Resources directory.

    /// <summary>
    /// Read and Return Country data from the json file
    /// </summary>
    public interface IJsonFileReader { Task<string> ReadJsonFileAsync(string filePath); }
    public class JsonFileReader : IJsonFileReader
    {

        private readonly string _baseDirectory;

        /// <summary>
        /// Get the abusolute path of the Resources directory
        /// </summary>
        public JsonFileReader()
        {
            var AbusoluteLocation = typeof(JsonFileReader).Assembly.Location;
            var Directory = Path.GetDirectoryName(AbusoluteLocation);
            _baseDirectory = Path.Combine(Directory!, "Resources");

        }
        public async Task<string> ReadJsonFileAsync(string filePath)
        {
            filePath = Path.Combine(_baseDirectory, filePath);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Data not found", filePath);
            }
            using var jsonFile = new StreamReader(filePath);
            return await jsonFile.ReadToEndAsync();
        }
    }
}