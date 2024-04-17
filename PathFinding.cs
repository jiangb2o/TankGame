using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankGame
{
    class PathFinding
    {
        public static List<int>[][] MapGrid;
        public static int GridWidth {set; get; }
        public static int GridHeight {set; get; }
        public static int MapWidth { set; get; }
        public static int MapHeight { set; get; }

        public static void InitMapGrid(int gridWidth, int gridHeight, int width, int height)
        {
            GridWidth = gridWidth;
            GridHeight = gridHeight;
            MapWidth = width;
            MapHeight = height;

            MapGrid = new List<int> [MapHeight / GridHeight][];
            for(int i = 0; i < MapGrid.Count(); ++i)
            {
                MapGrid[i] = new List<int>[MapWidth / GridWidth];
            }
        }


        public static void Dijkstra()
        {

        }
    }
}
