using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using E2DFramework.Graphics;

using Proeve.UI;
using Proeve.Resources;

namespace Proeve.States
{
    class ResultState : State
    {
        public Vector2 ReturnButtonPosition { get { return new Vector2(780, 374); } }

        public Sprite background;

        public override void Initialize()
        {
            background = Armies.army[0].IsDead ? ArtAssets.LosePopUp : ArtAssets.WinPopUp;
            background.position = new Vector2(84, 43);

            buttons.Add(new Button(ArtAssets.ReturnButton, ReturnButtonPosition));
            buttons[0].ClickEvent = Quit;

            Globals.diamonds = Globals.earnedDiamonds;
            Globals.earnedDiamonds = 0;

            while (true)
            {
                try
                {
                    if (File.Exists("data1.txt"))
                        File.Delete("data1.txt");
                    if (File.Exists("data2.txt"))
                        File.Delete("data2.txt");
                    break;
                }
                catch { }
            }
        }

        public override void Update(GameTime gameTime)
        {
            

            
        }

        private void Quit()
        {
            Globals.multiplayerConnection = null;
            StateManager.ChangeState(Settings.STATES.ArmyEditor);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            StateManager.GetState(1).Draw(spriteBatch);

            background.Draw(spriteBatch);
        }
    }
}
