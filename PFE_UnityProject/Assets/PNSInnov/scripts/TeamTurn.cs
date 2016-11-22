using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;

public class TeamTurn : MonoBehaviour, IObserver, IObservable {
    [SerializeField]
    List<GameObject> gameObjectWithObservers;                 //those who watch the turns
    IList<IObserver> observers = new List<IObserver>();

    [SerializeField]
    private int nbTeams;

    [SerializeField]
    private InputField inputP1;

    [SerializeField]
    private InputField inputP2;

    private bool next;
    private String player;
    private String nextText;
    
    private int teamTurn;
    public int GetTeamTurn
    {
        get
        {
            return teamTurn;
        }
    }

    void Start()
    {
        teamTurn = 0;

        foreach (GameObject gameobject in gameObjectWithObservers)
        {
            foreach (IObserver obs in gameobject.GetComponents<IObserver>())
            {
                if (!obs.Equals(this))
                {
                    observers.Add(obs);
                }
            }
        }
        nextText = "";
        player = "";
        next = false;
    }

    void Update()
    {
        SelectPlayer();
    }

    public void NextTurn()
    {
        NextTurnWithoutNotification();
        NotifyAll();
    }

    public void NextTurnWithoutNotification()
    {
        teamTurn = (1 + teamTurn) % nbTeams;
        next = true;
        nextText = "Au tour de " + player + " de jouer.";
    }

    void OnGUI()
    {
        if (next)
        {
            GUIStyle myStyle = new GUIStyle();
            myStyle.fontSize = 20;
            GUI.Label(new Rect(500, 100, 1000, 30), nextText, myStyle);
        }
    }

    private void SelectPlayer()
    {
        if (teamTurn == 1)
        {
            player = inputP1.text;
        } else
        {
            player = inputP2.text;
        }
    }

	public void OnNotify(IList<EventTurn> events)
    {
        bool notifiedBefore = false;
		foreach(EventTurn ev in events)
        {
			if (ev.Type == EventTurnType.EndTurn)
            {
                notifiedBefore = true;
                break;
            }
        }

        if (!notifiedBefore) {
            NextTurnWithoutNotification();
            NotifyAll(events);
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
		events.Add(new EventTurn(EventTurnType.EndTurn, "the turn is finished"));
        foreach (IObserver obs in observers)
        {
            obs.OnNotify(events);
        }
    }
}
