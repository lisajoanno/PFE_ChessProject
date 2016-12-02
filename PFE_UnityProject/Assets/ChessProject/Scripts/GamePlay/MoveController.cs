using UnityEngine;
using System.Collections;
using System;

public class MoveController : MonoBehaviour {

    //the component managing the teams
    private TeamTurn teamTurn;

    private ConnexionManager connexionManager;

    void Start()
    {
        // Initialisation of the team turn component
        teamTurn = GetComponentInParent<TeamTurn>();

        connexionManager = GameObject.FindGameObjectWithTag("ConnexionManager").GetComponent<ConnexionManager>();
    }

    /// <summary>
    /// Tries to make a move if a pawn, but checks first that it's possible : 
    /// - it's of the current team playing
    /// - there is an other pawn on the square
    /// ...
    /// </summary>
    /// <param name="pawn">the pawn to move</param>
    /// <param name="square">the square where to move it</param>
    /// <returns>true if the move was successful, false if not</returns>
    public bool Move(Pawn pawn, Square square)
    {
        // Light check that the pawn is on the right team
        if (pawn.Team != teamTurn.CurrentTeamPlaying) Debug.Log("Warning : the pawn you're moving is not of the right team.");

        // Check that the movement is possible for a given pawn
        if (!pawn.PossibleMoveCases.Contains(square.Position)) return false;

        // Is there a pawn on the square ? 
        Pawn pawnOnBoard = square.GetComponentInChildren<Pawn>();

        // If there is a pawn but it's on the same team : nothing happens
        if (pawnOnBoard != null && (pawnOnBoard.Team == pawn.Team)) return false;

        // If the pawn is on the other team, we eat it
        if (pawnOnBoard != null && (pawnOnBoard.Team != pawn.Team)) EatPawn(pawnOnBoard);


        // ---------------------------------------
        // --  Send of the move to the server.  --
        // ---------------------------------------
        Position oldPos = pawn.Position;
        Position newPos = square.Position;
        connexionManager.MakeAMove(0, (int)oldPos.coo.x, (int)oldPos.coo.y, 0, (int)newPos.coo.x, (int)newPos.coo.y);
        
        // Real, physical move
        MakeMove(pawn, square);
        // The pawn was moved : the team can change
        teamTurn.ChangeTeam();
        
        return true;
    }

    public void MakeMoveFromOtherPlayer(Position[] positions)
    {
        Square newSquare = teamTurn.AllBoard[0].GetSquare(positions[1].coo).GetComponent<Square>();
        Pawn oldPawn = teamTurn.AllBoard[0].GetSquare(positions[0].coo).GetComponent<Square>().GetComponentInChildren<Pawn>();

        Move(oldPawn, newSquare);
    }

    /// <summary>
    /// Eats a pawn.
    /// </summary>
    /// <param name="pawnOnBoard">the pawn to eat</param>
    private void EatPawn(Pawn pawnOnBoard)
    {
        GameObject.Destroy(pawnOnBoard.gameObject);
    }


    /// <summary>
    /// Physical move of a pawn game object to an other square.
    /// </summary>
    /// <param name="pawn">the pawn to move</param>
    /// <param name="square">the square where to move it</param>
    public void MakeMove(Pawn pawn, Square square)
    {
        pawn.Position = square.Position;
        Board board = pawn.GetComponent<Pawn>().Position.board;
        Vector2 pos = pawn.GetComponent<Pawn>().Position.coo;

        //lol RIP en paix les deux heures passees la dessus
        pawn.gameObject.transform.SetParent(null);
        // Set of the rigth direction : forward compared to the case (not the board..)
        pawn.gameObject.transform.up = -(board.GetSquare(pos).gameObject.transform.forward);
        // Set of the rigth position, on the right board
        pawn.gameObject.transform.position = board.GetSquare(pos).gameObject.transform.position;
        // The pawn needs to be on the case
        pawn.gameObject.transform.position += (pawn.gameObject.transform.up) * 3;
        pawn.gameObject.transform.SetParent(board.GetSquare(pos).gameObject.transform);

        // We update the selectable cases of the pawn just moved
        pawn.UpdateSelectableCases();
    }
}
 