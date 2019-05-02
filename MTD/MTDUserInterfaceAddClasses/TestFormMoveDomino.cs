using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MTDClasses;

namespace MTDUserInterface
{
    public partial class TestFormMoveDomino : Form
    {
        private bool dragging = false;
        private int xPosOfMouseInDomino;
        private int yPosOfMouseInDomino;

        private int xPosOriginalOfDomino;
        private int yPosOriginalOfDomino;

        private int count = 0;

        public TestFormMoveDomino()
        {
            InitializeComponent();
        }

        private void LoadDomino(PictureBox pb, Domino d)
        {
            pb.Image = Image.FromFile(System.Environment.CurrentDirectory
                        + "\\..\\..\\Dominos\\" + d.Filename);
        }

        private void pictureBox3_MouseMove(object sender, MouseEventArgs e)
        {
            count++;
            label2.Text = "Picture box mouse move (" + count.ToString() + ") "+ e.Location.ToString();
            
            PictureBox thisPB = (PictureBox)sender;
            if (dragging && thisPB != null)
            {
                thisPB.Top = e.Y + thisPB.Top - yPosOfMouseInDomino;
                thisPB.Left = e.X + thisPB.Left - xPosOfMouseInDomino;
            }
        }

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            label2.Text = "Picture box mouse down " + e.Location.ToString();
            PictureBox thisPB = (PictureBox)sender;
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                xPosOfMouseInDomino = e.X;
                yPosOfMouseInDomino = e.Y;

                xPosOriginalOfDomino = thisPB.Top;
                yPosOriginalOfDomino = thisPB.Left;
            }
        }

        private void pictureBox3_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
                        
            PictureBox thisPB = (PictureBox)sender;
            if (tableLayoutPanel1.ClientRectangle.Contains(thisPB.Location))
            {
                // check to see if this domino is playable on this train
                // snap to empty spot in train
            }
            else
            {
                thisPB.Top = xPosOriginalOfDomino;
                thisPB.Left = yPosOriginalOfDomino;
            }

        }

        private void TestFormMoveDomino_MouseMove(object sender, MouseEventArgs e)
        {
            label2.Text = "Form mouse move " + e.Location.ToString();
        }

        private void tableLayoutPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            label2.Text = "panel mouse move " + e.Location.ToString();
        }
    }
}
