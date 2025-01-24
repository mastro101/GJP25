using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eastermaster.TreeBehaviour
{
    public class GenericTree : TreeBehaviour<IContext>
    {
        [SerializeInterface(typeof(IContext))] GameObject ctx;

        private void Start()
        {
            Init(ctx.GetComponent<IContext>());
        }
    }
}
