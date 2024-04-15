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
        public bool IsDestroy = false;

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
                    IsDestroy = true;
                    return;
            }

            int xAnimation = this.X + Width / 2;
            int yAnimation = this.Y + Height / 2;
            // 碰撞检测
            // Unmovable: wall, steel, base
            // enemy
            Rectangle rect = GetRectangle();
            UnMovable collidedObject = null;
            collidedObject = GameObjectManager.CollidedWhichWall(rect);
            if (collidedObject != null)
            {
                IsDestroy = true;
                if (collidedObject.MyType == UnmovableType.Steel)
                {
                    //SoundManager.Playhit();
                } else
                {
                    if (collidedObject.MyType == UnmovableType.Wall)
                    {
                        GameObjectManager.DestroyWall(collidedObject);                    }
                    else if (collidedObject.MyType == UnmovableType.TheBase)
                    {
                        GameObjectManager.DestroyTheBase(collidedObject);
                        GameFramwork.GameOver();
                    }
                    SoundManager.PlayBlast();
                    GameObjectManager.CreateAnimation(xAnimation, yAnimation, AnimationType.Explosion);
                }
            }

            // 与敌人进行碰撞检测
            if(this.BelongTo == BulletBelong.Player)
            {
                Enemy enemy = null;
                enemy = GameObjectManager.CollidedWichEnemy(rect);
                if(enemy != null)
                {
                    IsDestroy = true;
                    enemy.IsDestroy = true;
                    SoundManager.PlayBlast();
                    GameObjectManager.CreateAnimation(xAnimation, yAnimation, AnimationType.Explosion);
                }
            } else if (this.BelongTo == BulletBelong.Enemy)
            {
                Player player = null;
                player = GameObjectManager.CollidedPlayer(rect);
                if(player != null)
                {
                    IsDestroy = true;
                    player.TakeDamage();
                    SoundManager.PlayHit();
                    GameObjectManager.CreateAnimation(xAnimation, yAnimation, AnimationType.Explosion);
                }
            }
            
        }
    }
}
