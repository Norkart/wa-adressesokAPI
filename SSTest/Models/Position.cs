namespace SSTest.Models
{
    public class Position
    {
        public float Lon { get; set; }
        public float Lat { get; set; }
        public Position(float lon, float lat)
        {
            Lon = lon;
            Lat = lat;
        }
    }
}