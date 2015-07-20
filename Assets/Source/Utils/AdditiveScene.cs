using UnityEngine;
using System.Collections;

namespace Caveman.Utils
{
    public abstract class AdditiveScene : MonoBehaviour
    {
        public abstract string scene { get; }

        protected virtual bool BeforeLoad()
        {
            return true;
        }

        protected virtual void AfterLoad()
        {
            Destroy(this);
        }

        public virtual IEnumerator Start()
        {
            if (BeforeLoad())
            {
                yield return Application.LoadLevelAdditiveAsync(scene);
                AfterLoad();
            }
        }
    }
}