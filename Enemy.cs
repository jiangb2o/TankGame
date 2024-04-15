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
        private int attackSpeed { get; set; }
        private int attackCount = 0;
        private int turnSpeed { get; set; }
        private int turnCount = 0;

        public bool IsDestroy = false;

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
            this.attackSpeed = 60;
            this.turnSpeed = rd.Next(50, 100);
        }

        public override void Update()
        {
            MoveCheck();
            Move();
            AttackCheck();
            AutoTurn();

            base.Update();
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
            //if (GameObjectManager.CollidedWhichWall(rectNext) != null)
            //{
            //    ChangeDirection();
            //}
            List<GameObject> collided = GameObjectManager.Collided(rectNext, this);
            foreach(var collidedObj in collided)
            {
                // 包含了与墙和player以及别的enemy tank的碰撞
                // 若在当前位置重生了一个坦克, 则无论如何ChangeDirection都会碰撞, 造成溢出.
                if(collidedObj.GetType() != typeof(Bullet))
                {
                    Console.WriteLine(collidedObj.GetType());
                    ChangeDirection();
                    return;
                }
            }
        }

        private void AutoTurn()
        {
            turnCount++;
            if (turnCount < turnSpeed) return;

            turnCount = 0;
            turnSpeed = rd.Next(50, 100);
            ChangeDirection();
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

        private void AttackCheck()
        {
            attackCount++;
            if (attackCount < attackSpeed) return;
            
            attackCount = 0;
            Attack();
            
        }

        private void Attack()
        {
            int x = this.X;
            int y = this.Y;
            switch (Dir)
            {
                case Direction.Up:
                    x += Width / 2;
                    break;
                case Direction.Down:
                    x += Width / 2;
                    y += Height;
                    break;
                case Direction.Left:
                    y += Height / 2;
                    break;
                case Direction.Right:
                    x += Width;
                    y += Height / 2;
                    break;
            }
            GameObjectManager.CreateBullet(x, y, Dir, BulletBelong.Enemy);
        }

    }
}
