using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
///     Chronometer that go to 0 each time it reaches its end
/// </summary>
public class Chrono : NetworkBehaviour, IObserver, IObservable {
    [SerializeField]
    List<GameObject> gameObjectWithObservers;                 //those who watch the chrono

    IList<IObserver> observers = new List<IObserver>();

    [SerializeField]
    List<GameObject> gameObjectItWillObserve;                 //those who will be observed by the chrono
    
    public int chronoTime;            //the number of time you have at each turn
    
    [SyncVar]
    private float leftTime;                     //the number of time you have for this turn

    public float LeftTime
    {
        get
        {
            return leftTime;
        }

        set
        {
            this.leftTime = value;
        }
    }

    [SerializeField]
    UnityEngine.UI.Text chronoText;               //the text box we need to update at each frame

	// Use this for initialization
	void Start () {
        
        if(chronoTime <= 0)
        {
            Debug.LogError("chrono time is less than 0");
            Application.Quit();
        }
        leftTime = chronoTime + 1;
        foreach(GameObject gameobject in gameObjectWithObservers)
        {
            foreach(IObserver obs in gameobject.GetComponents<IObserver>())
            {
                if (!obs.Equals(this))
                {
                    observers.Add(obs);
                }
            }
        }

        foreach(GameObject gameobject in gameObjectItWillObserve)
        {
            foreach(IObservable obs in gameobject.GetComponents<IObservable>())
            {
                obs.AddObserver(this);
            }
        }
	}
	
	/// <summary>
    /// At each frame, we remove time. If the time left is less or equals than 0
    /// we go back to the max time and we pass the turn
    /// </summary>
	public void Update () {

        //we pass the turn and reinitialize the chrono if we reach 0
        if (leftTime <= 0)
        {
            ResetChrono();
        }

        chronoText.text = (Mathf.FloorToInt(leftTime)) + "s";

        //remove time since the last frame
        leftTime -= Time.deltaTime;
	}

    /// <summary>
    ///     Reset the chrono to its original value and notify the observers
    /// </summary>
    public void ResetChrono()
    {
        ResetChronoWithoutNotification();
        NotifyAll();
    }

    /// <summary>
    ///     Reset the chronot without notify the observers
    /// </summary>
    public void ResetChronoWithoutNotification()
    {
        leftTime = chronoTime + 1;
    }

	public void OnNotify(IList<EventTurn> eventNotify)
    {
        bool notifiedBefore = false;
        //we check if we have been notified by this event before
		foreach (EventTurn ev in eventNotify)
        {
			if (ev.Type == EventTurnType.Timeout)
            {
                notifiedBefore = true;
                break;
            }
        }

        //if no notification before we do our job and we send our own notification
        if (!notifiedBefore)
        {
            ResetChronoWithoutNotification();
            NotifyAll(eventNotify);
        }
    }

    public void AddObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyAll()
    {
		NotifyAll(new List<EventTurn>());
    }

	public void NotifyAll(IList<EventTurn> events)
    {
		events.Add(new EventTurn(EventTurnType.Timeout, "timer reach 0"));

        foreach (IObserver obs in observers)
        {
            obs.OnNotify(events);
        }
    }

}