using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameManagement;

namespace Shred.Lib {
    using MenuButtonExtensions;

    public class ModSettingsCategoryButton : MenuButton {
        public ModSettingsController modSettingsController;

        public TMP_Text label;

        protected override void Awake() {
            base.Awake();
            label = gameObject.transform.Find("TextMeshPro Text").GetComponent<TMP_Text>();
        }

        protected override void Start()
        {
            this.SetupStyles();
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