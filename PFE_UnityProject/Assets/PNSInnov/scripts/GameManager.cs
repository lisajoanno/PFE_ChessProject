using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(BoardGenerator))]
[RequireComponent(typeof(PositionPawns))]
[RequireComponent(typeof(GameGoal))]
[RequireComponent(typeof(TeamTurn))]
[RequireComponent(typeof(PositionPawn))]
public class GameManager : NetworkManager {
    short playerControllerHighestId;
    private BoardGenerator plateau;
    private PositionPawn pp;
	private PositionPawns positionPawns;
    private GameGoal gameGoal;
    private TeamTurn teamTurn;

    Board[] boards;

    [SerializeField]
	private int sizeBoard;

    // Use this for initialization

    void Start() {
        boards = new Board[6];
        boards[0] = new Board(sizeBoard, sizeBoard);
        boards[1] = new Board(sizeBoard, sizeBoard);
        boards[2] = new Board(sizeBoard, sizeBoard);
        boards[3] = new Board(sizeBoard, sizeBoard);
        boards[4] = new Board(sizeBoard, sizeBoard);
        boards[5] = new Board(sizeBoard, sizeBoard);


        plateau = GetComponent<BoardGenerator>();
		plateau.initSize (sizeBoard);
		pp = GetComponent<PositionPawn>();
		positionPawns = GetComponent<PositionPawns>();
		gameGoal = GetComponent<GameGoal>();
        teamTurn = GetComponent<TeamTurn>();

        plateau.DessinerEchiquier(boards);
        
        positionPawns.PositionSomeTypePawns(plateau, boards);
        gameGoal.InitGameGoal(positionPawns.GetPositions, boards);

        // everything is ready

        GameObject daddyGO = GameObject.Find("pawns");
        foreach (PawnElement pe in daddyGO.GetComponentsInChildren<PawnElement>())
        {
            teamTurn.AddObserver(pe);
            pe.PositionPawns = positionPawns;
        }
    }
}
