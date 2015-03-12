using ServiceStack;
using SSTest.Requests;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;

namespace SSTest.Services
{
    public class AddressSearchService : Service
    {
        //TODO: move to config
        private const string ElasticSearchUrl = "http://192.168.46.199:9202/adresser/_search";

        private static string GetAddressElQuery(string query)
        {
            var obj = new JObject(
                new JProperty("size", 7),
                new JProperty("query", new JObject(                    
                    new JProperty("match", new JObject(
                        new JProperty("full_gate_adresse.edgegram", new JObject(
                            new JProperty("query", query)
                        ))
                    ))
                ))
            );

            return obj.ToString();
        }

        private string QueryElasticSearch(string elQuery)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(ElasticSearchUrl);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "POST";
            httpWebRequest.Accept = "application/json; charset=utf-8";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(elQuery);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return result;
                }
            }
        }


        public object Get(AddressSearch request)
        {
            var value = Request.QueryString["query"];
            if (value == null)
            {
                return HttpError.NotFound("Not found");
            }

            var query = GetAddressElQuery(value);
            var result = QueryElasticSearch(query);
            if (result != null)
            {
                return new ElasticSearchResponse(result);
            }
            return HttpError.NotFound("Ooops!");
        }
    }
}   