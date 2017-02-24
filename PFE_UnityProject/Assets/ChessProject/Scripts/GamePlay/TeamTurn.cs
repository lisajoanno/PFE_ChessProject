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
            switch (this.yourTeam)
            {
                case 0:

                    this.YourTeamText.text = "Equipe blanche";
                    this.YourTeamText2.text = "Equipe blanche";
                    break;
                case 1:
                    this.YourTeamText.text = "Equipe noire";
                    this.YourTeamText2.text = "Equipe noire";
                    break;
            }
        }
    }

    /// <summary>
    /// Returns true if a pawn can play.
    /// </summary>
    /// <param name="team"></param>
    /// <returns></returns>
    public bool thisTeamCanPlay(int team)
    {
        return (team == currentTeamPlaying && team == yourTeam);
        // ((team > 1) || (team < 0) || currentTeamPlaying > 1 || (currentTeamPlaying < 0)) 
    }

    /// <summary>
    /// Method to change the current team playing
    /// (after a pawn was moved for example).
    /// </summary>
    public void ChangeTeam()
    {

        if (currentTeamPlaying == 0)
        {
            currentTeamPlaying = 1;
        }
        else if (currentTeamPlaying == 1) currentTeamPlaying = 0;
        else
        {
            Debug.Log("The current team playing is neither 0 or 1 ?");
        }

        UpdateText();

        // update of selectable cases of all pawns
        UpdateAllSelectableCases();
    }

    private void CheckObjectives()
    {
        GameObject whiteKing = GameObject.FindGameObjectWithTag("blackKing");
        GameObject blackKing = GameObject.FindGameObjectWithTag("whiteKing");

        if(whiteKing == null)
        {
            Debug.Log("Black wins!");
            this.YourTeamText.text = "Black wins!";
            currentTeamPlaying = 2;
        }
        if(blackKing == null)
        {
            Debug.Log("White wins!");
            this.YourTeamText.text = "White wins!";
            currentTeamPlaying = 2;
        }
    }

    /// <summary>
    /// Updates the text of the team currently playing.
    /// </summary>
    private void UpdateText()
    {
        switch (currentTeamPlaying){
            case 0:
                teamTurnText.text = "Tour des blancs";
                teamTurnText2.text = "Tour des blancs";
                break;
            case 1:
                teamTurnText.text = "Tour des noirs";
                teamTurnText2.text = "Tour des noirs";
                break;
        }
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
