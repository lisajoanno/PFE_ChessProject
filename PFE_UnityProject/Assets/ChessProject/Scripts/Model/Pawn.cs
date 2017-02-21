using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pawn : ChessElement {

    [System.Serializable]
    public class Move
    {
        public Vector2 position;
        public bool expansion;
        public int limit;

        public Move(Vector2 coo, bool expansion, int limit)
        {
            this.position = coo;
            this.expansion = expansion;
            this.limit = limit;
        }
    }

    // List of relative vectors of selectable case
    List<Move> moveCasesIn = new List<Move>();

    private ArrayList possibleMoveCases = new ArrayList();
    public ArrayList PossibleMoveCases
    {
        get
        {
            return this.possibleMoveCases;
        }
    }

    // Team (can be 0 or 1).
    private int team;
    public int Team
    {
        get
        {
            return team;
        }

        set
        {
            this.team = value;
            // If the team is 0, the color is white
            // If the team is 1, it's the adverse team so the color is black
            Color colorTeam = (team == 0) ? Color.white : Color.black;
            this.Color = colorTeam;
            ResetChessElementColor();
        }
    }


    void Start()
    {
        Move mv = new Move(new Vector2(1, 1), true, 10);
        moveCasesIn.Add(mv);
        mv = new Move(new Vector2(-1, -1), true, 10);
        moveCasesIn.Add(mv);
        mv = new Move(new Vector2(1, -1), true, 10);
        moveCasesIn.Add(mv);
        mv = new Move(new Vector2(-1, 1), true, 10);
        moveCasesIn.Add(mv);

        UpdateSelectableCases();
    }



    /// <summary>
    ///     Updates the list of selectable cases for the current pawn, 
    ///     according to the relative cases, the expansion, if it can jump
    ///     and if it can dig.
    /// </summary>
    public void UpdateSelectableCases()     
    {
        possibleMoveCases.Clear();
        foreach (Move mv in moveCasesIn)
        {
            if (!mv.expansion) // No expansion, just once
            {
                Position posFinale = this.Position.board.GetPositionFromTo(this.Position.coo, mv.position);
                possibleMoveCases.Add(posFinale);
            }
            else // Expansion, so as many times as the limit
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
                    Debug.Log(current.board == null);
                    current = current.board.GetPositionFromTo(current.coo, newMove, ref convertMatrices);

                    // If there is a pawn on the next square, it stops the movement, so canContinue = false
                    Pawn p = current.board.GetSquare(current.coo).gameObject.GetComponentInChildren<Pawn>();
                    if (p != null) canContinue = false;

                    if (p == null || (p.Team != Team)) possibleMoveCases.Add(current);

                    //rebuild the convert matrix for the next movement
                    foreach (int[,] convertMatrix in convertMatrices)
                    {
                        tmpMatrix[0, 0] = convert[0, 0] * convertMatrix[0, 0] + convert[0, 1] * convertMatrix[1, 0];
                        tmpMatrix[0, 1] = convert[0, 0] * convertMatrix[0, 1] + convert[0, 1] * convertMatrix[1, 1];
                        tmpMatrix[1, 0] = convert[1, 0] * convertMatrix[0, 0] + convert[1, 1] * convertMatrix[1, 0];
                        tmpMatrix[1, 1] = convert[1, 0] * convertMatrix[0, 1] + convert[1, 1] * convertMatrix[1, 1];
                        convert = (int[,])tmpMatrix.Clone();
                    }


                    
                }
            }
        }

        // if canDig is activated for this pawn, then the square at the opposite of the current position is also an available movement
        /*
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
        */
    }

    /// <summary>
    /// Allows to change the color of the chess element (needs to be overrid by each type of chess element).
    /// </summary>
    /// <param name="newColor">the color to set to the material</param>
    public override void SetChessElementColor(Color newColor)
    {
        Renderer[] tabChildren = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in tabChildren)
        {
            r.material.color = newColor;
        }
        GetComponent<Renderer>().material.color = newColor;
    }

}
