using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Spine;

using Proeve.UI;

namespace Proeve.States
{
    class State
    {
        public List<Button> buttons = new List<Button>();

        public virtual void Initialize() { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
        public virtual void DrawAnimation(SkeletonMeshRenderer skeletonRenderer) { }
        public virtual void DrawForeground(SpriteBatch spriteBatch) { }

        public State Clone()
        {
            return (State) MemberwiseClone();
        }
    }
}
