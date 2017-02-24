using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PawnsInitializer : MonoBehaviour {

    // Pour le test (automatisé par la suite)
    public GameObject go1;
    public GameObject go2;
    public GameObject go3;
    public GameObject go4;

    // List of all PAWNS of the game (by Position, so Board and Vector2)
    private IDictionary<Position, Pawn> positions = new Dictionary<Position, Pawn>();
    public IDictionary<Position, Pawn> GetPositions
    {
        get
        {
            return positions;
        }
    }

    // To get the tab of boards
    private Board[] getBoards()
    {
        return GameObject.FindGameObjectWithTag("BoardInitializer").GetComponent<BoardInitializer>().boards;
    }

    private void initPawn(GameObject goTest, int team, string name, int board, Vector2 pos)
    {
        goTest.GetComponent<Pawn>().Position = new Position(getBoards()[board], pos);
        GameObject go = PlacePawn(goTest);
        go.gameObject.name = name;
        go.GetComponent<Pawn>().Team = team;
        go.GetComponent<Pawn>().Position = new Position(getBoards()[board], pos);
    }

    // Use this for initialization
    public void Initialize() {
        initPawn(go1, 0, "hehe1", 0, new Vector2(0, 0));
        initPawn(go2, 0, "hehee1", 0, new Vector2(1, 0));
        initPawn(go2, 0, "heheee1", 0, new Vector2(2, 0));
        initPawn(go3, 0, "heheeee1", 0, new Vector2(3, 0));

        initPawn(go1, 1, "hehe2", 3, new Vector2(0, 0));
        initPawn(go2, 1, "hehee2", 3, new Vector2(1, 0));
        initPawn(go2, 1, "heheee2", 3, new Vector2(2, 0));
        initPawn(go3, 1, "heheeee2", 3, new Vector2(3, 0));

    }

    /// <summary>
    /// Places a pawn at the right position.
    /// </summary>
    /// <param name="pawn"></param>
    GameObject PlacePawn(GameObject pawn)
    {
        GameObject pawnCreated = GameObject.Instantiate(pawn);
        Board board = pawn.GetComponent<Pawn>().Position.board;
        Vector2 pos = pawn.GetComponent<Pawn>().Position.coo;
        
        MoveController moveCtrl = GameObject.FindGameObjectWithTag("GamePlay").GetComponent<MoveController>();
        moveCtrl.MakeMove(pawnCreated.GetComponent<Pawn>(), board.GetSquare(pos).GetComponent<Square>());
        return pawnCreated;
    }

}
