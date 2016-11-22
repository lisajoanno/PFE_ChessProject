using UnityEngine;
using System.Collections.Generic;

public class PositionSquares : MonoBehaviour{
    private IDictionary<Vector2, GameObject> positions = new Dictionary<Vector2, GameObject>();
    public IDictionary<Vector2, GameObject> GetPositions
    {
        get
        {
            return positions;
        }
    }
}
