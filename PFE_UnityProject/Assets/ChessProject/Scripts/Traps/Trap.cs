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
        this.square = gameObject.GetComponentInParent<Square>();
    }

    public virtual void Apply()
    {
        Debug.Log("[TRAP]The trap is abstract, it doesn't do anything.");
    }
    public virtual void ApplyEffectOnServer(ConnexionManager coo)
    {
        Debug.Log("[TRAP][SERVER]The trap is abstract, it doesn't do anything on the server.");
    }
}
