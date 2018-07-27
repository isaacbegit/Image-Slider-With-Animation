using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlideImagesWithAnimation
{
    public partial class Form1 : Form
    {
        int SliderCounter = 0;
        string[] Imgs_slider;
        Random rnd = new Random();
        Util.Effect EffectType=Util .Effect .Center;

        public Form1()
        {
            InitializeComponent();
        }
        public static class Util
        {
            public enum Effect { Roll, Slide, Center, Blend }

            public static void Animate(Control ctl, Effect effect, int msec, int angle)
            {
                int flags = effmap[(int)effect];

                if (ctl.Visible)
                {
                    flags |= 0x10000; angle += 180;
                }
                else
                {
                    if (ctl.TopLevelControl == ctl) flags |= 0x20000;
                    else if (effect == Effect.Blend) throw new ArgumentException();
                }

                flags |= dirmap[(angle % 360) / 45];
                bool ok = AnimateWindow(ctl.Handle, msec, flags);
                if (!ok) throw new Exception("Animation failed");
                ctl.Visible = !ctl.Visible;
            }

            private static int[] dirmap = { 1, 5, 4, 6, 2, 10, 8, 9 };
            private static int[] effmap = { 0, 0x40000, 0x10, 0x80000 };

            [DllImport("user32.dll")]
            private static extern bool AnimateWindow(IntPtr handle, int msec, int flags);
        }

        static string[] LstImgs(string FilePath)
        {
            string Imagepaths = Application.StartupPath + @"\" + FilePath;
            DirectoryInfo d = new DirectoryInfo(Imagepaths);
            FileInfo[] Files = d.GetFiles(); // Getting Files

            string[] ret = new string[Files.Length];
            for (int x = 0; x < Files.Length; x++)
            {
                ret[x] = Files[x].Name;
            }
            return ret;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            Slideimg.Image = Image.FromFile(Application.StartupPath + @"\slides\1.jpg");// initial image at startup app
            Imgs_slider = LstImgs("slides");// load all images names 
        }
       
        private void timer1_Tick(object sender, EventArgs e)
        {
            int rand_effect = rnd.Next(0, Imgs_slider.Length - 1);

            if (Imgs_slider.Length > SliderCounter)
            {
                //Sliderimg2.Image = Image.FromFile(Application.StartupPath + @"\sliders\" + Imgs_slider[rand_effect]);
                Slideimg.Image = Image.FromFile(Application.StartupPath + @"\slides\" + Imgs_slider[SliderCounter]);
                Util.Animate(Slideimg, EffectType, 250, 360);
                SliderCounter += 1;

            }
            else
            {
                SliderCounter = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                EffectType = Util.Effect.Center;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                EffectType = Util.Effect.Roll;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                EffectType = Util.Effect.Slide ;
            }
        }
    }
}
