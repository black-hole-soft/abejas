using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;

namespace Abejas
{
    public class Abeja
    {
        int x, y, d, desp, v, madCount = 0, happyCount = -1;
        internal int screenWitdh = 800, screenHeight = 600;
        bool shk = false, mad = false;
        String mood = "h";
        
        public bool free = true;
        public PictureBox bee;
       
        Random rand = new Random();
        Point p;
        int ax;

        static bool disturb = false;
        static Point mouse;
        
        public Abeja(int i)
        {
            Thread.Sleep(1);
            d = rand.Next(3);
            Thread.Sleep(1);
            x = rand.Next(screenWitdh);
            Thread.Sleep(1);
            y = rand.Next(screenHeight);
            Thread.Sleep(1);
            v = rand.Next(17,25);
            Thread.Sleep(1);
            desp = rand.Next(7,15);
            bee = new PictureBox();
            bee.Location = new System.Drawing.Point(x, y);
            bee.Size = new System.Drawing.Size(50, 50);
            bee.Image = Image.FromFile("h" + d + "bee.png");
            bee.Tag = i;
        }
        public void disturbBees(bool dist)
        {
            disturb = dist;
        }
        public void objetivoAbejas(Point ob)
        {
            mouse = ob;
        }
        delegate void mueveAbeja();
        internal void flyBee()
        {
            if (bee.InvokeRequired)
                bee.Invoke(new mueveAbeja(flyBee));
            else
                bee.Location = p;
        }
        delegate void humorAbeja(String h, String d);
        internal void humorBee(String h, String d)
        {
            if (bee.InvokeRequired)
                bee.Invoke(new humorAbeja(humorBee),h,d);
            else
                bee.Image = Image.FromFile(h + d + "bee.png");
        }
        public void vuelo()
        {
            while (true)
            {
                if (!mad && disturb && madCount == 0 && happyCount == -1)
                    happyCount = rand.Next(40);
                if (happyCount > 0)
                    happyCount--;
                else
                {
                    if (happyCount != -1)
                    {
                        mad = true;
                        mood = "a";
                        v = rand.Next(7, 15);
                        desp = rand.Next(17, 35);
                        madCount = rand.Next(50, 100);
                        humorBee(mood, d.ToString());
                        happyCount = -1;
                    }
                }
                if (mad && !disturb)
                {
                    if (madCount > 0)
                        humorBee(mood, d.ToString());
                    else
                    {
                        mad = false;
                        mood = "h";
                        v = rand.Next(17, 25);
                        desp = rand.Next(7, 15);
                        humorBee(mood, d.ToString());
                    }
                }
                if (free)
                {
                    Thread.Sleep(v);
                    if (madCount > 0 && !disturb)
                        madCount--;
                    p = bee.Location;
                    if (mad && disturb)
                    {
                        ax = d;
                        if (mouse.X - p.X >= 0 && mouse.Y - p.Y >= 0)
                            d = 0;
                        if (mouse.X - p.X >= 0 && mouse.Y - p.Y < 0)
                            d = 1;
                        if (mouse.X - p.X < 0 && mouse.Y - p.Y >= 0)
                            d = 2;
                        if (mouse.X - p.X < 0 && mouse.Y - p.Y < 0)
                            d = 3;
                        if(ax != d)
                            humorBee(mood, d.ToString());
                        p.X = p.X + ((mouse.X - p.X) / 20);
                        p.Y = p.Y + ((mouse.Y - p.Y) / 20);
                        //linea(p.X, p.Y, mouse.X, mouse.Y);
                    }
                    else
                    {
                        if (d == 0)
                        {
                            p.X += desp;
                            p.Y += desp;
                        }
                        if (d == 1)
                        {
                            p.X += desp;
                            p.Y -= desp;
                        }
                        if (d == 2)
                        {
                            p.X -= desp;
                            p.Y += desp;
                        }
                        if (d == 3)
                        {
                            p.X -= desp;
                            p.Y -= desp;
                        }
                        if (p.X < 0)
                        {
                            p.X = 0;
                            if (d == 3)
                                d = 1;
                            if (d == 2)
                                d = 0;
                            humorBee(mood, d.ToString());
                        }
                        if (p.Y < 0)
                        {
                            p.Y = 0;
                            if (d == 3)
                                d = 2;
                            if (d == 1)
                                d = 0;
                            humorBee(mood, d.ToString());
                        }
                        if (p.X > screenWitdh)
                        {
                            p.X = screenWitdh;
                            if (d == 0)
                                d = 2;
                            if (d == 1)
                                d = 3;
                            humorBee(mood, d.ToString());
                        }
                        if (p.Y > screenHeight)
                        {
                            p.Y = screenHeight;
                            if (d == 0)
                                d = 1;
                            if (d == 2)
                                d = 3;
                            humorBee(mood, d.ToString());
                        }
                    }
                    flyBee();
                }
                else
                {
                    Thread.Sleep(20);
                    if (shk)
                    {
                        humorBee("a", "1");
                        shk = false;
                    }
                    else
                    {
                        humorBee("a", "3");
                        shk = true;
                    }
                }
            }
        }
        void linea(int xa, int ya, int xb, int yb)
        {
            int dx, dy, tx, ty, p, p0, p1, x, y, aux;
            bool f = false;
            if (xa == xb && ya == yb)
                return;
            dx = Math.Abs(xb - xa);
            dy = Math.Abs(yb - ya);
            if (dx < dy)
            {
                f = true;
                aux = xa; xa = ya; ya = aux;
                aux = xb; xb = yb; yb = aux;
                aux = dx; dx = dy; dy = aux;
            }
            tx = (xb - xa) > 0 ? 1 : -1;
            ty = (yb - ya) > 0 ? 1 : -1;
            x = xa;
            y = ya;
            p0 = 2 * dy;
            p1 = 2 * (dy - dx);
            p = p0 - dx;
            while (x != xb)
            {
                if (p < 0)
                    p += p0;
                else
                {
                    y += ty;
                    p += p1;
                }
                if (f)
                    this.p = new Point(y, x);
                else
                    this.p = new Point(x, y);
                x += tx;
                Thread.Sleep(v);
            }
        }
    }
}
