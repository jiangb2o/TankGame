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

        public void KeyDown(KeyEventArgs args)
        {
            IsMoving = true;
            switch(args.KeyCode)
            {
                case Keys.W: Dir = Direction.Up; break;
                case Keys.S: Dir = Direction.Down; break;
                case Keys.A: Dir = Direction.Left; break;
                case Keys.D: Dir = Direction.Right; break;
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
