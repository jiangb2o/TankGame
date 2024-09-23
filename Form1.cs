using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;

namespace TankGame
{
    public partial class Form1 : Form
    {
        private Thread mainThread;
        private static Graphics windowGraphics;
        private static Bitmap tmpBitmap;

        public Form1()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            // this.StartPosition = FormStartPosition.Manual;
            // 主屏幕左上角为(0, 0)
            // y x--
            // | 
            // this.Location = new Point(960, -540);
            windowGraphics = this.CreateGraphics();

            // 先绘制到临时画布上
            tmpBitmap = new Bitmap(windowWidth, windowHeight);
            Graphics tmpGraphics = Graphics.FromImage(tmpBitmap);
            GameFramwork.g = tmpGraphics;

            mainThread = new Thread(new ThreadStart(GameMainThread));
            mainThread.Start();


        }

        [System.Runtime.InteropServices.DllImport("winmm")]
        static extern uint timeGetTime();


        // 静态方法, 直接通过类调用
        private static void GameMainThread()
        {
            // GameFramwork
            GameFramwork.SetWindowSize(windowWidth, windowHeight);
            GameFramwork.Start();

            float sleepTime = 1000 / 75;
            int cnt = 0;
            uint frameBegin = timeGetTime();
            while(true)
            {
                // 先清空画布为黑色, 再绘制物体, 会出现闪烁
                // GameFramwork.g = windowGraphics;
                // GameFramwork.Clear(Color.Blcak);
                // GameFramwork.Update();
                
                uint begin = timeGetTime();
                
                // 先全部绘制到临时画布, 再将临时画布一次性绘制到窗口
                GameFramwork.g.Clear(Color.Black);
                GameFramwork.Update();
                windowGraphics.DrawImage(tmpBitmap, 0, 0);

                cnt++;
                if (cnt == 10)
                {
                    uint deltaTime = timeGetTime() - frameBegin;
                    Console.WriteLine(cnt / (float)deltaTime * 1000 + "fps");

                    cnt = 0;
                    frameBegin = timeGetTime();
                }

                uint end = timeGetTime();

                // 60 fps is enough
                if (sleepTime - (end - begin) > 0)
                {
                    Thread.Sleep((int)(sleepTime - (end - begin)));
                }


            }

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 窗口关闭后退出主线程
            mainThread.Abort();
        }

        // 监听按键, 按下移动, 抬起停止移动
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            GameObjectManager.KeyDown(e);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            GameObjectManager.KeyUp(e);
        }
    }
}
