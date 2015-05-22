using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using E2DFramework.Input;

using Proeve.Resources;
using Proeve.UI;

namespace Proeve.States
{
    class StateManager
    {
        private static List<State> stateList;

        public static void Launch()
        {
            stateList = new List<State>();
            AddState(0);

            Globals.mouseState = new E2DMouseState();
        }

        public static void AddState(Settings.STATES s)
        {
            stateList.Add(Settings.states[s].Clone());

            stateList[stateList.Count-1].Initialize();
        }

        public static void RemoveState()
        {
            stateList.Remove(stateList.Last());
        }

        public static void ChangeState(Settings.STATES s)
        {
            RemoveState();
            AddState(s);
        }

        public static State GetState(int i = 0)
        {
            return stateList[(stateList.Count - (i + 1) >= 0 ? stateList.Count - (i + 1) : 0)];
        }

        public static void Update(GameTime gameTime)
        {
            Globals.mouseState.Begin();

            foreach (Button b in stateList[stateList.Count - 1].buttons)
                b.Update(gameTime);

            stateList[stateList.Count-1].Update(gameTime);

            Globals.mouseState.End();
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            stateList[stateList.Count-1].Draw(spriteBatch);

            foreach (Button b in stateList[stateList.Count - 1].buttons)
                b.Draw(spriteBatch);

            spriteBatch.End();

            Globals.skeletonRenderer.Begin();
            stateList[stateList.Count - 1].DrawAnimation(Globals.skeletonRenderer);
            Globals.skeletonRenderer.End();

            spriteBatch.Begin();
            stateList[stateList.Count - 1].DrawForeground(spriteBatch);
            spriteBatch.End();
        }
    }
}
