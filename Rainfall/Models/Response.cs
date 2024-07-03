using Newtonsoft.Json.Serialization;
using System.Text.Json.Nodes;

namespace Rainfall.Models
{
    public class Response
    {
        public string @context { get; set; }

        public JsonObject Meta { get; set; }

        public List<Item> Items { get; set; }
    }
}
