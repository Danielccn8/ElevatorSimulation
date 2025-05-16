using ElevatorSimulation.Controllers;
using ElevatorSimulation.Enums;
using ElevatorSimulation.Models;
using ElevatorSimulation.View;

namespace ElevatorSimulation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var elevator = new Elevator();
            var elevatorConfig = new ElevatorConfig();
            var controler = new ElevatorContoller(elevator,elevatorConfig);

            PrintElevatorState(elevator);

            Console.WriteLine("\nElevator Simulation Starting\n");

            //Run the elevator simulation so the user can interact.
            SimulationUI.Run(elevatorConfig);
            Console.WriteLine("All Requests Complete");
        }

        static void PrintElevatorState(Elevator elevator)
        {
            Console.WriteLine("\nElevator State: ");
            Console.WriteLine($"Current Floor: {elevator.CurrentFloor}");
            Console.WriteLine($"Direction: {elevator.CurrentDirection}");
            Console.WriteLine($"Door State: {elevator.DoorState}\n\n");
        }
    }
}