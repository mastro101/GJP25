using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Eastermaster
{
    public class PoolManager<T> where T : IPoolable
    {
        T poolable;
        T[] objs;

        public PoolManager(T _poolable, int nPoolable)
        {
            poolable = _poolable;
            GameObject parent = new GameObject("Poolable");
            objs = new T[nPoolable];
            for (int i = 0; i < nPoolable; i++)
            {
                objs[i] = InitObj();
            }
        }

        T InitObj()
        {
            T _obj = Object.Instantiate(poolable.gameObject).GetComponent<T>();
            _obj.onPool = true;
            _obj.gameObject.SetActive(false);
            _obj.onReturn += Return;
            return _obj;
        }

        public T Get(Vector3 pos, Vector3 dir, bool active = true)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                T obj = objs[i];
                if (obj.onPool)
                {
                    obj.onPool = false;
                    obj.gameObject.SetActive(true);
                    obj.Spawn(pos, dir);
                    return obj;
                }
            }

            return AddObj();
        }

        public void Return(IPoolable obj)
        {
            if (!obj.onPool)
            {
                obj.gameObject.SetActive(false);
                obj.onPool = true;
            }
        }

        T AddObj()
        {
            T[] oldObjs = objs;
            objs = new T[oldObjs.Length + 1];
            int l = oldObjs.Length;
            for (int i = 0; i < l; i++)
            {
                objs[i] = oldObjs[i];
            }
            return objs[objs.Length - 1] = InitObj();
        }
    } 
}