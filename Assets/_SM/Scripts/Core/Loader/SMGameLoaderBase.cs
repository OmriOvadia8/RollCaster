using System;

namespace SM_Core
{
    public class SMGameLoaderBase : SMMonoBehaviour
    {
        public virtual void StartLoad(Action onComplete)
        {
            onComplete.Invoke();
        }
    }
}