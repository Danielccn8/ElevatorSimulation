using ElevatorSimulation.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation.Models
{
    internal class Floor
    {
        public int FloorNumber { get; set; }
        public bool UpButtonPressed {  get; private set; }
        public bool DownButtonPressed { get; private set; }

        public Floor(int floorNumber)
        {
            FloorNumber = floorNumber;
            UpButtonPressed = false;
            DownButtonPressed = false;
        }

        public void PressButton(Direction direction)
        {
            if(direction == Direction.Up)
            {
                UpButtonPressed = true;
                Console.WriteLine($"Up button pressed on floor {FloorNumber}");
            }
        }
        public void ResetButton(Direction direction)
        {
            if (direction == Direction.Up)
                UpButtonPressed = false;
            else if (direction == Direction.Down)
                DownButtonPressed = false;
        }
    }
}
