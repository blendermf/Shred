using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Shred.Lib {

    public class RadioButton : ListViewMenuControlItem {
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

        protected override void OnEnable()
        {
            base.OnEnable();

            StartCoroutine(SetupUI());
        }

        IEnumerator SetupUI()
        {
            yield return new WaitUntil(() => gameObject.activeInHierarchy);
            yield return new WaitForEndOfFrame();

            RectTransform t = transform as RectTransform;

            t.anchoredPosition = new Vector2(0, -64);
            t.anchorMin = new Vector2(0, 1);
            t.anchorMax = new Vector2(1, 1);
            t.offsetMin = new Vector2(52, 0);
            t.offsetMax = new Vector2(-52, 0);
            t.pivot = new Vector2(0.5f, 1);


            RectTransform textTransform = label.transform as RectTransform;

            label.alignment = TextAlignmentOptions.Center;
            label.overflowMode = TextOverflowModes.Overflow;

            textTransform.anchorMin = new Vector2(0, 1);
            textTransform.offsetMin = new Vector2(20, 0);
            textTransform.offsetMax = new Vector2(-20, 0);
            textTransform.pivot = new Vector2(0.5f, 1);
            textTransform.sizeDelta = new Vector2(-40, 40);
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

            GameObject left = UnityEngine.Object.Instantiate<GameObject>(Util.FindGameObjectByName("Left Arrow"));
            GameObject text = UnityEngine.Object.Instantiate<GameObject>(Util.FindGameObjectByName("TextMeshPro Text"));
            GameObject right = UnityEngine.Object.Instantiate<GameObject>(Util.FindGameObjectByName("Right Arrow"));

            left.transform.SetParent(transform, false);
            text.transform.SetParent(transform, false);
            right.transform.SetParent(transform, false);

            texts = GetComponentsInChildren<TMP_Text>().ToList();

            label = text.GetComponent<TMP_Text>();
        }

        protected override void Start() {
            base.Start();
            UpdateList();
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