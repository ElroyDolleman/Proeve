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
            None,
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

        public Special special;
        public Weapon weapon;
        public Rank rank;
        public Army army;

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

                if (sprite != null)
                    sprite.position = value;
            }
        }

        public Character()
        {

        }

        public Character(Sprite sprite, int hp, int move, Rank rank, Army army = Army.Normal, Special special = Special.None)
        {
            this.sprite = sprite;

            this.hp = hp;
            this.move = move;

            this.special = special;
            this.weapon = Weapon.Sword;

            this.rank = rank;
            this.army = army;
        }

        public Character Clone()
        {
            Character clone = (Character)this.MemberwiseClone();
            clone.sprite = (Sprite)this.sprite.Clone();

            return clone;
        }
    }
}
