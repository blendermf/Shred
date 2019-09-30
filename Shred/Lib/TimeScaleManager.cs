using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Shred.Lib {
    class TimeScaleManager : MonoBehaviour {
        private static readonly object padlock = new object();

        public static TimeScaleManager Instance { get; private set; }

        private void Awake() {
            lock (padlock) {
                if (TimeScaleManager.Instance != null) {
                    Debug.LogError("More than one TimeScaleManager");
                } else {
                    TimeScaleManager.Instance = this;
                }
            }
        }
    }
}
