using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;

/// <summary>
///     Controller for the selection of object in game
/// </summary>
[RequireComponent(typeof(BoardMovement))]
[RequireComponent(typeof(TeamTurn))]
public class SelectController : MonoBehaviour, IObserver, IObservable {
    [SerializeField]
    List<GameObject> gameObjectWithObservers;                 //those who watch the chrono

    IList<IObserver> observers = new List<IObserver>();

    private Select[] select;

    [SerializeField]
    private LayerMask whatFirstCanSelect;   //the layer of the first select script
    [SerializeField]
    private LayerMask whatSecondCanSelect;  //the layer of the second select script
    [SerializeField]
    private Color selectColor;              //the color that gameobject will have when selected
    [SerializeField]
    private Color possibleSquaresColor;     //the color of the squares you can select after choosing a pawn
    private BoardMovement move;             //the component that permits to make movement
    private TeamTurn teamTurn;

    /// <summary>
    ///     Initialize the controller of the selection
    /// </summary>
    void Awake()
    {
        select = new Select[2];
        move = GetComponent<BoardMovement>();
        teamTurn = GetComponent<TeamTurn>();
        select[0] = new SelectPawn(selectColor, whatFirstCanSelect, teamTurn);
        select[1] = new Select(selectColor, whatSecondCanSelect);
    }

    void Start()
    {
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
    }

    /// <summary>
    ///     Catch the input of the mouse and use it
    ///     to select an object
    /// </summary>
    void Update()
    {
        //Fire1 is the button for selecting
        if (Input.GetButtonDown("Fire1"))
        {
            bool aSelectionHasBeenDone = select[0].LaunchSelect(Input.mousePosition);
            //If we select a pawn, the possible movement will have a special color
            if (aSelectionHasBeenDone)
            {
                //a new selection has been done, so we remove the previous one
                ResetPossibleMovementColor();

                if (select[0].HasSthSelected)
                {
                    PawnElement element1 = select[0].LastSelected.GetComponent<PawnElement>();

                    //put a special color on the squares you can move
                    foreach (Position pos in element1.MoveCases)
                    {
                        pos.board.GetSquare(pos.coo).GetComponent<Renderer>().material.color = possibleSquaresColor;
                    }
                }
            }
            //we need to have the first select activated to select a second gameobject
            //and we need to have selected a gameobject before this frame
            if (select[0].HasSthSelected && !aSelectionHasBeenDone)
            {
                select[1].LaunchSelect(Input.mousePosition);
            }
            //we remove the selection of the second gameobject if the first is unselected and
            //if a second gameobject has been selected
            else if (select[1].HasSthSelected)
            {
                select[1].RemoveSelection();
            }
        }

        //Submit is the button for the validation of our movement
        if (Input.GetButtonDown("Submit"))
        {
            
            //we have to select 2 gameobject before the validation of the movement
            if (select[0].HasSthSelected && select[1].HasSthSelected)
            {
                PawnElement element1 = select[0].LastSelected.GetComponent<PawnElement>();
                if (element1.GetTeam == teamTurn.GetTeamTurn)
                {
                    SquareElement element2 = select[1].LastSelected.GetComponent<SquareElement>();
                    //do the move; eat the pawn at the selected square if needed
                    if (move.Move(element1, element2))
                    {
                        //we notify to all the observers that a move has been done
                        NotifyAll();
                    }
                    ResetAllSelection();
                }
            }
        }
    }

    private void ResetPossibleMovementColor()
    {
        IList<GameObject> gameobjects = GameObject.FindObjectsOfType<GameObject>();
        IList<GameObject> squares = new List<GameObject>();
        foreach(GameObject gameobject in gameobjects)
        {
            if (whatSecondCanSelect == (whatSecondCanSelect | (1 << gameobject.layer)))
            {
                squares.Add(gameobject);
            }
        }

        //reset all the squares to their first color
        foreach (GameObject square in squares)
        {
            square.GetComponent<Renderer>().material.color = square.GetComponent<SquareElement>().Color;
        }
    }

    /// <summary>
    /// Reset all selection we could have done
    /// </summary>
    public void ResetAllSelection()
    {
        if(select[0].HasSthSelected)
            select[0].RemoveSelection();
        if(select[1].HasSthSelected)
            select[1].RemoveSelection();
        ResetPossibleMovementColor();
    }

    /// <summary>
    ///     On a notification we reset all the selection
    /// </summary>
	public void OnNotify(IList<EventTurn> events)
    {
        bool notifiedBefore = false;
		foreach(EventTurn ev in events)
        {
			if (ev.Type == EventTurnType.MovementDone)
            {
                notifiedBefore = true;
            }
        }
        if (!notifiedBefore)
        {
            ResetAllSelection();
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
		events.Add(new EventTurn(EventTurnType.MovementDone, "movement done"));
        foreach (IObserver obs in observers)
        {
            obs.OnNotify(events);
        }
    }
}
