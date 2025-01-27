using System;
using UnityEngine;

public class TestDetectable : MonoBehaviour, IDetectable
{
    public bool Detectable => true;

    public event Action<bool> OnDetectableChange;

    void asdf()
    {
        if (OnDetectableChange != null)
        {
            //bravo
        }
    }
}