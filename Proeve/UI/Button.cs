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
        public bool enabled = true;
        public delegate void OnClickEvent();

        public OnClickEvent ClickEvent;

        private Sprite graphic;
        private Rectangle hitbox;

        public bool Down { get; private set; }

        public Button(Sprite graphic)
        {
            this.hitbox = graphic.sourceRectangle;
            this.hitbox.X = 0;
            this.hitbox.Y = 0;

            this.graphic = graphic;

            this.graphic.position = new Vector2(hitbox.X, hitbox.Y);

            this.Down = false;
        }

        public Button(Sprite graphic, Rectangle hitbox)
        {
            this.graphic = graphic;
            this.hitbox = hitbox;

            this.graphic.position = new Vector2(hitbox.X, hitbox.Y);

            this.Down = false;
        }

        public Button(Sprite graphic, Vector2 position)
        {
            this.graphic = graphic;
            this.hitbox = new Rectangle((int)position.X, (int)position.Y, graphic.sourceRectangle.Width, graphic.sourceRectangle.Height);

            this.graphic.position = new Vector2(position.X, position.Y);

            this.Down = false;
        }

        public Button(Sprite graphic, int x, int y)
          : this(graphic, new Vector2(x, y))
        {

        }

        public void Update(GameTime gametime)
        {
            if (Globals.mouseState.LeftButtonPressed && Main.WindowRectangle.Contains(Globals.mouseState.Position))
            {
                if (hitbox.Contains(Globals.mouseState.Position))
                    Down = true;
            }
            if (Globals.mouseState.LeftButtonReleased && Main.WindowRectangle.Contains(Globals.mouseState.Position))
            {
                if (Down)
                {
                    if (ClickEvent != null && hitbox.Contains(Globals.mouseState.Position))
                        ClickEvent();

                    Down = false;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            graphic.Draw(spriteBatch);
        }
    }
}
