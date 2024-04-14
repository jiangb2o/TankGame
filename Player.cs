using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TankGame
{
    class Player : Movable
    {
        private int originalX;
        private int originalY;
        private int originalHP = 4;

        public bool IsMoving { get; set; }
        public int HP { get; set; }


        public Player(int x, int y, int speed, int windowWidth, int windowHeight, Direction dir = Direction.Up)
        {
            IsMoving = false;
            this.X = x;
            this.Y = y;
            originalX = x;
            originalY = y;
            this.Speed = speed;
            this.WindowWidth = windowWidth;
            this.WindowHeigth = windowHeight;

            BitmapUp = Properties.Resources.MyTankUp;
            BitmapDown = Properties.Resources.MyTankDown;
            BitmapLeft = Properties.Resources.MyTankLeft;
            BitmapRight = Properties.Resources.MyTankRight;
            // Dir set方法中使用了Bitmap, 所以先对Bitmap复制再给Dir赋值
            this.Dir = dir;

            this.HP = originalHP;
        }

        // KeyDown 可能与 GameMainThread 中 GetImage 同时使用Bitmap造成冲突
        public void KeyDown(KeyEventArgs args)
        {
            if(args.KeyCode == Keys.W || args.KeyCode == Keys.S || args.KeyCode == Keys.A || args.KeyCode == Keys.D)
            {
                IsMoving = true;
            }
            switch(args.KeyCode)
            {
                // 方向相同时不进行改变, 降低方向改变时使用Bitmap频率, 降低锁冲突
                case Keys.W when Dir != Direction.Up: 
                    Dir = Direction.Up; 
                    break;
                case Keys.S when Dir != Direction.Down: 
                    Dir = Direction.Down;
                    break;
                case Keys.A when Dir != Direction.Left: 
                    Dir = Direction.Left;
                    break;
                case Keys.D when Dir != Direction.Right: 
                    Dir = Direction.Right;
                    break;
                // 发射子弹
                case Keys.Space:
                    Attack();
                    break;
            }
        }

        public override void Update()
        {
            MoveCheck();
            if (IsMoving)
            {
                Move();
            }
            base.Update();
        }

        private void MoveCheck()
        {
            // 不超出窗体
            switch(Dir)
            {
                case Direction.Up when Y - Speed < 0: 
                case Direction.Down when Y + Speed + Height > WindowHeigth: 
                case Direction.Left when X - Speed < 0: 
                case Direction.Right when X + Speed + Width > WindowWidth: IsMoving = false; break;
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
                IsMoving = false;
            }
        }

        private void Attack()
        {
            SoundManager.PlayFire();
            int x = this.X;
            int y = this.Y;
            switch(Dir)
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
            // 可能与GameMainThread 中 GameObjectManager Update中遍历同时进行发生冲突
            GameObjectManager.CreateBullet(x, y, Dir, BulletBelong.Player);
        }

        public void KeyUp(KeyEventArgs args)
        {
            IsMoving = false;
        }

        public void TakeDamage()
        {
            HP--;
            if(HP <= 0)
            {
                X = originalX;
                Y = originalY;
                HP = originalHP;
            }

        }

    }
}
