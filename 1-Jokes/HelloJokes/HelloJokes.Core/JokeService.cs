using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
namespace HelloJokes.Core
{
    public class JokeService
    {
        public async Task<DadJoke> GetJoke()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var responseJson = await client.GetStringAsync("https://icanhazdadjoke.com");

            return DadJoke.FromJson(responseJson);
        }
    }
}
