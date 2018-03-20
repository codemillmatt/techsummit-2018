using System;
using System.Collections.Generic;
using System.Net;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HelloJokes.Core
{
    public partial class DadJoke
    {
        public const string JokeUrl = "https://dadjoke.azurewebsites.net/api/GetJoke";

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("joke")]
        public string Joke { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }
    }

    public partial class DadJoke
    {
        public static DadJoke FromJson(string json) => JsonConvert.DeserializeObject<DadJoke>(json);
    }
}

