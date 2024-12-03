using IdentityModel.Client; // Necesario para manejar OAuth2
using Newtonsoft.Json; // Para manejar JSON
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Capa4_Persistencia.SqlServer.ModuloBase
{
    public class Cie11ApiClient
    {
        private const string BaseUrl = "https://id.who.int/icd/";
        private const string TokenUrl = "https://icdaccessmanagement.who.int";
        private readonly HttpClient httpClient;

        public Cie11ApiClient()
        {
            httpClient = new HttpClient();
        }
        private async Task<string> ObtenerTokenAsync()
        {
            string clientId = "9030d770-794a-4854-b8ba-2a98a8d0c0af_9ccaccaf-a75b-4f10-a0c7-ac4c05e39895";
            string clientSecret = "yCtWI4GWBuUpUQLjoensOEPBaOYsEPoqg2V710CpkIE=";

            var discoveryResponse = await httpClient.GetDiscoveryDocumentAsync(TokenUrl);
            if (discoveryResponse.IsError)
            {
                throw new Exception($"Error al obtener el documento de descubrimiento: {discoveryResponse.Error}");
            }

            var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryResponse.TokenEndpoint,
                ClientId = clientId,
                ClientSecret = clientSecret,
                Scope = "icdapi_access"
            });

            if (tokenResponse.IsError)
            {
                throw new Exception($"Error al obtener el token: {tokenResponse.Error}");
            }

            return tokenResponse.AccessToken; // Devuelve el token
        }

        public async Task<string> BuscarTerminoAsync(string term)
        {
            string token = await ObtenerTokenAsync(); // Obtén el token

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("es")); // Idioma
            httpClient.DefaultRequestHeaders.Add("API-Version", "v2"); // Versión de la API

            // Realiza la solicitud GET
            var response = await httpClient.GetAsync($"{BaseUrl}release/11/2021-05/mms/search?q={term}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error en la búsqueda: {response.StatusCode} - {response.ReasonPhrase}");
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<dynamic> BuscarTerminoFormateadoAsync(string term)
        {
            string jsonResponse = await BuscarTerminoAsync(term);
            return JsonConvert.DeserializeObject(jsonResponse); // Convierte la respuesta JSON en un objeto dinámico
        }
    }
}
