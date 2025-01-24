using UnityEngine;
using Eastermaster.TreeBehaviour;

public class KatanaBehaviourTree : TreeBehaviour<Katana>
{
    [SerializeField] Katana katana;

    private void Start()
    {
        Init(katana);
    }
}