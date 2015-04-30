using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using E2DFramework.Graphics;

using Proeve.Entities;
using Proeve.Resources;
using Proeve.Resources.Calculations;

namespace Proeve.States
{
    class ArmyEditorState : State
    {
        

        public ArmyEditorState()
        {

        }

        public override void Initialize()
        {
            Armies.army = new List<Character>();
            Armies.army.Add(Armies.GetCharacter(Armies.CharacterRanks.Marshal));
            Armies.army.Add(Armies.GetCharacter(Armies.CharacterRanks.General));
            Armies.army.Add(Armies.GetCharacter(Armies.CharacterRanks.General));
            Armies.army.Add(Armies.GetCharacter(Armies.CharacterRanks.Majoor));
            Armies.army.Add(Armies.GetCharacter(Armies.CharacterRanks.Majoor));
            Armies.army.Add(Armies.GetCharacter(Armies.CharacterRanks.Captain));
            Armies.army.Add(Armies.GetCharacter(Armies.CharacterRanks.Captain));
            Armies.army.Add(Armies.GetCharacter(Armies.CharacterRanks.Special));
            Armies.army.Add(Armies.GetCharacter(Armies.CharacterRanks.Bomb));
            Armies.army.Add(Armies.GetCharacter(Armies.CharacterRanks.Bomb));

            Armies.army[0].Position = Grid.ToPixelLocation(new Point(0, 0), new Point(50, 50), new Point(82, 82)).ToVector2();
            Armies.army[1].Position = Grid.ToPixelLocation(new Point(1, 0), new Point(50, 50), new Point(82, 82)).ToVector2();
            Armies.army[2].Position = Grid.ToPixelLocation(new Point(2, 0), new Point(50, 50), new Point(82, 82)).ToVector2();
            Armies.army[3].Position = Grid.ToPixelLocation(new Point(3, 0), new Point(50, 50), new Point(82, 82)).ToVector2();
            Armies.army[4].Position = Grid.ToPixelLocation(new Point(4, 0), new Point(50, 50), new Point(82, 82)).ToVector2();
            Armies.army[5].Position = Grid.ToPixelLocation(new Point(5, 0), new Point(50, 50), new Point(82, 82)).ToVector2();
            Armies.army[6].Position = Grid.ToPixelLocation(new Point(6, 0), new Point(50, 50), new Point(82, 82)).ToVector2();
            Armies.army[7].Position = Grid.ToPixelLocation(new Point(7, 0), new Point(50, 50), new Point(82, 82)).ToVector2();
            Armies.army[8].Position = Grid.ToPixelLocation(new Point(0, 1), new Point(50, 50), new Point(82, 82)).ToVector2();
            Armies.army[9].Position = Grid.ToPixelLocation(new Point(1, 1), new Point(50, 50), new Point(82, 82)).ToVector2();
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < 3; y++)
                for (int x = 0; x < 8; x++)
                {
                    spriteBatch.DrawRectangle(new Vector2(x * 82 + 50, y * 82 + 50), 81, 81, Color.White);
                }

            foreach(Character c in Armies.army)
            {
                c.sprite.Draw(spriteBatch);
            }
        }
    }
}
