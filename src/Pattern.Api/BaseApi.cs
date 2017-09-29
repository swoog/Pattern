namespace Pattern.Api
{
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    public abstract class BaseApi
    {
        protected virtual Task Prepost(HttpClient client, string url, string data)
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
                await Prepost(client, url, postData);
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
                await Prepost(client, url, value);
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
                await Prepost(client, uri, postData);
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
                await Prepost(client, uri, string.Empty);
                await client.PostAsync(url, new StringContent(string.Empty, Encoding.UTF8, "application/json"));
            }
        }


        protected virtual HttpClient CreateClient()
        {
            return new HttpClient();
        }

        protected abstract string GetBaseUrl();
    }
}
