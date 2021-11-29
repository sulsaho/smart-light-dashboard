using System;
using System.ComponentModel.DataAnnotations;

namespace LightWebAPI.Models
{
    public class LightState
    {
        [Key]
        public int Id { get; set; }
        
        public bool IsOn { get; set; }
        
        //Deprecated
        //public string Color { get; set; }
        
        public string Brightness { get; set; }
        
        public string TimeStamp { get; set; }
        
        public bool EnableSunriseSunSetFeature { get; set; }
    }
}