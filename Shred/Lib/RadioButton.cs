using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shred.Lib {
    public class RadioButton : MenuButton {
        public List<string> options = new List<string>() { "Test Option 1", "Test Option 2", "Test Option 3" };
        public TMP_Text label;

        public int _currentIndex = 0;
        public int CurrentIndex {
            get {
                return _currentIndex;
            }
            set {
                _currentIndex = value % options.Count;
                if (_currentIndex < 0) { _currentIndex += options.Count; }
            }
        }

        public void NextOption() {
            CurrentIndex++;
            UpdateList();
        }

        public void PreviousOption() {
            CurrentIndex--;
            UpdateList();
        }

        public void UpdateList() {
            SetOption(options[CurrentIndex]);
        }

        protected override void Awake() {
            base.Awake();
            label = gameObject.transform.Find("TextMeshPro Text").GetComponent<TMP_Text>();
        }

        public override Selectable FindSelectableOnLeft() {
            if (Application.isPlaying) {
                PreviousOption();
            }
            return null;
        }

        public override Selectable FindSelectableOnRight() {
            if (Application.isPlaying) {
                NextOption();
            }
            return null;
        }

        protected override void OnClicked() {
            base.OnClicked();
            NextOption();
        }

        internal void SetOption(string option) {
            this.label.text = option;
        }
    }
}