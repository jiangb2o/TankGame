using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TankGame
{
    enum SoundType
    {
        Start,
        Add,
        Blast,
        Fire,
        Hit,
        Count
    }

    class SoundManager
    {
        // 无法同时播放同一个音效
        //private static SoundPlayer startPlayer = new SoundPlayer();
        //private static SoundPlayer addPlayer = new SoundPlayer();
        //private static SoundPlayer blastPlayer = new SoundPlayer();
        //private static SoundPlayer firePlayer = new SoundPlayer();
        //private static SoundPlayer hitPlayer = new SoundPlayer();
 
        private static String dirPath = @"E:\CS\tank\TankGame\asset\Sounds";
        private static String [] filePath = new String[(int)SoundType.Count];
        private static String[] aliases = new String[(int)SoundType.Count];

        [DllImport("winmm.dll", EntryPoint = "mciSendString", CharSet = CharSet.Unicode)]
        extern static ulong MciSendString(string command, string buffer, int bufferSize, IntPtr callback);

        public static void InitSound()
        {
            filePath[(int)SoundType.Start] = dirPath + @"\start.wav";
            filePath[(int)SoundType.Add] = dirPath + @"\add.wav";
            filePath[(int)SoundType.Blast] = dirPath + @"\blast.wav";
            filePath[(int)SoundType.Fire] = dirPath + @"\fire.wav";
            filePath[(int)SoundType.Hit] = dirPath + @"\hit.wav";

            aliases[(int)SoundType.Start] = "start";
            aliases[(int)SoundType.Add] = "add";
            aliases[(int)SoundType.Blast] = "blast";
            aliases[(int)SoundType.Fire] = "fire";
            aliases[(int)SoundType.Hit] = "hit";
        }

        public static void PlaySound(SoundType soundType)
        {
            MciSendString( $"open {filePath[(int)soundType]} alias {aliases[(int)soundType]}", null, 0, IntPtr.Zero);
            // 改变播放位置为开始重新播放
            if(soundType != SoundType.Start)
            {
                MciSendString($"seek {aliases[(int)soundType]} to start", null, 0, IntPtr.Zero);
            }
            MciSendString($"play {aliases[(int)soundType]}", null, 0, IntPtr.Zero);
        }

        public static void PlayStart()
        {
            PlaySound(SoundType.Start);
        }

        public static void PlayAdd()
        {

            PlaySound(SoundType.Add);
        }

        public static void PlayBlast()
        {

            PlaySound(SoundType.Blast);
        }

        public static void PlayFire()
        {

            PlaySound(SoundType.Fire);
        }

        public static void PlayHit()
        {

            PlaySound(SoundType.Hit);
        }

    }
}
