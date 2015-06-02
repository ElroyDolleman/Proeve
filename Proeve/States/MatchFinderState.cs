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
        private SpineAnimation searchMatchAnimation;

        public MatchFinderState()
        {
            
        }

        public override void Initialize()
        {
            Globals.multiplayerConnection = new MultiplayerConnection();

            Globals.multiplayerConnection.ReceiveArmy += ReceivedArmy;
            Globals.multiplayerConnection.ReceiveConnection += ReceivedConnection;

            searchMatchAnimation = AnimationAssets.MatchFinderAnimation;
            searchMatchAnimation.Position = Main.WindowCenter;
        }

        private void ReceivedConnection()
        {
            
        }

        private void ReceivedArmy()
        {
            ((GameState)StateManager.GetState(1)).SetEnemyArmy(Armies.opponentArmy);

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
