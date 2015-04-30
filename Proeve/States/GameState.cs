using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using E2DFramework.Graphics;

using Proeve.Entities;
using Proeve.Resources;

namespace Proeve.States
{
    class GameState : State
    {
        private int[,] level;
        private List<Character> army;
        private List<Character> enemyArmy;

        public GameState()
        {

        }

        public override void Initialize()
        {
            level = Levels.grassLevel;
            StateManager.AddState(Settings.STATES.GameUI);
        }

        public override void Update(GameTime gameTime)
        {
            //
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < level.GetLength(0); i++)
                for (int j = 0; j < level.GetLength(1); j++)
                {
                    if (level[i, j] == 0)
                    {
                        spriteBatch.DrawRectangle(new Vector2(200 + j * 50 + 1, 50 + i * 50 + 1), 48, 48, Color.White);
                    }
                }
        }

    }
}
