#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Proeve.Resources;
#endregion

namespace Proeve.States
{
    class MatchFinderState : State
    {
        private SpriteFont font;
        private byte you, opponent;

        public MatchFinderState()
        {
            
        }

        public override void Initialize()
        {
            font = ArtAssets.normalFont;
            Globals.multiplayerConnection = new MultiplayerConnection();

            Globals.multiplayerConnection.RecieveArmy += RecievedArmy;
            Globals.multiplayerConnection.RecieveConnection += RecievedConnection;

            if (Globals.multiplayerConnection.isHosting) {
                you = 1;
                opponent = 2;
            }
            else {
                you = 2;
                opponent = 1;
            }
        }

        private void RecievedConnection()
        {
            
        }

        private void RecievedArmy()
        {
            ((GameState)StateManager.GetState(1)).SetEnemyArmy(Armies.opponentArmy);
            ((GameState)StateManager.GetState(1)).MatchStarts();

            StateManager.ChangeState(Settings.STATES.GameUI);
        }

        public override void Update(GameTime gameTime)
        {
            Globals.multiplayerConnection.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, "player " + you, new Vector2(100, 64), Color.White);

            if (Globals.multiplayerConnection.Connected)
                spriteBatch.DrawString(font, "player " + opponent, new Vector2(Main.WindowWidth - 100, 64), Color.White);
            else
                spriteBatch.DrawString(font, "searching...", new Vector2(Main.WindowWidth - 100, 64), Color.White);
        }
    }
}
