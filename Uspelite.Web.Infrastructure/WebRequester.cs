namespace Uspelite.Web.Infrastructure
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Enums;

    public class WebRequester
    {
        public async Task<string> Request(string url, MethodType methodType, StringContent data = null, string credentials = null, string contentType = "application/json", string authenticationType = "Basic")
        {
            if (methodType != MethodType.Get && data == null)
            {
                throw new InvalidOperationException("Data object is not specified");
            }

            using (HttpClient client = new HttpClient())
            {
                var mediaTypeHeader = new MediaTypeWithQualityHeaderValue(contentType);
                client.DefaultRequestHeaders.Accept.Add(mediaTypeHeader);

                if (!string.IsNullOrEmpty(credentials))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authenticationType, credentials);
                }

                HttpResponseMessage response = new HttpResponseMessage();

                switch (methodType)
                {
                    case MethodType.Get:
                        response = await client.GetAsync(url);
                        break;
                    case MethodType.Post:
                        response = await client.PostAsync(url, data);
                        break;
                    case MethodType.Put:
                        response = await client.PutAsync(url, data);
                        break;
                    default:
                        throw new InvalidOperationException("Not supported or invalid metod type");
                }

                var body = await response.Content.ReadAsStringAsync();
                return body;
            }
        }
    }
}
