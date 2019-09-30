using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Shred.Lib {
    class CursorManager : MonoBehaviour {
        private static object padlock = new object();

        public static CursorManager Instance { get; private set; }

        private void Awake() {
            lock (padlock) {
                if (CursorManager.Instance != null) {
                    Debug.LogError("More than one CursorManager");
                } else {
                    CursorManager.Instance = this;
                }
            }
        }
    }
}
