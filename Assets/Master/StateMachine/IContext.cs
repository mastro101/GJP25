using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eastermaster
{
    public interface IContext
    {
        Transform transform { get; }
    }
}
