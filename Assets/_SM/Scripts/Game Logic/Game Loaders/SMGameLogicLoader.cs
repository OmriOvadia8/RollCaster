using System;
using SM_Core;

namespace SM_GameLoad
{
    public class SMGameLogicLoader : SMGameLoaderBase
    {
        public override void StartLoad(Action onComplete)
        {
            var dbGameLogic = new SMGameLogic();
            dbGameLogic.LoadManager(() => base.StartLoad(onComplete));
        }
    }
}