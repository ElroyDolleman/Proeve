#region Using Statements
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Spine;
#endregion

namespace Proeve.Entities
{
    class SpineAnimation : ICloneable
    {
        private Skeleton skeleton;
        private AnimationState animationState;

        public Vector2 Position {
            get { return new Vector2(skeleton.X, skeleton.Y); }
            set { skeleton.X = value.X; skeleton.Y = value.Y; }
        }

        public SpineAnimation()
        {

        }

        public void LoadAnimation(GraphicsDevice graphicsDevice, ContentManager contentManager, string folderPath, string fileName, string animationName = "animation")
        {
            // Skeleton
            string root = @contentManager.RootDirectory + "\\";

            Atlas atlas = new Atlas(root + folderPath + fileName + ".atlas", new XnaTextureLoader(graphicsDevice));

            SkeletonData skeletonData;

            SkeletonJson json = new SkeletonJson(atlas);
            skeletonData = json.ReadSkeletonData(root + folderPath + fileName + ".json");

            this.skeleton = new Skeleton(skeletonData);

            // Animation
            AnimationStateData animationStateData = new AnimationStateData(skeleton.Data);
            this.animationState = new AnimationState(animationStateData);

            this.animationState.SetAnimation(0, animationName, true);

            this.Position = Vector2.Zero;
            this.skeleton.UpdateWorldTransform();
        }

        public void Update(GameTime gameTime)
        {
            animationState.Update(gameTime.ElapsedGameTime.Milliseconds / 1000f);
            animationState.Apply(skeleton);
            skeleton.UpdateWorldTransform();
        }

        public void Draw(SkeletonMeshRenderer skeletonRenderer)
        {
            skeletonRenderer.Draw(skeleton);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
