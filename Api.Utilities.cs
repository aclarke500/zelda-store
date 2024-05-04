using Beedle.Entities;
namespace Beedle.Api.Utilities
{
    public static class FlaskApiUtil
{
    private static readonly HttpClient client = new HttpClient { BaseAddress = new Uri("http://127.0.0.1:5000/") };

    public static async Task<ClassificationResult> ClassifyItemAsync(string itemName)
    {
        try
        {
            var requestBody = new { word = itemName };
            HttpResponseMessage response = await client.PostAsJsonAsync("classify", requestBody);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ClassificationResult>();
            }
            else
            {
                // Log or handle non-success status code
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            // Log or handle exception
            throw new ApplicationException($"An error occurred while sending request to Flask API: {ex.Message}", ex);
        }
    }
}
}