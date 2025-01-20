using System.Collections.Generic;
using UnityEngine;

namespace Eastermaster.Helper
{
    public static class Helper
    {
        public static T[] GetComponentsInNearChild<T>(this MonoBehaviour mb) where T : MonoBehaviour
        {
            List<T> res = new List<T>();
            int l = mb.transform.childCount;
            for (int i = 0; i < l; i++)
            {
                T obj = mb.transform.GetChild(i).GetComponent<T>();
                if (obj != null)
                    res.Add(obj);
            }
            return res.ToArray();
        }
    }
}
