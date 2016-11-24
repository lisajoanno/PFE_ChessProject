using UnityEngine;

/// <summary>
///     A class to permit the selection of a pawn
/// </summary>
public class SelectPawn : Select
{
    //private TeamTurn teamTurn;

    public SelectPawn(Color color, LayerMask mask/*, TeamTurn teamTurn*/) : base(color, mask)
    {
        //this.teamTurn = teamTurn;
    }


    public override bool CanSelect(GameObject newSelected)
    {
        if (base.CanSelect(newSelected))
        {
            Pawn pElem = newSelected.GetComponent<Pawn>();
            return true;
            //return pElem.GetTeam == teamTurn.GetTeamTurn;
        }
        return false;
    }

    protected override void ColorNewlySelectedGameObject()
    {
        base.ColorNewlySelectedGameObject();

        //put a special color on the squares you can move
        Pawn pawn = LastSelected.GetComponent<Pawn>();
        /*foreach (Position pos in pawn.MoveCases)
        {
            pos.board.GetSquare(pos.coo).GetComponent<Renderer>().material.color = possibleSquaresColor;
        }*/

    }

    protected override void ResetColor()
    {
        base.ResetColor();
        Pawn pawn = LastSelected.GetComponent<Pawn>();

        Position pos = pawn.Position;
        //remove the special color on the squares you can move

        /*
        Square square;
        GameObject squareGameObject;
        foreach (Position pos in pawn.MoveCases)
        {
            squareGameObject = pos.board.GetSquare(pos.coo);
            square = squareGameObject.GetComponent<Square>();
            squareGameObject.GetComponent<Renderer>().material.color = square.Color;
        }*/
    }
}
