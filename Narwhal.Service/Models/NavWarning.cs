using System;
using System.Text.Json;

namespace Narwhal.Service.Models
{
    public class NavWarning
    {
        public string Source { get; set; }
        public DateTime Date { get; set; }
        public JsonElement Data { get; set; }
    }
}
