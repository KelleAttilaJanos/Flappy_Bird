using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Flappy_Bird
{
    public partial class Form1 : Form
    {
        int position = 200;
        int down = 4;
        int pipespeed = -1;
        int score = 0;
        public Form1()
        {
            hatter a = new hatter();
            InitializeComponent();
            PictureBox[,] oszlopok = new PictureBox[2, 7];
            int[] oposition = new int[7];
            int[] randomsz = new int[7];
            a.maxrecord=a.Import();
            label2.Text = "Record: " + a.maxrecord;
            Random szam = new Random();
            for (int i = 0; i < 7; i++)
            {
                int rszam = szam.Next(180, 400);
                oposition[i] = 1000+i*300;
                oszlopok[0, i] = new PictureBox();
                oszlopok[1, i] = new PictureBox();
                oszlopok[0, i].BackColor = Color.White;
                oszlopok[1, i].BackColor = Color.White;
                randomsz[i] = rszam;
                oszlopok[1, i].Location = new Point(oposition[i], randomsz[i]);
                oszlopok[0, i].Location = new Point(oposition[i], 0);
                oszlopok[0, i].Size = new Size(60, randomsz[i] - 130);
                oszlopok[1, i].Size = new Size(60, 1000 - randomsz[i]);
                this.Controls.Add(oszlopok[1, i]);
                this.Controls.Add(oszlopok[0, i]);
            }
            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(new Rectangle(0, 0, pictureBox2.Width - 1, pictureBox2.Height - 1));
            pictureBox2.Region = new Region(gp);
            pictureBox2.Location = new Point(50, position);
            KeyDown += (s, e) => { timer1.Start(); label2.Text ="0 pont"; };
            int time = 0;
            int tav = 0;
            timer1.Tick += (s, e) =>
            {
                time++;
                if(pipespeed<10)
                {
                    pipespeed = -1-Convert.ToInt32(time / 1000);
                }
                else
                {
                    pipespeed = 10;
                }
                if(position<0)
                {
                    down = 10;
                }
                tav += pipespeed * -1;
                position += down;
                pictureBox2.Location = new Point(50, position);
                for (int i = 0; i < 7; i++)
                {
                    oposition[i] += pipespeed;
                    if(oposition[i]<-1100)
                    {
                        oposition[i] = 1000;
                        randomsz[i] = szam.Next(180, 400);
                    }
                    oszlopok[0, i].Location = new Point(oposition[i], 0);
                    oszlopok[1, i].Location = new Point(oposition[i], randomsz[i]);
                    oszlopok[0, i].Size = new Size(60, randomsz[i] - 130);
                    oszlopok[1, i].Size = new Size(60, 1000 - randomsz[i]);
                }
                score = Convert.ToInt32((tav - 700) / 300);
                label1.Text = Convert.ToString(Convert.ToInt32(time / 70 / 60)) + ":" + Convert.ToString(Convert.ToInt32(time / 70 % 60));
                if (score > 0) { label2.Text = score + " pont";}
                for (int i = 0; i < 7; i++)
                {
                    if (oszlopok[0, i].Bounds.IntersectsWith(pictureBox2.Bounds) || oszlopok[1, i].Bounds.IntersectsWith(pictureBox2.Bounds))
                    {
                        timer1.Stop();
                        label1.Text = ("Game Over");
                        if (a.Export(score))
                        {
                            label2.Text = "New Record";
                        }
                        else
                        {
                            label2.Text = "Try Again";
                        }
                    }
                }
                if (pictureBox2.Bounds.IntersectsWith(pictureBox1.Bounds))
                {
                    timer1.Stop();
                    label1.Text = ("Game Over");
                    if (a.Export(score))
                    {
                        label2.Text = "New Record";
                    }
                    else
                    {
                        label2.Text = "Try Again";
                    }
                }
            };
            KeyDown += (s, e) => { down = -4; }; 
            KeyUp += (s, e) => { down = 3; };
            KeyDown += (s, e) => { if (label1.Text == "Game Over") { Application.Restart(); } };
        }
    }
}
