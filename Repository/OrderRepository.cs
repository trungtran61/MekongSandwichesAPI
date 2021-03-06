using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using MekongSandwichesAPI.Models;

namespace MekongSandwichesAPI.Repository
{
    public class OrderRepository : IDisposable
    {
        private AppSettings appSettings;
        private string msConnectionString;
        public OrderRepository(AppSettings _appSettings)
        {
            appSettings = _appSettings;
            msConnectionString = appSettings.MSConnectionString;
        }

        public int AddOrder(Order order)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(appSettings.MSConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("AddOrder", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CustomerName", SqlDbType.VarChar, 50).Value = order.name;
                        cmd.Parameters.Add("@Phone", SqlDbType.VarChar, 20).Value = order.phone;
                        cmd.Parameters.Add("@PickupTime", SqlDbType.VarChar, 8).Value = order.pickUpTime.Substring(0,8);
                        cmd.Parameters.Add("@PickupDate", SqlDbType.Date).Value = order.pickUpDate;
                        cmd.Parameters.Add("@OrderItems", SqlDbType.VarChar, 0).Value = JsonConvert.SerializeObject(order.orderItems);
                        con.Open();
                        order.id = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    con.Close();
                }
            }
            catch
            {
                throw;
            }

            return order.id;
        }

        public List<Order> GetOrders(OrderStatus OrderStatus)
        {
            List<Order> Orders = new List<Order>();

            using (SqlConnection con = new SqlConnection(appSettings.MSConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetOrders", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@OrderStatus", SqlDbType.VarChar, 20).Value = OrderStatus.ToString();
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            List<OrderItem> items = JsonConvert.DeserializeObject<List<OrderItem>>(reader["OrderItems"].ToString());
                            
                            Orders.Add(new Order
                            {
                                id = Convert.ToInt32(reader["ID"].ToString()),
                                name = reader["CustomerName"].ToString(),
                                phone = reader["Phone"].ToString(),
                                pickUpDate = Convert.ToDateTime(reader["PickupDate"].ToString()),
                                pickUpTime = reader["PickupTime"].ToString(),
                                orderItems = items.ToArray()
                            });
                        }
                    }
                }
            }
            return Orders;
        }

         public void UpdateOrderStatus(int orderId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(appSettings.MSConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("UpdateOrderStatus", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@OrderId", SqlDbType.Int).Value = orderId;                       
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
            }
            catch
            {
                throw;
            }            
        }
        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}