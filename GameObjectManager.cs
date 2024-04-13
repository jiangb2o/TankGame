using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TankGame
{
    class GameObjectManager
    {
        enum WallType
        {
            Wall,
            Steel,
        }

        public static int WindowWidth { get; set; }
        public static int WindowHeight { get; set; }

        private static Dictionary<WallType, Image> wallImage = new Dictionary<WallType, Image>()
        {
            [WallType.Wall] = Properties.Resources.wall,
            [WallType.Steel] = Properties.Resources.steel,
        };

        private static Dictionary<WallType, List<UnMovable>> wallList = new Dictionary<WallType, List<UnMovable>>()
        {
            [WallType.Wall] = new List<UnMovable>(),
            [WallType.Steel] = new List<UnMovable>(),
        };

        private static UnMovable theBase;
        private static Player player;

        private static int gridWidth = 15;
        private static int gridHeight = 15;

        public static void CreateMap()
        {
            CreateWall(2, 2, 2, 10, WallType.Wall);
            CreateWall(6, 2, 2, 10, WallType.Wall);
            CreateWall(10, 2, 2, 8, WallType.Wall);
            CreateWall(12, 7, 2, 2, WallType.Steel);
            CreateWall(14, 2, 2, 8, WallType.Wall);
            CreateWall(18, 2, 2, 10, WallType.Wall);
            CreateWall(22, 2, 2, 10, WallType.Wall);

            CreateWall(4, 14, 4, 2, WallType.Wall);
            CreateWall(18, 14, 4, 2, WallType.Wall);

            CreateWall(10, 12, 2, 2, WallType.Wall);
            CreateWall(14, 12, 2, 2, WallType.Wall);

            CreateWall(0, 15, 2, 1, WallType.Steel);
            CreateWall(24, 15, 2, 1, WallType.Steel);
            
            CreateWall(2, 18, 2, 10, WallType.Wall);
            CreateWall(6, 18, 2, 10, WallType.Wall);
            CreateWall(10, 16, 2, 8, WallType.Wall);
            CreateWall(12, 17, 2, 2, WallType.Wall);
            CreateWall(14, 16, 2, 8, WallType.Wall);
            CreateWall(18, 18, 2, 10, WallType.Wall);
            CreateWall(22, 18, 2, 10, WallType.Wall);

            CreateWall(11, 27, 1, 3, WallType.Wall);
            CreateWall(12, 27, 2, 1, WallType.Wall);
            CreateWall(14, 27, 1, 3, WallType.Wall);

            CreateBase(12, 28);
        }

        public static void Update()
        {
            foreach (var list in wallList.Values)
            {
                foreach (UnMovable wall in list)
                {
                    wall.Update();
                }
            }
            theBase.Update();
            player.Update();
        }
        

        // TODO: 四叉树优化
        public static UnMovable CollidedWhichWall(Rectangle rect)
        {
            // 墙体
            foreach (var list in wallList.Values)
            {
                foreach (UnMovable wall in list)
                {
                    if(wall.GetRectangle().IntersectsWith(rect))
                     {
                        return wall;
                    }
                }
            }
            // 基地
            if (theBase.GetRectangle().IntersectsWith(rect))
                return theBase;

            return null;
        }

        public static void CreatePlayer(int x, int y, int speed, Direction dir = Direction.Up)
        {
            player = new Player(x * gridWidth, y * gridHeight, speed, WindowWidth, WindowHeight, dir);
        }

        // x,y: grid axis
        // create count wall x to x + width; y to y + height
        private static void CreateWall(int x, int y, int width, int height, WallType walltype)
        {
            int xPosition = x * gridWidth;
            int yPosition = y * gridHeight;

            int endXPosition = xPosition + width * gridWidth;
            int endYposition = yPosition + height * gridHeight;

            for (int i = xPosition; i < endXPosition; i += gridWidth)
            {
                for (int j = yPosition; j < endYposition; j += gridHeight)
                {
                    // create a wall
                    UnMovable wall = new UnMovable(i, j, wallImage[walltype]);

                    wallList[walltype].Add(wall);
                }
            }

        }

        private static void CreateBase(int x, int y)
        {
            theBase = new UnMovable(x * gridWidth, y * gridHeight, Properties.Resources.Base);
        }

        public static void KeyDown(KeyEventArgs args)
        {
            player.KeyDown(args);
        }

        public static void KeyUp(KeyEventArgs args)
        {
            player.KeyUp(args);
        }
    }
}
