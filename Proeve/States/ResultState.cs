using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using E2DFramework.Helpers;
using E2DFramework.Graphics;

using Proeve.UI;
using Proeve.Resources;

namespace Proeve.States
{
    class ResultState : State
    {
        private Vector2 DiamondsNumbersPosition { get { return new Vector2(630, 397); } }
        private Vector2 ReturnButtonPosition { get { return new Vector2(780, 374); } }
        private Vector2 BackgroundPosition { get { return new Vector2(84, 43); } }
        private Vector2 RewardTextPosition { get { return new Vector2(384, 374); } }

        private Sprite background;
        private Sprite rewardText;

        private List<Sprite> diamondsDisplay;
        private const int DIAMONDS_LIMIT = 99999;

        public int Diamonds
        {
            get
            {
                int score = 0;
                for (int i = 0; i < diamondsDisplay.Count; i++)
                    score += (diamondsDisplay[i].CurrentFrame - 1) * (1 * (int)Math.Pow(10, i));

                return Math.Min(score, DIAMONDS_LIMIT);
            }
            set
            {
                int[] digits = MathHelp.GetDigits(value);

                diamondsDisplay = new List<Sprite>();

                for (int i = 0; i < digits.Length; i++)
                {
                    diamondsDisplay.Add(ArtAssets.BigNumbers);
                    Sprite number = diamondsDisplay[i];
                    number.CurrentFrame = digits[i] + 1;
                    number.position = DiamondsNumbersPosition + new Vector2(i * number.sourceRectangle.Width, 0);
                }
            }
        }

        public override void Initialize()
        {
            background = Armies.army[0].IsDead ? ArtAssets.LosePopUp : ArtAssets.WinPopUp;
            background.position = BackgroundPosition;

            rewardText = ArtAssets.RewardText;
            rewardText.position = RewardTextPosition;

            buttons.Add(new Button(ArtAssets.ReturnButton, ReturnButtonPosition));
            buttons[0].ClickEvent = Quit;

            Diamonds = Globals.earnedDiamonds;
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
            rewardText.Draw(spriteBatch);

            foreach (Sprite number in diamondsDisplay)
                number.Draw(spriteBatch);
        }
    }
}
