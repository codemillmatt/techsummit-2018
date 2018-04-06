
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace TextSentimentFunction
{
    public static class TextSentiment
    {
        [FunctionName("TextSentiment")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string sentence = req.Query["sentence"];

            //string requestBody = new StreamReader(req.Body).ReadToEnd();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;

            var score = await AnalysisService.AnalyzeWords(sentence);


            return (ActionResult)new OkObjectResult(score);

            //return sentence != null
            //    ? (ActionResult)new OkObjectResult($"Hello, {sentence}")
            //    : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
