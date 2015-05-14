using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using E2DFramework.Graphics;

using Proeve.Entities;
using Proeve.Resources;

namespace Proeve.States
{
    class FightState : State
    {
        Character attacker;
        Character defender;

        public FightState()
        {

        }

        public override void Initialize()
        {
            /*Console.WriteLine("attacker hp: {0}", attacker.hp);
            Console.WriteLine("attacker weapon: {0}", attacker.weapon);

            Console.WriteLine("defender hp: {0}", defender.hp);
            Console.WriteLine("defender weapon: {0}", defender.weapon);*/
        }

        public void SetUnits(Character attacker, Character defender)
        {
            while (!attacker.IsDead && !defender.IsDead)
            {
                defender.hp--;

                if (!defender.IsDead)
                    attacker.hp--;
            }

            Console.WriteLine("attacker hp: {0}", attacker.hp);
            Console.WriteLine("attacker weapon: {0}", attacker.weapon);

            Console.WriteLine("defender hp: {0}", defender.hp);
            Console.WriteLine("defender weapon: {0}", defender.weapon);
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            StateManager.GetState(1).Draw(spriteBatch);

            spriteBatch.DrawRectangle(Main.WindowRectangle, Color.Black * .5f);
        }
    }
}
