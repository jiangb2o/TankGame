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
        public bool IsMoving { get; set; }

        public Player(int x, int y, int speed, int windowWidth, int windowHeight, Direction dir = Direction.Up)
        {
            IsMoving = false;
            this.X = x;
            this.Y = y;
            this.Speed = speed;
            this.WindowWidth = windowWidth;
            this.WindowHeigth = windowHeight;

            BitmapUp = Properties.Resources.MyTankUp;
            BitmapDown = Properties.Resources.MyTankDown;
            BitmapLeft = Properties.Resources.MyTankLeft;
            BitmapRight = Properties.Resources.MyTankRight;

            this.Dir = dir;
        }

        // KeyDown 可能与 GameMainThread 中 GetImage 同时使用Bitmap造成冲突
        public void KeyDown(KeyEventArgs args)
        {
            IsMoving = true;
            switch(args.KeyCode)
            {
                // 方向相同时不进行改变, 降低方向改变时使用Bitmap频率, 降低锁冲突
                case Keys.W when Dir != Direction.Up: Dir = Direction.Up; break;
                case Keys.S when Dir != Direction.Down: Dir = Direction.Down; break;
                case Keys.A when Dir != Direction.Left: Dir = Direction.Left; break;
                case Keys.D when Dir != Direction.Right: Dir = Direction.Right; break;
            }
        }

        public override void Update()
        {
            MoveCheck();
            Move();
            base.Update();
        }

        private void Move()
        {
            if(IsMoving)
            {
                switch (Dir)
                {
                    case Direction.Up: Y -= Speed; break;
                    case Direction.Down: Y += Speed; break;
                    case Direction.Left: X -= Speed; break;
                    case Direction.Right: X += Speed; break;
                }

            }
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

        public void KeyUp(KeyEventArgs args)
        {
            IsMoving = false;
        }
    }
}
