using ElevatorSimulation.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation.Models
{
    public class Elevator
    {
        public int CurrentFloor { get; set; }
        public Direction CurrentDirection { get; set; }
        public DoorState DoorState { get; private set; }

        public Elevator(int startFloor = 3, int maxFloor = 5)
        {

            CurrentFloor = startFloor;
            CurrentDirection = Direction.Idle;
            DoorState = DoorState.Closed;
        }
        public void OpenDoor()
        {
            DoorState = DoorState.Open;
            //Console.WriteLine("Door's open.");
        }
        public void CloseDoor()
        {
            DoorState = DoorState.Closed;
            //Console.WriteLine("Door's closed");
        }
        public void MoveUp()
        {
            if (CurrentFloor < 5)
            {
                CurrentFloor++;
                CurrentDirection = Direction.Up;
                //Console.WriteLine($"Elevator moved up to floor: {CurrentFloor}");
            }
            else
            {
                //Console.WriteLine("Already at the top floor");
            }
        }
        public void MoveDown()
        {
            if (CurrentFloor > 1)
            {
                CurrentFloor--;
                CurrentDirection = Direction.Down;
                //Console.WriteLine($"Elevator moved down to floor {CurrentFloor}");
            }
            else
            {
                //Console.WriteLine($"Already at the ground floor, floor: {CurrentFloor}");
            }
        }

        public void SetIdle()
        {
            CurrentDirection = Direction.Idle;
        }

    }
}
