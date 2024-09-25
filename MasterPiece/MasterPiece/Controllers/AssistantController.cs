using RestSharp;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MasterPiece.Controllers
{
    public class AssistantController : Controller
    {
        private readonly string _apiKey = "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";  // Replace with your OpenAI API key

        public ActionResult RecommendTests()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> RecommendTests(string symptoms)
        {
            try
            {
                var client = new RestClient("https://api.openai.com");  // Base URL for the OpenAI API
                var request = new RestRequest("/v1/chat/completions", Method.Post); // Correct endpoint for chat-based models

                // Add headers
                request.AddHeader("Authorization", $"Bearer {_apiKey}");
                request.AddHeader("Content-Type", "application/json");

                // Create the prompt as a conversation message
                var prompt = $"A patient has the following symptoms: {symptoms}. What laboratory tests would you recommend?";

                // Set up the request body for GPT-3.5-turbo or GPT-4
                var body = new
                {
                    model = "gpt-3.5-turbo",  // You can also use "gpt-4" if needed
                    messages = new[]
                    {
                    new { role = "system", content = "You are a helpful assistant." },
                    new { role = "user", content = prompt }
                },
                    max_tokens = 100
                };
                request.AddJsonBody(body);

                // Execute the request
                var response = await client.ExecuteAsync(request);

                // Check if the response is successful
                if (response.IsSuccessful)
                {
                    var result = response.Content; // Process the response to display test recommendations
                    return View("Recommendations", model: result); // Pass the result to the view
                }

                // Handle errors
                ViewBag.ErrorMessage = $"API request failed. Status Code: {response.StatusCode}, Content: {response.Content}";
                return View("Error");
            }
            catch (Exception ex)
            {
                // Handle exceptions
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return View("Error");
            }
        }
        // This action handles the display of the test recommendations from GPT-3
        [HttpGet]
        public ActionResult Recommendations(string result)
        {
            // Pass the result (the GPT-3 response) to the Recommendations view
            return View(model: result);
        }
    }
}