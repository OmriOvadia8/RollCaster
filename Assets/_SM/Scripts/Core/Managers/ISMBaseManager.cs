using System;

namespace SM_Core
{
    public interface ISMBaseManager
    {
        public void LoadManager(Action onComplete);
    }
}