using System;
using SD_Core;

namespace SD_GameLoad
{
    public class SDGameLogicLoader : SDGameLoaderBase
    {
        public override void StartLoad(Action onComplete)
        {
            var dbGameLogic = new SDGameLogic();
            dbGameLogic.LoadManager(() => base.StartLoad(onComplete));
        }
    }
}