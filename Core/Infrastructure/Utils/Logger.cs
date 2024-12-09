using UnityEngine;

namespace Core.Infrastructure.Utils
{
    public static class Logger
    {
        private static readonly Color LogColor = Color.green;
        private static readonly Color WarningColor = Color.yellow;
        private static readonly Color ErrorColor = Color.red;
        
        public static void Log(object obj)
        {
            LogInternal(GetClassName(), obj, LogColor);
        }
        public static void Log(this object thisObj, object obj)
        {
            LogInternal(thisObj.GetType().ToString(), obj, LogColor);
        }
        
        public static void LogWarning(object obj)
        {
            LogInternal(GetClassName(), obj, WarningColor);
        }
        public static void LogWarning(this object thisObj, object obj)
        {
            LogInternal(thisObj.GetType().ToString(), obj, WarningColor);
        }
        
        public static void LogError(object obj)
        {
            LogInternal(GetClassName(), obj, ErrorColor);
        }
        public static void LogError(this object thisObj, object obj)
        {
            LogInternal(thisObj.GetType().ToString(), obj, ErrorColor);
        }


        private static string GetClassName()
        {
            var stackTrace = new System.Diagnostics.StackTrace();
            var className = stackTrace.GetFrame(stackTrace.FrameCount - 2).GetMethod().Name;

            return className;
        }
        private static void LogInternal(string className, object obj, Color color)
        {
            var message = "[" + className  + "] " + obj;
            var coloredMessage = $"<color=#{(byte)(color.r * 255f):X2}{(byte)(color.g * 255f):X2}{(byte)(color.b * 255f):X2}>{message}</color>";
            Debug.Log(coloredMessage);
        }
    }
}