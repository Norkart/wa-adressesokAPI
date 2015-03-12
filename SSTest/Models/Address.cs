namespace SSTest.Models
{
    public class Address
    {
        public string Postnummer { get; set; }
        public string Husbokstav { get; set; }
        public int Kommuneid { get; set; }
        public int Postnummerstr { get; set; }
        public string Kommuneidstr { get; set; }
        public int Husnummer { get; set; }
        public string Fylkenavn { get; set; }
        public string AdresseType { get; set; }
        public string GateId { get; set; }
        public int Fylkenr { get; set; }
        public string Poststed { get; set; }
        public string Gatenavn { get; set; }
        public string Kommunenavn { get; set; }
        public string AdresseId { get; set; }
        public string Alternativtnavn { get; set; }
        public int Koordinatkvalitetkode { get; set; }
        public string Kortnavn { get; set; }
        public Position Position { get; set; }
    }
}