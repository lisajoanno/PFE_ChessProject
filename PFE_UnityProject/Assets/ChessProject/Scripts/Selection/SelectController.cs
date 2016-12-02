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

    [SerializeField]
    private GameObject cubeObjTarget;

    private MoveController moveCtrl;        //the component that permits to make movement
    private ColliderManager cubeCollide;    //the collider manager component

    //TODO: remove those var
    private bool pawnOk, squareOk;

    private Pawn pawn;
    private Square square;

    
    void Start()
    {
        cubeCollide = cubeObjTarget.GetComponent<ColliderManager>();
        pawnOk = false;
        squareOk = false;
        pawn = new Pawn();
        square = new Square();
    }
    
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
        if (cubeCollide.PawnIsSelected())
        {
            pawn = cubeCollide.GetComponent<ColliderManager>().SelectedGameObject().GetComponent<Pawn>();
            ManageSelection(pawn.gameObject);
            cubeCollide.SetPawnIsSelected(false);
            pawnOk = true;
        }

        if (cubeCollide.SquareIsSelected() && pawnOk)
        {
            cubeCollide.SetSumbitIsSelected(false);
            //we manage the selection only if the user select a valid square
            if (cubeCollide.GetComponent<ColliderManager>().SelectedGameObject().GetComponent<Square>().GetComponent<Renderer>().material.color == possibleSquaresColor)
            {
                square = cubeCollide.GetComponent<ColliderManager>().SelectedGameObject().GetComponent<Square>();
                ManageSelection(square.gameObject);
                cubeCollide.SetSquareIsSelected(false);
                squareOk = true;
            }
        }

        if(cubeCollide.SubmitIsSelected() && squareOk)
        {
            ResetAllSelection();
            moveCtrl.Move(pawn, square);
        }

        //Fire1 is the button for selecting
        if (Input.GetButtonDown("Fire1"))
        {
            // The player tries to select a pawn or a square.
            DetectRay();
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

                    moveCtrl.Move(pawn, square);
                }
            }
        }
    }


    /// <summary>
    /// Finds the game object the player clicked on. If it's of the right layer, it lauches the selection.
    /// </summary>
    private void DetectRay()
    {
        //create a ray from the specified position
        Ray rayToSelect = Camera.main.ScreenPointToRay(Input.mousePosition); 
        RaycastHit hit = new RaycastHit();
        //if the ray hit an object with a layer we can select
        if (Physics.Raycast(rayToSelect, out hit, float.MaxValue, whatFirstCanSelect | whatSecondCanSelect))
        {
            GameObject newSelected = hit.transform.gameObject;
            ManageSelection(newSelected);
        }
    }

    void ManageSelection(GameObject newSelected)
    {
        ResetAllColorModel();
        // The selected GO contains the 'whatFirstCanSelect' layer
        if (((2 << (newSelected.layer - 1)) & whatFirstCanSelect) == whatFirstCanSelect)
        {
            select[0].LaunchSelect(newSelected);
        }
        // The selected GO contains the 'whatSecondCanSelect' layer
        else if (((2 << (newSelected.layer - 1)) & whatSecondCanSelect) == whatSecondCanSelect)
        {
            if (select[0].HasSthSelected) select[1].LaunchSelect(newSelected);
        }
        RecolorAllModel();
    }



    /// <summary>
    /// Resets all colors of all Select.
    /// </summary>
    private void ResetAllColorModel()
    {
        foreach (Select s in select)
        {
            s.ResetColorModel();
        }
    }

    /// <summary>
    /// Recolors are Select.
    /// </summary>
    private void RecolorAllModel()
    {
        foreach (Select s in select)
        {
            if (s.HasSthSelected) s.RecolorModel();
        }
    }







    /// <summary>
    /// Reset all selection we could have done
    /// </summary>
    public void ResetAllSelection()
    {
        ResetAllColorModel();
        if (select[0].HasSthSelected)
        {
            select[0].RemoveSelection();
        }
        if (select[1].HasSthSelected)
        {
            select[1].RemoveSelection();
        }
    }
}
