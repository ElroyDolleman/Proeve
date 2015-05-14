using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using E2DFramework.Entities;
using E2DFramework.Graphics;
using E2DFramework.Helpers;

using Proeve.Resources;
using Proeve.Resources.Calculations;

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
        public List<Point> waypoints;

        public Special special;
        public Weapon weapon;
        public Rank rank;
        public Army army;

        public Rectangle Hitbox{ get { return new Rectangle((int)position.X, (int)position.Y, 82, 82); } }

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

        public Point GridPosition
        {
            get { return Grid.ToGridLocation(new Point((int)position.X + Globals.TILE_WIDTH/2, (int)position.Y + Globals.TILE_WIDTH/2), Globals.GridLocation, Globals.TileDimensions); }
            set { Position = Grid.ToPixelLocation(value, Globals.GridLocation, Globals.TileDimensions).ToVector2(); }
        }

        public Character()
        {
            waypoints = new List<Point>();
        }

        public Character(Sprite sprite)
            : this()
        {
            this.sprite = sprite;
        }

        public Character(Sprite sprite, int hp, int move, Rank rank, Army army = Army.Normal, Special special = Special.None)
            : this()
        {
            this.sprite = sprite;

            this.hp = hp;
            this.move = move;

            this.special = special;
            this.weapon = Weapon.Sword;

            this.rank = rank;
            this.army = army;
        }

        #region Move In Direction
        /// <summary>
        /// Move in a specific direction.
        /// </summary>
        /// <param name="velocity">The amount of speed.</param>
        /// <param name="degree">The amount of degree.</param>
        #endregion
        public virtual void MoveInDirection(float degree, float velocity)
        {
            this.position.X += (float)Math.Cos(degree * (Math.PI / 180)) * velocity;
            this.position.Y += (float)Math.Sin(degree * (Math.PI / 180)) * velocity;
            this.Position = position;
        }

        #region Move Towards
        /// <summary>
        /// Move towards an other position.
        /// </summary>
        /// <param name="destination">The position it needs to move towards to.</param>
        /// <param name="velocity">The amount of speed.</param>
        #endregion
        public virtual void MoveTowards(Vector2 destination, float velocity)
        {
            float degree = MathHelper.ToDegrees((float)position.GetAngleTo(destination));

            this.MoveInDirection(degree, velocity);
        }

        public Character Clone()
        {
            Character clone = (Character)this.MemberwiseClone();
            clone.sprite = (Sprite)this.sprite.Clone();
            clone.waypoints = new List<Point>();

            return clone;
        }
    }
}
