using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proeve.States;

namespace Proeve.Resources
{
    class Settings
    {
        public enum STATES
        {
            Loading,
            MainMenu,
            Game,
            ArmyEditor,
            MatchFinder,
            GameUI
        }
        public static Dictionary<STATES, State> states = new Dictionary<STATES, State>();

        public static void setDictionary()
        {
            states.Add(STATES.Loading, new LoadingState());
            states.Add(STATES.MainMenu, new MainMenuState());
            states.Add(STATES.ArmyEditor, new ArmyEditorState());
            states.Add(STATES.MatchFinder, new MatchFinderState());
            states.Add(STATES.GameUI, new GameUIState());
        }
    }
}
