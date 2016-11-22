using UnityEngine;

/// <summary>
///     A class to permit the selection of a pawn
/// </summary>
public class SelectPawn : Select
{
    private TeamTurn teamTurn;

    public SelectPawn(Color color, LayerMask mask, TeamTurn teamTurn) : base(color, mask)
    {
        this.teamTurn = teamTurn;
    }


    public override bool CanSelect(GameObject newSelected)
    {
        if (base.CanSelect(newSelected))
        {
            PawnElement pElem = newSelected.GetComponent<PawnElement>();
            return pElem.GetTeam == teamTurn.GetTeamTurn;
        }
        return false;
    }
}
