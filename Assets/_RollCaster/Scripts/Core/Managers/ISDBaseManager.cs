using System;

namespace SD_Core
{
    public interface ISDBaseManager
    {
        public void LoadManager(Action onComplete);
    }
}