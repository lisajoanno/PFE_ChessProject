using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;

/// <summary>
///     Controller for the selection of object in game
/// </summary>
//[RequireComponent(typeof(BoardMovement))]
//[RequireComponent(typeof(TeamTurn))]
public class SelectController : MonoBehaviour
{
    private Select[] select;

    [SerializeField]
    private LayerMask whatFirstCanSelect;   //the layer of the first select script
    [SerializeField]
    private LayerMask whatSecondCanSelect;  //the layer of the second select script
    [SerializeField]
    private Color selectColor;              //the color that gameobject will have when selected
    [SerializeField]
    private Color possibleSquaresColor;     //the color of the squares you can select after choosing a pawn

    private MoveController moveCtrl;        //the component that permits to make movement

    /// <summary>
    ///     Initialize the controller of the selection
    /// </summary>
    void Awake()
    {
        select = new Select[2];
        
        // Search for the components in parent (GamePlay)
        moveCtrl = GetComponentInParent<MoveController>();

        select[0] = new SelectPawn(selectColor, possibleSquaresColor, whatFirstCanSelect);
        select[1] = new Select(selectColor, whatSecondCanSelect);
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
                Pawn pawn = select[0].LastSelected.GetComponent<Pawn>();
                //if (pawn.GetTeam == teamTurn.GetTeamTurn)
                {
                    Square square = select[1].LastSelected.GetComponent<Square>();
                    //do the move; eat the pawn at the selected square if needed
                    
                    ResetAllSelection();

                    if (moveCtrl.Move(pawn, square))
                    {
                        //we notify to all the observers that a move has been done
                        //NotifyAll();
                    }
                    
                }
            }
        }
    }

    /// <summary>
    /// Reset all selection we could have done
    /// </summary>
    public void ResetAllSelection()
    {
        if (select[0].HasSthSelected)
            select[0].RemoveSelection();
        if (select[1].HasSthSelected)
            select[1].RemoveSelection();
    }
}
