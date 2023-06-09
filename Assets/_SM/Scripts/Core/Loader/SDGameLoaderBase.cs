using System;

namespace SD_Core
{
    public class SDGameLoaderBase : SDMonoBehaviour
    {
        public virtual void StartLoad(Action onComplete)
        {
            onComplete.Invoke();
        }
    }
}