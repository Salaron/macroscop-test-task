using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Client.Network
{
    public class PalindromeResponse
    {
        [JsonProperty]
        public bool Result { get; set; }
    }
}
