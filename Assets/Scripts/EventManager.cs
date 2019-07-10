using System;
using System.Collections.Generic;

public enum EVENT { EatFood, AddMovingAnimal, RemoveMovingAnimal, Reproduce }; // ... Other events
public static class EventManager
{
	// Stores the delegates that get called when an event is fired
	private static Dictionary<EVENT, Action<EventArgs>> eventTable
	= new Dictionary<EVENT, Action<EventArgs>>();

	// Adds a delegate to get called for a specific event
	public static void AddHandler(EVENT evnt, Action<EventArgs> action)
	{
		if (!eventTable.ContainsKey(evnt)) eventTable[evnt] = action;
		else eventTable[evnt] += action;
	}

	// Fires the event
	public static void Broadcast(EVENT evnt, EventArgs args)
	{
		if (eventTable[evnt] != null) eventTable[evnt](args);
	}
}