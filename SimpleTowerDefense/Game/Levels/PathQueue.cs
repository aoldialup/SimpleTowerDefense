
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace SimpleTowerDefense
{
    internal class PathQueue
    {
        private List<Point> paths;

        public int Count
        {
            get
            {
                return paths.Count;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return Count == 0;
            }
        }

        public PathQueue()
        {
            paths = new List<Point>();
        }

        public PathQueue(PathQueue other)
        {
            paths = new List<Point>(other.paths);
        }

        public void Enqueue(Point path)
        {
            paths.Add(path);
        }

        public Point Dequeue()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Queue is empty");
            }

            Point frontPath = paths[0];
            paths.RemoveAt(0);

            return frontPath;
        }

        public Point Peek(int distance = 0)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Queue is empty");
            }

            return paths[distance];
        }
    }
}
