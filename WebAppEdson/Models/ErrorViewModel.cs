using System;

namespace WebAppEdson.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private static string m_RError = "";
        public static string RError
        {
            get { return m_RError; }
            set { m_RError = value; }
        }
    }
}