using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Proeve.Resources
{
    static class Debug
    {
        public static SpriteFont spriteFont;

        public static void DrawDebugText(this SpriteBatch spriteBatch, string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(spriteFont, text, position, color);
        }

        public static void DrawDebugText(this SpriteBatch spriteBatch, string text, Point position, Color color)
        {
            DrawDebugText(spriteBatch, text, position.ToVector2(), color);
        }

        public static void DrawDebugText(this SpriteBatch spriteBatch, string text, int x, int y, Color color)
        {
            DrawDebugText(spriteBatch, text, new Vector2(x, y), color);
        }
    }
}
