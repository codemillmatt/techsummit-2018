using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HelloJokes.Core
{
    public class JokeService
    {
        const string jokeUrl = "https://dadjoke.azurewebsites.net/api/GetJoke";

        public async Task<DadJoke> GetJoke()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var jokeResonseJson = await client.GetStringAsync(jokeUrl);

            return DadJoke.FromJson(jokeResonseJson);
        }

    }
}
