namespace SM_Core
{
    public class SMMonoManagerObject : SMMonoBehaviour
    {
        private void OnApplicationPause(bool pauseStatus)
        {
            Manager.EventsManager.InvokeEvent(SMEventNames.OnPause, pauseStatus);
        }
    }
}