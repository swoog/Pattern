namespace Pattern.Api
{
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    public abstract class BaseApi
    {
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
                await client.PostAsync($"{baseUrl}{uri}", new StringContent(postData, Encoding.UTF8, "application/json"));
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
                var result = await client.PostAsync($"{baseUrl}{uri}", new StringContent(value, Encoding.UTF8, "application/x-www-form-urlencoded"));
                return JsonConvert.DeserializeObject<TResult>(await result.Content.ReadAsStringAsync());
            }
        }

        protected async Task<TResult> PostAsync<T, TResult>(string uri, T value)
        {
            var baseUrl = this.GetBaseUrl();

            using (var client = this.CreateClient())
            {
                var postData = JsonConvert.SerializeObject(value);
                var result = await client.PostAsync($"{baseUrl}{uri}", new StringContent(postData, Encoding.UTF8, "application/json"));
                return JsonConvert.DeserializeObject<TResult>(await result.Content.ReadAsStringAsync());
            }
        }

        protected async Task PostAsync(string uri)
        {
            var baseUrl = this.GetBaseUrl();

            using (var client = this.CreateClient())
            {
                await client.PostAsync($"{baseUrl}{uri}", new StringContent(string.Empty, Encoding.UTF8, "application/json"));
            }
        }


        protected virtual HttpClient CreateClient()
        {
            return new HttpClient();
        }

        protected abstract string GetBaseUrl();
    }
}
