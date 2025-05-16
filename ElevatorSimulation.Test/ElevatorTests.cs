using ElevatorSimulation.Controllers;
using ElevatorSimulation.Enums;
using ElevatorSimulation.Models;
using ElevatorSimulation.View.Components;

namespace ElevatorSimulation.Test;

public class ElevatorTests
{
    public ElevatorTests()
    {
        ElevatorVisualizer.IsEnabled = false;
    }

    [Fact]
    public void Base_case_tests()
    {
        var elevator = new Elevator(startFloor: 3, maxFloor: 5);
        var controller = new ElevatorContoller(elevator, new ElevatorConfig
        {
            StartFloor = 3,
            MaxFloor = 5
        });

        var baseTime = DateTime.Now;

        controller.AddRequest(new ElevatorRequest(1, Direction.Up, true, baseTime));
        controller.AddRequest(new ElevatorRequest(2, Direction.Down, true, baseTime.AddSeconds(1)));
        controller.AddRequest(new ElevatorRequest(3, Direction.Up, true, baseTime.AddSeconds(2)));
        controller.AddRequest(new ElevatorRequest(4, Direction.Up, true, baseTime.AddSeconds(3)));
        controller.AddRequest(new ElevatorRequest(5, Direction.Down, true, baseTime.AddSeconds(4)));

        controller.ProcessRequests();

        Assert.Equal(5, elevator.CurrentFloor);
        Assert.Equal(DoorState.Closed, elevator.DoorState);
        Console.WriteLine("\n\nBase case test complete\n\n");
    }

    [Fact(Skip = "Testing")]
    public void Single_Request_To_Lower_Floor()
    {
        var elevator = new Elevator(startFloor: 5, maxFloor: 5);
        var controller = new ElevatorContoller(elevator, new ElevatorConfig
        {
            StartFloor = 5,
            MaxFloor = 5
        });

        var time = DateTime.Now;
        controller.AddRequest(new ElevatorRequest(2, Direction.Down, true, time));

        controller.ProcessRequests();

        Assert.Equal(2, elevator.CurrentFloor);
        Assert.Equal(DoorState.Closed, elevator.DoorState);
        Console.WriteLine("\n\nSingle request down test complete\n\n");
    }

    [Fact(Skip = "Testing")]
    public void Multiple_Internal_Requests_Same_Direction()
    {
        var elevator = new Elevator(startFloor: 1, maxFloor: 5);
        var controller = new ElevatorContoller(elevator, new ElevatorConfig
        {
            StartFloor = 1,
            MaxFloor = 5
        });

        var time = DateTime.Now;
        controller.AddRequest(new ElevatorRequest(2, Direction.Up, false, time));
        controller.AddRequest(new ElevatorRequest(3, Direction.Up, false, time.AddSeconds(1)));
        controller.AddRequest(new ElevatorRequest(4, Direction.Up, false, time.AddSeconds(2)));

        controller.ProcessRequests();

        Assert.Equal(4, elevator.CurrentFloor);
        Assert.Equal(DoorState.Closed, elevator.DoorState);
        Console.WriteLine("\n\nMultiple internal up requests test complete\n\n");
    }

    [Fact(Skip = "Testing")]
    public void Idle_If_No_Requests()
    {
        var elevator = new Elevator(startFloor: 3, maxFloor: 5);
        var controller = new ElevatorContoller(elevator, new ElevatorConfig
        {
            StartFloor = 3,
            MaxFloor = 5
        });

        controller.ProcessRequests();

        Assert.Equal(3, elevator.CurrentFloor);
        Assert.Equal(DoorState.Closed, elevator.DoorState);
        Assert.Equal(Direction.Idle, elevator.CurrentDirection);
        Console.WriteLine("\n\nIdle test complete\n\n");
    }

    [Fact(Skip = "Testing")]
    public void Mixed_Direction_Requests()
    {
        var elevator = new Elevator(startFloor: 3, maxFloor: 5);
        var controller = new ElevatorContoller(elevator, new ElevatorConfig
        {
            StartFloor = 3,
            MaxFloor = 5
        });

        var time = DateTime.Now;
        controller.AddRequest(new ElevatorRequest(5, Direction.Up, true, time));        // First request is UP
        controller.AddRequest(new ElevatorRequest(1, Direction.Down, true, time.AddSeconds(1)));  // Then DOWN

        controller.ProcessRequests();

        Assert.Equal(1, elevator.CurrentFloor);
        Assert.Equal(DoorState.Closed, elevator.DoorState);
        Console.WriteLine("\n\nMixed direction requests test complete\n\n");
    }
}
