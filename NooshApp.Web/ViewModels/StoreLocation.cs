using System;
using System.Collections.Generic;

namespace NooshApp.Web.ViewModels
{
    public class StoreHoursLine
    {
        public string Label { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public bool IsClosure { get; set; } = false;
    }


    public class StoreLocation
    {
        public string Name { get; set; } = string.Empty;
        public string MallName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string WhatsAppNumber { get; set; } = string.Empty;
        public List<StoreHoursLine> HoursSchedule { get; set; } = new();
        public string MapEmbedUrl { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string UberEatsUrl { get; set; } = string.Empty;
        public string MrDUrl { get; set; } = string.Empty;

        public string DirectionsUrl =>
            $"https://www.google.com/maps/dir/?api=1&destination={Uri.EscapeDataString(Address)}";
    }
}
