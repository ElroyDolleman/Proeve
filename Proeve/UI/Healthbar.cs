using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using E2DFramework.Graphics;

namespace Proeve.UI
{
    class Healthbar
    {
        private List<Sprite> bars;
        private readonly int barWidth, barHeight;

        public int hp;
        public readonly int maxHP;

        public Vector2 Center
        {
            get { return position + new Vector2(barWidth*maxHP/2, barHeight/2); }
            set { position = value - new Vector2(barWidth*maxHP/2, barHeight/2); }
        }

        public Rectangle Hitbox { get { return new Rectangle((int)position.X, (int)position.Y, barWidth, barHeight); } }

        public Vector2 position;

        public Healthbar(Sprite bar, int hp, Vector2 position)
        {
            this.position = position;

            this.maxHP = hp;
            this.hp = hp;

            bars = new List<Sprite>();

            for (int i = 0; i < maxHP; i++)
                bars.Add(bar);

            this.barWidth = bar.sourceRectangle.Width;
            this.barHeight = bar.sourceRectangle.Height;
        }

        public Healthbar(Sprite bar, int hp)
            : this(bar, hp, Vector2.Zero)
        {

        }

        public Healthbar(Sprite bar, int maxHP, int currentHP, Vector2 position)
            : this(bar, maxHP, position)
        {
            this.hp = currentHP;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < bars.Count; i++)
            {
                if (i+1 <= hp)
                    bars[i].CurrentFrame = 2;
                else
                    bars[i].CurrentFrame = 1;

                spriteBatch.DrawSprite(bars[i], position + Vector2.UnitX * (i * barWidth));
            }
        }
    }
}
