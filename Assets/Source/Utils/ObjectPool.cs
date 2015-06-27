using System;
using System.Collections.Generic;
using Caveman.Weapons;
using UnityEngine;

namespace Caveman.Utils
{
    public class ObjectPool : MonoBehaviour
    {
        public Func<ObjectPool> RelatedPool;
        //public Func<ObjectPool>  

        private Stack<Transform> stack;
        private Transform prefab;

        public void CreatePool(Transform prefab, int initialBufferSize)
        {
            stack = new Stack<Transform>(initialBufferSize);
            this.prefab = prefab;
        }


        //todo объекты не знают о своем пуле 
        public Transform New()
        {
            if (stack.Count > 0)
            {
                var t = stack.Pop();
                t.gameObject.SetActive(true);
                return t;
            }
            else
            {
                var t = Instantiate(prefab);
                if (t.GetComponent<WeaponModelBase>())
                {
                    t.GetComponent<WeaponModelBase>().SetPool(this);    
                }
                if (RelatedPool != null && t.GetComponent<StoneModel>())
                {
                    t.GetComponent<StoneModel>().SetPoolSplash(RelatedPool());
                }

                t.gameObject.SetActive(true);
                return t;
            }
        }

        public void Store(Transform obj)
        {
            obj.gameObject.SetActive(false);
            obj.gameObject.transform.position = new Vector3(100, 100, 100);
            stack.Push(obj);            
        }
    }
}
