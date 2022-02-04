using System;

class Person
{
	public int floorCall { get; }
	public int floorDest { get; }
	public int time { get; }
	public int timeEnd { get; set; }
	public Person(int floorCall, int floorDest, int time)
	{ 
		this.floorCall = floorCall;
		this.floorDest = floorDest;
		this.time = time;
	}
	/* 
	 * Param: Current floor the lift is on.
	 * Return: Boolean as to whether this is the called floor.
	 */
	public bool IsOnFloor(int floor)
    {
		return floor == floorCall;
    }

	/* 
	 * Param: Current floor the lift is on.
	 * Return: Boolean as to whether this is the destination floor.
	 */
	public bool AtDestFloor(int floor)
    {
		return floor == floorDest;
    }
}
