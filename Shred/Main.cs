using UnityEngine;
using Harmony12;
using System.Reflection;
using UnityModManagerNet;
using System;

namespace Shred.Mod {

    [Serializable]
    public class Settings : UnityModManager.ModSettings {

        public Settings() : base() {

        }

        public override void Save(UnityModManager.ModEntry modEntry) {

            UnityModManager.ModSettings.Save<Settings>(this, modEntry);

        }
    }

    static class Main {

        public static bool enabled = false;
        public static Settings settings;
        public static String modId;
        public static HarmonyInstance harmonyInstance;

        static bool Load(UnityModManager.ModEntry modEntry) {

            settings = Settings.Load<Settings>(modEntry);
            modId = modEntry.Info.Id;
            modEntry.OnToggle = OnToggle;

            return true;

        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {

            if (enabled == value) return true;
            enabled = value;

            if (enabled) {
                // Do patching on enable
                harmonyInstance = HarmonyInstance.Create(modEntry.Info.Id);
                harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
            } else {
                // Remove patches when disabled
                harmonyInstance.UnpatchAll(harmonyInstance.Id);
            }

            return true;

        }
    }

}