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
        SpriteFont font;

        public MatchFinderState()
        {
            
        }

        public override void Initialize()
        {
            font = ArtAssets.normalFont;
            Globals.multiplayerConnection = new MultiplayerConnection();
        }

        public override void Update(GameTime gameTime)
        {
            Globals.multiplayerConnection.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}
