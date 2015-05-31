using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using E2DFramework.Graphics;

using Proeve.Entities;
using Proeve.Resources;
using Proeve.Resources.Calculations;
using Proeve.Resources.Calculations.Pathfinding;

namespace Proeve.States
{
    class GameState : State
    {
        private const float MOVEMENT_VELOCITY = 5.2f;

        private int[,] level;
        private List<Character> army;
        private List<Character> enemyArmy;

        private E2DTexture background;
        private SpineAnimation shineForeground;

        public GameState()
        {

        }

        public override void Initialize()
        {
            level = Levels.grassLevel;
            StateManager.AddState(Settings.STATES.ArmyEditor);

            background = ArtAssets.backgroundGrassLevel;
            shineForeground = AnimationAssets.ShineEffect;
            shineForeground.Position = new Vector2(768 / 2, 768 / 2);
        }

        public override void Update(GameTime gameTime)
        {
            UpdateArmies(army, gameTime);
            UpdateArmies(enemyArmy, gameTime);
            shineForeground.Update(gameTime);
        }

        private void UpdateArmies(List<Character> tempArmy, GameTime gameTime)
        {
            for (int i = 0; i < tempArmy.Count; i++)
            {
                Character c = tempArmy[i];
                if (c.waypoints.Count > 0)
                {
                    Vector2 wp = Grid.ToPixelLocation(c.waypoints[c.waypoints.Count - 1], Globals.GridLocation, Globals.TileDimensions).ToVector2();

                    if (Vector2.Distance(c.position, wp) > MOVEMENT_VELOCITY)
                        c.MoveTowards(wp, MOVEMENT_VELOCITY);
                    else
                    {
                        c.position = wp;
                        c.waypoints.RemoveAt(c.waypoints.Count - 1);
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawE2DTexture(background, Vector2.Zero);

            foreach (Character c in army)
                if (!c.IsDead)
                    c.Draw(spriteBatch);

            if (enemyArmy != null)
                foreach (Character c in enemyArmy)
                    if (!c.IsDead)
                        c.Draw(spriteBatch);
        }

        public override void DrawAnimation(Spine.SkeletonMeshRenderer skeletonRenderer)
        {
            shineForeground.Draw(skeletonRenderer);
        }

        public void SetArmy(List<Character> sendArmy)
        {
            army = sendArmy;
        }

        public void SetEnemyArmy(List<Character> sendArmy)
        {
            enemyArmy = sendArmy;
        }

        public List<Character> GetArmy()
        {
            return army;
        }

        public List<Character> GetEnemyArmy()
        {
            return enemyArmy;
        }

        public int[,] GetLevel()
        {
            return level;
        }

        public void MoveUnit(Character unit, Point gridPosition)
        {
            int[,] tempLevel = DuplicateLevel();

            SetUnwalkable(ref tempLevel, unit);

            Point GPos = unit.GridPosition;
            tempLevel[GPos.X, GPos.Y] = 0;

            List<List<Node>> field = new List<List<Node>>();
            for (int i = 0; i < tempLevel.GetLength(0); i++)
            {
                field.Add(new List<Node>());
                for (int j = 0; j < tempLevel.GetLength(1); j++)
                {
                    field[i].Add(new Node(i, j, tempLevel[i, j]));
                }
            }
            AStar.SetField(field);
            Point gridPos = Grid.ToGridLocation(new Point((int)unit.position.X, (int)unit.position.Y),Globals.GridLocation,Globals.TileDimensions);
            AStar.Path(gridPos.X,gridPos.Y,gridPosition.X,gridPosition.Y);

            List<Node> path = AStar.GetPath();
            if (path != null)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    unit.waypoints.Add(new Point(path[i].x, path[i].y));
                }
            }
        }

        public void AttackUnit(Character attacker, Character defender)
        {
            StateManager.AddState(Settings.STATES.Fight);
            ((FightState)StateManager.GetState(0)).SetUnits(attacker, defender);
        }

        public int[,] DuplicateLevel()
        {
            int[,] tempLevel = new int[level.GetLength(0), level.GetLength(1)];

            for (int i = 0; i < level.GetLength(0); i++)
            for (int j = 0; j < level.GetLength(1); j++)
            {
                tempLevel[i, j] = level[i, j];
            }

            return tempLevel;
        }

        public void SetUnwalkable(ref int[,] level, Character c)
        {
            for (int i = 0; i < Armies.army.Count(); i++)
            {
                Point gridpos = Armies.army[i].GridPosition;

                if (!Armies.army[i].IsDead)
                    level[gridpos.X, gridpos.Y] = 1;
            }

            for (int i = 0; i < Armies.opponentArmy.Count(); i++)
            {
                Point gridpos = Armies.opponentArmy[i].GridPosition;

                if (!Armies.opponentArmy[i].IsDead)
                    level[gridpos.X, gridpos.Y] = 1;
            }

            Point Gpos = c.GridPosition;
            level[Gpos.X, Gpos.Y] = 0;
        }
    }
}
