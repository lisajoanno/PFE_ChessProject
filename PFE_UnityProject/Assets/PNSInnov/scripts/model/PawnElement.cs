using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PawnElement : ChessElement, IObserver {
    private PositionPawns pp;
    public PositionPawns PositionPawns
    {
        set
        {
            pp = value;
        }
    }

    [SerializeField]
    private int team;
    public int GetTeam
    {
        get
        {
            return team;
        }
    }
    
    [System.Serializable]
    public class Move
    {
        public Vector2 position;
        public Boolean expansion;
        public int limit;
    }

    // List of relative vectors of selectable case
    [SerializeField]
    List<Move> moveCasesIn;

    
    private ArrayList moveCases = new ArrayList();
    public ArrayList MoveCases
    {
        get
        {
            return this.moveCases;
        }
    }

    // The possibility to jump over a pawn : if it's on the way, if canJump is true, the current pawn can still move further
	[SerializeField]
	Boolean canJump;
	public Boolean GetJump
	{
		get
		{
			return canJump;
		}
	}

    // The possibility to have an extra move : the current square on the opposite square
    [SerializeField]
    Boolean canDig;
    public Boolean GetDig
    {
        get
        {
            return canDig;
        }
    }

    // TODO not implemented yet -- if killable cases are different from movable cases
    private ArrayList killCases = new ArrayList();
    public ArrayList KillCases
    {
        get
        {
            return this.killCases;
        }
    }

    // Use this for initialization
    void Start () {
        UpdateSelectableCases();
    }

    void Awake()
    {
        
    }

    /// <summary>
    ///     Updates the list of selectable cases for the current pawn, 
    ///     according to the relative cases, the expansion, if it can jump
    ///     and if it can dig.
    /// </summary>
    public void UpdateSelectableCases () {
        moveCases.Clear();
        foreach (Move mv in moveCasesIn)
        {
            if (!mv.expansion) // No expansion, just once
            {
                Position posFinale = this.Position.board.GetPositionFromTo(this.Position.coo, mv.position);
                moveCases.Add(posFinale);
            } else // Expansion, so as many times as the limit
            {
                Position current = new Position(this.Position.board, this.Position.coo);
                bool canContinue = true;
                int[,] convert = new int[2, 2] { {1,0},
                                                 {0,1} };
                int[,] tmpMatrix = new int[2, 2];
                Vector2 newMove;
                int moveX;
                int moveY;
                IList<int[,]> convertMatrices;
                for (int i = 0; i < mv.limit && canContinue; i++)
                {
                    //convert the vector into the new coordinate system
                    moveX = (int)(mv.position.x * convert[0, 0] + mv.position.y * convert[1, 0]);
                    moveY = (int)(mv.position.x * convert[0, 1] + mv.position.y * convert[1, 1]);
                    newMove = new Vector2(moveX, moveY);

                    //prepare the list of matrices
                    convertMatrices = new List<int[,]>();
                    current = current.board.GetPositionFromTo(current.coo, newMove, ref convertMatrices);
                    moveCases.Add(current);

                    //rebuild the convert matrix for the next movement
                    foreach(int[,] convertMatrix in convertMatrices)
                    {
                        tmpMatrix[0, 0] = convert[0, 0] * convertMatrix[0, 0] + convert[0, 1] * convertMatrix[1, 0];
                        tmpMatrix[0, 1] = convert[0, 0] * convertMatrix[0, 1] + convert[0, 1] * convertMatrix[1, 1];
                        tmpMatrix[1, 0] = convert[1, 0] * convertMatrix[0, 0] + convert[1, 1] * convertMatrix[1, 0];
                        tmpMatrix[1, 1] = convert[1, 0] * convertMatrix[0, 1] + convert[1, 1] * convertMatrix[1, 1];
                        convert = (int[,])tmpMatrix.Clone();
                    }

                    if (pp != null)
                    {
                        if ((pp.IsThereAPawn(current)) && !canJump) { // if the pawn can't jump, then we don't go further in this position
                            canContinue = false;
                        }
                    }                    
                }
            }
        }

        // if canDig is activated for this pawn, then the square at the opposite of the current position is also an available movement
        if (canDig)
        {
            
            Vector3 origin = this.transform.position; // position of the current game object (pawn)
            Vector3 direction = this.Position.board.SquaresOfTheBoard[this.Position.coo].transform.up; // up of the square where the pawn is placed
            Ray rayToSelect = new Ray(origin, direction); // origin + direction make the ray

            RaycastHit hit = new RaycastHit(); // future object hit
            LayerMask mask = -1;
            
            if (Physics.Raycast(rayToSelect, out hit, float.MaxValue, mask.value)) // hit game object
            {
                SquareElement ce = hit.transform.gameObject.GetComponent<SquareElement>(); // the element just hit must be a square element
                if (ce.Position.board != this.Position.board) // the square element juste hit must be on a different board
                {
                    moveCases.Add(new Position(ce.Position.board, ce.Position.coo)); // we add it to the possible move cases.
                }
            }
        }
    }

	public void OnNotify(IList<EventTurn> eventNotify)
    {
        if (this != null)
        {
            UpdateSelectableCases();
        }
        
    }
}
