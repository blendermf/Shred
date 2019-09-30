using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GameManagement;

namespace Shred.Lib {
    class StateManager : MonoBehaviour {
        private Dictionary<Type, HashSet<Type>> allowedTransitions = new Dictionary<Type, HashSet<Type>>();
        private static object padlock = new object();

        public static StateManager Instance { get; private set; }

        private void Awake() {
            lock (padlock) {
                if (StateManager.Instance != null) {
                    Debug.LogError("More than one StateManager");
                } else {
                    StateManager.Instance = this;
                }
            }
        }

        public void SetAllowedTransition(Type fromState, Type toState) {
            if (!allowedTransitions.ContainsKey(fromState)) {
                allowedTransitions[fromState] = new HashSet<Type>();
            }

            allowedTransitions[fromState].Add(toState);
        }

        public void RemoveAllowedTransition(Type fromState, Type toState) {
            if (allowedTransitions.ContainsKey(fromState)) {
                allowedTransitions[fromState].Remove(toState);

                if (!allowedTransitions[fromState].Any()) {
                    allowedTransitions.Remove(fromState);
                }
            }
        }

        public bool CanDoTransition(Type fromState, Type toState) {
            return allowedTransitions.ContainsKey(fromState) && allowedTransitions[fromState].Contains(toState);
        }
    }
}
