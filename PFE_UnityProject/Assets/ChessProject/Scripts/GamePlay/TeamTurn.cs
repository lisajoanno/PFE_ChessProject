using UnityEngine;
using System.Collections;

public class TeamTurn : MonoBehaviour {

    /// <summary>
    /// The team manager needs to know the board in order to update all possible move cases of all pawns.
    /// </summary>
    private Board[] allBoards;
    public Board[] AllBoard
    {
        set
        {
            allBoards = value;
        }
    }

    // the current team playing
    // Initialized : 0
    private int currentTeamPlaying = 0;
    public int CurrentTeamPlaying
    {
        get
        {
            return currentTeamPlaying;
        }
    }

    /// <summary>
    /// Method to change the current team playing
    /// (after a pawn was moved for example).
    /// </summary>
    public void ChangeTeam()
    {
        if (currentTeamPlaying == 0) currentTeamPlaying = 1;
        else if (currentTeamPlaying == 1) currentTeamPlaying = 0;
        else
        {
            Debug.Log("The current team playing is neither 0 or 1 ?");
        }

        // update of selectable cases of all pawns
        UpdateAllSelectableCases();
    }

    /// <summary>
    /// Updates all the possible move cases for all pawn on the board.
    /// </summary>
    private void UpdateAllSelectableCases()
    {
        Pawn pawn;
        foreach (Board board in allBoards)
        {
            foreach (GameObject square in board.SquaresOfTheBoard.Values)
            {
                pawn = square.GetComponentInChildren<Pawn>();
                if (pawn)
                {
                    pawn.UpdateSelectableCases();
                }
            }
        }
    }
}
