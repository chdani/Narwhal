using Narwhal.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Narwhal.Service.HelperFunctions
{
    public static class CordinatesValuesCalculations
    {
        public static double CalculateDistance(LocationCordinates point1, LocationCordinates point2)
        {
            try
            {
                var d1 = point1.Latitude * (Math.PI / 180.0);
                var num1 = point1.Longitude * (Math.PI / 180.0);
                var d2 = point2.Latitude * (Math.PI / 180.0);
                var num2 = point2.Longitude * (Math.PI / 180.0) - num1;
                var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                         Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
                return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
            }
            catch (Exception ex)
            {
                return 0;
            }

        }




        public static IEnumerable<TrackingPoint> CalculateVesselTrackInfo(IEnumerable<TrackingPoint> trackingCollectionCalculating)
        {

            try
            {

                List<TrackingPoint> vesselTracked = new List<TrackingPoint>();
                List<TrackingPoint> finalTrackingCollection = new List<TrackingPoint>();


                foreach (var vessel in trackingCollectionCalculating.Select(x => x.Vessel).Distinct())
                {

                    vesselTracked = trackingCollectionCalculating.Where(x => x.Vessel == vessel).OrderBy(b => b.Date).ToList();




                    for (int i = 0; i < vesselTracked.Count(); i++)
                    {



                        LocationCordinates L1 = new LocationCordinates();
                        LocationCordinates L2 = new LocationCordinates();



                        L1.Latitude = vesselTracked[i].Latitude;
                        L1.Longitude = vesselTracked[i].Longitude;
                        L2.Latitude = (i == 0 ? 0 : vesselTracked[i - 1].Latitude);
                        L2.Longitude = (i == 0 ? 0 : vesselTracked[i - 1].Longitude);

                        // Distance with respect to last provided cordinates.
                        vesselTracked[i].DistnaceFromLastPoint = (((i == 0 ? 0 : CordinatesValuesCalculations.CalculateDistance(L1, L2)) / 1000) * 0.539957);

                        // Speed calculated from the last cordinate to current cordinate distance in knots
                        string time = vesselTracked[i].Date.Subtract(i == 0 ? vesselTracked[i].Date : vesselTracked[i - 1].Date).ToString();
                        vesselTracked[i].SpeedFromLastPoint = (i == 0 ? 0 : ((vesselTracked[i].DistnaceFromLastPoint / TimeSpan.Parse(time).TotalHours)));

                        // Adding all distnaces from previous cordinates to calculate total distance
                        vesselTracked[i].TotalDistance = (i == 0 ? 0 : (vesselTracked[i].DistnaceFromLastPoint + vesselTracked[i - 1].TotalDistance));

                        //Averages speed = all speeds divide by total cordinates provided except start one where speed is zero (total cordinates count -1 )
                        vesselTracked[i].AvgSpeed = (i == 0 ? 0 : vesselTracked.AsEnumerable().Sum(o => o.SpeedFromLastPoint) / i);



                    }
                    finalTrackingCollection.AddRange(vesselTracked);


                }




                return finalTrackingCollection.AsEnumerable();
            }
            catch (Exception)
            {

                return trackingCollectionCalculating;
            }

        }
    }
}
