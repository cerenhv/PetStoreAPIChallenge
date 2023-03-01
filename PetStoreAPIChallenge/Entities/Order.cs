using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStoreAPIChallenge.Entities
{
    public partial class Order
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("petId")]
        public int petId { get; set; }

        [JsonProperty("quantity")]
        public int quantity { get; set; }

        [JsonProperty("shipDate")]
        public DateTime shipDate { get; set; }

        [JsonProperty("status")]
        public string status { get; set; }
        
        [JsonProperty("complete")]
        public bool complete { get; set; }
    }
}
