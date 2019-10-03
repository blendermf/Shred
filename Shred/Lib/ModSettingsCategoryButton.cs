using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shred.Lib {
    public class ModSettingsCategoryButton : MenuButton {
        public ModSettingsController modSettingsController;

        public TMP_Text label;

        protected override void Awake() {
            base.Awake();
            label = gameObject.transform.Find("TextMeshPro Text").GetComponent<TMP_Text>();
        }

        public override Selectable FindSelectableOnLeft() {
            if (Application.isPlaying) {
                this.modSettingsController.PreviousCategory();
            }
            return null;
        }

        public override Selectable FindSelectableOnRight() {
            if (Application.isPlaying) {
                this.modSettingsController.NextCategory();
            }
            return null;
        }

        protected override void OnClicked() {
            base.OnClicked();
            this.modSettingsController.NextCategory();
        }

        internal void SetCategory(string modSettingCategory) {
            this.label.text = modSettingCategory;
        }
    }
 }