#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using E2DFramework.Graphics;

using Proeve.Entities;
using Proeve.Resources;
#endregion

namespace Proeve.States
{
    class MatchFinderState : State
    {
        private SpriteFont font;
        private byte you, opponent;

        private SpineAnimation searchMatchAnimation;

        public MatchFinderState()
        {
            
        }

        public override void Initialize()
        {
            font = ArtAssets.normalFont;
            Globals.multiplayerConnection = new MultiplayerConnection();

            Globals.multiplayerConnection.ReceiveArmy += ReceivedArmy;
            Globals.multiplayerConnection.ReceiveConnection += ReceivedConnection;

            if (Globals.multiplayerConnection.isHosting) {
                you = 1;
                opponent = 2;
            }
            else {
                you = 2;
                opponent = 1;
            }

            searchMatchAnimation = AnimationAssets.MatchFinderAnimation;
            searchMatchAnimation.Position = Main.WindowCenter;
        }

        private void ReceivedConnection()
        {
            
        }

        private void ReceivedArmy()
        {
            ((GameState)StateManager.GetState(1)).SetEnemyArmy(Armies.opponentArmy);
            //((GameState)StateManager.GetState(1)).MatchStarts();

            StateManager.ChangeState(Settings.STATES.GameUI);
        }

        public override void Update(GameTime gameTime)
        {
            Globals.multiplayerConnection.Update(gameTime);
            searchMatchAnimation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            StateManager.GetState(1).Draw(spriteBatch);
            spriteBatch.DrawRectangle(Main.WindowRectangle, Color.Black * .75f);
        }

        public override void DrawAnimation(Spine.SkeletonMeshRenderer skeletonRenderer)
        {
            searchMatchAnimation.Draw(skeletonRenderer);
        }
    }
}
