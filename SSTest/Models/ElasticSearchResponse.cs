using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace SSTest
{
    public class ElasticSearchResponse
    {
        public List<Address> Hits { get; set; }

        public ElasticSearchResponse(string data)
        {
            Hits = new List<Address>();
            dynamic json = JValue.Parse(data);

            foreach (dynamic hit in json.hits.hits)
            {
                float lon = hit._source.koordinater[0];
                float lat = hit._source.koordinater[1];
                var addr = new Address
                {
                    Postnummer = hit._source.postnummer,
                    Husbokstav = hit._source.husbokstav,
                    Kommuneid = hit._source.kommuneid,
                    Postnummerstr = hit._source.postnummerstr,
                    Kommuneidstr = hit._source.kommuneidstr,
                    Husnummer = hit._source.husnummer,
                    Fylkenavn = hit._source.fylkenavn,
                    Adresse_type = hit._source.adresse_type,
                    Gate_id = hit._source.gate_id,
                    Fylkenr = hit._source.fylkenr,
                    Poststed = hit._source.poststed,
                    Gatenavn = hit._source.gatenavn,
                    Kommunenavn = hit._source.kommunenavn,
                    Adresse_id = hit._source.adresse_id,
                    Alternativtnavn = hit._source.alternativtnavn,
                    Koordinatkvalitetkode = hit._source.koordinatkvalitetkode,
                    Kortnavn = hit._source.kortnavn,
                    Position = new Position(lon, lat)
                };
                Hits.Add(addr);
            }
        }
    }
}
