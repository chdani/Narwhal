using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using MongoDB.Bson;
using MongoDB.Driver;
using Narwhal.Service.HelperFunctions;
using Narwhal.Service.Models;

using Narwhal.Service.Services;
using Nest;

namespace Narwhal.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrackingController : ControllerBase
    {
        private readonly ILogger<TrackingController> _logger;
        private readonly DatabaseService _databaseService;
        private readonly MessagingService _messagingService;

        public TrackingController(
            ILogger<TrackingController> logger,
            DatabaseService databaseService,
            MessagingService messagingService)
        {
            _logger = logger;
            _databaseService = databaseService;
            _messagingService = messagingService;
        }

        [HttpGet("Get")]
        public IEnumerable<TrackingPoint> Get(DateTime? from = null, DateTime? to = null, int limit = 10000)
        {
            from ??= DateTime.MinValue;
            to ??= DateTime.MaxValue;

            var trackingCollection = _databaseService.GetTrackingCollection();
            var eventsCollection = _databaseService.GetEventsCollection();



            eventsCollection.InsertOne(new BsonDocument()
            {
                { "Timestamp", DateTime.UtcNow },
                { "Source", HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress?.ToString() },
                { "Browser", Request.Headers["User-Agent"].FirstOrDefault() ?? null },
                { "Action", $"{nameof(TrackingController)}.{nameof(Get)}" }
            });

            IEnumerable<TrackingPoint> trackingCollectionCalculating =  trackingCollection
                .Find(Builders<BsonDocument>.Filter.Gte("Date", from) & Builders<BsonDocument>.Filter.Lt("Date", to))
                .ToEnumerable()
                .ToArray()
                .Select(b => new TrackingPoint()
                {
                    Vessel = b["Vessel"].AsInt32,
                    Date = b["Date"].AsDateTime,
                    Latitude = b["Position"][1].AsDouble,
                    Longitude = b["Position"][0].AsDouble
                })
                .Take(limit);

             


            List<TrackingPoint> VesselTracked = new List<TrackingPoint>();
            List<TrackingPoint> FinaltrackingCollection = new List<TrackingPoint>();


            foreach (var vessel in trackingCollectionCalculating.Select(x => x.Vessel).Distinct())
            {

                VesselTracked = trackingCollectionCalculating.Where(x => x.Vessel == vessel).OrderBy(b => b.Date).ToList();



                   
                for (int i = 0; i < VesselTracked.Count(); i++)
                {



                    LocationCordinates L1 = new LocationCordinates();
                    LocationCordinates L2 = new LocationCordinates();

                     

                    L1.Latitude = VesselTracked[i].Latitude;
                    L1.Longitude = VesselTracked[i].Longitude;
                    L2.Latitude = (i==0 ? 0 : VesselTracked[i - 1].Latitude);
                    L2.Longitude = (  i==0 ? 0 : VesselTracked[i - 1].Longitude);

                    // Distance with respect to last provided cordinates 
                    VesselTracked[i].DistnaceFromLastPoint = (i == 0 ? 0 : CordinatesValuesCalculations.CalculateDistance(L1, L2))/1000;

                    // Speed calculated from the last cordinate to current cordinate distance
                    double d = VesselTracked[i].DistnaceFromLastPoint;
                    string time  = VesselTracked[i].Date.Subtract(i == 0 ? VesselTracked[i].Date : VesselTracked[i - 1].Date).ToString();

                    // Multiplying 0.539957 to convert the speed from km/h to knots
                    VesselTracked[i].SpeedFromLastPoint =  (i == 0 ? 0: ((d / TimeSpan.Parse(time).TotalHours)*0.539957));

                    // Adding all distnaces from previous cordinates to calculate total distance
                    VesselTracked[i].TotalDistance = (i == 0 ? 0 :( VesselTracked[i].DistnaceFromLastPoint +  VesselTracked[i - 1].TotalDistance)) ;

                    //Averages speed = all speeds divide by total cordinates provided except start one where speed is zero (total cordinates count -1 )
                    VesselTracked[i].AvgSpeed =( i==0 ?0 : VesselTracked.AsEnumerable().Sum(o => o.SpeedFromLastPoint) /  i);



                }
                FinaltrackingCollection.AddRange(VesselTracked);


            }




            return FinaltrackingCollection.AsEnumerable();







        }
    }
}
