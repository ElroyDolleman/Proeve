#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using E2DFramework.Graphics;

using Proeve.UI;
using Proeve.Entities;
using Proeve.Resources;
using Proeve.Resources.Calculations.Pathfinding;
#endregion

namespace Proeve.States
{
    class LoadingState : State
    {
        public LoadingState()
        {
            
        }

        public override void Initialize()
        {
            Node.size = Globals.TILE_WIDTH;

            ArtAssets.LoadTextures();
            ArtAssets.LoadFont(Globals.contentManager);
            AudioAssets.Load(Globals.contentManager);
            AnimationAssets.LoadAnimations(Globals.graphicsDevice, Globals.contentManager);

            Armies.SetUpCharacters();

            StateManager.ChangeState(Settings.STATES.MainMenu);
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}
