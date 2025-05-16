using ElevatorSimulation.Models;
using ElevatorSimulation.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulation.View;
using ElevatorSimulation.View.Components;

namespace ElevatorSimulation.Controllers
{
    public class ElevatorContoller
    {

        private readonly Elevator _elevator;
        private List<ElevatorRequest> _requestList;
        private List<ElevatorRequest> _requestListCopy;
        private readonly ElevatorConfig _config;
        private List<ElevatorRequest> _floorPlan = new List<ElevatorRequest>();
        private List<int> _visitedFloors = new List<int>();

        public ElevatorContoller(Elevator elevator, ElevatorConfig config)
        {
            _elevator = elevator;
            _config = config;
            _requestList = new List<ElevatorRequest>();
            _requestListCopy = new List<ElevatorRequest>();
        }
        public void AddRequest(ElevatorRequest request)
        {
            if (request.RequestedFloor < 1 || request.RequestedFloor > _config.MaxFloor)
            {
                string messages = $"Invalid floor {request.RequestedFloor}. Request ignored.";
                ElevatorVisualizer.LogAction(messages);
                Console.WriteLine(messages);
                return;
            }

            //Check for duplicates
            bool isDuplicate = _requestList.Any(x => 
                x.RequestedFloor == request.RequestedFloor &&
                x.Direction == request.Direction);

            if (isDuplicate)
            {
                string messages = $"Duplicate request for flor {request.RequestedFloor}. Request Ignored";
                ElevatorVisualizer.LogAction(messages);
                Console.WriteLine(messages);
                return;
            }

            _requestList.Add(request);
            //_FloorList.Add((Int32)request.RequestedFloor);

            //string message = $"Added: {request}";
            //ElevatorVisualizer.LogAction(message);
            //Console.WriteLine(message);

        }

