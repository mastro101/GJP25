using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public virtual void OnEnter()     { }
    public virtual void OnExit()      { }
    public virtual void OnTick()      { }
    public virtual void OnFixedTick() { }
}
