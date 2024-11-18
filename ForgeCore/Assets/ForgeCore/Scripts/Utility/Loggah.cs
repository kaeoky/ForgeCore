using Debug = UnityEngine.Debug;
using System.Diagnostics;
using System;
using UnityEngine;

namespace ForgeCore.Utility
{
    public static class Loggah
    {
        public static void Log(string message, LogType logType = LogType.Log, string classColor = "brown",
            string messageColor = "white")
        {
            var i = 1;
            string callingClass;
            
            // Get Class that called the Log Method
            do
            {
                callingClass = new StackTrace().GetFrame(i).GetMethod().ReflectedType?.Name;
                i++;
            } while (callingClass == nameof(Loggah));

            // Setup Message
            var caller = string.Concat($"<b><color={classColor}><", callingClass, "></color></b>");
            var log = string.Concat($"<color={messageColor}>", message, "</color>");

            // Log the Message depending on LogType
            switch (logType)
            {
                case LogType.Log:
                    Debug.Log(string.Concat(caller, " ", log));
                    break;
                case LogType.Warning:
                    Debug.LogWarning(string.Concat(caller, " ", log));
                    break;
                case LogType.Error:
                    Debug.LogError(string.Concat(caller, " ", log));
                    break;
                case LogType.Exception:
                    Debug.LogException(new Exception(string.Concat(caller, " ", log)));
                    break;
                case LogType.Assert:
                    Debug.LogAssertion(string.Concat(caller, " ", log));
                    break;
                default:
                    Debug.Log(string.Concat(caller, " ", log));
                    break;
            }
        }

        public static void Log(string message, string classColor, string messageColor = "white")
        {
            Log(message, LogType.Log, classColor, messageColor);
        }
    }
}