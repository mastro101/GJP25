using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eastermaster
{
    public class ViewTrigger : MonoBehaviour
    {
        [SerializeField][HideInInspector] protected float radius = 1f;
        [SerializeField][HideInInspector] protected float viewAngle = 45f;

        [SerializeField][SerializeInterface(typeof(IPoolable))] Object g;
    }
}