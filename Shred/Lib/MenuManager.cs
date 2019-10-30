using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using GameManagement;
using System.IO;
using UnityEngine.SceneManagement;

namespace Shred.Lib {
    class MenuManager : MonoBehaviour
    {
        private static readonly object padlock = new object();

        public static MenuManager Instance { get; private set; }

        private GameObject ModSettingsButton;
        private GameObject QuitButton;

        public GameObject ModSettingsMenu;

        private TMP_FontAsset normalFont;
        private TMP_FontAsset highlightedFont;
        private Selectable.Transition transition;
        private ColorBlock color;

        private void Awake()
        {
            lock (padlock)
            {
                if (MenuManager.Instance != null)
                {
                    Debug.LogError("More than one MenuManager");
                }
                else
                {
                    MenuManager.Instance = this;
                }
            }
        }

        private void Start()
        {
            Init();
            SetupMainMenuButtons();
            SetupModSettingsMenu();
        }

        private void Init()
        {
            LevelCategoryButton levelCategoryButton = GameStateMachine.Instance.LevelSelectionObject.GetComponentInChildren<LevelCategoryButton>();

            normalFont = levelCategoryButton.normalFont;
            highlightedFont = levelCategoryButton.highlightedFont;
            transition = levelCategoryButton.transition;
            color = levelCategoryButton.colors;
        }

        private GameObject AddMainMenuButton(string name, UnityAction action = null, int siblingIndex = -1)
        {
            GameObject button = UnityEngine.Object.Instantiate<GameObject>(FindGameObjectByName("Gear Button"));
            button.GetComponentInChildren<TextMeshProUGUI>().SetText(name);

            button.GetComponentInChildren<MenuButton>().onClick = new Button.ButtonClickedEvent();
            if (!(action == null))
            {
                button.GetComponentInChildren<MenuButton>().onClick.AddListener(action);
            }

            button.transform.SetParent(FindGameObjectByName("Buttons").transform, false);

            if (siblingIndex > 0)
            {
                button.transform.SetSiblingIndex(4);
            }

            return button;
        }

        private void SetupButtonStyle(MenuButton button)
        {
            button.normalFont = normalFont;
            button.highlightedFont = highlightedFont;
            button.transition = transition;
            button.colors = color;
        }

        private void SetupMainMenuButtons()
        {
            ModSettingsButton = AddMainMenuButton("Mod Settings", () => { GameStateMachine.Instance.RequestTransitionTo(typeof(ModSettingsState)); }, 4);
            QuitButton = AddMainMenuButton("Quit", () => { Application.Quit(); });
        }

        private void SetupModSettingsMenu()
        {
            StateManager.Instance.SetAllowedTransition(typeof(PauseState), typeof(ModSettingsState));
            StateManager.Instance.SetAllowedTransition(typeof(PlayState), typeof(ModSettingsState));
            StateManager.Instance.SetAllowedTransition(typeof(ModSettingsState), typeof(PauseState));
            StateManager.Instance.SetAllowedTransition(typeof(ModSettingsState), typeof(PlayState));

            ModSettingsMenu = UnityEngine.Object.Instantiate<GameObject>(GameStateMachine.Instance.LevelSelectionObject);

            Transform levelList = ModSettingsMenu.transform.Find("Panel/LevelList");
            Transform levelCategoryButton = ModSettingsMenu.transform.Find("Panel/Category Button");

            // Not great, might move code after to happen in a coroutine in the next frame so I can use normal Destroy,
            // the false being passed at least makes it a little safer
            DestroyImmediate(levelList.GetComponent<LevelSelectionController>(), false);
            DestroyImmediate(levelCategoryButton.GetComponent<LevelCategoryButton>(), false);

            ModSettingsController modSettingsController = levelList.gameObject.AddComponent<ModSettingsController>() as ModSettingsController;
            ModSettingsCategoryButton modSettingsCategoryButton = levelCategoryButton.gameObject.AddComponent<ModSettingsCategoryButton>() as ModSettingsCategoryButton;

            SetupButtonStyle(modSettingsCategoryButton);
            modSettingsCategoryButton.texts = modSettingsCategoryButton.GetComponentsInChildren<TMP_Text>().ToList();

            modSettingsController.modSettingsCategoryButton = modSettingsCategoryButton;
            modSettingsCategoryButton.modSettingsController = modSettingsController;

            Transform content = levelList.Find("Viewport/Content");

            Destroy(content.Find("Instruction Button").gameObject);

            GameObject button = new GameObject("Radio Button", typeof(RectTransform));
            //GameObject button = UnityEngine.Object.Instantiate<GameObject>(GameStateMachine.Instance.LevelSelectionObject.transform.Find("Panel/Category Button").gameObject);
            //DestroyImmediate(button.GetComponent<LevelCategoryButton>(), false);
            
            
            button.transform.SetParent(content, false);
            
            GameObject left = UnityEngine.Object.Instantiate<GameObject>(FindGameObjectByName("Left Arrow"));
            GameObject text = UnityEngine.Object.Instantiate<GameObject>(FindGameObjectByName("TextMeshPro Text"));
            GameObject right = UnityEngine.Object.Instantiate<GameObject>(FindGameObjectByName("Right Arrow"));
            left.transform.SetParent(button.transform, false);
            text.transform.SetParent(button.transform, false);
            right.transform.SetParent(button.transform, false);
            left.GetComponent<TMP_Text>().ForceMeshUpdate(true);
            text.GetComponent<TMP_Text>().ForceMeshUpdate(true);
            right.GetComponent<TMP_Text>().ForceMeshUpdate(true);

            RadioButton radio = button.AddComponent<RadioButton>();
            SetupButtonStyle(radio);
            modSettingsController.AddControl(radio);
            radio.texts = radio.GetComponentsInChildren<TMP_Text>().ToList();


            Util.DumpGameObject(button, "   ", true);
        }

        private static GameObject FindGameObjectByName(string name)
        {
            foreach (GameObject gameObject in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
            {
                if (gameObject.name == name)
                {
                    return gameObject;
                }
            }
            return null;
        }

        private void OnDestroy()
        {
            Destroy(ModSettingsButton);
            Destroy(QuitButton);
            Instance = null;
        }
    }
}
