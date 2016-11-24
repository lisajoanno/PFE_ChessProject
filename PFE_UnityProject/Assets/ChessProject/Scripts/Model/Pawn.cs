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
            // If the team is 1, it's the adverse team so the color is black
            if (team == 1)
            {
                // all renderers need to change color
                Renderer[] tabChildren = gameObject.GetComponentsInChildren<Renderer>();
                foreach (Renderer r in tabChildren)
                {
                    r.material.color = Color.black;
                }
            }
        }
    }

    
}
