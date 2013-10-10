using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProxomoApp.Models
{
    public class CustomDataModel
    {
        public int ID { get; set; }
        public string PartitionKey { get; set; }
        public string TableName { get; set; }
        public DateTime Timestamp { get; set; }
        public string Transaction { get; set; }
    }
}