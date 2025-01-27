using UnityEngine;

public interface IDetectable
{
    Transform transform { get; }
    bool Detectable { get; }

    event System.Action<bool> OnDetectableChange;
}