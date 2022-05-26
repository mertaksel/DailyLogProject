using System;

namespace DailyLogProject.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public string HeaderTitle { get; set; }

        public string DescriptionTitle { get; set; }   
        public string ButtonKayit { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
