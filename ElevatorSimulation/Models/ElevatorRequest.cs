using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulation.Enums;

namespace ElevatorSimulation.Models
{
    public class ElevatorRequest
    {
        public int RequestedFloor { get; }
        public Direction Direction { get; }
        public bool IsExternal { get; }
        public DateTime RequestDate { get; }

        public ElevatorRequest(int requestedFloor, Direction direction, bool isExternal, DateTime ? dateRequest = null)
        {
            RequestedFloor = requestedFloor;
            Direction = direction;
            IsExternal = isExternal;
            RequestDate = dateRequest ?? DateTime.Now;
        }

        public override string ToString()
        {
            var type = IsExternal ? "External" : "Internal";
            return $"{type} request to floor {RequestedFloor} ({Direction})";
        }
    }
   
}
