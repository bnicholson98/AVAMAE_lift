using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * Class to control movements and data operations of the lift.
 */
class Lift
{
    private const int Max_Capacity = 8;
    public int floor { get; set; }
    public int targetFloor { get; set; }
 
    private Queue<int> floorQueue = new Queue<int>();
    private List<int> personQueue = new List<int>();
    private List<int> peopleInside = new List<int>();
    public Status status { get; set; }
    public int moveTime { get; set; }
    private StringBuilder output = new StringBuilder();


    /*
     * Construct the lift with intial properties.
     */
    public Lift(int floorStart)
    {
        this.floor = floorStart;
        this.status = Status.Idle;
        this.output.AppendLine("Time, People in lift, Current floor, Floor queue");
        this.output.AppendLine("1, , 5, ");
    }
 
    public void AddToFloorQueue(int floor)
    {
        if (!this.floorQueue.Contains(floor)){
            this.floorQueue.Enqueue(floor);
        }
        
    }

    public Queue<int> GetFloorQueue()
    {
        return floorQueue;
    }

    public List<int> GetPeopleInside()
    {
        return peopleInside;
    }

    public void AddToPersonQueue(int personId)
    {
        this.personQueue.Add(personId);
    }

    public List<int> GetPersonQueue()
    {
        return personQueue;
    }

    public bool QueueIsEmpty()
    {
        return (this.floorQueue.Count == 0);
    }
    
    public int GetFrontOfQueue()
    {
        return ((int)this.floorQueue.Peek());
    }

    /*
     * Param: floor to be checked.
     * Return: boolean of whether this floor is
     *         in the floor queue.
     */
    public bool CheckFloor(int currentFloor)
    {
        return (this.floorQueue.Contains(currentFloor));
    }

    public void RemoveFromFloorQueue(int currentFloor)
    {
        this.floorQueue = new Queue<int>(this.floorQueue.Where(x => x != currentFloor)); 
    }

    public void Move()
    {
        this.moveTime++;
        // Once 10 seconds have passed the lift has moved floors.
        if (this.moveTime >= 10)
        {
            if (this.status == Status.Up)
            {
                this.floor++;
                this.status = Status.Idle;
            }
            else if (this.status == Status.Down)
            {
                this.floor--;
                this.status = Status.Idle;
            }
            
        }
        
        
    }

    /*
     * Method to initiate the start of the lifts movement.
     * 
     * Param: time that the moved started, used for the output file.
     */
    public void StartMove(int runtime)
    {
        this.moveTime = 1;

        // Only update if we are moving to a new floor.
        if (this.floor != this.targetFloor)
        {
            var strPeopleInside = string.Join(' ', peopleInside);
            var strFloorQueue = string.Join(' ', floorQueue);

            var newLine = string.Format("{0},{1},{2},{3}", runtime, strPeopleInside, floor, strFloorQueue);
            output.AppendLine(newLine);

            if (this.floor < this.targetFloor)
            {
                this.status = Status.Up;
            }
            else if (this.floor > this.targetFloor)
            {
                this.status = Status.Down;
            }
        }
        
    }

    public void AddPersonToLift(int person)
    {
        this.peopleInside.Add(person);
    }

    public void RemoveFromPersonQueue(int person)
    {
        this.personQueue.Remove(person);
    }

    public void RemoveFromPeopleInside(int person)
    {
        this.peopleInside.Remove(person);
    }

    public bool AtMaxCapacity()
    {
        return (this.peopleInside.Count == Max_Capacity);
    }

    public StringBuilder GetOutput()
    {
        return output;
    }
}

public enum Status
{
    Down,
    Idle,
    Up
}