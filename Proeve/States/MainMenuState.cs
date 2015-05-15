using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using E2DFramework.Graphics;

using Proeve.Resources;

namespace Proeve.States
{
    class MainMenuState : State
    {

        public MainMenuState()
        {
            
        }

        public override void Initialize()
        {
            StateManager.ChangeState(Settings.STATES.Game);
        }
        
        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}
