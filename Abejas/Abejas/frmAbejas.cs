using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;

namespace Abejas
{
    public partial class frmAbejas : Form
    {
        public Abeja[] bee;
        public Thread[] hilo;
        int bees = 10, cb = -1;
        public bool ciclar = true;
        public Point p, x;
        public frmAbejas()
        {
            bee = new Abeja[bees];
            hilo = new Thread[bees];
            InitializeComponent();
            inicializarAbejas();
        }
        public void inicializarAbejas()
        {
            this.TransparencyKey = Color.Black;
            int i;
            for (i = 0; i < bees; i++)
            {
                bee[i] = new Abeja(i);
                bee[i].bee.MouseMove += new System.Windows.Forms.MouseEventHandler(MouseMove);
                bee[i].bee.MouseDown += new System.Windows.Forms.MouseEventHandler(MouseEvent);
                bee[i].bee.MouseUp += new System.Windows.Forms.MouseEventHandler(MouseEvent);
                this.Controls.Add(bee[i].bee);
                hilo[i] = new Thread(new ThreadStart(bee[i].vuelo));
                hilo[i].Start();
            }
        }
        private void MouseEvent(object sender, MouseEventArgs e)
        {
            PictureBox b = (PictureBox)sender;
            cb = (int)b.Tag;
            if (bee[cb].free)
            {
                bee[cb].disturbBees(true);
                bee[cb].free = false;
                x = e.Location;
            }
            else
            {
                bee[cb].disturbBees(false);
                bee[cb].free = true;
                cb = -1;
            }
        }
        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (cb >= 0)
            {
                p = bee[cb].bee.Location;
                p.X = p.X + e.Location.X;
                p.Y = p.Y + e.Location.Y;
                bee[cb].objetivoAbejas(p);
                p.X = p.X - x.X;
                p.Y = p.Y - x.Y;
                bee[cb].bee.Location = p;
            }
        }
        private void frmAbejas_FormClosing(object sender, FormClosingEventArgs e)
        {
            int i;
            for (i = 0; i < bees;i++ )
                hilo[i].Abort();
        }
        private void frmAbejas_Load(object sender, EventArgs e)
        {
            foreach (Abeja b in bee)
            {
                b.screenWitdh = this.Size.Width;
                b.screenHeight = this.Size.Height;
            }
        }
    }
}
