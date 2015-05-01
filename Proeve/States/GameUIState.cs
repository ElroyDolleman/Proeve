﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Proeve.States
{
    class GameUIState : State
    {


        public GameUIState()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            StateManager.GetState(1).Draw(spriteBatch);
        }
    }
}
