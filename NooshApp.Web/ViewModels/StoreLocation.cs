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
}