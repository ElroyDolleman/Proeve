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
        private int[,] level;
        private List<Character> army;
        private List<Character> enemyArmy;

        private E2DTexture background;

        public GameState()
        {

        }

        public override void Initialize()
        {
            level = Levels.grassLevel;
            StateManager.AddState(Settings.STATES.ArmyEditor);
            background = ArtAssets.backgroundGrassLevel;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Character c in army)
            {
                if (c.waypoints.Count > 0)
                {
                    c.Position = Grid.ToPixelLocation(new Point(c.waypoints[0].X,c.waypoints[0].Y),Globals.GridLocation,Globals.TileDimensions).ToVector2();
                    if (Vector2.Distance(c.Position, c.waypoints[0].ToVector2()) > 4)
                    {
                        c.MoveTowards(c.waypoints[0].ToVector2(), 4);
                    }
                    else
                    {
                        c.waypoints.RemoveAt(0);
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawE2DTexture(background, Vector2.Zero);

            /*for (int i = 0; i < level.GetLength(0); i++)
            for (int j = 0; j < level.GetLength(1); j++)
            {
                if (level[i, j] == 0)
                {
                    spriteBatch.DrawRectangle(new Vector2(200 + j * 50 + 1, 50 + i * 50 + 1), 48, 48, Color.White);
                }
            }*/

            foreach (Character c in army)
            {
                c.sprite.Draw(spriteBatch);
            }

            foreach (Character c in enemyArmy)
            {
                c.sprite.Draw(spriteBatch);
            }
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
            int[,] tempLevel = new int[level.GetLength(0), level.GetLength(1)];

            for (int i = 0; i < tempLevel.GetLength(0); i++)
            for (int j = 0; j < tempLevel.GetLength(1); j++)
            {
                tempLevel[i, j] = level[i, j];
            }


            for (int i = 0; i < ((GameState)StateManager.GetState(1)).GetArmy().Count(); i++)
            {
                Point gridpos = Grid.ToGridLocation(new Point((int)army[i].Position.X, (int)army[i].Position.Y), Globals.GridLocation, Globals.TileDimensions);

                //Console.WriteLine(gridpos);

                tempLevel[gridpos.X, gridpos.Y] = 1;
            }
            for (int i = 0; i < ((GameState)StateManager.GetState(1)).GetEnemyArmy().Count(); i++)
            {
                Point gridpos = Grid.ToGridLocation(new Point((int)enemyArmy[i].Position.X, (int)enemyArmy[i].Position.Y), Globals.GridLocation, Globals.TileDimensions);
                tempLevel[gridpos.X, gridpos.Y] = 1;
            }

            Point GPos = Grid.ToGridLocation(new Point((int)unit.Position.X, (int)unit.Position.Y), Globals.GridLocation, Globals.TileDimensions);
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
            Point gridPos = Grid.ToGridLocation(new Point((int)unit.Position.X, (int)unit.Position.Y),Globals.GridLocation,Globals.TileDimensions);
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
    }
}
