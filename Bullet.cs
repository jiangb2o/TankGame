using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankGame
{
    enum BulletBelong
    {
        Player,
        Enemy
    }

    class Bullet : Movable
    {
        public BulletBelong BelongTo;
        public bool isDestroy = false;

        public Bullet(int x, int y, int speed, int windowWidth, int windowHeight, Direction dir, BulletBelong belong)
        {
            this.X = x;
            this.Y = y;
            this.Speed = speed;
            this.WindowWidth = windowWidth;
            this.WindowHeigth = windowHeight;
            this.BelongTo = belong;

            BitmapUp = Properties.Resources.BulletUp;
            BitmapDown = Properties.Resources.BulletDown;
            BitmapLeft = Properties.Resources.BulletLeft;
            BitmapRight = Properties.Resources.BulletRight;

            this.Dir = dir;
            // 调整子弹图片中心为输入的(x, y)坐标
            this.X -= this.Width / 2;
            this.Y -= this.Height / 2;
        }

        public override void Update()
        {
            MoveCheck();
            Move();
            base.Update();
        }

        private void MoveCheck()
        {
            // 不超出窗体
            // 不需要预判下一个位置
            // 要考虑子弹在图片中心, 子弹出窗体才销毁
            switch (Dir)
            {
                case Direction.Up when Y + Height / 2 < 0:
                case Direction.Down when Y + Height / 2 > WindowHeigth:
                case Direction.Left when X + Width / 2 < 0:
                case Direction.Right when X + Width / 2 > WindowWidth:
                    // 标记要销毁的子弹, 不能直接销毁, 外面正在遍历bulletList, 不能直接从bulletList中删除
                    isDestroy = true;
                    return;
            }

            // 碰撞检测
            // Unmovable: wall, steel, base
            // enemy
            Rectangle rect = GetRectangle();
            UnMovable collidedObject = null;
            collidedObject = GameObjectManager.CollidedWhichWall(rect);
            if (collidedObject != null)
            {
                isDestroy = true;
                if(collidedObject.MyType == UnmovableType.Wall)
                { 
                    GameObjectManager.DestroyWall(collidedObject);
                }
                else if(collidedObject.MyType == UnmovableType.TheBase)
                {
                    GameObjectManager.DestroyTheBase(collidedObject);
                }
            }

            // 与敌人进行碰撞检测
            if(this.BelongTo == BulletBelong.Player)
            {
                Enemy enemy = null;
                enemy = GameObjectManager.CollidedWichEnemy(rect);
                if(enemy != null)
                {
                    isDestroy = true;
                    GameObjectManager.DestroyEnemy(enemy);
                }
            }
        }
    }
}
