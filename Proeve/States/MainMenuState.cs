using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using E2DFramework.Graphics;

using Proeve.UI;
using Proeve.Resources;

namespace Proeve.States
{
    class MainMenuState : State
    {
        E2DTexture background;

        public MainMenuState()
        {
            
        }

        public override void Initialize()
        {
            buttons.Add(new Button(ArtAssets.StartGameButton, new Vector2(386, 455)));
            buttons[0].ClickEvent = StartGame;

            background = ArtAssets.backgroundStartScreen;
        }
        
        public override void Update(GameTime gameTime)
        {
            
        }

        private void StartGame()
        {
            StateManager.ChangeState(Settings.STATES.Game);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawE2DTexture(background, Vector2.Zero);
        }
    }
}
