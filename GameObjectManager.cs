using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TankGame
{
    enum UnmovableType
    {
        Wall,
        Steel,
        TheBase,
    }
    class GameObjectManager
    {
        public static int WindowWidth { get; set; }
        public static int WindowHeight { get; set; }

        private static int gridWidth = 15;
        private static int gridHeight = 15;

        private static Dictionary<UnmovableType, Image> wallImage = new Dictionary<UnmovableType, Image>()
        {
            [UnmovableType.Wall] = Properties.Resources.wall,
            [UnmovableType.Steel] = Properties.Resources.steel,
        };
        private static Dictionary<UnmovableType, List<UnMovable>> wallList = new Dictionary<UnmovableType, List<UnMovable>>()
        {
            [UnmovableType.Wall] = new List<UnMovable>(),
            [UnmovableType.Steel] = new List<UnMovable>(),
        };
        private static List<Enemy> enemyTankList = new List<Enemy>();
        private static List<Bullet> bulletList = new List<Bullet>();

        private static UnMovable theBase;
        private static Player player;

        private static Point[] enemyInitialPostions;

        // one per 60 frame
        private static int enemyCreateSpeed = 60;
        // 初始生成一个敌人
        private static int enemyCreateCount = 60;

        public static void Start()
        {
            enemyInitialPostions = new Point[3];
            enemyInitialPostions[0] = new Point(0, 0);
            enemyInitialPostions[1] = new Point(12 * gridWidth, 0);
            enemyInitialPostions[2] = new Point(24 * gridWidth, 0);

            Enemy.InitializationEnemy();
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
            if(theBase != null)
            {
                theBase.Update();
            }

            player.Update();

            EnemyCreate();
            foreach (var tank in enemyTankList)
            {
                tank.Update();
            }

            DestroyBullet();
            foreach (var bullet in bulletList)
            {
                bullet.Update();
            }

        }

        public static void CreateMap()
        {
            CreateWall(2, 2, 2, 10, UnmovableType.Wall);
            CreateWall(6, 2, 2, 10, UnmovableType.Wall);
            CreateWall(10, 2, 2, 8, UnmovableType.Wall);
            CreateWall(12, 7, 2, 2, UnmovableType.Steel);
            CreateWall(14, 2, 2, 8, UnmovableType.Wall);
            CreateWall(18, 2, 2, 10, UnmovableType.Wall);
            CreateWall(22, 2, 2, 10, UnmovableType.Wall);

            CreateWall(4, 14, 4, 2, UnmovableType.Wall);
            CreateWall(18, 14, 4, 2, UnmovableType.Wall);

            CreateWall(10, 12, 2, 2, UnmovableType.Wall);
            CreateWall(14, 12, 2, 2, UnmovableType.Wall);

            CreateWall(0, 15, 2, 1, UnmovableType.Steel);
            CreateWall(24, 15, 2, 1, UnmovableType.Steel);

            CreateWall(2, 18, 2, 10, UnmovableType.Wall);
            CreateWall(6, 18, 2, 10, UnmovableType.Wall);
            CreateWall(10, 16, 2, 8, UnmovableType.Wall);
            CreateWall(12, 17, 2, 2, UnmovableType.Wall);
            CreateWall(14, 16, 2, 8, UnmovableType.Wall);
            CreateWall(18, 18, 2, 10, UnmovableType.Wall);
            CreateWall(22, 18, 2, 10, UnmovableType.Wall);

            CreateWall(11, 27, 1, 3, UnmovableType.Wall);
            CreateWall(12, 27, 2, 1, UnmovableType.Wall);
            CreateWall(14, 27, 1, 3, UnmovableType.Wall);

            CreateBase(12, 28);
        }

        public static void CreatePlayer(int x, int y, int speed, Direction dir = Direction.Up)
        {
            player = new Player(x * gridWidth, y * gridHeight, speed, WindowWidth, WindowHeight, dir);
        }

        public static void CreateBullet(int x, int y, Direction dir, BulletBelong belong)
        {
            Bullet bullet = new Bullet(x, y, 8, WindowWidth, WindowHeight, dir, belong);
            bulletList.Add(bullet);
        }

        public static void DestroyBullet()
        {
            List<Bullet> needDestroy = new List<Bullet>();
            foreach(Bullet bullet in bulletList)
            {
                if(bullet.isDestroy)
                {
                    needDestroy.Add(bullet);
                }
            }
            foreach(Bullet bullet in needDestroy)
            {
                bulletList.Remove(bullet);
            }
        }

        public static void DestroyWall(UnMovable wall)
        {
            wallList[UnmovableType.Wall].Remove(wall);
        }

        public static void DestroyTheBase(UnMovable collidedObject)
        {
            theBase = null;
        }

        public static void DestroyEnemy(Enemy enemy)
        {

        }
            


        private static void EnemyCreate()
        {
            enemyCreateCount++;
            if (enemyCreateCount < enemyCreateSpeed) return;

            enemyCreateCount = 0;

            Random rd = new Random();
            int index = rd.Next(0, 3);
            Point position = enemyInitialPostions[index];

            EnemyType tanktype = (EnemyType)rd.Next(0, (int)EnemyType.Count);
            Enemy tank = new Enemy(position.X, position.Y, WindowWidth, WindowHeight, tanktype);
            enemyTankList.Add(tank);
        }

        // TODO: 四叉树优化
        public static UnMovable CollidedWhichWall(Rectangle rect)
        {
            // 墙体
            foreach (var list in wallList.Values)
            {
                foreach (UnMovable wall in list)
                {
                    if (wall.GetRectangle().IntersectsWith(rect))
                    {
                        return wall;
                    }
                }
            }
            // 基地
            if (theBase != null && theBase.GetRectangle().IntersectsWith(rect))
                return theBase;

            return null;
        }

        // x,y: grid axis
        // create count wall x to x + width; y to y + height
        private static void CreateWall(int x, int y, int width, int height, UnmovableType walltype)
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
                    UnMovable wall = new UnMovable(i, j, wallImage[walltype], walltype);

                    wallList[walltype].Add(wall);
                }
            }

        }

        private static void CreateBase(int x, int y)
        {
            theBase = new UnMovable(x * gridWidth, y * gridHeight, Properties.Resources.Base, UnmovableType.TheBase);
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
