using System;
using System.Threading.Tasks;
using SharpToken;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using TeamWeeklyStatus.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace TeamWeeklyStatus.Infrastructure.Shared.Services.AI
{
    public class AiService : IAiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;
        private readonly string _model;
        private readonly Uri _openAiApiUri;

        public AiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _apiKey = _configuration["OpenAISettings:ApiKey"];
            _openAiApiUri = new Uri(_configuration["OpenAISettings:ApiUrl"]);
            _model = _configuration["OpenAISettings:Model"];
        }

        public async Task<string> EnhanceTextAsync(string prompt)
        {
            var encoding = GptEncoding.GetEncoding("cl100k_base");

            var tokenCounter = new TokenCounter();
            int promptTokens = tokenCounter.CountTokens(prompt, _model);
            int maxResponseTokens = 500;
            int totalTokens = promptTokens + maxResponseTokens;
            int modelTokenLimit = 8192;

            if (totalTokens > modelTokenLimit)
            {
                // Truncate input text
                int maxPromptTokens = modelTokenLimit - maxResponseTokens;
                prompt = TruncateTextToFit(prompt, maxPromptTokens, encoding);
            }

            var messages = new[]
            {
            new { role = "system", content = "You are an assistant that improves the grammar and fluency of texts while preserving the original formatting, including bullet points and indentation." },
            new { role = "user", content = prompt }
        };

            var requestBody = new
            {
                model = _model,
                messages,
                max_tokens = maxResponseTokens,
                temperature = 0.7
            };

            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, _openAiApiUri);
            httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
            httpRequest.Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(httpRequest);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"OpenAI API Error: {response.StatusCode}\n{errorContent}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            using JsonDocument json = JsonDocument.Parse(responseContent);

            string enhancedText = json.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return enhancedText;
        }

        private string TruncateTextToFit(string text, int maxTokens, GptEncoding encoding)
        {
            var tokens = encoding.Encode(text);
            if (tokens.Count <= maxTokens)
                return text;

            var truncatedTokens = tokens.Take(maxTokens).ToList();
            return encoding.Decode(truncatedTokens);
        }
    }
    public class TokenCounter
    {
        public int CountTokens(string text, string model)
        {
            string encodingName = GetEncodingNameForModel(model);

            var encoding = GptEncoding.GetEncoding(encodingName);
            var tokens = encoding.Encode(text);

            return tokens.Count;
        }

        private string GetEncodingNameForModel(string model)
        {
            if (model.StartsWith("gpt-4"))
                return "cl100k_base";
            else if (model.StartsWith("gpt-3.5-turbo"))
                return "cl100k_base";
            else if (model.StartsWith("text-davinci"))
                return "p50k_base";
            else
                return "r50k_base";
        }
    }
}
