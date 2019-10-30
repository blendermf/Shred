using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Shred.Lib {
    public class ModSettingsController : ListViewController {
        public List<string> availableMods = new List<string>(){"TestMod1", "TestMod2", "TestMod3"};
        public ModSettingsCategoryButton modSettingsCategoryButton;
        public int _currentIndex = 0;
        public int CurrentIndex {
            get {
                return _currentIndex;
            }
            set {
                _currentIndex = value % availableMods.Count;
                if (_currentIndex < 0) { _currentIndex += availableMods.Count; }
            }
        }

        public void Start() {
            UpdateCategoryList();
        }

        public void NextCategory() {
            CurrentIndex++;
            UpdateCategoryList();
        }

        public void PreviousCategory() {
            CurrentIndex--;
            UpdateCategoryList();
        }

        public void UpdateCategoryList() {
            modSettingsCategoryButton.SetCategory(availableMods[CurrentIndex]);
        }

        protected override void Update() {
            base.Update();
            if (EventSystem.current.currentSelectedGameObject == null || !EventSystem.current.currentSelectedGameObject.activeInHierarchy) {
                EventSystem.current.SetSelectedGameObject(modSettingsCategoryButton.gameObject);
            }
            if (PlayerController.Instance.inputController.player.GetButtonDown("RB")) {
                this.NextCategory();
            }
            if (PlayerController.Instance.inputController.player.GetButtonDown("LB")) {
                this.PreviousCategory();
            }
        }
    }
}
