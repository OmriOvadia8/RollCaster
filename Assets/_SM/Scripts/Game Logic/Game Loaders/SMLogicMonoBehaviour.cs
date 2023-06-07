using SM_Core;

namespace SM_GameLoad
{
    public class SMLogicMonoBehaviour : SMMonoBehaviour
    {
        public SMGameLogic GameLogic => SMGameLogic.Instance;
    }
}