using UnityEngine;
using System.Collections;

public abstract class Trap : MonoBehaviour {
    protected Square square;
    public Square Square
    {
        get
        {
            return this.square;
        }
        set
        {
            this.square = value;
        }
    }

    void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        this.square = gameObject.GetComponentInParent<Square>();
    }

    public virtual void Apply()
    {
        Debug.Log("[TRAP]The trap is abstract, it doesn't do anything.");
    }
}
