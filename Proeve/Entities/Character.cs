#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using E2DFramework.Entities;
using E2DFramework.Graphics;
using E2DFramework.Helpers;

using Proeve.Resources;
using Proeve.Resources.Calculations;

using Spine;
#endregion

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
        public bool IsDead { get { return !(hp > 0); } }

        public int hp;
        public int move;
        public List<Point> waypoints;

        public Color ColorEffect {
            get { return layerSprite.colorEffect; }
            set { layerSprite.colorEffect = value; }
        }

        public Special special;
        public Weapon weapon;
        public Rank rank;
        public Army army;

        public Rectangle Hitbox{ get { return new Rectangle((int)position.X, (int)position.Y, 82, 82); } }

        public SpineAnimation animation;
        public Sprite sprite;
        public Sprite layerSprite;
        public Vector2 position;

        public Point GridPosition
        {
            get { return (waypoints.Count == 0 ? Grid.ToGridLocation(new Point((int)position.X + Globals.TILE_WIDTH/2, (int)position.Y + Globals.TILE_WIDTH/2), Globals.GridLocation, Globals.TileDimensions) : waypoints[0]); }
            set { position = Grid.ToPixelLocation(value, Globals.GridLocation, Globals.TileDimensions).ToVector2(); }
        }

        public Character()
        {
            waypoints = new List<Point>();
        }

        public Character(Sprite sprite, SpineAnimation animation)
            : this()
        {
            this.sprite = sprite;
            this.animation = animation;
            this.layerSprite = (Sprite)this.sprite.Clone();
            this.ResetColorEffect();
        }

        public Character(Sprite sprite, SpineAnimation animation, int hp, int move, Rank rank, Army army = Army.Normal, Special special = Special.None)
            : this(sprite, animation)
        {
            this.hp = hp;
            this.move = move;

            this.special = special;
            this.weapon = Weapon.Sword;

            this.rank = rank;
            this.army = army;
        }

        public void UpdateSpriteSheetAnimation(GameTime gameTime)
        {
            sprite.UpdateAnimation(gameTime);
        }

        public void UpdateSpineAnimation(GameTime gameTime)
        {
            animation.Update(gameTime);
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

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawSprite(this.sprite, this.position);

            if (ColorEffect != Color.White * 0f)
                spriteBatch.DrawSprite(this.layerSprite, this.position);
        }

        public void DrawAnimation(SkeletonMeshRenderer skeletonRenderer)
        {
            animation.Draw(skeletonRenderer);
        }

        public Character Clone()
        {
            Character clone = (Character)this.MemberwiseClone();
            clone.sprite = (Sprite)this.sprite.Clone();
            clone.waypoints = new List<Point>();
            clone.layerSprite = (Sprite)this.layerSprite.Clone();

            return clone;
        }

        public void ResetColorEffect() { ColorEffect = Color.White * 0f; }

        public bool IsNextTo(Character c) 
        {
            if(Math.Pow(GridPosition.X - c.GridPosition.X, 2) == 1
            && Math.Pow(GridPosition.Y - c.GridPosition.Y, 2) == 0
            || Math.Pow(GridPosition.X - c.GridPosition.X, 2) == 0
            && Math.Pow(GridPosition.Y - c.GridPosition.Y, 2) == 1)
                return true;
            return false;
        }
    }
}
