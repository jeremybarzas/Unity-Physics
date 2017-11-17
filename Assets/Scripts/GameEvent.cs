using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Create a GameEvent in the Project View
/// This Event should be something meaningful. Ex: GameStart
/// The GameStart.asset will have its reference used in the project
/// through the GameListener MonoBehaviour
/// </summary>
[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
	/// <summary>
	/// list of subscribers to this event
	/// </summary>
	private List<GameEventListener> listeners = new List<GameEventListener>();

	/// <summary>
	/// raise our event
	/// we go backwards to ensure that callers do not cause race conditions
	/// </summary>
	public void Raise()
	{
		for (int i = listeners.Count - 1; i >= 0; i--)
		{
			listeners[i].OnEventRaised();
		}
	}
	/// <summary>
	/// subscribe a listener to this event, this will happen when the listener becomes enabled
	/// </summary>
	/// <param name="listener">the monobehaviour we are listening 'from'</param>
	public void RegisterListener(GameEventListener listener)
	{
		if(listeners.Contains(listener))
		{
			throw new InvalidOperationException("Duplicate key");
		}
		listeners.Add(listener);
	}
	/// <summary>
	/// remove a listener, this will happen when the listener object becomes disabled
	/// </summary>
	/// <param name="listener">the monobehaviour we are listening 'from'</param>
	public void UnregisterListener(GameEventListener listener)
	{
		if(listeners.Contains(listener))
		{
			listeners.Remove(listener);
		}		
		else
		{
			throw new InvalidOperationException("No listener to remove");
		}
	}
}