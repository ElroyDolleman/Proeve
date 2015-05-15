#region Using Statements
using System;
using System.IO;

using Microsoft.Xna.Framework;

using E2DFramework;

using Spine;

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
            Globals.graphicsDevice = this.GraphicsDevice;
            Globals.skeletonRenderer = new SkeletonMeshRenderer(this.GraphicsDevice);

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

        protected override void EndRun()
        {
            base.EndRun();

            try
            {
                if (File.Exists("data1.txt"))
                    File.Delete("data1.txt");
                if (File.Exists("data2.txt"))
                    File.Delete("data2.txt");
            }
            catch
            {

            }
        }
    }
}