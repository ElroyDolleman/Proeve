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
        Sprite sprite;

        public MainMenuState()
        {
            
        }

        public override void Initialize()
        {
            StateManager.ChangeState(Settings.STATES.ArmyEditor);

            E2DTexture texture;
            texture.Load("Characters\\TikiAnimSWF_Spritesheet423x466");
            sprite = new Sprite(texture, new Rectangle(0, 0, 423, 466), 29, 15f, 6);
        }
        
        public override void Update(GameTime gameTime)
        {
            sprite.UpdateAnimation(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }
    }
}
