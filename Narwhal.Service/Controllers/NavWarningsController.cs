using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using MongoDB.Bson;
using MongoDB.Driver;

using Narwhal.Service.Models;
using Narwhal.Service.Services;

namespace Narwhal.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NavWarningsController : ControllerBase
    {
        private readonly ILogger<NavWarningsController> _logger;
        private readonly DatabaseService _databaseService;
        private readonly MessagingService _messagingService;

        public NavWarningsController(ILogger<NavWarningsController> logger, DatabaseService databaseService, MessagingService messagingService)
        {
            _logger = logger;
            _databaseService = databaseService;
            _messagingService = messagingService;
        }

        [HttpGet("Get")]
        public IEnumerable<NavWarning> Get(int limit = 100)
        {
            var navWarningCollection = _databaseService.GetNavWarningCollection();

            return navWarningCollection
                .AsQueryable()
                .ToArray()
                .Select(b => new NavWarning()
                {
                    Source = b["Source"].AsString,
                    Date = b["Date"].AsDateTime,
                    Data = Jsonify(b["Data"].AsBsonDocument)
                })
                .Take(limit);
        }

        private static JsonElement Jsonify(BsonDocument bsonDocument)
        {
            string json = bsonDocument.ToJson();
            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
            Utf8JsonReader jsonReader = new Utf8JsonReader(jsonBytes);
            JsonDocument jsonDocument = JsonDocument.ParseValue(ref jsonReader);
            return jsonDocument.RootElement;
        }
    }
}
