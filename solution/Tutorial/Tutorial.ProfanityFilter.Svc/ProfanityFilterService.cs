using System;
using System.Net;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Tutorial.ProfanityFilter.Svc
{
    /// <summary>
    /// This service uses http://www.purgomalum.com apis to filter profanities from given inputs.
    /// </summary>
    public static class ProfanityFilterService
    {
        /// <summary>
        /// Filters the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static async Task<string> Filter(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return await FilterAsync(input);
        }

        /// <summary>
        /// Filters the asynchronous.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private static async Task<string> FilterAsync(string input)
        {
            
            var url = $"http://www.purgomalum.com/service/plain?text={WebUtility.UrlEncode(input)}";
            var task = Task.Factory.FromAsync((cb, o) => 
                ((HttpWebRequest)o).BeginGetResponse(cb, o), res => 
                    ((HttpWebRequest)res.AsyncState).EndGetResponse(res), WebRequest.CreateHttp(url));
            var result = await task;
            var resp = result;
            var stream = resp.GetResponseStream();
            if (stream == null) return input;
            var sr = new System.IO.StreamReader(stream);
            return await sr.ReadToEndAsync();
        }

    }
}