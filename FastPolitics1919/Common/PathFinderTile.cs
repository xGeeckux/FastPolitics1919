using FastPolitics1919.Data.Common;
using FastPolitics1919.Data.Common.MapObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.Common
{
    public class PathFinderTile : MapObject
    {
        private PathFinderTile Parrent { get; set; }

        private double DistanceStart { get; set; }
        private double DistanceEnd { get; set; }
        private double DistanceCombined { get { return DistanceStart + DistanceEnd; } }

        #region Map
        public HexagonTile Hex
        {
            get
            {
                foreach (HexagonTile tile in Engine.Map.Provinces)
                {
                    if (tile != null && tile.ID == ID)
                        return tile;
                }
                return null;
            }
        }
        public List<PathFinderTile> GetPathNeighbours()
        {
            List<PathFinderTile> tiles = new List<PathFinderTile>();
            foreach (HexagonTile tile in Hex.GetNeighbours())
            {
                tiles.Add(tile.Tile);
            }
            return tiles;
        }
        #endregion

        #region PathFinder
        public Tile[] FindPath(PathFinderTile end)
        {
            List<PathFinderTile> open_set = new List<PathFinderTile>();
            List<PathFinderTile> closed_set = new List<PathFinderTile>();

            open_set.Add(this);

            while (open_set.Count > 0)
            {
                PathFinderTile current = open_set[0];
                for (int i = 1; i < open_set.Count; i++)
                    if (open_set[i].DistanceCombined < current.DistanceCombined || (open_set[i].DistanceCombined == current.DistanceCombined && open_set[i].DistanceEnd < current.DistanceEnd))
                        current = open_set[i];
                open_set.Remove(current);
                closed_set.Add(current);

                if (current == end)
                    return RetracePath(this, end).ToArray();

                foreach (PathFinderTile neighbour in current.GetPathNeighbours())
                {
                    if (neighbour == null || closed_set.Contains(neighbour))
                        continue;

                    double distance_to_neighbour = current.DistanceStart + GetDistance(current, neighbour);
                    if (distance_to_neighbour < neighbour.DistanceStart || !open_set.Contains(neighbour))
                    {
                        neighbour.DistanceStart = distance_to_neighbour;
                        neighbour.DistanceEnd = GetDistance(neighbour, end);
                        neighbour.Parrent = current;

                        if (!open_set.Contains(neighbour))
                            open_set.Add(neighbour);
                    }
                }
            }
            return null;
        }
        private List<Tile> RetracePath(PathFinderTile start, PathFinderTile target)
        {
            List<Tile> items = new List<Tile>();
            PathFinderTile current = target;
            while (current != start)
            {
                items.Add((Tile)current);
                current = current.Parrent;
            }
            items.Reverse();
            return items;
        }
        public static double GetDistance(PathFinderTile one, PathFinderTile two)
        {
            double[] cor_one = one.Hex.GetCanvasCors();
            double[] cor_two = two.Hex.GetCanvasCors();

            double x_distance = Math.Abs(cor_one[0] - cor_two[0]);
            double y_distance = Math.Abs(cor_one[1] - cor_two[1]);

            return Math.Sqrt(x_distance * x_distance + y_distance * y_distance);
        }
        #endregion
    }
}
