using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Shred.Lib {
    class MenuManager : MonoBehaviour {
        private static object padlock = new object();

        public static MenuManager Instance { get; private set; }

        private void Awake() {
            lock (padlock) {
                if (MenuManager.Instance != null) {
                    Debug.LogError("More than one MenuManager");
                } else {
                    MenuManager.Instance = this;
                }
            }
        }
    }
}
