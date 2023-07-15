namespace SD_Core
{
    public class SDMonoManagerObject : SDMonoBehaviour
    {
        private void OnApplicationPause(bool pauseStatus)
        {
            if (Manager == null) return;
            if (Manager.EventsManager == null) return;

            Manager.EventsManager.InvokeEvent(SDEventNames.OnPause, pauseStatus);
        }
    }
}