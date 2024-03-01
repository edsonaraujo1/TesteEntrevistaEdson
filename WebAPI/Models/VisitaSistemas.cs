using System;

namespace WebAPI.Models
{
    public class VisitaSistemas
    {
        public int id { get; set; }
        public DateTime data { get; set; }
        public string ip { get; set; }
        public string sistema { get; set; }
    }
}
