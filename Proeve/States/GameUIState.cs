using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Proeve.Entities;
using Proeve.Resources;

namespace Proeve.States
{
    class GameUIState : State
    {
        private bool isTurn = true;
        private int selected = -1;
        private List<bool> canMove;

        public GameUIState()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            if (isTurn)
            {
                if (selected == -1)
                {
                    bool contains = false;
                    for (int i = 0; i < ((GameState)StateManager.GetState(1)).GetArmy().Count; i++)
                    {
                        if (((GameState)StateManager.GetState(1)).GetArmy()[i].Hitbox.Contains(Globals.mouseState.Position))
                        {
                            selected = i;
                            contains = true;
                        }
                    }

                    if (!contains)
                    {
                        selected = -1;
                    }
                    else if (selected != -1)
                    {

                    }
                }
                else
                {

                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            StateManager.GetState(1).Draw(spriteBatch);
            if (selected >= 0)
            {
                if (canMove[selected] == true)
                {

                }
            }
        }

        private List<Character> GetArmy()
        {
            return ((GameState)StateManager.GetState(1)).GetArmy();
        }

        public List<Character> GetEnemyArmy()
        {
            return ((GameState)StateManager.GetState(1)).GetEnemyArmy(); ;
        }
    }
}
