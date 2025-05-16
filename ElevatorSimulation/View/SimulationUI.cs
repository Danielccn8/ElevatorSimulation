using ElevatorSimulation.Controllers;
using ElevatorSimulation.Models;
using ElevatorSimulation.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation.View
{
    public static class SimulationUI
    {
        private static List<string> _logLines = new List<string>();

        public static void Run(ElevatorConfig config)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            bool running = true;
            while (running)
            {

                Console.Clear();
                Console.WriteLine($"Current Configuration:  Starting Floor: {config.StartFloor} | Max Floor: {config.MaxFloor} \n");
                Console.WriteLine("=== Elevator Simulation ===");
                Console.WriteLine("1. Change Elevator Configurtation");
                Console.WriteLine("2. Start Elevator Simulation");
                Console.WriteLine("3. Exit");

                Console.Write("Select an option: ");
                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        OptionMenu(config);
                        break;
                    case "2":
                        Console.WriteLine("Starting Elevator Simulation...");
                        StartSimulation(config);
                        break;
                    case "3":
                        Console.WriteLine("Exiting...");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        public static void StartSimulation(ElevatorConfig config)
        {
            var elevator = new Elevator(config.StartFloor, config.MaxFloor);
            var controller = new ElevatorContoller(elevator, config);
            bool simulationRunning = true;

            while (simulationRunning)
            {
                Console.Clear();
                Console.WriteLine("============== ELEVATOR STATUS ==============");
                Console.WriteLine($"Current Floor: {elevator.CurrentFloor} | Direction: {(elevator.CurrentDirection == Direction.Idle? "-" : (elevator.CurrentDirection == Direction.Up? "\u2191" : "\u2193"))} | Door: {elevator.DoorState}");
                Console.WriteLine("==================================================");

                Console.WriteLine("\n---------- Pending Requests ----------");
                var requests = controller.GetPendingRequests();
                if (requests.Any())
                {
                    foreach (var request in requests)
                    {
                        string directionArrow = request.Direction == Direction.Up ? "\u2191" : "\u2193";
                        string requestType = request.IsExternal ? "External" : "Internal";
                        Console.WriteLine($"Floor: {request.RequestedFloor} | Direction: {directionArrow} | Type: {requestType}");
                    }
                }
                else
                {
                    Console.WriteLine("No pending requests.");
                }

                Console.WriteLine("\n1. Call Elevator (External)");
                Console.WriteLine("2. Select Destination (Internal)");
                Console.WriteLine("3. Run Elevator");
                Console.WriteLine("4. Exit Simulation");
                Console.Write("\nChoose an option: ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1": //Call the elevator from an external floor
                        HandleExternalRequest(controller, elevator, config);
                        break;
                    case "2": //Call the elevator from an internal floor
                        HandleInternalRequest(controller, elevator, config);
                        break;
                    case "3": // Run the elevator simulation
                        controller.ProcessRequests();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    case "4": //Exit simulation
                        simulationRunning = false;
                        Console.WriteLine("Exiting...");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }
        public static void HandleInternalRequest(ElevatorContoller controller, Elevator elevator, ElevatorConfig config)
        {
            int destination = AskForNumber($"Enter the floor you wanna go to ({config.MinFloor} - {config.MaxFloor}) : ", config.MinFloor, config.MaxFloor);
            
            Direction direction = destination > elevator.CurrentFloor ? Direction.Up : Direction.Down;
            var request = new ElevatorRequest(destination, direction, false);
            controller.AddRequest(request);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        public static void HandleExternalRequest(ElevatorContoller controller, Elevator elevator, ElevatorConfig config)
        {
            int floor = AskForNumber($"Enter the floor to are on {config.MinFloor} - {config.MaxFloor}: ", config.MinFloor, config.MaxFloor);
            Direction direction;
            if(floor == config.MinFloor)
            {
                Console.WriteLine("You are at the ground floor, you can only go up. Direction set to UP (\u2191).\n");
                direction = Direction.Up;
            }
            else if (floor == config.MaxFloor)
            {
                Console.WriteLine("You are at the top floor, you can only go down. Direction set to DOWN (\u2193).\n");
                direction = Direction.Down;
            }
            else
            {
                Console.Write("Select direction: (1) UP \u2191, (2) DOWN \u2193: ");
                var input = "0";
                while (true)
                {
                     input = Console.ReadLine();
                    if (input == "1" || input == "2")
                        break;
                    Console.Write("Invalid input.\n Please select (1) UP \u2191 or (2) DOWN \u2193: ");
                }
                direction = input == "2" ? Direction.Down : Direction.Up;
            }

            var request = new ElevatorRequest(floor, direction,  true);
            controller.AddRequest(request);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
       

        public static void OptionMenu(ElevatorConfig _elevatorConfig)
        {
            Console.Clear();
            Console.WriteLine("=== Options Menu ===");
            _elevatorConfig.MaxFloor = AskForNumber("Enter the number of floors between 2 and 99 = ",2,99);
            _elevatorConfig.StartFloor = AskForNumber($"Select the elevator starting potition {_elevatorConfig.MinFloor} and {_elevatorConfig.MaxFloor} = ", _elevatorConfig.MinFloor, _elevatorConfig.MaxFloor);
        }
       
        static int AskForNumber(string prompt, int min = int.MinValue, int max = int.MaxValue)
        {
            int result;
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();

                if (int.TryParse(input, out result) && result >= min && result <= max)
                    return result;

                Console.WriteLine($"Please enter a valid number between {min} and {max}.");
            }
        }

    }
}
