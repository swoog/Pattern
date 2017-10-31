namespace Pattern.Api
{
    using System.Globalization;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    public abstract class BaseApi
    {
        protected virtual Task Prepost(HttpClient client, string url, ref string data)
        {
            return Task.FromResult(0);
        }

        protected async Task<T> GetAsync<T>(string uri)
        {
            var baseUrl = this.GetBaseUrl();

            using (var client = this.CreateClient())
            {
                var result = await client.GetStringAsync($"{baseUrl}{uri}");
                return JsonConvert.DeserializeObject<T>(result);
            }
        }

        protected async Task PostAsync<T>(string uri, T value)
        {
            var baseUrl = this.GetBaseUrl();

            using (var client = this.CreateClient())
            {
                var postData = JsonConvert.SerializeObject(value);
                var url = $"{baseUrl}{uri}";
                await this.Prepost(client, url, ref postData);
                await client.PostAsync(url, new StringContent(postData, Encoding.UTF8, "application/json"));
            }
        }

        protected async Task DeleteAsync(string uri)
        {
            var baseUrl = this.GetBaseUrl();

            using (var client = this.CreateClient())
            {
                await client.DeleteAsync($"{baseUrl}{uri}");
            }
        }

        protected async Task<TResult> PostStringAsync<TResult>(string uri, string value)
        {
            var baseUrl = this.GetBaseUrl();

            using (var client = this.CreateClient())
            {
                var url = $"{baseUrl}{uri}";
                await this.Prepost(client, url, ref value);
                var result = await client.PostAsync(url, new StringContent(value, Encoding.UTF8, "application/x-www-form-urlencoded"));
                return JsonConvert.DeserializeObject<TResult>(await result.Content.ReadAsStringAsync());
            }
        }

        protected async Task<TResult> PostAsync<T, TResult>(string uri, T value)
        {
            var baseUrl = this.GetBaseUrl();

            using (var client = this.CreateClient())
            {
                var url = $"{baseUrl}{uri}";
                var postData = JsonConvert.SerializeObject(value);
                await this.Prepost(client, uri, ref postData);
                var result = await client.PostAsync(url, new StringContent(postData, Encoding.UTF8, "application/json"));
                return JsonConvert.DeserializeObject<TResult>(await result.Content.ReadAsStringAsync());
            }
        }

        protected async Task PostAsync(string uri)
        {
            var baseUrl = this.GetBaseUrl();

            using (var client = this.CreateClient())
            {
                var url = $"{baseUrl}{uri}";
                var content = string.Empty;
                await this.Prepost(client, uri, ref content);
                await client.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));
            }
        }


        protected virtual HttpClient CreateClient()
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.AcceptLanguage.TryParseAdd(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);

            return httpClient;
        }

        protected abstract string GetBaseUrl();
    }
}
