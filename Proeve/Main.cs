#region Using Statements
using System;

using Microsoft.Xna.Framework;

using E2DFramework;

using Proeve.States;
using Proeve.Resources;
using Proeve.Resources.Calculations;
#endregion

namespace Proeve
{
    public class Main : E2DGame
    {
        public Main()
            : base()
        {
            Content.RootDirectory = "Content";
            Window.Title = "Proeve";

            WindowWidth = 1024;
            WindowHeight = 768;

            IsMouseVisible = true;
            backgroundColor = Color.Black;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Globals.contentManager = this.Content;

            Settings.setDictionary();
            StateManager.Launch();
        }

        protected override void Update(GameTime gameTime)
        {
            StateManager.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            StateManager.Draw(spriteBatch);
        }
    }
}