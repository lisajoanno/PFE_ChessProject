using UnityEngine;

/// <summary>
///     A ChessElement is an object necessited by a chess game
///     as the square of the board or the pawns
/// </summary>
public class ChessElement : MonoBehaviour {

    private Position position;            //the position of the element for the chessboard (eg (3, 4) for D5)
    public Position Position
    {
        get
        {
            return position;
        }
        set
        {
            position = value;
        }
    }

    // The color
    private Color color;
    public Color Color
    {
        get
        {
            return color;
        }
        set
        {
            this.color = value;
        }
    }

    /// <summary>
    /// Colors all pawn element to its original color (if it's a pawn : set at the beginning, depending on the team).
    /// </summary>
    public void ResetChessElementColor()
    {
        SetChessElementColor(this.color);
    }

    /// <summary>
    /// Allows to change the color of the chess element (needs to be overrid by each type of chess element).
    /// </summary>
    /// <param name="newColor">the color to set to the material</param>
    public virtual void SetChessElementColor(Color newColor) { }

}
