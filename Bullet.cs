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
            List<GameObject> collidedList = GameObjectManager.Collided(rect, this);
            for(int i = 0; i < collidedList.Count; ++i)
            {
                // 玩家子弹不与玩家进行检测, 敌人子弹不与敌人进行检测
                if(this.BelongTo == BulletBelong.Player && collidedList[i].GetType() == typeof(Player))
                {
                    continue;
                } else if (this.BelongTo == BulletBelong.Enemy && collidedList[i].GetType() == typeof(Enemy))
                {
                    continue;
                }

                IsDestroy = true;
                if(collidedList[i].GetType() == typeof(UnMovable))
                {
                    UnMovable tmp = (UnMovable)collidedList[i];
                    if (tmp.MyType == UnmovableType.Wall)
                    {
                        GameObjectManager.DestroyWall(tmp);
                    }
                    else if (tmp.MyType == UnmovableType.TheBase)
                    {
                        GameObjectManager.DestroyTheBase(tmp);
                        GameFramwork.GameOver();
                    }
                    SoundManager.PlayBlast();
                } else if (this.BelongTo == BulletBelong.Player && collidedList[i].GetType() == typeof(Enemy))
                {
                    collidedList[i].IsDestroy = true;
                    SoundManager.PlayBlast();
                } else if (this.BelongTo == BulletBelong.Enemy && collidedList[i].GetType() == typeof(Player))
                {
                    collidedList[i].IsDestroy = true;
                    Player tmp = (Player)collidedList[i];
                    tmp.TakeDamage();
                    SoundManager.PlayHit();
                }
                GameObjectManager.CreateAnimation(xAnimation, yAnimation, AnimationType.Explosion);
                break;
            }

            //UnMovable collidedObject = null;
            //collidedObject = GameObjectManager.CollidedWhichWall(rect);
            //if (collidedObject != null)
            //{
            //    IsDestroy = true;
            //    if (collidedObject.MyType == UnmovableType.Steel)
            //    {
            //        //SoundManager.Playhit();
            //    } else
            //    {
            //        if (collidedObject.MyType == UnmovableType.Wall)
            //        {
            //            GameObjectManager.DestroyWall(collidedObject);                    }
            //        else if (collidedObject.MyType == UnmovableType.TheBase)
            //        {
            //            GameObjectManager.DestroyTheBase(collidedObject);
            //            GameFramwork.GameOver();
            //        }
            //        SoundManager.PlayBlast();
            //        GameObjectManager.CreateAnimation(xAnimation, yAnimation, AnimationType.Explosion);
            //    }
            //}

            //// 与敌人进行碰撞检测
            //if(this.BelongTo == BulletBelong.Player)
            //{
            //    Enemy enemy = null;
            //    enemy = GameObjectManager.CollidedWichEnemy(rect);
            //    if(enemy != null)
            //    {
            //        IsDestroy = true;
            //        enemy.IsDestroy = true;
            //        SoundManager.PlayBlast();
            //        GameObjectManager.CreateAnimation(xAnimation, yAnimation, AnimationType.Explosion);
            //    }
            //} else if (this.BelongTo == BulletBelong.Enemy)
            //{
            //    Player player = null;
            //    player = GameObjectManager.CollidedPlayer(rect);
            //    if(player != null)
            //    {
            //        IsDestroy = true;
            //        player.TakeDamage();
            //        SoundManager.PlayHit();
            //        GameObjectManager.CreateAnimation(xAnimation, yAnimation, AnimationType.Explosion);
            //    }
            //}
            
        }
    }
}
