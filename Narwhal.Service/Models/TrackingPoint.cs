using System;
using System.Text.Json;

namespace Narwhal.Service.Models
{
    public class TrackingPoint
    {
        public int Vessel { get; set; }
        public DateTime Date { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
