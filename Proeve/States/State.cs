using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proeve.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Proeve.States
{
    class State
    {
        public List<Button> buttons = new List<Button>();

        public virtual void Initialize() { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }

        public State Clone()
        {
            return (State) MemberwiseClone();
        }
    }
}
