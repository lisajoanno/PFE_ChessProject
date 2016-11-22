using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Structure to permit to have the new coordinate
///     and the new movement when we pass from a board to another one
/// </summary>
public struct Passing
{
    public delegate Vector2 NewBeginning(Vector2 coo, int rows, int columns);

    public int[,] convertMatrix;                       //the matrix to convert the vector2 movement
    public NewBeginning newBeginning;//the lambda expression to have the new origin on the other board
}

public enum Direction { NONE=0, UP, RIGHT, LEFT, DOWN }

/// <summary>
///     Model of a board.
///     It can be linked to 4 other board
/// </summary>
public class Board {
    private IDictionary<Direction, Passing> convertMatrices; //the matrices to change a movement from a board to another one

    IDictionary<Direction, Board> neighbour;                //All the board neighbour of the board
    public IDictionary<Direction, Board> GetNeighbour
    {
        get
        {
            return neighbour;
        }
    }

    private IDictionary<Vector2, GameObject> squaresOfTheBoard;
    public IDictionary<Vector2, GameObject> SquaresOfTheBoard
    {
        get
        {
            return squaresOfTheBoard;
        }
    }

    private int rows;
    public int Rows
    {
        get
        {
            return rows;
        }
    }
    private int columns;
    public int Columns
    {
        get
        {
            return columns;
        }
    }

    public Board() : this(8,8)
    {    }

    public Board(int rows, int columns)
    {
        neighbour = new Dictionary<Direction, Board>();
        convertMatrices = new Dictionary<Direction, Passing>();

        squaresOfTheBoard = new Dictionary<Vector2, GameObject>();
        this.rows = rows;
        this.columns = columns;
    }

    /// <summary>
    ///     Add a neighbour board to this one
    /// </summary>
    /// <param name="dir">The direction where the other board is</param>
    /// <param name="board">the new neighbour</param>
    public void AddBoard(Direction dir, Board board, Passing p)
    {
        neighbour.Add(dir, board);
        convertMatrices.Add(dir, p);
    }

    /// <summary>
    /// Get the square at the position that is specified
    /// </summary> 
    /// <param name="pos">the position of the square we want to get</param>
    /// <returns>the square we wanted</returns>
    public GameObject GetSquare(Vector2 pos)
    {
        return this.squaresOfTheBoard[pos];
    }

    public Position GetPositionFromTo(Vector2 coo, Vector2 movement)
    {
        IList<int[,]> osef  = new List<int[,]>();
        return GetPositionFromTo(coo, movement, ref osef);
    }

    /// <summary>
    ///     Return the Position of the square you reach if you add the movement to
    ///     the coordinate passed in parameter on this board
    /// </summary>
    /// <param name="coo">the start coordinate on this board</param>
    /// <param name="movement">the movement you do</param>
    /// <returns>The Position you reach after this movement</returns>
    public Position GetPositionFromTo(Vector2 coo, Vector2 movement, ref IList<int[,]> convertMatrices)
    {

        Position p = new Position();
        Vector2 carry = coo+movement;
        bool passing = false;
        Direction dir = Direction.NONE;

        Vector2 newMovement = new Vector2(movement.x, movement.y);
        //depassement droite
        if (carry.x > columns)
        {
            passing = true;
            dir = Direction.RIGHT;
            newMovement.x -= (columns - coo.x)+1;
            /*p = newBoard.GetPositionFromTo(new Vector2(newBoard.Columns, (rows - coo.y) + 1),
                new Vector2((columns-coo.x)+1-movement.x, -movement.y));
            */
        }
        else if(carry.x <= 0)
        {
            passing = true;
            dir = Direction.LEFT;
            newMovement.x += coo.x;
            /*
            Board newBoard = this.neighbour[Direction.LEFT];
            p = newBoard.GetPositionFromTo(new Vector2(1, (rows - coo.y) + 1),
                new Vector2(-movement.x-((coo.x - 1) + 1), -movement.y));*/
        }
        else if(carry.y > rows)
        {
            passing = true;
            dir = Direction.UP;
            newMovement.y -= (rows - coo.y) + 1;
            /*
            Board newBoard = this.neighbour[Direction.UP];
            p = newBoard.GetPositionFromTo(new Vector2(coo.x, 1),
                new Vector2(movement.x, movement.y - ((rows - coo.y)+1)));*/
        }
        else if(carry.y <= 0)
        {
            passing = true;
            dir = Direction.DOWN;
            newMovement.y += coo.y;
            /*
            Board newBoard = this.neighbour[Direction.DOWN];
            p = newBoard.GetPositionFromTo(new Vector2(coo.x, newBoard.rows),
                new Vector2(movement.x, carry.y));*/
        }
        else
        {
            p = new Position(this, carry);
        }

        if (passing)
        {
            Board newBoard = this.neighbour[dir];
            Passing pass = this.convertMatrices[dir];
            Vector2 newPos = pass.newBeginning(coo, this.rows, this.columns);
            int moveX = (int)(newMovement.x * pass.convertMatrix[0, 0] + newMovement.y * pass.convertMatrix[1, 0]);
            int moveY = (int)(newMovement.x * pass.convertMatrix[0, 1] + newMovement.y * pass.convertMatrix[1, 1]);
            Vector2 newMove = new Vector2(moveX, moveY);
            convertMatrices.Add(pass.convertMatrix);

            p = newBoard.GetPositionFromTo(newPos, newMove, ref convertMatrices);
        }
        return p;
    }

    /// <summary>
    /// Check if the specified position exist for this board
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool IsOnBoard(Vector2 pos)
    {
        return this.squaresOfTheBoard.ContainsKey(pos);
    }

    /// <summary>
    ///     Add a square to the dictionnary of the positions
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="square"></param>
    public void AddASquare(Vector2 pos, GameObject square)
    {
        squaresOfTheBoard.Add(pos, square);
    }

}
