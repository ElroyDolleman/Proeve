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

        public bool loop;
        public bool IsPlayingAnimation { get { return !(!loop && animationState.GetCurrent(0).Time >= animationState.GetCurrent(0).EndTime);} }

        public Vector2 Position {
            get { return new Vector2(skeleton.X, skeleton.Y); }
            set { skeleton.X = value.X; skeleton.Y = value.Y; }
        }

        public SpineAnimation()
        {

        }

        public void LoadAnimation(GraphicsDevice graphicsDevice, ContentManager contentManager, string folderPath, string fileName, string animationName = "animation", bool loop = true)
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
            this.animationState.SetAnimation(0, this.animationName, loop);

            this.Position = Vector2.Zero;
            this.skeleton.UpdateWorldTransform();

            this.loop = loop;
        }

        public void Play()
        {
            animationState.GetCurrent(0).Time = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (IsPlayingAnimation)
            {
                animationState.Apply(skeleton);
                animationState.Update(gameTime.ElapsedGameTime.Milliseconds / 1000f);
                skeleton.UpdateWorldTransform();
            }
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
