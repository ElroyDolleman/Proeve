using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Proeve.Resources;

namespace Proeve.States
{
    class ResultState : State
    {
        public override void Initialize()
        {
            
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
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
                catch
                {

                }
            }

            Globals.diamonds = Globals.earnedDiamonds;
            Globals.earnedDiamonds = 0;

            Globals.multiplayerConnection = null;
            StateManager.ChangeState(Settings.STATES.ArmyEditor);
        }
    }
}
