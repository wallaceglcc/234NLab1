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
    public partial class PlayMTD : Form
    {
        private BoneYard pack;

        private Hand userHand;
        private List<PictureBox> userHandPBs;
        private PlayerTrain userTrain;
        private List<PictureBox> userTrainPBs;

        private Hand computerHand;
        private PlayerTrain computerTrain;
        private List<PictureBox> computerTrainPBs;

        private MexicanTrain mexicanTrain;
        private List<PictureBox> mexicanTrainPBs;

        private Domino highestDoubleDomino;

        private Domino userDominoInPlay;
        private int indexOfDominoInPlay = -1;

        private int nextDrawIndex = 16;

        private int whosTurn = -1;
        private const int COMPUTER = 0;
        private const int USER = 1;

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

        private PictureBox CreateUserHandPB(int index)
        {
            PictureBox pb = new PictureBox();
            pb.Visible = true;
            pb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pb.Location = new System.Drawing.Point(24 + (index % 5) * 110, 366 + (index / 5) * 60);
            pb.Size = new System.Drawing.Size(100, 50);
            pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Controls.Add(pb);
            //pb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.handPB_MouseDown);
            return pb;
        }

        private void EnableHandPB(PictureBox pb)
        {
            pb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.handPB_MouseDown);
        }

        private void EnableUserHandPBs()
        {
            for (int i = 0; i < userHandPBs.Count; i++)
            {
                PictureBox pb = userHandPBs[i];
                EnableHandPB(pb);
            }
        }

        private void DisableHandPB(PictureBox pb)
        {
            pb.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.handPB_MouseDown);
        }

        private void DisableUserHandPBs()
        {
            for (int i = 0; i < userHandPBs.Count; i++)
            {
                PictureBox pb = userHandPBs[i];
                DisableHandPB(pb);
            }

        }

        public void SetupTrainPB(PictureBox pb, bool playable)
        {
            // TODO:  debug this
            pb.AllowDrop = false;
            pb.DragDrop -= new System.Windows.Forms.DragEventHandler(trainPB_DragDrop);
            pb.DragEnter -= new System.Windows.Forms.DragEventHandler(trainPB_DragEnter);
            if (playable)
            {
                pb.AllowDrop = true;
                pb.DragDrop += new System.Windows.Forms.DragEventHandler(trainPB_DragDrop);
                pb.DragEnter += new System.Windows.Forms.DragEventHandler(trainPB_DragEnter);
            }
            else
            {
                pb.AllowDrop = false;
                pb.DragDrop -= new System.Windows.Forms.DragEventHandler(trainPB_DragDrop);
                pb.DragEnter -= new System.Windows.Forms.DragEventHandler(trainPB_DragEnter);
            }
        }

        public void ScrollTrain(Train train, List<PictureBox> trainPBs)
        {
            LoadDomino(trainPBs[0], train[train.Count - 1]);
            for (int i = 1; i < 5; i++)
                trainPBs[i].Image = null;
        }

        public bool MustScroll(Train train)
        {
            // 5, 9, 13, 17
            if (train.Count < 5)
                return false;
            else
                return ((4 * (train.Count / 4) + 1) == train.Count);
        }

        public int NextPBIndex(Train train)
        {
            if (train.Count == 0)
                return 0;
            else if (train.Count % 4 == 0)
                return 4;
            else
                return (train.Count % 4);
        }

        public void UserPlayOnTrain(Domino d, PictureBox trainPB, Train train, List<PictureBox> trainPBs)
        {
            userHand.Play(d, train);

            // add the domino to train pb
            LoadDomino(trainPB, d);

            // remove the domino from the pbs
            PictureBox handPB = userHandPBs[indexOfDominoInPlay];
            handPB.Image = null;
            handPB.Visible = false;
            userHandPBs.RemoveAt(indexOfDominoInPlay);
            this.Controls.Remove(handPB);
            handPB = null;

            // make the next trainPB playable
            SetupTrainPB(trainPB, false);
            if (MustScroll(train))
                ScrollTrain(train, trainPBs);
            SetupTrainPB(trainPBs[NextPBIndex(train)], true);

            // ignore doubles for right now
            if (train == userTrain)
                userTrain.Close();

            // disable the users hand and update labels in UI
            if (userTrain.IsOpen)
            {
                userTrainStatusLabel.Text = "Open";
                userTrainStatusLabel.ForeColor = Color.Green;
            }
            else
            {
                userTrainStatusLabel.Text = "Closed";
                userTrainStatusLabel.ForeColor = Color.Red;
            }

            computerLabel.ForeColor = Color.Green;
            userLabel.ForeColor = Color.Red;
            DisableUserHandPBs();

            drawButton.Enabled = false;
            passButton.Enabled = false;

            whosTurn = COMPUTER;
        }

        public void ComputerPlayOnTrain(Domino d, Train train, List<PictureBox> trainPBs, int pbIndex)
        {
            PictureBox trainPB = trainPBs[pbIndex];
            // add the domino to train pb
            LoadDomino(trainPB, d);

            // make the next trainPB playable
            SetupTrainPB(trainPB, false);
            if (MustScroll(train))
                ScrollTrain(train, trainPBs);
        }

        public bool MakeComputerMove(bool canDraw)
        {
            Domino playedDomino;
            int pbIndex;
            try
            {
                pbIndex = NextPBIndex(computerTrain);
                playedDomino = computerHand.Play(computerTrain);
                ComputerPlayOnTrain(playedDomino, computerTrain, computerTrainPBs, pbIndex);
                return true;
            }
            catch
            {
                try
                {
                    pbIndex = NextPBIndex(mexicanTrain);
                    playedDomino = computerHand.Play(mexicanTrain);
                    ComputerPlayOnTrain(playedDomino, mexicanTrain, mexicanTrainPBs, pbIndex);
                    return true;
                }
                catch
                {
                    try
                    {
                        pbIndex = NextPBIndex(userTrain);
                        playedDomino = computerHand.Play(userTrain);
                        ComputerPlayOnTrain(playedDomino, userTrain, userTrainPBs, pbIndex);
                        return true;
                    }
                    catch
                    {
                        if (canDraw)
                        {
                            Domino d = pack.Draw();
                            computerHand.Add(d);
                            return false;
                        }
                        else
                        {
                            computerTrain.Open();
                            return false;
                            
                        }
                    }
                }
            }
        }

        public void CompleteComputerMove()
        {
            if (!MakeComputerMove(true))
            {
                if (!MakeComputerMove(false))
                {
                    // open the computers train
                    computerTrain.Open();
                }

            }
            if (computerTrain.IsOpen)
            {
                computerTrainStatusLabel.Text = "Open";
                computerTrainStatusLabel.ForeColor = Color.Green;
                SetupTrainPB(computerTrainPBs[NextPBIndex(computerTrain)], true);
            }
            else
            {
                computerTrainStatusLabel.Text = "Closed";
                computerTrainStatusLabel.ForeColor = Color.Red;
                SetupTrainPB(computerTrainPBs[NextPBIndex(computerTrain)], false);
            }

            computerLabel.Text = "Crafty Computer's Train (" + computerHand.Count + ")";
            computerLabel.ForeColor = Color.Red;
            userLabel.ForeColor = Color.Green;
            EnableUserHandPBs();
            SetupTrainPB(userTrainPBs[NextPBIndex(userTrain)], true);
            SetupTrainPB(mexicanTrainPBs[NextPBIndex(mexicanTrain)], true);

            drawButton.Enabled = true;
            passButton.Enabled = true;

            whosTurn = USER;
        }

        public PlayMTD()
        {
            InitializeComponent();

            pack = new BoneYard(9);
            pack.Shuffle();
            userHand = new Hand(pack, 2);
            computerHand = new Hand(pack, 2);

            int userHighest = userHand.IndexOfHighDouble();
            int computerHighest = computerHand.IndexOfHighDouble();

            // ToDo: what if one or both hands don't have a double?
            if (userHand[userHighest].Side1 > computerHand[computerHighest].Side1)
            {
                highestDoubleDomino = userHand[userHighest];
                userHand.RemoveAt(userHighest);
                LoadDomino(enginePB, highestDoubleDomino);
                whosTurn = USER;
            }
            else
            {
                highestDoubleDomino = computerHand[computerHighest];
                computerHand.RemoveAt(computerHighest);
                LoadDomino(enginePB, highestDoubleDomino);
                whosTurn = COMPUTER;

            }

            userTrain = new PlayerTrain(userHand, highestDoubleDomino.Side1);
            computerTrain = new PlayerTrain(computerHand, highestDoubleDomino.Side1);
            mexicanTrain = new MexicanTrain(highestDoubleDomino.Side1);

            // setup up - put all picture boxes in appropriate list of pbs
            // user hand
            userHandPBs = new List<PictureBox>();
            for (int i = 0; i < userHand.Count; i++ )
            {
                PictureBox pb = CreateUserHandPB(i);
                userHandPBs.Add(pb);
            }
            LoadHand(userHandPBs, userHand);

            // user train
            userTrainPBs = new List<PictureBox>();
            userTrainPBs.Add(userTrainPB1);
            userTrainPBs.Add(userTrainPB2);
            userTrainPBs.Add(userTrainPB3);
            userTrainPBs.Add(userTrainPB4);
            userTrainPBs.Add(userTrainPB5);

            // computer train
            computerTrainPBs = new List<PictureBox>();
            computerTrainPBs.Add(compTrainPB1);
            computerTrainPBs.Add(compTrainPB2);
            computerTrainPBs.Add(compTrainPB3);
            computerTrainPBs.Add(compTrainPB4);
            computerTrainPBs.Add(compTrainPB5);

            // mexican train
            mexicanTrainPBs = new List<PictureBox>();
            mexicanTrainPBs.Add(mexTrainPB1);
            mexicanTrainPBs.Add(mexTrainPB2);
            mexicanTrainPBs.Add(mexTrainPB3);
            mexicanTrainPBs.Add(mexTrainPB4);
            mexicanTrainPBs.Add(mexTrainPB5);

            // both user and player trains are closed
            userTrainStatusLabel.Text = "Closed";
            userTrainStatusLabel.ForeColor = Color.Red;
            computerTrainStatusLabel.Text = "Closed";
            computerTrainStatusLabel.ForeColor = Color.Red;
            computerLabel.Text = "Crafty Computer's Train (" + computerHand.Count + ")";

            // make last picture box in user's train and mexican train playable
            if (whosTurn == USER)
            {
                SetupTrainPB(userTrainPBs[0], true);
                SetupTrainPB(computerTrainPBs[0], false);
                SetupTrainPB(mexicanTrainPBs[0], true);
                EnableUserHandPBs();
                drawButton.Enabled = true;
                passButton.Enabled = true;
                computerLabel.ForeColor = Color.Red;
                userLabel.ForeColor = Color.Green;
            }
            else
            {
                SetupTrainPB(userTrainPBs[0], false);
                SetupTrainPB(computerTrainPBs[0], false);
                SetupTrainPB(mexicanTrainPBs[0], false);
                drawButton.Enabled = false;
                passButton.Enabled = false;
                computerLabel.ForeColor = Color.Green;
                userLabel.ForeColor = Color.Red;
                CompleteComputerMove();
            }
        }

        private void handPB_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox handPB = (PictureBox)sender;
            indexOfDominoInPlay = userHandPBs.IndexOf(handPB);

            if (indexOfDominoInPlay != -1)
            {
                userDominoInPlay = userHand[indexOfDominoInPlay];
                handPB.DoDragDrop(userDominoInPlay, DragDropEffects.Move);
            }
        }

        private void trainPB_DragEnter(object sender, DragEventArgs e)
        {
            PictureBox trainPB = (PictureBox)sender;
            string whichTrain = trainPB.Name.Substring(0, 3);

            Domino d = (Domino)e.Data.GetData("MTDClasses.Domino");
            bool mustFlip = false;

            switch(whichTrain)
            {
                case "use":
                    if (userTrain.IsPlayable(d, out mustFlip))
                        e.Effect = DragDropEffects.Move;
                    else
                        e.Effect = DragDropEffects.None;
                    break;
                case "mex":
                    if (mexicanTrain.IsPlayable(d, out mustFlip))
                        e.Effect = DragDropEffects.Move;
                    else
                        e.Effect = DragDropEffects.None;
                    break;
                case "com":
                    if (computerTrain.IsPlayable(d, out mustFlip))
                        e.Effect = DragDropEffects.Move;
                    else
                        e.Effect = DragDropEffects.None;
                    break;
            }
        }

        private void trainPB_DragDrop(object sender, DragEventArgs e)
        {
            PictureBox trainPB = (PictureBox)sender;
            string whichTrain = trainPB.Name.Substring(0, 3);
            Domino d = (Domino)e.Data.GetData("MTDClasses.Domino");
            bool mustFlip = false;

            switch (whichTrain)
            {
                case "use":
                    // ToDo: write a method for this - each case is largely the same
                    if (userTrain.IsPlayable(d, out mustFlip))
                    {
                        UserPlayOnTrain(d, trainPB, userTrain, userTrainPBs);
                        CompleteComputerMove();
                    }
                    else
                    {
                        // make a sound or something that let's player know the domino is not playable here
                    }
                    break;
                case "mex":
                    if (mexicanTrain.IsPlayable(d, out mustFlip))
                    {
                        UserPlayOnTrain(d, trainPB, mexicanTrain, mexicanTrainPBs);
                        CompleteComputerMove();
                    }
                    else
                    {
                        // make a sound or something that let's player know the domino is not playable here
                    }
                    break;
                case "com":
                    if (computerTrain.IsPlayable(d, out mustFlip))
                    {
                        UserPlayOnTrain(d, trainPB, computerTrain, computerTrainPBs);
                        CompleteComputerMove();
                    }
                    else
                    {
                        // make a sound or something that let's player know the domino is not playable here
                    }
                    break;
            }

            indexOfDominoInPlay = -1;
            userDominoInPlay = null;
        }

        private void drawButton_Click(object sender, EventArgs e)
        {
            Domino d = pack.Draw();
            userHand.Add(d);
            PictureBox pb = CreateUserHandPB(nextDrawIndex ++);
            userHandPBs.Add(pb);
            LoadDomino(pb, d);
            EnableHandPB(pb);
        }

        private void passButton_Click(object sender, EventArgs e)
        {
            userTrain.Open();
            userTrainStatusLabel.Text = "Open";
            userTrainStatusLabel.ForeColor = Color.Green;
            CompleteComputerMove();
        }

        private void PlayMTD_Load(object sender, EventArgs e)
        {

        }
    }
}
