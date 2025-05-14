using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pong_Test
{
    public partial class Form1 : Form
    {
        //Create Timer
        Timer gameTimer = new Timer();
        //create Paddles
        Rectangle player1;
        Rectangle player2;
        Rectangle ball; //create ball
        //initial velocity
        int ballSpeedX = 5;
        int ballSpeedY = 5;

        //Track movement
        bool moveUp = false;
        bool moveDown = false;

        //scoring
        int player1Score = 0;
        int player2Score = 0;
        int winningScore = 5;
        //game status
        bool gameOver = false;

        //ai reaction timer
        int aiReactionTimeCounter = 0;
        int aiReactionTimeLength = 10;
        public Form1()
        {
            InitializeComponent();

            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;

            //Initialize Timer
            gameTimer.Interval = 16;
            gameTimer.Tick += GameLoop;
            gameTimer.Start();
            player1 = new Rectangle(20, 100, 10, 60);
            player2 = new Rectangle(450, 100, 10, 60);
            ball = new Rectangle(100, 100, 10, 10);

            //lock the window
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

        }
        private void GameLoop(object sender, EventArgs e)
        {
            //called every tick

            if (moveUp == true)
            {
                if (player1.Y > 0)
                { player1.Y -= 5; }
            }
            if (moveDown == true)
            {
                if (player1.Y + player1.Height < this.ClientSize.Height)
                { player1.Y += 5; }
            }
            ball.X += ballSpeedX;
            ball.Y += ballSpeedY;

            //bounce off bottom
            if(ball.Y + ball.Height >= this.ClientSize.Height)
            {
                ballSpeedY *= -1;
            }

            //bounce off top
            if(ball.Y <= 0)
            {  
                ballSpeedY *= -1;
            }

            //bounce off paddle
            if(ball.IntersectsWith(player1))
            {
                ball.X = player1.Right;
                ballSpeedX *= -1;
            }
            if(ball.IntersectsWith(player2))
            {
                ball.X = player2.Left - ball.Width;
                ballSpeedX *= -1;
            }

            if(ball.X <= 0)
            {
                player2Score++; //player2 increase score
                BallReset(); //reset ball
            }
            if(ball.X + ball.Width >= this.ClientSize.Width)
            {
                player1Score++; //player1 incrase score
                BallReset(); //reset ball
            }

            if (!gameOver)
            {
                //Player 1 Win
                if (player1Score >= winningScore)
                {
                    gameOver = true;
                    gameTimer.Stop();
                    MessageBox.Show("You Win!");
                    ResetGame();
                }
                //Player 2 Win
                if (player2Score >= winningScore)
                {
                    gameOver = true;
                    gameTimer.Stop();
                    MessageBox.Show("You lose!");
                    ResetGame();
                }
            }

            //THIS IS THE AI!!!
            if(ballSpeedX > 0)
            {
                if(aiReactionTimeCounter < aiReactionTimeLength)
                {
                    aiReactionTimeCounter++;
                }
                else
                {
                    if (ball.Y + ball.Height / 2 > player2.Y + player2.Height / 2)
                    {
                        player2.Y += 4;
                    }
                    if (ball.Y + ball.Height / 2 < player2.Y + player2.Height / 2)
                    {
                        player2.Y -= 4;
                    }
                }
            }
            else
            {
                aiReactionTimeCounter = 0;
            }


                Invalidate(); //Triggers painting

        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.FillRectangle(Brushes.White, player1); // render paddle
            e.Graphics.FillRectangle(Brushes.White, player2); // render paddle
            e.Graphics.FillRectangle(Brushes.White, ball); //render ball
            //render scoreboard
            e.Graphics.DrawString($"Player 1: {player1Score}", new Font("Arial", 16), Brushes.White, new PointF(50, 20));
            e.Graphics.DrawString($"Player 2: {player2Score}", new Font("Arial", 16), Brushes.White, new PointF(200, 20));
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                moveUp = true;
            }
            if(e.KeyCode == Keys.S)
            {
                moveDown = true;
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            { moveUp = false;}
            if (e.KeyCode == Keys.S)
            { moveDown = false;}
        }
        private void BallReset()
        {
            ball.X = this.ClientSize.Width / 2 - ball.Width / 2;
            ball.Y = this.ClientSize.Height / 2 - ball.Height / 2;
            // Centers Ball

            //Random direction
            Random rand = new Random();
            ballSpeedX = rand.Next(2) == 0 ? 3 : -3;
        }
        
        private void ResetGame()
        {
            player1Score = 0;
            player2Score = 0;
            gameOver = false;
            BallReset();
            gameTimer.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
