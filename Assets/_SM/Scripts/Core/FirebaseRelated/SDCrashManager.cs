using System;
using System.Diagnostics;
using Firebase.Crashlytics;

namespace SD_Core
{
    public class SDCrashManager
    {
        public SDCrashManager()
        {
            SDDebug.Log($"SDCrashManager");

            Crashlytics.ReportUncaughtExceptionsAsFatal = true;
        }

        public void LogExceptionHandling(string message)
        {
            Crashlytics.LogException(new Exception(message));
            SDDebug.LogException(message);
        }

        public void LogBreadcrumb(string message)
        {
            Crashlytics.Log(message);
            SDDebug.Log(message);
        }
    }
}