using System;
using Firebase.Crashlytics;

namespace SD_Core
{
    /// <summary>
    /// Manages logging and reporting of exceptions and errors for the game using Firebase Crashlytics.
    /// </summary>
    public class SDCrashManager
    {
        /// <summary>
        /// Initializes a new instance of the SDCrashManager class and sets Crashlytics to report uncaught exceptions as fatal.
        /// </summary>
        public SDCrashManager()
        {
            SDDebug.Log($"SDCrashManager");

            Crashlytics.ReportUncaughtExceptionsAsFatal = true;
        }

        /// <summary>
        /// Logs an exception message to Crashlytics and to the debug console.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        public void LogExceptionHandling(string message)
        {
            Crashlytics.LogException(new Exception(message));
            SDDebug.LogException(message);
        }

        /// <summary>
        /// Logs a breadcrumb message to Crashlytics and to the debug console.
        /// Breadcrumbs are used to mark point of interest in your code for debugging.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        public void LogBreadcrumb(string message)
        {
            Crashlytics.Log(message);
            SDDebug.Log(message);
        }
    }
}
