using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

namespace Eastermaster.Helper
{
    public static class Helper
    {
        public static T[] GetComponentsInNearChildren<T>(this Component mb)
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

        public static T GetComponentInNearChildren<T>(this Component mb)
        {
            int l = mb.transform.childCount;
            for (int i = 0; i < l; i++)
            {
                T obj = mb.transform.GetChild(i).GetComponent<T>();
                if (obj != null)
                    return obj;
            }
            return default;
        }

        public static T GetComponentInNearParents<T>(this Component mb)
        {
            T res = mb.GetComponent<T>();
            if (res != null)
                return res;
            
            Transform _parent = mb.transform.parent;
            while ( _parent != null )
            {
                res = _parent.GetComponent<T>();
                if (res != null)
                    return res;
                _parent = _parent.parent;
            }

            return default;
        }
    }
}
