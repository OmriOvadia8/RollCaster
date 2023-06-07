using System;
using System.Diagnostics;
using Firebase.Crashlytics;

namespace SM_Core
{
    public class SMCrashManager
    {
        public SMCrashManager()
        {
            SMDebug.Log($"SMCrashManager");

            Crashlytics.ReportUncaughtExceptionsAsFatal = true;
        }

        public void LogExceptionHandling(string message)
        {
            Crashlytics.LogException(new Exception(message));
            SMDebug.LogException(message);
        }

        public void LogBreadcrumb(string message)
        {
            Crashlytics.Log(message);
            SMDebug.Log(message);
        }
    }
}