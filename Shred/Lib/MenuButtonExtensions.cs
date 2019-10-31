using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameManagement;

namespace Shred.Lib.MenuButtonExtensions
{
    public static class MenuButtonExtensions
    {
        public static void SetupStyles(this MenuButton button)
        {
            LevelCategoryButton levelCategoryButton = GameStateMachine.Instance.LevelSelectionObject.GetComponentInChildren<LevelCategoryButton>();

            button.normalFont = levelCategoryButton.normalFont;
            button.highlightedFont = levelCategoryButton.highlightedFont;
            button.transition = levelCategoryButton.transition;
            button.colors = levelCategoryButton.colors;
        }
    }
}
