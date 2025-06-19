using System;
using System.Collections.Generic;
using System.Linq;

namespace Deliverr.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public List<Product> Products { get; set; }
    public List<DeliveryState> DeliveryStates { get; set; }

    public DeliveryState? LatestDeliveryState =>
        DeliveryStates?.OrderByDescending(ds => ds.DateTime).FirstOrDefault();

    public string DeliveryStatus
    {
        get
        {
            if (DeliveryStates == null || !DeliveryStates.Any())
                return "No Status";

            var latestState = DeliveryStates
                .OrderByDescending(ds => ds.DateTime)
                .FirstOrDefault();

            return latestState?.State switch
            {
                1 => "Pending",
                2 => "Shipping",
                3 => "Delivered",
                4 => "Cancelled",
                _ => "Onbekend"
            };
        }
    }

}
