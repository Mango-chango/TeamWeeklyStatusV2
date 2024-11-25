using SharpToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TeamWeeklyStatus.Application.Interfaces.AI;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Infrastructure.Shared.Services.AI
{
    public class OpenAIContentEnhancer : IAIContentEnhancer
    {
        private readonly HttpClient _httpClient;
        private readonly TeamAIConfiguration _config;

        public OpenAIContentEnhancer(HttpClient httpClient, TeamAIConfiguration config)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public async Task<string> EnhanceContentAsync(string prompt)
        {
            var encoding = GptEncoding.GetEncoding("cl100k_base");

            var tokenCounter = new TokenCounter();
            int promptTokens = tokenCounter.CountTokens(prompt, _config.Model);
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
                model = _config.Model,
                messages,
                max_tokens = maxResponseTokens,
                temperature = 0.7
            };

            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, _config.ApiUrl);
            httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _config.ApiKey);
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
                .GetString() ?? string.Empty;

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
