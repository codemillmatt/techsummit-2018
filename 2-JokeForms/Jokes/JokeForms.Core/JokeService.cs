using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;


namespace JokeForms.Core
{
    public class JokeService
    {
        public JokeService()
        {

        }

        public async Task<DadJoke> GetJoke()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var jokeResponseJson = await client.GetStringAsync("https://dadjoke.azurewebsites.net/api/GetJoke");

            return DadJoke.FromJson(jokeResponseJson);
        }

        public async Task<JokeSearchResult> GetJoke(string term)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var jokeResponseJson = await client.GetStringAsync(
                $"https://icanhazdadjoke.com/search?term={term}");

            return JokeSearchResult.FromJson(jokeResponseJson);
        }
    }
}
