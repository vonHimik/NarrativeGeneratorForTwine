using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenAI_API;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;

namespace Narrative_Generator
{
    /// <summary>
    /// Class that manages requests to the OpenAI API.
    /// </summary>
    public class OpenAI
    {
        /// <summary>
        /// A method that configures and creates a request to OpenAI to create a text completion.
        /// </summary>
        /// <param name="inputPrompt">Prompt, indicating the direction of the request for text generation.</param>
        /// <param name="apiKey">Key for user authentication and access to OpenAI.</param>
        /// <returns>Generated text completion.</returns>
        public static async Task<string> CompletionRequest (string inputPrompt, string apiKey)
        {
            // API URL
            string apiUrl = "https://api.openai.com/v1/engines/text-davinci-003/completions";

            // Prompt configuration.
            string prompt;
            if (inputPrompt.Contains("no one")) { prompt = inputPrompt; } 
            else { prompt = inputPrompt + ". And no one else."; }
            prompt = prompt.Insert(prompt.Length, " Description of the location and those who are in it: ");

            // Creating an HTTP Client.
            using (HttpClient client = new HttpClient())
            {
                // Setting Authorization Header.
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                // Creating a request body in JSON format.
                string requestBody = $"{{\"prompt\": \"{prompt}\", \"max_tokens\": 200}}";
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                Thread.Sleep(5000);

                // Sending an asynchronous POST request to the OpenAI API.
                HttpResponseMessage response = await client.PostAsync(apiUrl, content).ConfigureAwait(false);

                // Processing the received response.
                string responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string returningText = OpenAI.GetText(responseBody);

                return returningText;
            }
        }

        /// <summary>
        /// A method that separates the generated text completion from the general response body, which also contains technical information.
        /// </summary>
        /// <param name="input">Response body.</param>
        /// <returns>Text completion.</returns>
        public static string GetText (string input)
        {
            string startWord = "\"text\": \"\\n\\n";
            string endWord = "\"";

            int startIndex = input.IndexOf(startWord);
            if (startIndex == -1)
            {
                // Start word not found.
                return string.Empty;
            }

            startIndex += startWord.Length;

            int endIndex = input.IndexOf(endWord, startIndex);
            if (endIndex == -1)
            {
                // End word not found.
                return string.Empty;
            }

            return input.Substring(startIndex, endIndex - startIndex);
        }
    }
}
