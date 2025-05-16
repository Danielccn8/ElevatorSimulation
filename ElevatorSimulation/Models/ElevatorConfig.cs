using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation.Models
{
    public class ElevatorConfig
    {
        public int MinFloor { get; set; } = 1;
        public int MaxFloor { get; set; } = 5;
        public int StartFloor { get; set; } = 3;

    }
}
