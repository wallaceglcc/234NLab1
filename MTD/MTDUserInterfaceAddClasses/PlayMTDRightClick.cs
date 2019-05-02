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
    public partial class PlayMTDRightClick : Form
    {
        /// <summary>
        /// Boneyard for the game being played
        /// </summary>
        private BoneYard pack;

        /// <summary>
        /// Hands and train for the player
        /// </summary>
        private Hand userHand;
        private List<PictureBox> userHandPBs;
        private PlayerTrain userTrain;
        private List<PictureBox> userTrainPBs;

        /// <summary>
        /// Hand and train for the computer
        /// </summary>
        private Hand computerHand;
        private PlayerTrain computerTrain;
        private List<PictureBox> computerTrainPBs;

        /// <summary>
        /// MexicanTrain
        /// </summary>
        private MexicanTrain mexicanTrain;
        private List<PictureBox> mexicanTrainPBs;

        /// <summary>
        /// This seems to be a temporary Domino to store a domino selected by the user to use in methods
        /// </summary>
        private Domino userDominoInPlay;
        private int indexOfDominoInPlay = -1;

        /// <summary>
        /// I'm guessing this is for drawing from the boneyard?
        /// </summary>
        private int nextDrawIndex = 0;

        /// <summary>
        /// For keeping track of turns
        /// </summary>
        private int whosTurn = -1;
        private const int COMPUTER = 0;
        private const int USER = 1;

        #region Methods

        // loads the image of a domino into a picture box
        // verify that the path for the domino files is correct
        private void LoadDomino(PictureBox pb, Domino d)
        {
            pb.Image = Image.FromFile(System.Environment.CurrentDirectory
                        + "\\..\\..\\Dominos\\" + d.Filename);
        }

        // loads all of the dominos in a hand into a list of pictureboxes
        private void LoadHand(List<PictureBox> pbs, Hand h)
        {
            for (int i = 0; i < pbs.Count; i++)
            {
                PictureBox pb = pbs[i];
                Domino d = h[i];
                LoadDomino(pb, d);
            }
        }

        // dynamically creates the "next" picture box for the user's hand
        // the instance variable nextDrawIndex should be passed as a parameter
        // if you change the layout of the form, you'll have to change the math here
        private PictureBox CreateUserHandPB(int index)
        {
            PictureBox pb = new PictureBox();
            pb.Visible = true;
            pb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pb.Location = new System.Drawing.Point(24 + (index % 5) * 110, 366 + (index / 5) * 60);
            pb.Size = new System.Drawing.Size(100, 50);
            pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Controls.Add(pb);
            return pb;
        }

        // adds the mouse down event handler to a picture box
        private void EnableHandPB(PictureBox pb)
        {
            pb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.handPB_MouseDown);
        }

        // adds the mouse down event handler to all of the picture boxes in the users hand pb list
        private void EnableUserHandPBs()
        {
            for (int i = 0; i < userHandPBs.Count; i++)
            {
                PictureBox pb = userHandPBs[i];
                EnableHandPB(pb);
            }
        }

        // removes the mouse down event handler from a picture box
        private void DisableHandPB(PictureBox pb)
        {
            pb.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.handPB_MouseDown);
        }

        // removes all of the mouse down event handlers from the picture boxes in the users hand pb list
        private void DisableUserHandPBs()
        {
            for (int i = 0; i < userHandPBs.Count; i++)
            {
                PictureBox pb = userHandPBs[i];
                DisableHandPB(pb);
            }
        }

        // unloads the domino image from a picture box in a train
        public void RemoveDominoFromTrainPB(int index, List<PictureBox> trainPBs)
        {
            PictureBox trainPB = trainPBs[index];
            trainPB.Image = null;
        }
        
        // unloads the domino image and removes a picture box from a hand
        public void RemoveDominoFromHandPB(int index)
        {
            PictureBox handPB = userHandPBs[index];
            handPB.Image = null;
            handPB.Visible = false;
            userHandPBs.RemoveAt(index);
            this.Controls.Remove(handPB);
            handPB = null;
        }

        // determines the index of the picture box into which a domino should be played
        // in a list of pictureboxes.  Calls ScrollTrain to Scroll the dominos 
        // one picture box to the left if necessary
        public int NextTrainPBIndex(Train train, List<PictureBox> trainPBs)
        {
            if (train.Count <= trainPBs.Count)
                return train.Count - 1;
            else
            {
                ScrollTrain(train, trainPBs);
                return trainPBs.Count - 1;
            }

        }

        // scrolls the dominos one picture box to the left if all of the pbx
        // for a train are filled.  Assumes 5 picture boxes per train
        public void ScrollTrain(Train train, List<PictureBox> trainPBs)
        {
            for (int i = 0; i < 4; i++)
                LoadDomino(trainPBs[i], train[(train.Count - 5) + i]);
            trainPBs[4].Image = null;
        }

        // plays a domino on a train.  Loads the appropriate train pb, 
        // removes the domino pb from the hand, updates the train status label ,
        // disables the hand pbs and disables appropriate buttons
        public void UserPlayOnTrain(Domino d, Train train, List<PictureBox> trainPBs)
        {

            userHand.Play(d, train);

            // add the domino to train pb
            int nextPBIndex = NextTrainPBIndex(train, trainPBs);
            PictureBox trainPB = trainPBs[nextPBIndex];
            LoadDomino(trainPB, d);

            // remove the domino from the pbs
            RemoveDominoFromHandPB(indexOfDominoInPlay);

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

        // adds a domino picture to a train
        public void ComputerPlayOnTrain(Domino d, Train train, List<PictureBox> trainPBs, int pbIndex)
        {
            PictureBox trainPB = trainPBs[pbIndex];
            LoadDomino(trainPB, d);
        }

        // ai for computer move.
        // calls play for on the computer's hand for each train, gets the next pb index and 
        // plays the domino on the train.  
        // BECAUSE play throws an exception, tries to first play on one train and
        // in the catch block tries the next and so on
        // when the computer can not play on any train, the computer draws and returns false.
        // if the method is called with canDraw = false, this last step is omitted and the method
        // returns false
        public bool MakeComputerMove(bool canDraw)
        {
            Domino playedDomino;
            int pbIndex;
            try
            {
                playedDomino = computerHand.Play(computerTrain);
                pbIndex = NextTrainPBIndex(computerTrain, computerTrainPBs);
                ComputerPlayOnTrain(playedDomino, computerTrain, computerTrainPBs, pbIndex);
                return true;
            }
            catch
            {
                try
                {
                    playedDomino = computerHand.Play(mexicanTrain);
                    pbIndex = NextTrainPBIndex(mexicanTrain, mexicanTrainPBs);
                    ComputerPlayOnTrain(playedDomino, mexicanTrain, mexicanTrainPBs, pbIndex);
                    return true;
                }
                catch
                {
                    try
                    {
                        playedDomino = computerHand.Play(userTrain);
                        pbIndex = NextTrainPBIndex(userTrain, userTrainPBs);
                        ComputerPlayOnTrain(playedDomino, userTrain, userTrainPBs, pbIndex);
                        return true;
                    }
                    catch
                    {
                        if (canDraw)
                        {
                            if (!pack.IsEmpty())
                            {
                                Domino d = pack.Draw();
                                computerHand.Add(d);
                                return false;
                            }
                            else
                            {
                                whosTurn = -1;
                                return false;
                            }
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

        // update labels on the UI and disable the users hand
        // call MakeComputerMove (maybe twice)
        // update the labels on the UI
        // determine if the computer won or if it's now the user's turn
        public void CompleteComputerMove()
        {
            drawButton.Enabled = false;
            passButton.Enabled = false;
            computerLabel.ForeColor = Color.Green;
            userLabel.ForeColor = Color.Red;
            DisableUserHandPBs();
           
            if (!MakeComputerMove(true) && whosTurn != -1)
                MakeComputerMove(false);

            if (computerTrain.IsOpen)
                computerTrainStatusLabel.Text = "Open";
            else
                computerTrainStatusLabel.Text = "Closed";
            computerTrainStatusLabel.ForeColor = Color.Red;
            computerLabel.Text = "Crafty Computer's Train (" + computerHand.Count + ")";

            if (whosTurn != -1 && computerHand.Count > 0)
            {
                whosTurn = USER;
            }
            else
            {
                whosTurn = -1;
                MessageBox.Show("Game Over");
            }
        }

        // enable the hand pbs, buttons and update labels on the UI
        public void EnableUserMove()
        {
            if (!pack.IsEmpty())
            {
                EnableUserHandPBs();
                drawButton.Enabled = true;
                passButton.Enabled = true;
                computerLabel.ForeColor = Color.Red;
                userLabel.ForeColor = Color.Green;
            }
        }

        // instantiate boneyard and hands
        // find the highest double in each hand
        // determine who should go first, remove the highest double from the appropriate hand
        // and display the highest double in the UI
        // instantiate trains now that you know the engine value
        // create all of the picture boxes for the user's hand and load the dominos for the hand
        // Add the picture boxes for each train to the appropriate list of picture boxes
        // update the labels on the UI
        // if it's the computer's turn, let her play
        // if it's the user's turn, enable the pbs so she can play
        public void SetUp()
        {
            pack = new BoneYard(9);
            pack.Shuffle();
            userHand = new Hand(pack, 2);
            computerHand = new Hand(pack, 2);
            nextDrawIndex = userHand.Count;

            int userHighest = userHand.IndexOfHighDouble();
            int computerHighest = computerHand.IndexOfHighDouble();
            Domino highestDoubleDomino;

            // ToDo: what if neither has a double
            if ((userHighest != -1 && computerHighest == -1) ||
                (userHand[userHighest].Side1 > computerHand[computerHighest].Side1))
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
            for (int i = 0; i < userHand.Count; i++)
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

            // let the computer make a move if it's her turn
            if (whosTurn == COMPUTER)
            {
                CompleteComputerMove();
                // now it's the user's turn - always
            }

            if (whosTurn == USER)
            {
                EnableUserMove();
            }
        }

        // remove all of the domino pictures for each train
        // remove all of the domino pictures for the user's hand
        // reset the nextDrawIndex to 15
        public void TearDown()
        {
            // train pbs
            for (int i = 0; i < 5; i++)
                RemoveDominoFromTrainPB(i, userTrainPBs);
            for (int i = 0; i < 5; i++)
                RemoveDominoFromTrainPB(i, computerTrainPBs);
            for (int i = 0; i < 5; i++)
                RemoveDominoFromTrainPB(i, mexicanTrainPBs);
            // user hand pbs
            int handCount = userHandPBs.Count;
            for (int i = 0; i < handCount; i++)
                RemoveDominoFromHandPB(0);

            nextDrawIndex = 15;
        }
        #endregion

        public PlayMTDRightClick()
        {
            InitializeComponent();
            SetUp();
        }

        // when the user right clicks on a domino, a context sensitive menu appears that
        // let's the user know which train is playable.  Green means playable.  Red means not playable.
        // the event handler for the menu item is enabled and disabled appropriately.
        private void whichTrainMenu_Opening(object sender, CancelEventArgs e)
        {
            bool mustFlip = false;
            if (userDominoInPlay != null)
            {
                mexicanTrainItem.Click -= new System.EventHandler(this.mexicanTrainItem_Click);
                computerTrainItem.Click -= new System.EventHandler(this.computerTrainItem_Click);
                myTrainItem.Click -= new System.EventHandler(this.myTrainItem_Click);

                if (mexicanTrain.IsPlayable(userHand, userDominoInPlay, out mustFlip))
                {
                    mexicanTrainItem.ForeColor = Color.Green;
                    mexicanTrainItem.Click += new System.EventHandler(this.mexicanTrainItem_Click);
                }
                else
                {
                    mexicanTrainItem.ForeColor = Color.Red;
                } 
                if (computerTrain.IsPlayable(userHand, userDominoInPlay, out mustFlip))
                {
                    computerTrainItem.ForeColor = Color.Green;
                    computerTrainItem.Click += new System.EventHandler(this.computerTrainItem_Click);
                }
                else
                {
                    computerTrainItem.ForeColor = Color.Red;
                }
                if (userTrain.IsPlayable(userHand, userDominoInPlay, out mustFlip))
                {
                    myTrainItem.ForeColor = Color.Green;
                    myTrainItem.Click += new System.EventHandler(this.myTrainItem_Click);
                }
                else
                {
                    myTrainItem.ForeColor = Color.Red;
                }
            }
        }

        // displays the context sensitve menu with the list of trains
        // sets the instance variables indexOfDominoInPlay and userDominoInPlay
        private void handPB_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox handPB = (PictureBox)sender;
            indexOfDominoInPlay = userHandPBs.IndexOf(handPB);
            if (indexOfDominoInPlay != -1)
            {
                userDominoInPlay = userHand[indexOfDominoInPlay];
                if (e.Button == MouseButtons.Right)
                {
                    whichTrainMenu.Show(handPB, 
                        handPB.Size.Width - 20, handPB.Size.Height - 20);
                }
            }
        }

        // play on the mexican train, lets the computer take a move and then enables
        // hand pbs so the user can make the next move.
        private void mexicanTrainItem_Click(object sender, EventArgs e)
        {
            UserPlayOnTrain(userDominoInPlay, mexicanTrain, mexicanTrainPBs);
            userDominoInPlay = null;
            CompleteComputerMove();
            if (whosTurn != -1)
                EnableUserMove();
        }

        // play on the computer train, lets the computer take a move and then enables
        // hand pbs so the user can make the next move.
        private void computerTrainItem_Click(object sender, EventArgs e)
        {
            UserPlayOnTrain(userDominoInPlay, computerTrain, computerTrainPBs);
            userDominoInPlay = null;
            CompleteComputerMove();
            if (whosTurn != -1)
                EnableUserMove();
        }

        // play on the user train, lets the computer take a move and then enables
        // hand pbs so the user can make the next move.
        private void myTrainItem_Click(object sender, EventArgs e)
        {
            UserPlayOnTrain(userDominoInPlay, userTrain, userTrainPBs);
            userDominoInPlay = null;
            CompleteComputerMove();
            if (whosTurn != -1)
                EnableUserMove();
        }

        // tear down and then set up
        private void newHandButton_Click(object sender, EventArgs e)
        {
            TearDown();
            SetUp();
        }

        // draw a domino, add it to the hand, create a new pb and enable the new pb
        private void drawButton_Click(object sender, EventArgs e)
        {
            Domino d = pack.Draw();
            userHand.Add(d);
            PictureBox pb = CreateUserHandPB(nextDrawIndex++);
            userHandPBs.Add(pb);
            LoadDomino(pb, d);
            EnableHandPB(pb);
        }

        // open the user's train, update the ui and let the computer make a move
        // enable the hand pbs so the user can make a move
        private void passButton_Click(object sender, EventArgs e)
        {
            userTrain.Open();
            userTrainStatusLabel.Text = "Open";
            userTrainStatusLabel.ForeColor = Color.Green;
            CompleteComputerMove();
            if (whosTurn != -1)
                EnableUserMove();
        }
        
        private void PlayMTDRightClick_Load(object sender, EventArgs e)
        {
            // register the event and it's delegate here
            pack.Empty
                += new BoneYard.EmptyHandler(RespondToEmpty);
            userHand.Empty += new Hand.EmptyHandler(RespondToEmptyHand);
        }

        private void RespondToEmpty(BoneYard by)
        {
            MessageBox.Show("The Boneyard must be empty");
        }

        private void RespondToEmptyHand(Hand h)
        {
            if (h == userHand)
                MessageBox.Show("User wins");
            else if (h == computerHand)
                MessageBox.Show("Computer wins");
            else
                MessageBox.Show("Something is tragicly wrong");
        }

    }
}
