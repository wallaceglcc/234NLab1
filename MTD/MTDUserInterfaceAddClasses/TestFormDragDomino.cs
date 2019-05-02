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
    public partial class TestFormDragDomino : Form
    {
        private BoneYard pack;

        private Hand userHand;
        private List<PictureBox> userHandPBs;

        private PlayerTrain userTrain;
        private List<PictureBox> userTrainPBs;

        private Domino userDominoInPlay;
        private int indexOfDominoInPlay = -1;

        private void LoadDomino(PictureBox pb, Domino d)
        {
            pb.Image = Image.FromFile(System.Environment.CurrentDirectory
                        + "\\..\\..\\Dominos\\" + d.Filename);
        }

        private void LoadHand(List<PictureBox> pbs, Hand h)
        {
            for (int i = 0; i < pbs.Count; i++)
            {
                PictureBox pb = pbs[i];
                Domino d = h[i];
                LoadDomino(pb, d);
            }
        }

        public TestFormDragDomino()
        {
            InitializeComponent();

            pack = new BoneYard(9);
            userHand = new Hand(pack, 2);
            userTrain = new PlayerTrain(userHand, 0);

            // setup up - put all picture boxes in appropriate list of pbs
            // hand
            userHandPBs = new List<PictureBox>();
            // generally, there will be a method here with a loop in it
            userHandPBs.Add(pictureBox1);
            LoadHand(userHandPBs, userHand);
            // train
            userTrainPBs = new List<PictureBox>();
            userTrainPBs.Add(pictureBox2);
            userTrainPBs.Add(pictureBox3);

            // make last picture box in user's train playable
            pictureBox2.AllowDrop = true;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox handPB = (PictureBox)sender;

            indexOfDominoInPlay = userHandPBs.IndexOf(handPB);
            userDominoInPlay = userHand[indexOfDominoInPlay];

            handPB.DoDragDrop(userDominoInPlay, DragDropEffects.Move);
        }

        private void pictureBox2_DragEnter(object sender, DragEventArgs e)
        {
            PictureBox trainPB = (PictureBox)sender;
            Domino d = (Domino)e.Data.GetData("MTDClasses.Domino");
            bool mustFlip = false;

            if (userTrain.IsPlayable(d, out mustFlip))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void pictureBox2_DragDrop(object sender, DragEventArgs e)
        {
            PictureBox trainPB = (PictureBox)sender;
            Domino d = (Domino)e.Data.GetData("MTDClasses.Domino");
            bool mustFlip = false;

            if (userTrain.IsPlayable(d, out mustFlip))
            {
                if (mustFlip)
                    d.Flip();

                userHand.Play(d, userTrain);

                // add the domino to train pb
                LoadDomino(trainPB, d);
                // remove the domino from the pbs
                PictureBox handPB = userHandPBs[indexOfDominoInPlay];
                handPB.Image = null;
                // probably would be better here to have hand and train take care of themselves
            }
            else
                // make a sound or something that let's player know the domino is not playable here

            indexOfDominoInPlay = -1;
            userDominoInPlay = null;
        }
    }
}
