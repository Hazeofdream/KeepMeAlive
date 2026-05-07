namespace KeepMeAlive.Helpers
{
    //====================[ RevivalDebugLog ]====================
    internal static class RevivalDebugLog
    {
        //====================[ Flags ]====================
        public static bool IsDebugLogsEnabled => KeepMeAliveSettings.ENABLE_DEBUG_LOGS;

        public static bool IsReviveFlowEnabled => IsDebugLogsEnabled && (KeepMeAliveSettings.DEBUG_REVIVE_FLOW);

        public static bool IsNetworkTraceEnabled => IsDebugLogsEnabled && (KeepMeAliveSettings.DEBUG_NETWORK_TRACE);

        public static bool IsSelfReviveTraceEnabled => IsDebugLogsEnabled && (KeepMeAliveSettings.DEBUG_SELF_REVIVE_TRACE);

        //====================[ Logging API ]====================
        public static void LogDebug(string message)
        {
            if (!IsDebugLogsEnabled) return;
            Plugin.LogSource.LogDebug(message);
        }

        public static void LogReviveFlow(string message)
        {
            if (!IsReviveFlowEnabled) return;
            Plugin.LogSource.LogInfo(message);
        }

        public static void LogNetworkTrace(string message)
        {
            if (!IsNetworkTraceEnabled) return;
            Plugin.LogSource.LogInfo(message);
        }

        public static void LogSelfReviveTrace(string message)
        {
            if (!IsSelfReviveTraceEnabled) return;
            Plugin.LogSource.LogInfo(message);
        }
    }
}