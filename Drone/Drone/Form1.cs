using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using OpenCvSharp;
using System.Threading;
using System.Diagnostics;

namespace Drone
{
    public partial class Form1 : Form
    {
        static int count=0;
        static bool run = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }


        private void StartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            run = true;
        }
        public void mode1(String FileName, int gap)
        {
            //************************ open cv start ***************************************
            //http://m.blog.naver.com/darknata/70100445630

            //Cv.NamedWindow("Image View");//, WindowMode.AutoSize
            CvCapture capture = Cv.CreateFileCapture(FileName);//"redsea.mp4");
            IplImage imageframe;
            char Keyboard_Code;

            Boolean sig=false;

            while (true)
            {
                imageframe = Cv.QueryFrame(capture);
                if (imageframe == null) break;


                //red calculate

                //color select
                //selectHSVColorage(58, 72);
                //CvPoint[] points = new CvPoint[imageframe.Width * imageframe.Height];

                int startx_red = 0, starty_red = 0, endx_red = 0, endy_red = 0;
                int startx_green = 0, starty_green = 0, endx_green = 0, endy_green = 0;
                using (IplImage dst = new IplImage(imageframe.Size, BitDepth.U8, 3))
                using (IplImage h = new IplImage(imageframe.Size, BitDepth.U8, 1))
                using (IplImage s = new IplImage(imageframe.Size, BitDepth.U8, 1))
                using (IplImage v = new IplImage(imageframe.Size, BitDepth.U8, 1))
                {
                    Cv.CvtColor(imageframe, dst, ColorConversion.BgrToHsv);
                    Cv.Split(dst, h, s, v, null);
                    Cv.InRangeS(h, 4, 6, h);
                    Cv.InRangeS(v, 50, 200, v);
                    dst.SetZero();
                    Cv.Copy(imageframe, dst, h);
                    Cv.Copy(imageframe, dst, v);
                    //Cv.copy(img, dst, s);
                    imageframe = dst;

                    //for pointer collect
                    int count_red = 0;
                    int count_green = 0;

                    for (int x = 0; x < dst.Width; x+=gap)
                    {
                        for (int y = 0; y < dst.Height; y+=gap)
                        {
                            CvColor c = dst[y, x];
                            //black            green             blue except
                            //if ((c.R < 50) || (c.G < 50) || (c.B < 50) ||(c.G>200 && c.B>200 && c.R>200))
                            //{
                            //    continue;
                            //}

                            if (c.B + c.G + c.R > 270) continue;
                            //if(c.R<100) if (c.B > 50 || c.G > 50) continue;
                            if (c.R > 100 && c.G < 90 && c.B < 90)
                            {
                                if (count_red == 0)
                                {
                                    startx_red = x;
                                    starty_red = y;
                                }
                                else
                                {
                                    if (startx_red > x) startx_red = x;
                                    if (starty_red > y) starty_red = y;
                                    if (endx_red < x) endx_red = x;
                                    if (endy_red < y) endy_red = y;
                                }
                                count_red++;
                            }
                            else if (c.G > 100 && c.R < 90 && c.B < 90)
                            {
                                if (count_green == 0)
                                {
                                    startx_green = x;
                                    starty_green = y;
                                }
                                else
                                {
                                    if (startx_green > x) startx_green = x;
                                    if (starty_green > y) starty_green = y;
                                    if (endx_green < x) endx_green = x;
                                    if (endy_green < y) endy_green = y;
                                }
                                count_green++;
                            }
                            
                            //Console.WriteLine(x + " " + y);
                        }
                    }
                    if (count_red > 1) imageframe.Rectangle(startx_red, starty_red, endx_red, endy_red, new CvColor(255, 0, 0), 2);
                    if (count_green > 1) imageframe.Rectangle(startx_green, starty_green, endx_green, endy_green, new CvColor(0, 255, 0), 2);
                    
                    Cv.ShowImage("LoadVedio", imageframe);

                    Keyboard_Code = (char)Cv.WaitKey(1);
                    //Console.Write(Keyboard_Code);
                    if (Keyboard_Code == 's') break;
                    if (Keyboard_Code == 'p')
                    {
                        while (true)
                        {
                            Keyboard_Code = (char)Cv.WaitKey(1);
                            if (Keyboard_Code == 'p') break;
                        }
                    }
                    //Refresh();


                    if (count_red*gap*gap > imageframe.Width * imageframe.Height / 256)
                    {
                        if (!sig) System.Windows.Forms.MessageBox.Show("RED Warnning");
                        sig = true;
                    }
                    if (count_green*gap*gap > imageframe.Width * imageframe.Height / 16384)//16384
                    {
                        if (!sig) System.Windows.Forms.MessageBox.Show("GREEN Warnning");
                        sig = true;
                    }
                }


                //Cv.ReleaseCapture(capture);
                ////Cv.ShowImage("imgee", imageframe);

                //Cv.WaitKey();
                //Cv.DestroyWindow("Image View");
                //Cv.ReleaseImage(imageframe);
                //new CvSeqReader();
            }
            Cv.DestroyAllWindows();
        }

        public void pictureTake()
        {
            try
            {
                String filename = count + ".jpeg";
                //Console.Write();
                Process myProcess;

                myProcess = Process.Start("node", "fileIO.js " + count);
                count++;
                count = count % 8;
                // Display physical memory usage 5 times at intervals of 2 seconds.

                Thread.Sleep(10500);//1000
                // Close process by sending a close message to its main window.
                myProcess.CloseMainWindow();
                // Free resources associated with process.
                myProcess.Close();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("The following exception was raised: ");
                Console.WriteLine(ex.Message);
            }
        }

        public void cmdTake(char key)
        {
            try
            {
                Process myProcess;

                myProcess = Process.Start("node", "index.js " + key);
                // Display physical memory usage 5 times at intervals of 2 seconds.

                Thread.Sleep(1000);
                // Close process by sending a close message to its main window.
                myProcess.CloseMainWindow();
                // Free resources associated with process.
                myProcess.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine("The following exception was raised: ");
                Console.WriteLine(ex.Message);
            }
        }

        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //file name choose
            OpenFileDialog choofdlog = new OpenFileDialog();
            choofdlog.Filter = "MOV Files (.mov)|*.mov|MP4 Files (.mp4)|*.mp4|JPEG Files (.jpeg)|*.jpeg|All Files (*.*)|*.*";
            choofdlog.FilterIndex = 1;

            choofdlog.Multiselect = true;
            choofdlog.ShowDialog();

            String FileName = choofdlog.FileName;
            if (FileName.Contains("DJI")) mode1(FileName, 4);
            else mode1(FileName, 1);
        }

        private void QuitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Console.WriteLine(e.KeyChar);

            if (run)
            {
                char key = e.KeyChar;
                if (key == 'p') run = false;
                else if (key == '5') pictureTake();
                else if (key == '6')
                {
                    IplImage img = Cv.LoadImage(count + ".jpeg");

                    int startx_red = 0, starty_red = 0, endx_red = 0, endy_red = 0;
                    int startx_green = 0, starty_green = 0, endx_green = 0, endy_green = 0;
                    using (IplImage dst = new IplImage(img.Size, BitDepth.U8, 3))
                    using (IplImage h = new IplImage(img.Size, BitDepth.U8, 1))
                    using (IplImage s = new IplImage(img.Size, BitDepth.U8, 1))
                    using (IplImage v = new IplImage(img.Size, BitDepth.U8, 1))
                    {
                        Cv.CvtColor(img, dst, ColorConversion.BgrToHsv);
                        Cv.Split(dst, h, s, v, null);
                        Cv.InRangeS(h, 4, 6, h);
                        Cv.InRangeS(v, 50, 200, v);
                        dst.SetZero();
                        Cv.Copy(img, dst, h);
                        Cv.Copy(img, dst, v);
                        //Cv.copy(img, dst, s);
                        img= dst;

                        //for pointer collect
                        int count_red = 0;
                        int count_green = 0;

                        for (int x = 0; x < dst.Width; x ++)
                        {
                            for (int y = 0; y < dst.Height; y ++)
                            {
                                CvColor c = dst[y, x];
                                //black            green             blue except
                                //if ((c.R < 50) || (c.G < 50) || (c.B < 50) ||(c.G>200 && c.B>200 && c.R>200))
                                //{
                                //    continue;
                                //}

                                if (c.B + c.G + c.R > 270) continue;
                                //if(c.R<100) if (c.B > 50 || c.G > 50) continue;
                                if (c.R > 100 && c.G < 90 && c.B < 90)
                                {
                                    if (count_red == 0)
                                    {
                                        startx_red = x;
                                        starty_red = y;
                                    }
                                    else
                                    {
                                        if (startx_red > x) startx_red = x;
                                        if (starty_red > y) starty_red = y;
                                        if (endx_red < x) endx_red = x;
                                        if (endy_red < y) endy_red = y;
                                    }
                                    count_red++;
                                }
                                else if (c.G > 100 && c.R < 90 && c.B < 90)
                                {
                                    if (count_green == 0)
                                    {
                                        startx_green = x;
                                        starty_green = y;
                                    }
                                    else
                                    {
                                        if (startx_green > x) startx_green = x;
                                        if (starty_green > y) starty_green = y;
                                        if (endx_green < x) endx_green = x;
                                        if (endy_green < y) endy_green = y;
                                    }
                                    count_green++;
                                }

                                //Console.WriteLine(x + " " + y);
                            }
                        }
                        if (count_red > 1) img.Rectangle(startx_red, starty_red, endx_red, endy_red, new CvColor(255, 0, 0), 2);
                        if (count_green > 1) img.Rectangle(startx_green, starty_green, endx_green, endy_green, new CvColor(0, 255, 0), 2);

                        Cv.ShowImage("LoadVedio", img);
                        

                        if (count_red > img.Width * img.Height / 256)
                        {
                            System.Windows.Forms.MessageBox.Show("RED Warnning");
                            
                        }
                        if (count_green > img.Width * img.Height / 16384)//16384
                        {
                            System.Windows.Forms.MessageBox.Show("GREEN Warnning");
                            
                        }
                    }
                    img.Dispose();
                }
                else if (key == 'w' || key == 's' || key == 'a' || key == 'd' || key == 'q' || key == 'e' || key == '1' || key == '2' || key == '3' || key == '4' || key == 'f' || key == 'g' || key == 'o')
                {
                    cmdTake(key);
                }
            }


        }
    }
}

public class Worker
{
    // This method will be called when the thread is started.
    public void DoWork()
    {
        while (!_shouldStop)
        {
            Console.WriteLine("worker thread: working...");
        }
        Console.WriteLine("worker thread: terminating gracefully.");
    }
    public void RequestStop()
    {
        _shouldStop = true;
    }
    // Volatile is used as hint to the compiler that this data
    // member will be accessed by multiple threads.
    private volatile bool _shouldStop;
}