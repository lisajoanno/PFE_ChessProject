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

        int oldBoard = 0;
        oldBoard = GetIntFromBoard(oldPos.board);
        int newBoard = 0;
        newBoard = GetIntFromBoard(newPos.board);

        //TODO: uncomment the following line
        connexionManager.MakeAMoveOnServer(oldBoard, (int)oldPos.coo.x, (int)oldPos.coo.y, newBoard, (int)newPos.coo.x, (int)newPos.coo.y);
        




        // Real, physical move
        MakeMove(pawn, square);
        // The pawn was moved : the team can change
        teamTurn.ChangeTeam();
        
        return true;
    }

    private Board GetBoardObjectFromIndex(int index)
    {
        return teamTurn.AllBoard[index];
    }

    private int GetIntFromBoard(Board board)
    {
        for (int i = 0; i<teamTurn.AllBoard.Length; i++)
        {
            if (teamTurn.AllBoard[i] == board) return i;
        }
        return -1;
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



    /************     Multiplayer management     *************/

    /// <summary>
    /// Receives a move from the other player and actually moves the pawn.
    /// First it need to parse the json on parameters.
    /// </summary>
    /// <param name="data">the json string to be parsed</param>
    public void MakeMoveFromOtherPlayer(string data)
    {
        // Extracts the Position of the pawn to be moved and the square where to move it
        // position[0] is the position of the pawn
        // position[1] is the position of the square where to move the pawn
        Position[] positions = TranslatePositions(data);
        Board board1 = GetBoardObjectFromIndex(GetIntFromBoard(positions[1].board));
        Square newSquare = board1.GetSquare(positions[1].coo).GetComponent<Square>();

        Board board2 = GetBoardObjectFromIndex(GetIntFromBoard(positions[0].board));
        Pawn oldPawn = board2.GetSquare(positions[0].coo).GetComponent<Square>().GetComponentInChildren<Pawn>();
        
        // Makes the move
        Move(oldPawn, newSquare);
    }

    /// <summary>
    /// A class used to parse the data received from the server.
    /// It is serializable and an object is creatable from a json string.
    /// </summary>
    [System.Serializable]
    public class DataReceived
    {

        public int face_old;
        public int x_old;
        public int y_old;
        public int face_new;
        public int x_new;
        public int y_new;

        public static DataReceived CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<DataReceived>(jsonString);
        }
    }

    /// <summary>
    /// From the data received from the server, creates 2 Position :
    ///     - the first one is the original position (so the pawn)
    ///     - the second one is the square where to move it.
    /// </summary>
    /// <param name="data">the JSON string (WARNING : string with lower s)</param>
    /// <returns>a table of 2 positions</returns>
    private Position[] TranslatePositions(string data)
    {
        DataReceived dataReceived = DataReceived.CreateFromJSON(data);
        Position oldPos = new Position(GetBoardObjectFromIndex(dataReceived.face_old), new Vector2(dataReceived.x_old, dataReceived.y_old));
        Position newPos = new Position(GetBoardObjectFromIndex(dataReceived.face_new), new Vector2(dataReceived.x_new, dataReceived.y_new));
        Position[] positions = new Position[2];
        positions[0] = oldPos;
        positions[1] = newPos;
        return positions;
    }
}
 