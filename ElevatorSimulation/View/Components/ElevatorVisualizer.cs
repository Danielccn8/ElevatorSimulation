using ElevatorSimulation.Enums;
using ElevatorSimulation.Models;

namespace ElevatorSimulation.View.Components
{
    public static class ElevatorVisualizer
    {
        private static List<string> _logLines = new List<string>();
        public static bool IsEnabled { get; set; } = true;

        public static void DrawElevatorShaft(Elevator elevator, int minFloor, int maxFloor)
        {
            if (!IsEnabled) return;

            if (!Console.IsOutputRedirected)
                Console.Clear();

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            //Clear only for interactive user 
            if (Console.IsOutputRedirected == false)
                Console.Clear();


            Console.WriteLine("============== ELEVATOR MOVEMENT ==============");
            Console.WriteLine($"Direction: {(elevator.CurrentDirection == Direction.Idle ? "-" : (elevator.CurrentDirection == Direction.Up ? "\u2191" : "\u2193"))} | Current Floor: {elevator.CurrentFloor}");
            Console.WriteLine("===============================================\n");

            for (int floor = maxFloor; floor >= minFloor; floor--)
            {
                if (floor == elevator.CurrentFloor)
                    Console.WriteLine($"{floor} | █");
                else
                    Console.WriteLine($"{floor} |");
            }

            Console.WriteLine("\n\n------------- STATUS LOG -------------");
            foreach (var line in _logLines)
            {
                Console.WriteLine(line);
            }
            Console.WriteLine("--------------------------------------");

            Thread.Sleep(600);
        }

        public static void ShowMovementSummary(List<ElevatorRequest> firstRequests, List<ElevatorRequest> remainingRequests, List<ElevatorRequest> allRequest, List<ElevatorRequest> floorPlan, List<int> visitedFloors)
        {
            //if (!IsEnabled) return;

            //if (!Console.IsOutputRedirected)
            //    Console.Clear();

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("\n\n========== ELEVATOR SUMMARY ==========");

            Console.WriteLine("--------------------------------------");

            Console.Write("All Requests: ");
            foreach (var req in allRequest)
            {
                string arrow = req.Direction == Direction.Up ? "\u2191" : "\u2193";
                string type = req.IsExternal ? "External" : "Internal";
                Console.Write($"({req.RequestedFloor} {arrow} {type}) ");
            }
            Console.WriteLine("\n========================================");

            Console.Write("Plan to Follow : ");
            foreach (var req in floorPlan)
            {
                string arrow = req.Direction == Direction.Up ? "\u2191" : "\u2193";
                string type = req.IsExternal ? "External" : "Internal";
                Console.Write($"({req.RequestedFloor} {arrow} {type}) ");
            }
            Console.WriteLine("\n========================================");

            Console.Write("Order followed: ");
            Console.WriteLine($"({string.Join(", ", visitedFloors)})");
            Console.WriteLine("========================================");

            Console.Write("========== MOVEMET SUMMARY =============== \n");
            foreach (var line in _logLines)
            {
                Console.WriteLine(line);
            }
            Console.WriteLine("========================================");

            if (firstRequests.Any())
            {
                Console.Write("Initial Direction Stops: ");
                foreach (var req in firstRequests)
                {
                    string arrow = req.Direction == Direction.Up ? "\u2191" : "\u2193";
                    Console.Write($"({req.RequestedFloor} {arrow}) ");
                }
                Console.WriteLine();
            }

            if (remainingRequests.Any())
            {
                Console.Write("Reversed Direction Stops: ");
                foreach (var req in remainingRequests)
                {
                    string arrow = req.Direction == Direction.Up ? "\u2191" : "\u2193";
                    Console.Write($"({req.RequestedFloor} {arrow}) ");
                }
                Console.WriteLine();
            }

            Console.WriteLine("======================================");

            ExportLogToFile();

            Console.WriteLine("Press any key to continue...");
            //Only if user is interactive
            if (Console.IsOutputRedirected == false)
                Console.ReadKey();

        }

        public static void ExportLogToFile(string filePath = "../../../elevator_log.txt")
        {
            try
            {
                File.WriteAllLines(filePath, _logLines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write log to file: {ex.Message}");
            }
        }


        public static void LogAction(string message)
        {
            _logLines.Add(message);
        }

        public static void ResetLog()
        {
            _logLines.Clear();
        }

    }
}
