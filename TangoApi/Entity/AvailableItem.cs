using System.Collections.Generic;

namespace TangoApi.Entity
{
    public class AvailableItem
    {
        public string description { get; set; } 
        public string image_url { get; set; } 
        public List<Reward> rewards { get; set; } 
    }
}