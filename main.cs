using System;
using System.Text;
using System.Runtime.CompilerServices;

class Program
{
	static void Main(string[] args)
	{

		// Lift set to begin at floor 5 (default floor).
		Lift lift = new Lift(5);

		var filePath = Directory.GetCurrentDirectory(); ;
		var directory = "C:/Users/benzi/OneDrive/Documents/AVAMAE_lift/";
		var path = Path.Combine(directory, "input_data.csv");
		Console.WriteLine(path);
		var reader = new StreamReader(path);

		// Read first line to ignore headers.		
		string headerLine = reader.ReadLine();
		// Dictionary to contain all users of the lift.
		Dictionary<int, Person> personDict = new Dictionary<int, Person>();
		int key = 0;

		// Read each line of the input csv file and
		// add the person to the dictionary.
		while (!reader.EndOfStream)
        {
			var line = reader.ReadLine();
			var values = line.Split(',');
			key = int.Parse(values[0]);
			int floorCall = int.Parse(values[1]);
			int floorDest = int.Parse(values[2]);
			int time = int.Parse(values[3]);

			Person newPerson = new Person(floorCall, floorDest, time);
			personDict.Add(key, newPerson);
        }
		reader.Close();

		// Initiate values for the algorithm.
		int runtime = 0;
		int idCount = 1;
		int idTotal = personDict.Count;
		List<int> waitTimes = new List<int>();

		// Start main loop of algorithm execution.
		Console.WriteLine("Starting lift operations...\n");
		while (idCount <= idTotal | lift.GetFloorQueue().Count != 0)
        {
			runtime++;
			if (idCount <= idTotal)
			{
				// Find each person who has called the lift at this time
				while (personDict[idCount].time == runtime)
				{
					lift.AddToFloorQueue(personDict[idCount].floorCall);
					lift.AddToPersonQueue(idCount);

					idCount++;
					if (idCount > idTotal)
					{
						break;
					}
				}
			}

			// Perform lift actions based on the lifts status.
			switch (lift.status)
            {
				case Status.Idle:				
					if (lift.CheckFloor(lift.floor))
                    {
						// Remove people inside the lift that are at their
						// destination floor.
						foreach (var person in lift.GetPeopleInside().ToList())
                        {
							if (personDict[person].AtDestFloor(lift.floor))
							{
								personDict[person].timeEnd = runtime;
								var waitTime = personDict[person].timeEnd - personDict[person].time;
								lift.RemoveFromPeopleInside(person);
								waitTimes.Add(waitTime);
							}
						}


						// Add people to the lift that are at their called floor.
						int? neglectedFloor = null;
						foreach (var person in lift.GetPersonQueue().ToList())
                        {
							if (personDict[person].IsOnFloor(lift.floor))
                            {
								if (!lift.AtMaxCapacity())
                                {
									lift.AddPersonToLift(person);
									lift.AddToFloorQueue(personDict[person].floorDest);
									lift.RemoveFromPersonQueue(person);
								}
                                else
                                {
									neglectedFloor=lift.floor;
                                }
								
                            }
                        }
						
						// Re-add the 'neglected floor' to the queue
						// if the lift was at max capacity.
						lift.RemoveFromFloorQueue(lift.floor);
						if (!(neglectedFloor is null))
                        {
							lift.AddToFloorQueue((int)neglectedFloor);
						}
						


					}
					if (!lift.QueueIsEmpty())
                    {
						lift.targetFloor = lift.GetFrontOfQueue();
						lift.StartMove(runtime);
						
					}
                    else
                    {
						lift.targetFloor = lift.floor;
						lift.StartMove(runtime);
						
					}
						           
					break;
				case Status.Up:
					lift.Move();
					break;
				case Status.Down:
					lift.Move();
					break;
				
            }

		}

		// Algorithm has finished, write necessary info to console.
		Console.WriteLine();
		Console.WriteLine("Lift has completed all it's operations.");
		Console.WriteLine("This was completed in {0} seconds.", runtime);
		Console.WriteLine("The average journey time was: {0}s", waitTimes.Average());
		Console.WriteLine("The longest journey time was: {0}s", waitTimes.Max());
		Console.WriteLine("The shortest journey time was: {0}s", waitTimes.Min());

		// Add last line of output once lift is empty.
		var output = lift.GetOutput();
		var newLine = string.Format("{0},{1},{2},{3}", runtime, "", lift.floor, "");
		output.AppendLine(newLine);

		// Send output to csv.
		var outputPath = Path.Combine(directory, "output_data.csv");
		File.WriteAllText(outputPath, output.ToString());

		Console.WriteLine("\nPress any key to exit.");
		Console.ReadKey();


	}

	
}
