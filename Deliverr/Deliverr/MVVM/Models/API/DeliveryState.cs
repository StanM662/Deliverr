using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deliverr.Models;
namespace Deliverr.Models;

public partial class DeliveryState
{
    public int id;
    public DeliveryStateEnum state;
    public string dateTime;
    public int orderId;
    public Order order;
    public int deliveryServiceId;
    public DeliveryService deliveryService;

}
