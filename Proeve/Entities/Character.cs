using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using E2DFramework.Entities;
using E2DFramework.Graphics;

namespace Proeve.Entities
{
    class Character
    {
        public enum Rank
        {
            Marshal,
            General,
            Majoor,
            Captain,
            Special,
            Bomb
        }

        public enum Army
        {
            Normal,
            Tiki
        }

        public enum Special
        {
            Normal,
            Spy,
            Healer,
            Minor,
            Bomb
        }

        public enum Weapon
        {
            Axe,
            Sword,
            Shield
        }

        public int Level { get { return hp + move; } }

        public int hp;
        public int move;

        public Weapon weapon;
        public Special special;

        public Rectangle Hitbox
        {
            get { return new Rectangle((int)position.X, (int)position.Y, 82, 82); }
        }

        public Sprite sprite;
        private Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { 
                position = value;
                sprite.position = value;
            }
        }

        public Character()
        {

        }

        public Character(Sprite sprite, int hp, int move, Special special = Special.Normal)
        {
            this.sprite = sprite;

            this.hp = hp;
            this.move = move;

            this.special = special;
        }

        public Character Clone()
        {
            Character clone = (Character)this.MemberwiseClone();
            clone.sprite = (Sprite)this.sprite.Clone();

            return clone;
        }
    }
}
