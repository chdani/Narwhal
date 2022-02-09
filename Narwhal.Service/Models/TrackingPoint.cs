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

        public double DistnaceFromLastPoint { get; set; }


        public double SpeedFromLastPoint { get; set; }

        public double TotalDistance { get; set; }

        public double AvgSpeed { get; set; }


    }
}
