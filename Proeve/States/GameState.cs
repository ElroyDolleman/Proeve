using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using E2DFramework.Graphics;

using Proeve.Entities;
using Proeve.Resources;
using Proeve.Resources.Calculations;

namespace Proeve.States
{
    class GameState : State
    {
        private int[,] level;
        private E2DTexture background;

        private List<Character> army;
        private List<Character> enemyArmy;

        private bool MyTurn { get { return Globals.multiplayerConnection.myTurn; } }

        public GameState()
        {

        }

        public override void Initialize()
        {
            level = Levels.grassLevel;
            StateManager.AddState(Settings.STATES.ArmyEditor);
            background = ArtAssets.backgroundGrassLevel;
        }

        public void MatchStarts()
        {
            Globals.multiplayerConnection.RecieveMove += RecievedMove;
            Globals.multiplayerConnection.RecieveFight += RecievedFight;
            Globals.multiplayerConnection.RecieveEndTurn += OtherPlayerEndedHisTurn;
        }

        public override void Update(GameTime gameTime)
        {
            Globals.multiplayerConnection.Update(gameTime);

            if (MyTurn)
            {
                // Ending Turn Test
                /*if (Globals.mouseState.RightButtonPressed && Main.WindowRectangle.Contains(Globals.mouseState.Position))
                {
                    Globals.multiplayerConnection.SendEndTurn();
                }*/

                // Fight Test
                /*if (Globals.mouseState.RightButtonPressed && Main.WindowRectangle.Contains(Globals.mouseState.Position))
                {
                    Globals.multiplayerConnection.SendFight(0, 0);

                    army[0].Position = new Vector2(0, 0);
                    enemyArmy[0].Position = new Vector2(82, 0);
                }*/

                // Move Character Test
                /*if (Globals.mouseState.RightButtonPressed && Main.WindowRectangle.Contains(Globals.mouseState.Position))
                {
                    Globals.multiplayerConnection.SendMove(0, Point.Zero);

                    army[0].Position = Grid.ToPixelLocation(Point.Zero, Globals.GridLocation, Globals.TileDimensions).ToVector2();
                }*/
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawE2DTexture(background, Vector2.Zero);

            /*for (int i = 0; i < level.GetLength(0); i++)
            for (int j = 0; j < level.GetLength(1); j++)
            {
                if (level[i, j] == 0)
                {
                    spriteBatch.DrawRectangle(new Vector2(200 + j * 50 + 1, 50 + i * 50 + 1), 48, 48, Color.White);
                }
            }*/

            foreach (Character c in army)
            {
                c.sprite.Draw(spriteBatch);
            }

            foreach (Character c in enemyArmy)
            {
                c.sprite.Draw(spriteBatch);
            }

#if DEBUG
            // Draw Debug Stuff
            spriteBatch.DrawDebugText("myTurn: " + MyTurn, 4, 4, Color.White);
            spriteBatch.DrawDebugText("WaitingForResponse: " + Globals.multiplayerConnection.IsWaitingForResponse, 4, 32, Color.White);
#endif
        }

        public void SetArmy(List<Character> sendArmy) { army = sendArmy; }
        public void SetEnemyArmy(List<Character> sendArmy) { enemyArmy = sendArmy; }


        #region Recieve Methods

        private void RecievedMove(int charIndex, Point gridLocation)
        {
            enemyArmy[charIndex].Position = Grid.ToPixelLocation(gridLocation, Globals.GridLocation, Globals.TileDimensions).ToVector2();
        }

        public void RecievedFight(int charIndexAttacker, int charIndexDefender)
        {
            // Attacker is the opponent. Defender is you.

            army[charIndexDefender].Position = new Vector2(0, 0);
            enemyArmy[charIndexAttacker].Position = new Vector2(82, 0);
        }

        private void OtherPlayerEndedHisTurn()
        {

        }
        #endregion
    }
}
