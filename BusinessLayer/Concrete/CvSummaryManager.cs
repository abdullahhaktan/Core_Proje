using BusinessLayer.Abstract;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class CvSummaryManager : ICvSummaryService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public CvSummaryManager(string apiKey)
        {
            _apiKey = apiKey;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        public async Task<string> GetSummaryAsync(string cvText)
        {
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
        new
        {
            role = "system",
            content = @"Sen bir insan kaynakları uzmanısın. Kullanıcının verdiği CV içeriğine göre:

- Bu kişinin teknik becerilerini net ve doğrudan tanımla.
- Hangi yazılım teknolojileri programlama dilleri veri tabanı bilgisi alanlarında uzmanlaştığını çokca belirt (php,laravel,.net,entityframework)
- Projelerdeki deneyimlerinden ne gibi sonuçlar çıkarılabileceğini yaz.
- Kısa ama vurucu, sanki bir danışman gibi konuş. Genel cümle kurma, doğrudan kişiye özel yorum yap.


."
        },
        new { role = "user", content = cvText }
    },
                temperature = 0.4
            };



            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"OpenAI API hatası: {errorContent}");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(responseString);

            return jsonDoc.RootElement
                          .GetProperty("choices")[0]
                          .GetProperty("message")
                          .GetProperty("content")
                          .GetString();
        }
    }
}
