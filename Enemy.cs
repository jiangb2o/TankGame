using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankGame
{
    // 添加Count代表enum的长度
    enum EnemyType
    {
        Gray,
        Green,
        Slow,
        Quick,
        Count
    }

    class Enemy : Movable
    {
        private static Bitmap[][] enemyBitmap = new Bitmap[(int)EnemyType.Count][];
        private static int[] enemySpeed = new int[(int)EnemyType.Count];

        private static Random rd = new Random();

        public static void InitializationEnemy()
        {
            enemyBitmap[(int)EnemyType.Gray] = new Bitmap[4]
            { Properties.Resources.GrayUp, Properties.Resources.GrayDown, Properties.Resources.GrayLeft, Properties.Resources.GrayRight };

            enemyBitmap[(int)EnemyType.Green] = new Bitmap[4]
            { Properties.Resources.GreenUp, Properties.Resources.GreenDown, Properties.Resources.GreenLeft, Properties.Resources.GreenRight };

            enemyBitmap[(int)EnemyType.Slow] = new Bitmap[4]
            { Properties.Resources.SlowUp, Properties.Resources.SlowDown, Properties.Resources.SlowLeft, Properties.Resources.SlowRight };

            enemyBitmap[(int)EnemyType.Quick] = new Bitmap[4]
            { Properties.Resources.QuickUp, Properties.Resources.QuickDown, Properties.Resources.QuickLeft, Properties.Resources.QuickRight };

            enemySpeed[(int)EnemyType.Gray] = 2;
            enemySpeed[(int)EnemyType.Green] = 2;
            enemySpeed[(int)EnemyType.Slow] = 1;
            enemySpeed[(int)EnemyType.Quick] = 4;
        }

        public Enemy(int x, int y, int windowWidth, int windowHeight, EnemyType enemyType, Direction dir = Direction.Down)
        {
            this.X = x;
            this.Y = y;
            this.WindowWidth = windowWidth;
            this.WindowHeigth = windowHeight;

            int et = (int)enemyType;
            this.Speed = enemySpeed[et];
            BitmapUp = enemyBitmap[et][0];
            BitmapDown = enemyBitmap[et][1];
            BitmapLeft = enemyBitmap[et][2];
            BitmapRight = enemyBitmap[et][3];

            this.Dir = dir;
        }

        public override void Update()
        {
            MoveCheck();
            Move();
            base.Update();
        }

        private void Move()
        {
            switch (Dir)
            {
                case Direction.Up: Y -= Speed; break;
                case Direction.Down: Y += Speed; break;
                case Direction.Left: X -= Speed; break;
                case Direction.Right: X += Speed; break;
            }
        }

        private void MoveCheck()
        {
            // 不超出窗体
            switch (Dir)
            {
                case Direction.Up when Y - Speed < 0:
                case Direction.Down when Y + Speed + Height > WindowHeigth:
                case Direction.Left when X - Speed < 0:
                case Direction.Right when X + Speed + Width > WindowWidth: ChangeDirection(); break;
            }

            // 碰撞检测
            Rectangle rectNext = GetRectangle();
            switch (Dir)
            {
                case Direction.Up: rectNext.Y -= Speed; break;
                case Direction.Down: rectNext.Y += Speed; break;
                case Direction.Left: rectNext.X -= Speed; break;
                case Direction.Right: rectNext.X += Speed; break;
            }
            if (GameObjectManager.CollidedWhichWall(rectNext) != null)
            {
                ChangeDirection();
            }
        }

        private void ChangeDirection()
        {
            int nextDir = rd.Next(0, 4);
            while(nextDir == (int)Dir)
            {
                nextDir = rd.Next(0, 4);
            }
            Dir = (Direction)nextDir;

            // 改变方向后要判断新方向是否碰撞
            MoveCheck();
        }
    }
}
