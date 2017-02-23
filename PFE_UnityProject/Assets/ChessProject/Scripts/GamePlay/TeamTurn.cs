using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
        get
        {
            return allBoards;
        }
    }

    // Text of the team turn
    [SerializeField]
    private Text teamTurnText;
    [SerializeField]
    private Text teamTurnText2;
    // Text of the team you are playing
    [SerializeField]
    private Text YourTeamText;
    [SerializeField]
    private Text YourTeamText2;

    void Start()
    {
        UpdateText();
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

    // The team of the pawns you are controlling. It is set by the game initializer at the beginning, when received from the server.
    private int yourTeam;
    public int YourTeam
    {
        get
        {
            return yourTeam;
        }
        set
        {
            this.yourTeam = value;
            this.YourTeamText.text = "Equipe " + this.yourTeam;
            this.YourTeamText2.text = "Equipe " + this.yourTeam;
        }
    }

    /// <summary>
    /// Returns true if a pawn can play.
    /// </summary>
    /// <param name="team"></param>
    /// <returns></returns>
    public bool thisTeamCanPlay(int team)
    {
        //if (team == currentTeamPlaying && team == yourTeam) return true;
        //else return false;
        return true;
        // ((team > 1) || (team < 0) || currentTeamPlaying > 1 || (currentTeamPlaying < 0)) 
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

        UpdateText();

        // update of selectable cases of all pawns
        UpdateAllSelectableCases();
    }

    /// <summary>
    /// Updates the text of the team currently playing.
    /// </summary>
    private void UpdateText()
    {
        teamTurnText.text = "Tour de l'équipe " + currentTeamPlaying;
        teamTurnText2.text = "Tour de l'équipe " + currentTeamPlaying;
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
                    //Debug.Log("je fais qqchose");
                    pawn.UpdateSelectableCases();
                }
            }
        }
    }
}
