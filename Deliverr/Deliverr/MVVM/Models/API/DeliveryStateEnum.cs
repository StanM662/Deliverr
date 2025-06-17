using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deliverr.Models;
namespace Deliverr.Models;

public partial class DeliveryStateEnum
{
    public enum DeliveryStates
    {
        Pending,
        Shipped,
        Delivered,
        Cancelled
    }

}
