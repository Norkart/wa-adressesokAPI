﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack;
using SSTest.Requests;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;

namespace SSTest
{
    public class AddressSearchService : Service
    {
        //TODO: move to config
        private static string elasticSearchUrl = "http://192.168.46.199:9202/adresser/_search";

        private string GetAddressElQuery(string query)
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
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(elasticSearchUrl);
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
                    return result.ToString();
                }
            }
        }


        public object Get(AddressSearch request)
        {
            var value = base.Request.QueryString["query"];
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