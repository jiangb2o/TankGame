using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TankGame
{
    class GameFramwork
    {
        public static Graphics g;

        public static void SetWindowSize(int width, int height)
        {
            GameObjectManager.WindowWidth = width;
            GameObjectManager.WindowHeight = height;
        }

        public static void Start()
        {
            // only create once
            GameObjectManager.Start();
            GameObjectManager.CreateMap();
            GameObjectManager.CreatePlayer(8, 28, 2);
        }

        public static void Update()
        {
            GameObjectManager.Update();

        }
    }
}
