using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TankGame
{
    enum GameState
    {
        Running,
        GameOver,
    }
    enum DrawMode
    { 
        Normal,
        Rectangle
    }

    class GameFramwork
    {
        public static Graphics g;
        private static GameState gameState = GameState.Running;

        private static int windowWidth;
        private static int windowHeight;

        public static DrawMode DrawMode = DrawMode.Normal;


        public static void SetWindowSize(int width, int height)
        {
            GameObjectManager.WindowWidth = width;
            GameObjectManager.WindowHeight = height;
            windowWidth = width;
            windowHeight = height;
        }

        public static void Start()
        {
            // only create once
            GameObjectManager.Start();
            GameObjectManager.CreateMap();
            GameObjectManager.CreatePlayer(8, 28, 2);

            SoundManager.InitSound();
            SoundManager.PlayStart();
        }

        public static void Update()
        {
            if(gameState == GameState.Running)
            {
                GameObjectManager.Update();
            }else if (gameState == GameState.GameOver)
            {
                GameOverUpdate();
            }

        }

        public static void GameOver()
        {
            gameState = GameState.GameOver;
        }

        private static void GameOverUpdate()
        {
            Bitmap bmp = Properties.Resources.GameOver;
            g.DrawImage(bmp, windowWidth / 2 - bmp.Width / 2, windowHeight / 2 - bmp.Height / 2);
        }
    }
}
