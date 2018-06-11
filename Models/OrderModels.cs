using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace MekongSandwichesAPI.Models
{
    public class Order
    {
        public int id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public DateTime pickUpTime { get; set; }
        public DateTime pickUpDate { get; set; }
        public OrderItem[] orderItems { get; set; }
    }

    public class OrderItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public int qty { get; set; }
        public decimal price { get; set; }
        public string instructions { get; set; }
    }

     public class DBResponse
    {
        public int ReturnCode { get; set; }
        public int RecordsAffected { get; set; }
        public string Message { get; set; }
    }

    public enum OrderStatus
    {
        All,
        New,
        SentToKitchen,
        Processed

    }
}