        public void ProcessRequests()
        {
            //Order requests by time and distance
            _requestList = _requestList
                .OrderBy(r => r.RequestDate)
                .ThenByDescending(r => Math.Abs(r.RequestedFloor - _elevator.CurrentFloor)) // Farther first
                .ToList();

            _requestListCopy = new List<ElevatorRequest>(_requestList);

            if (_requestList.Count == 0)
            {
                ElevatorVisualizer.LogAction("No pending requests.");
                _elevator.SetIdle();
                return;
            }

            //Determine initial direction based on first request.
            var firstRequest = _requestList.First();
            Direction direction = firstRequest.RequestedFloor > _elevator.CurrentFloor
                ? Direction.Up 
                : Direction.Down;

            _elevator.CurrentDirection = direction;

            //Process requests in current direction
            List<ElevatorRequest> firstRequests;


            ElevatorVisualizer.LogAction($"The elevator has decided to go {_elevator.CurrentDirection} first, since the first request is from {_requestList.First()}\n");
            //Console.WriteLine($"The elevator has decided to go {_elevator.CurrentDirection} first, since the first request is from {_requestList.First()}\n");

            //Sort base on Direction 
            if (direction == Direction.Up)
            {
                firstRequests = _requestList
                    .Where(r => (r.RequestedFloor >= _elevator.CurrentFloor && 
                        r.Direction == _elevator.CurrentDirection) ||
                        (r.RequestedFloor >= _elevator.CurrentFloor &&
                        r.RequestedFloor == _config.MaxFloor))
                    .OrderBy(r => r.RequestedFloor)
                    .ToList();
            }
            else
            {
                firstRequests = _requestList
                    .Where(r => (r.RequestedFloor <= _elevator.CurrentFloor &&
                        r.Direction == _elevator.CurrentDirection) || 
                        (r.RequestedFloor <= _elevator.CurrentFloor &&
                        r.RequestedFloor == _config.MinFloor))
                    .OrderByDescending(r => r.RequestedFloor)
                    .ToList();
            }
            //ElevatorVisualizer.LogAction($"The list of floors the elevator will go is the next: {string.Join(" ", _requestList.Select(x => x.RequestedFloor))}");
            //Console.WriteLine($"The list of floors the elevator will go is the next: {string.Join(" ", _requestList.Select(x => x.RequestedFloor))}");
            ElevatorVisualizer.LogAction($"First the elevator will go {_elevator.CurrentDirection}");
            //Console.WriteLine($"First the elevator will go {_elevator.CurrentDirection} stopping on these floors {string.Join(", ",firstRequests.Select(x => x.RequestedFloor))}");

            //Console.WriteLine("Press any key to continue.");
            //Console.ReadKey();

            foreach (var request in firstRequests)
            {
                //Console.WriteLine($"{request}");
                ProcessSingleRequest(request);
                _floorPlan.Add(request);
                _requestList.Remove(request);
            }

            //Reverse direction and process remaining requests
            var remainingRequests = _requestList.ToList();
            //ElevatorVisualizer.LogAction($"Next the elevator will go {_elevator.CurrentDirection}to the remaining floors, stopping on these floors {string.Join(", ", remainingRequests.Select(x => x.RequestedFloor))}");
            //Console.WriteLine($"Next the elevator will go {_elevator.CurrentDirection}to the remaining floors, stopping on these floors {string.Join(", ", remainingRequests.Select(x => x.RequestedFloor))}");

            if (remainingRequests.Count > 0)
            { 
                _elevator.CurrentDirection = _elevator.CurrentDirection == Direction.Up
                    ? Direction.Down 
                    : Direction.Up;

                List<ElevatorRequest> secondaryBatch = _elevator.CurrentDirection == Direction.Up
                    ? remainingRequests.OrderBy(r => r.RequestedFloor).ToList()
                    : remainingRequests.OrderByDescending(r => r.RequestedFloor).ToList();

                foreach (var request in secondaryBatch)
                {
                    ProcessSingleRequest(request);
                    _floorPlan.Add(request);
                    _requestList.Remove(request);
                }
            }
            _elevator.SetIdle();
            _visitedFloors.Add(_elevator.CurrentFloor);


            ElevatorVisualizer.ShowMovementSummary(firstRequests, remainingRequests, _requestListCopy, _floorPlan, _visitedFloors);

            _floorPlan.Clear();
            _visitedFloors.Clear();
            _requestList.Clear();
            _requestListCopy.Clear();
            //ElevatorVisualizer.ResetLog();

        }

        private void ProcessSingleRequest(ElevatorRequest request)
        {

            //Console.WriteLine($"Processing: {request} \n");

            while (_elevator.CurrentFloor < request.RequestedFloor)
            {
                _visitedFloors.Add(_elevator.CurrentFloor);
                _elevator.MoveUp();
                ElevatorVisualizer.LogAction($"Elevator moved {_elevator.CurrentDirection} to floor {_elevator.CurrentFloor}");
                ElevatorVisualizer.DrawElevatorShaft(_elevator, _config.MinFloor, _config.MaxFloor);
            }

            while (_elevator.CurrentFloor > request.RequestedFloor)
            {
                _visitedFloors.Add(_elevator.CurrentFloor);
                _elevator.MoveDown();
                ElevatorVisualizer.LogAction($"Elevator moved {_elevator.CurrentDirection} to floor {_elevator.CurrentFloor}");
                ElevatorVisualizer.DrawElevatorShaft(_elevator, _config.MinFloor, _config.MaxFloor);
            }
            DoorMovement();
        }

        private void DoorMovement()
        {
            _elevator.OpenDoor();
            Thread.Sleep(1000);
            _elevator.CloseDoor();
            ElevatorVisualizer.LogAction($"Doors opened");
            ElevatorVisualizer.LogAction($"Doors closed");

        }

        public List<ElevatorRequest> GetPendingRequests()
        {
            return _requestList;
        }

    }
}
