using System;
using ServiceStack;
using SSTest.Requests;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using SSTest.Models;

namespace SSTest.Services
{
    public class AddressSearchService : Service
    {
        //TODO: move to config
        private const string ElasticSearchUrl = "http://192.168.46.199:9202/adresser/_search";

        private static string GetAddressElQuery(string query, int limit=7)
        {
            var number = Regex.Match(query, @"\d+$").Value;
            string queryJson;
            if (!number.IsNullOrEmpty())
            {
                queryJson = @"
                {{
                    ""size"" : {0},
                    ""query"": {{
                        ""bool"": {{
                            ""should"" : [
                                {{""match"": {{""husnummer"": {{""query"": {1} }}}}}},
                                {{""match"": {{""full_gate_adresse.edgegram"": {{""query"": ""{2}"" }}}}}},
                                {{""match"": {{""gatenavn.vei"": {{""query"": ""{3}"" }}}}}}
                            ]
                        }}
                    }}
                }}";
                return String.Format(queryJson, limit, number, query, query);
            }
            
            queryJson = @"
            {{
                ""size"" : {0},
                ""query"": {{
                    ""bool"": {{
                        ""should"" : [
                            {{""match"": {{""full_gate_adresse.edgegram"": {{""query"": ""{1}"" }}}}}},
                            {{""match"": {{""gatenavn.vei"": {{""query"": ""{2}"" }}}}}}
                        ]
                    }}
                }}
            }}";
            return String.Format(queryJson, limit, query, query);
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