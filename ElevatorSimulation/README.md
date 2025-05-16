# Elevator Simulation for Global Payments  
**Author:** Daniel Cartagena Nunez
**Date:** 5/16/2025  

---

## Overview

This is a simulation of an elevator system for a building with 5 floors (configurable).  
The simulation was built under the following **simplified real-world assumptions**:

- The elevator processes all current requests in a single logical run.
- It cannot go up and down multiple times per run — it chooses an initial direction (based on the first request), serves all in that direction, then reverses if needed.
- Requests are handled chronologically and based on distance from the current floor.

The system automatically sets min floor = 1 and max floor = 5. But allows to change the max floor on a scale from 2 - 99, for adaptability.

---

##  Interactive Mode

The system allows users to test **interactive features**:

- You can **call the elevator from outside** (from any floor).
- You can **request a floor from inside** the elevator.
- The elevator moves logically, floor by floor, with visual feedback (can be disabled for test runs).
- Configuration options (number of floors, start floor) are available.

Also there's an **elevator animation** that shows the elevator moving up and down the building.

To run interactively:
**dotnet run --project ElevatorSimulation**


## Test mode
The system uses xUnit for unit testing. Using the project `ElevatorSimulation.Tests`, you can run the tests with the command:
**dotnet test**
This will run all the tests in the `ElevatorSimulation.Tests` project. And allows to add adittionall test in the 'ElevatorTests' file.

Start by setting up the elevator and the controller:

'''
    var elevator = new Elevator(startFloor: 3, maxFloor: 5);
    var controller = new ElevatorContoller(elevator, new ElevatorConfig
    {
        StartFloor = 3,
        MaxFloor = 5
    }); 
'''

These are the default values:

StartFloor = 3 -> the elevator begins at floor 3.

MaxFloor = 5 -> the building has 5 floors (you can increase this up to 99).

The minimum floor is always 1, set automatically by the system.

Then add the requests to the elevator:
'''
    controller.AddRequest(new ElevatorRequest(5, Direction.Down, true, baseTime.AddSeconds(4)));
'''

# Each parameter means:

 5 The requested floor. Where the user wants to go.

 Direction.Down → The direction the user intends to go.

Use Direction.Up if the user wants to go up.

 Use Direction.Down if they want to go down.

 true → IsExternal. Use:

 true when the request comes from outside the elevator (hallway call).

 false when the request comes from inside the elevator (selecting a floor).

 baseTime.AddSeconds(4) → The time the request was made.

 baseTime is set to DateTime.Now.

 AddSeconds(4) means this request occurred 4 seconds after the first one.

Finally Tu run the Simulation use 
'''
    controller.RunSimulation();
'''

And deppending on the run mode, you will see a log file created on the base folder of either ElevatorSimulation or ElevatorSimulation.Tests.






