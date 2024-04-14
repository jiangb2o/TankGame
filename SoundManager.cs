using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace TankGame
{
    enum SoundType
    {

    }

    class SoundManager
    {
        // 无法同时播放同一个音效
        //private static SoundPlayer startPlayer = new SoundPlayer();
        //private static SoundPlayer addPlayer = new SoundPlayer();
        //private static SoundPlayer blastPlayer = new SoundPlayer();
        //private static SoundPlayer firePlayer = new SoundPlayer();
        //private static SoundPlayer hitPlayer = new SoundPlayer();
        

        public static void InitSound()
        {
            //startPlayer.Stream = Properties.Resources.start;
            //addPlayer.Stream = Properties.Resources.add;
            //blastPlayer.Stream = Properties.Resources.blast;
            //firePlayer.Stream = Properties.Resources.fire;
            //hitPlayer.Stream = Properties.Resources.hit;

        }

        public static void PlayStart()
        {
            SoundPlayer startPlayer = new SoundPlayer();
            startPlayer.Stream = Properties.Resources.start;
            startPlayer.Play();
        }

        public static void PlayAdd()
        {
            SoundPlayer addPlayer = new SoundPlayer();
            addPlayer.Stream = Properties.Resources.add;
            addPlayer.Play();
        }

        public static void PlayBlast()
        {
            SoundPlayer blastPlayer = new SoundPlayer();
            blastPlayer.Stream = Properties.Resources.blast;
            blastPlayer.Play();
        }

        public static void PlayFire()
        {
            SoundPlayer firePlayer = new SoundPlayer();
            firePlayer.Stream = Properties.Resources.fire;
            firePlayer.Play();
        }

        public static void PlayHit()
        {
            SoundPlayer hitPlayer = new SoundPlayer();
            hitPlayer.Stream = Properties.Resources.hit;
            hitPlayer.Play();
        }

    }
}
