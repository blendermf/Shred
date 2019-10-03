﻿using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using GameManagement;

namespace Shred.Lib {
    class MenuManager : MonoBehaviour {
        private static readonly object padlock = new object();

        public static MenuManager Instance { get; private set; }

        private GameObject ModSettingsButton;
        private GameObject QuitButton;

        public GameObject ModSettingsMenu;

        private void Awake() {
            lock (padlock) {
                if (MenuManager.Instance != null) {
                    Debug.LogError("More than one MenuManager");
                } else {
                    MenuManager.Instance = this;
                }
            }
        }

        private void Start() {
            //StartCoroutine(AppendMenuButtons());
            AppendMenuButtons();
        }

        private void Update() {
        }

        private GameObject AddMainMenuButton(string name, UnityAction action = null, int siblingIndex = -1) {
            GameObject button = UnityEngine.Object.Instantiate<GameObject>(FindGameObjectByName("Gear Button"));
            button.GetComponentInChildren<TextMeshProUGUI>().SetText(name);

            button.GetComponentInChildren<MenuButton>().onClick = new Button.ButtonClickedEvent();
            if (!(action == null)) {
                button.GetComponentInChildren<MenuButton>().onClick.AddListener(action);
            }

            button.GetComponent<RectTransform>().SetParent(FindGameObjectByName("Buttons").GetComponent<RectTransform>(), false);

            if (siblingIndex > 0) {
                button.GetComponent<RectTransform>().SetSiblingIndex(4);
            }

            return button;
        }

        private void AppendMenuButtons() {
            //yield return new WaitForSecondsRealtime(0.1f);

            StateManager.Instance.SetAllowedTransition(typeof(PauseState), typeof(ModSettingsState));
            StateManager.Instance.SetAllowedTransition(typeof(PlayState), typeof(ModSettingsState));
            StateManager.Instance.SetAllowedTransition(typeof(ModSettingsState), typeof(PauseState));
            StateManager.Instance.SetAllowedTransition(typeof(ModSettingsState), typeof(PlayState));

            ModSettingsButton = AddMainMenuButton("Mod Settings", () => { GameStateMachine.Instance.RequestTransitionTo(typeof(ModSettingsState)); }, 4);
            QuitButton = AddMainMenuButton("Quit", () => { Application.Quit(); });

            ModSettingsMenu = UnityEngine.Object.Instantiate<GameObject>(GameStateMachine.Instance.LevelSelectionObject);

            LevelSelectionController levelSelectionController = ModSettingsMenu.GetComponentInChildren<LevelSelectionController>();
            LevelCategoryButton levelCategoryButton = ModSettingsMenu.GetComponentInChildren<LevelCategoryButton>();

            GameObject levelListObject = levelSelectionController.gameObject;
            GameObject levelCategoryButtonObject = levelCategoryButton.gameObject;

            TMP_FontAsset normalFont = levelCategoryButton.normalFont;
            TMP_FontAsset highlightedFont = levelCategoryButton.highlightedFont;
            Selectable.Transition transition = levelCategoryButton.transition;
            ColorBlock color = levelCategoryButton.colors;

            DestroyImmediate(levelSelectionController, false);
            DestroyImmediate(levelCategoryButton, false);

            ModSettingsController modSettingsController = levelListObject.gameObject.AddComponent<ModSettingsController>() as ModSettingsController;
            ModSettingsCategoryButton modSettingsCategoryButton = levelCategoryButtonObject.gameObject.AddComponent<ModSettingsCategoryButton>() as ModSettingsCategoryButton;
            modSettingsCategoryButton.normalFont = normalFont;
            modSettingsCategoryButton.highlightedFont = highlightedFont;
            modSettingsCategoryButton.transition = transition;
            modSettingsCategoryButton.colors = color;
            modSettingsCategoryButton.texts = modSettingsCategoryButton.GetComponentsInChildren<TMP_Text>().ToList();
            modSettingsCategoryButton.label = modSettingsCategoryButton.transform.Find("TextMeshPro Text").GetComponent<TextMeshProUGUI>();
            modSettingsCategoryButton.targetGraphic = modSettingsCategoryButton.transform.Find("TextMeshPro Text").GetComponent<TextMeshProUGUI>();
            modSettingsController.modSettingsCategoryButton = modSettingsCategoryButton;
            modSettingsCategoryButton.modSettingsController = modSettingsController;
            
            Util.DumpComponent(modSettingsCategoryButton, "    ", true);
            //yield return null;
        }

        private static GameObject FindGameObjectByName(string name) {
            foreach (GameObject gameObject in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[]) {
                if (gameObject.name == name) {
                    return gameObject;
                }
            }
            return null;
        }

        private void OnDestroy() {
            Destroy(ModSettingsButton);
            Destroy(QuitButton);
            Instance = null;
        }
    }
}
