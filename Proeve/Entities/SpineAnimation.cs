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

        private Atlas atlas;
        private string filesName, animationName;

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
            this.filesName = @contentManager.RootDirectory + "\\" + folderPath + fileName;

            this.atlas = new Atlas(this.filesName + ".atlas", new XnaTextureLoader(graphicsDevice));

            SkeletonData skeletonData;

            SkeletonJson json = new SkeletonJson(atlas);
            skeletonData = json.ReadSkeletonData(this.filesName + ".json");

            this.skeleton = new Skeleton(skeletonData);

            // Animation
            AnimationStateData animationStateData = new AnimationStateData(skeleton.Data);
            this.animationState = new AnimationState(animationStateData);

            this.animationName = animationName;
            this.animationState.SetAnimation(0, this.animationName, true);

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
            SpineAnimation clone = (SpineAnimation)this.MemberwiseClone();

            SkeletonData skeletonData;

            SkeletonJson json = new SkeletonJson(atlas);
            skeletonData = json.ReadSkeletonData(this.filesName + ".json");

            this.skeleton = new Skeleton(skeletonData);

            clone.skeleton = new Skeleton(skeletonData);

            AnimationStateData animationStateData = new AnimationStateData(skeleton.Data);

            clone.animationState = new AnimationState(animationStateData);
            clone.animationState.SetAnimation(0, this.animationName, true);

            clone.Position = Vector2.Zero;
            clone.skeleton.UpdateWorldTransform();

            return clone;
        }
    }
}
