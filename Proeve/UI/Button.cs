using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using E2DFramework.Graphics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Proeve.Resources;

namespace Proeve.UI
{
    class Button
    {
        private Sprite graphic;
        private Rectangle hitbox;

        public bool Down{ get { return graphic.CurrentFrame == 2; } }

        public Button(Sprite graphic)
        {
            this.hitbox = graphic.TextureFrame;
            this.graphic = graphic;

            this.graphic.position = new Vector2(hitbox.X, hitbox.Y);
        }

        public Button(Sprite graphic, Rectangle hitbox)
        {
            this.graphic = graphic;
            this.hitbox = hitbox;

            this.graphic.position = new Vector2(hitbox.X, hitbox.Y);
        }

        public Button(Sprite graphic, int width, int height)
        {
            this.graphic = graphic;
            this.hitbox = new Rectangle(0, 0, width, height);

            this.graphic.position = new Vector2(hitbox.X, hitbox.Y);
        }

        public void Update(GameTime gametime)
        {
            if (Globals.mouseState.LeftButtonPressed)
            {
                if (hitbox.Contains(Globals.mouseState.Position))
                    graphic.CurrentFrame = 2;
            }
            if (Globals.mouseState.LeftButtonReleased)
                graphic.CurrentFrame = 1;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            graphic.Draw(spriteBatch);
        }
    }
}
