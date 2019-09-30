using UnityEngine;
using Harmony12;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameManagement;
using System.Reflection;
using Shred.Mod;
using Shred.Lib;

namespace Shred.Patches {
    [HarmonyPatch(typeof(GameStateMachine), "Update")]
    static class GameStateMachine_Update_Patch {
  
        static bool Prefix(GameStateMachine __instance) {
            if (Main.enabled) {
                //__instance.CurrentState.OnUpdate();
                return true;
            } else {
                return true;
            }
        }
    }

    [HarmonyPatch(typeof(GameState), "CanDoTransitionTo")]
    static class PauseState_CanDoTransitionTo_Patch {
        private static string NullableToString(object obj) {
            return obj == null ? "(null)" : obj.ToString();
        }
        static bool Prefix(GameState __instance, Type targetState, ref bool __result, Type[] ___availableTransitions) {
            if (Main.enabled) {
                __result = ___availableTransitions.Contains(targetState) || StateManager.Instance.CanDoTransition(__instance.GetType(), targetState);

                return false;
            } else {
                return true;
            }
        }
    }
}
