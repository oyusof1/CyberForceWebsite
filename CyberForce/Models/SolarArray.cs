using System;
namespace CyberForce.Models
{
    public class SolarArray
    {
        public int ArrayId { get; set; }
        public int SolarStatus { get; set; }
        public int OutputVoltage { get; set; }
        public int OutputCurrent { get; set; }
        public int Temperature { get; set; }
        public int TrackerTilt { get; set; }
        public int AzimuthAngle { get; set; }
    }
}

