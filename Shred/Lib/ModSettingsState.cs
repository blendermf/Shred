using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameManagement;

namespace Shred.Lib {
    class ModSettingsState : GameState {
        public ModSettingsState() {
            this.availableTransitions = new Type[]
            {
            typeof(PauseState),
            typeof(PlayState)
            };
        }

        public override void OnEnter() {
            MenuManager.Instance.ModSettingsMenu.SetActive(true);
        }

        public override void OnUpdate() {
            if (PlayerController.Instance.inputController.player.GetButtonDown("B")) {
                MenuManager.Instance.ModSettingsMenu.SetActive(false);
                base.RequestTransitionTo(typeof(PauseState));
            }
        }

        public override void OnExit() {
            // make mod settings menu object inactive
        }
    }
}
