using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EventName
{
	StartFade,
	FadeDone
}

public class EventManager : MonoBehaviour 
{
	public class IntEvent : UnityEvent<int>{}
	public class Int2Event : UnityEvent<int, int>{}
	public class TransformEvent : UnityEvent<Transform>{}

	private Dictionary <EventName, UnityEvent> events;
	private Dictionary <EventName, IntEvent> intEvents;
	private Dictionary <EventName, Int2Event> int2Events;
	private Dictionary <EventName, TransformEvent> transformEvents;


	void Awake()
	{
		events = new Dictionary<EventName, UnityEvent>(new EnumComparer<EventName>());
		intEvents = new Dictionary<EventName, IntEvent>(new EnumComparer<EventName>());
		int2Events = new Dictionary<EventName, Int2Event>(new EnumComparer<EventName>());
		transformEvents = new Dictionary<EventName, TransformEvent>(new EnumComparer<EventName>());
	}

	#region Regular Unity Events

	/// <summary>
	/// Adds a listener to an event named eventName. If that event does not exist yet, it will be created.
	/// Warning: For every StartListening call, you MUST call StopListening to ensure there are no memory leaks.
	/// StartListening should be called on OnEnable and StopListening should be called on OnDisable.
	/// </summary>
	/// <param name="eventName">The name of the event to listen for.</param>
	/// <param name="listener">The UnityAction which will be called when the event is invoked.</param>
	public void StartListening(EventName eventName, UnityAction listener)
	{
		UnityEvent thisEvent = null;
		if (events.TryGetValue(eventName, out thisEvent))
		{
			thisEvent.AddListener(listener);
		}
		else
		{
			thisEvent = new UnityEvent();
			thisEvent.AddListener(listener);
			events.Add(eventName, thisEvent);
		}
	}
		
	public void StartListening(EventName eventName, UnityAction<int> listener)
	{
		IntEvent thisEvent = null;
		if (intEvents.TryGetValue(eventName, out thisEvent))
		{
			thisEvent.AddListener(listener);
		}
		else
		{
			thisEvent = new IntEvent();
			thisEvent.AddListener(listener);
			intEvents.Add(eventName, thisEvent);
		}
	}

	public void StopListening(EventName eventName, UnityAction listener)
	{
		UnityEvent thisEvent = null;
		if (events.TryGetValue(eventName, out thisEvent))
		{
			thisEvent.RemoveListener(listener);
		}
	}

	public void StopListening(EventName eventName, UnityAction<int> listener)
	{
		IntEvent thisEvent = null;
		if (intEvents.TryGetValue(eventName, out thisEvent))
		{
			thisEvent.RemoveListener(listener);
		}
	}

	public void TriggerEvent (EventName eventName, bool logError = true)
	{
		UnityEvent thisEvent = null;
		if (events.TryGetValue(eventName, out thisEvent))
			thisEvent.Invoke();
		else if (logError)
			Debug.Log("Attempted to trigger event \"" + eventName + "\", but it could not be found. Have you added it to the event dictionary yet?");
	}

	public void TriggerEvent (EventName eventName, int value, bool logError = true)
	{
		IntEvent thisEvent = null;
		if (intEvents.TryGetValue(eventName, out thisEvent))
			thisEvent.Invoke(value);
		else if (logError)
			Debug.Log("Attempted to trigger event \"" + eventName + "\", but it could not be found. Have you added it to the event dictionary yet?");
	}

	void OnDestroy()
	{
		foreach(KeyValuePair<EventName, UnityEvent> entry in events)
			entry.Value.RemoveAllListeners();

		foreach(KeyValuePair<EventName, IntEvent> entry in intEvents)
			entry.Value.RemoveAllListeners();
	}

	#endregion
}