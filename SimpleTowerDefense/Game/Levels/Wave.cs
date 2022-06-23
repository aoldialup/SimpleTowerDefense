
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using Engine;
using System.Linq;

namespace SimpleTowerDefense
{
    internal enum MovementDirection
    {
        NONE,
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    internal class Wave
    {
        private const char BRICK_TILE = 'B';
        private const char GRASS_TILE = 'G';
        private const char TOWER_TILE = 'T';

        private ZombieStack zombieStack;
        private List<Entity> entityLayout;
        private List<PathQueue> paths;

        public bool IsValid { get; } = true;

        public int TowerIndex { get; private set; }

        private int waveNumber;

        public Wave(Game1 game, string levelLayoutPath, int waveNumber)
        {
            this.waveNumber = waveNumber;

            zombieStack = new ZombieStack();
            entityLayout = new List<Entity>(Game1.WAVE_MIN_ENTITIES);
            paths = new List<PathQueue>();

            StreamReader inFile = StreamReader.Null;

            try
            {
                inFile = File.OpenText(levelLayoutPath);

                LoadTileLayout(game.Content, inFile);
                LoadPaths(inFile);
                LoadEnemyLayout(game.Content, inFile);
            }
            catch
            {
                IsValid = false;
            }
            finally
            {
                inFile?.Close();
            }
        }

        public ZombieStack GetZombies()
        {
            return new ZombieStack(zombieStack);
        }

        private PathQueue GetRandomPath()
        {
            return new PathQueue(Utility.Choice(paths));
        }

        public List<Entity> GetLayout()
        {
            return entityLayout;
        }

        private void LoadEnemyLayout(ContentManager content, StreamReader inFile)
        {
            try
            {
                foreach (char c in inFile.ReadLine())
                {
                    if (int.TryParse(c.ToString(), out int zombieType))
                    {
                        switch ((EntityType)zombieType)
                        {
                            case EntityType.EASY_ZOMBIE:
                            zombieStack.Push(new EasyZombie(content, GetRandomPath(), waveNumber));
                            break;

                            case EntityType.MEDIUM_ZOMBIE:
                            zombieStack.Push(new MediumZombie(content, GetRandomPath(), waveNumber));
                            break;

                            case EntityType.HARD_ZOMBIE:
                            zombieStack.Push(new HardZombie(content, GetRandomPath(), waveNumber));
                            break;

                            case EntityType.BOSS_ZOMBIE:
                            zombieStack.Push(new BossZombie(content, GetRandomPath(), waveNumber));
                            break;

                            default:
                            throw new Exception();
                        }
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private bool TryParsePoint(string args, out Point p)
        {
            string[] argsArray = args.Split(',');

            if (argsArray.Length == 2)
            {
                if (int.TryParse(argsArray[0], out int row) &&
                    int.TryParse(argsArray[1], out int col))
                {
                    if (Game1.IsCoordValid(row, col))
                    {
                        p = new Point(row, col);
                        return true;
                    }
                }
            }


            p = new Point(-1, -1);

            return false;
        }


        private void LoadTileLayout(ContentManager content, StreamReader inFile)
        {
            int tileRow = 0;
            string[] data;
            string line;

            try
            {
                while (!(line = inFile.ReadLine()).Equals(";"))
                {
                    data = line.Split(',');

                    if (data.Any(x => x.Equals("")))
                    {
                        throw new Exception();
                    }

                    if (data.Length != Game1.TILES_WIDE || tileRow == Game1.TILES_HIGH)
                    {
                        throw new Exception();
                    }

                    for (int i = 0; i < data.Length; i++)
                    {
                        for (int j = 0; j < data[i].Length; j++)
                        {
                            switch (data[i][j])
                            {
                                case GRASS_TILE:
                                entityLayout.Add(new GrassEntity(content, Game1.CoordToWorldPosition(i, tileRow)));
                                break;

                                case BRICK_TILE:
                                entityLayout.Add(new BrickEntity(content, Game1.CoordToWorldPosition(i, tileRow)));
                                break;

                                case TOWER_TILE:
                                if (entityLayout.Exists(x => x.Type == EntityType.TOWER))
                                {
                                    throw new Exception();
                                }

                                entityLayout.Add(new Tower(content, Game1.CoordToWorldPosition(i, tileRow)));
                                TowerIndex = entityLayout.Count - 1;
                                break;

                                default:
                                throw new Exception();
                            }
                        }
                    }

                    tileRow++;
                }

                if (tileRow != Game1.TILES_HIGH)
                {
                    throw new Exception();
                }

                if (!entityLayout.Exists(x => x.Type == EntityType.TOWER))
                {
                    throw new Exception();
                }
            }
            catch
            {
                throw;
            }
        }

        private void LoadPaths(StreamReader inFile)
        {
            string line;
            int pathIndex;
            string[] pathPointsRaw;

            try
            {
                while (!(line = inFile.ReadLine()).Equals(";"))
                {
                    pathPointsRaw = line.Split('|');

                    paths.Add(new PathQueue());
                    pathIndex = paths.Count - 1;

                    foreach (string pathPoint in pathPointsRaw)
                    {
                        if (TryParsePoint(pathPoint, out Point p))
                        {
                            paths[pathIndex].Enqueue(Game1.CoordToWorldPosition(p.Y, p.X));
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }

                    if (!IsPathValid(paths[pathIndex]))
                    {
                        throw new Exception();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private bool IsPathValid(PathQueue path)
        {
            Point currentPath = Game1.ZombieSpawnPoint;
            Point nextPath;

            for (int i = 0; i < path.Count; i++)
            {
                nextPath = path.Peek(i);

                if (GetMovementDirection(nextPath, currentPath) == MovementDirection.NONE)
                {
                    return false;
                }

                currentPath = nextPath;
            }

            return true;
        }

        private MovementDirection GetMovementDirection(Point nextPath, Point currentPath)
        {
            if (currentPath.Y == nextPath.Y && currentPath.X < nextPath.X)
            {
                return MovementDirection.RIGHT;
            }

            if (currentPath.Y == nextPath.Y && currentPath.X > nextPath.X)
            {
                return MovementDirection.LEFT;
            }

            if (currentPath.X == nextPath.X && currentPath.Y < nextPath.Y)
            {
                return MovementDirection.DOWN;
            }

            if (currentPath.X == nextPath.X && currentPath.Y > nextPath.Y)
            {
                return MovementDirection.UP;
            }

            return MovementDirection.NONE;
        }
    }
}