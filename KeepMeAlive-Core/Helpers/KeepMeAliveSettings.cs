//====================[ Imports ]====================
using BepInEx.Configuration;
using UnityEngine;

namespace KeepMeAlive.Helpers
{
    //====================[ KeepMeAliveSettings ]====================
    internal class KeepMeAliveSettings
    {
        //====================[ Local-Only Settings ]====================
        // Key Bindings
        public static ConfigEntry<KeyCode> SELF_REVIVAL_KEY;
        public static ConfigEntry<KeyCode> GIVE_UP_KEY;

        // Debug / Diagnostics
        public static ConfigEntry<bool> ENABLE_DEBUG_LOGS;
        public static ConfigEntry<bool> DEBUG_REVIVE_FLOW;
        public static ConfigEntry<bool> DEBUG_NETWORK_TRACE;
        public static ConfigEntry<bool> DEBUG_SELF_REVIVE_TRACE;
        public static ConfigEntry<bool> DEBUG_KEYBINDS;

        //====================[ Init ]====================
        public static void Init(ConfigFile config)
        {
            //====================[ Key Bindings ]====================
            SELF_REVIVAL_KEY = config.Bind(
                "1. Key Bindings",
                "Self Revival Key",
                KeyCode.F,
                "The key to press and hold to revive yourself when in critical state"
            );

            GIVE_UP_KEY = config.Bind(
                "1. Key Bindings",
                "Give Up Key",
                KeyCode.Backspace,
                "Press this key when in critical state to die immediately"
            );

            //====================[ Debug ]====================
            ENABLE_DEBUG_LOGS = config.Bind(
                "5. Development",
                "Enable Debug Logs",
                false,
                new ConfigDescription("Enables debug logging output for revival systems", null, new ConfigurationManagerAttributes { IsAdvanced = true })
            );

            DEBUG_REVIVE_FLOW = config.Bind(
                "5. Development",
                "Debug Revive Flow",
                false,
                new ConfigDescription("Logs detailed revive flow transitions (requires Enable Debug Logs)", null, new ConfigurationManagerAttributes { IsAdvanced = true })
            );

            DEBUG_NETWORK_TRACE = config.Bind(
                "5. Development",
                "Debug Network Trace",
                false,
                new ConfigDescription("Logs revive-related network packet traces (requires Enable Debug Logs)", null, new ConfigurationManagerAttributes { IsAdvanced = true })
            );

            DEBUG_SELF_REVIVE_TRACE = config.Bind(
                "5. Development",
                "Debug Self Revive Trace",
                false,
                new ConfigDescription("Logs self-revive lifecycle details (requires Enable Debug Logs)", null, new ConfigurationManagerAttributes { IsAdvanced = true })
            );

            DEBUG_KEYBINDS = config.Bind(
                "5. Development",
                "Debug Keybinds",
                false,
                new ConfigDescription("Enables debug keybinds: F7=Enter Ghost Mode, F8=Exit Ghost Mode", null, new ConfigurationManagerAttributes { IsAdvanced = true })
            );
        }
    }
}
