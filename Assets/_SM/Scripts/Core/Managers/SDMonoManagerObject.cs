namespace SD_Core
{
    public class SDMonoManagerObject : SDMonoBehaviour
    {
        private void OnApplicationPause(bool pauseStatus)
        {
            Manager.EventsManager.InvokeEvent(SDEventNames.OnPause, pauseStatus);
        }
    }
}