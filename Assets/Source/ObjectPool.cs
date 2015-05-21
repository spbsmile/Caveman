using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Source
{
    public class ObjectPool<T> where T : class ,new ()
    {
        private Stack<T> objectStack;

        private Action<T> resetAction;
        private Action<T> onetimeInitAction;

        public ObjectPool(int initialBufferSize, Action<T> ResetAction = null, Action<T> OnetimeInitAction = null) 
        {
            objectStack = new Stack<T>(initialBufferSize);
            resetAction = ResetAction;
            onetimeInitAction = OnetimeInitAction;
        }

        public T New()
        {
            if (objectStack.Count > 0)
            {
                T t = objectStack.Pop();
                if (resetAction != null)
                {
                    resetAction(t);
                }
                return t;
            }
            else
            {
                Debug.Log("SDFGSDFGSDFGSDFGSDFGSDFG");
                var t = new T();
                if (onetimeInitAction != null)
                {
                    onetimeInitAction(t);
                }
                return t;
            }
        }

        public void Store(T obj)
        {
             objectStack.Push(obj);            
        }
    }
}
