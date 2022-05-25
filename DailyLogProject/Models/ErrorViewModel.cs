using System;

namespace DailyLogProject.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public string HeaderTitle { get; set; }

        public string CommentTitle { get; set; }   

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
