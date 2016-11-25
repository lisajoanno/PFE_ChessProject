using UnityEngine;
using System.Collections;

public class Pawn : ChessElement {

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

            // all renderers need to change color
            Renderer[] tabChildren = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in tabChildren)
            {
                r.material.color = colorTeam;
            }
            
        }
    }

    
}
