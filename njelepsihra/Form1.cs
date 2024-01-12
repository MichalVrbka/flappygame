using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace njelepsihra
{
    public partial class Form1 : Form
    {
        private const int Gravity = 2;
        private const int JumpSpeed = -15;
        private const int PipeSpeed = 10;

        private bool isJumping;
        private int birdSpeed;
        private double score;

        private List<PictureBox> pipes;
        private PictureBox bird;
        private Random random = new Random();

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            isJumping = false;
            birdSpeed = 0;
            score = 0;

            pipes = new List<PictureBox>();
            timer1.Interval = 20;

            SpawnBird();
            SpawnPipes();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            this.WindowState = FormWindowState.Maximized;
        }

        private void SpawnBird()
        {
            bird = new PictureBox
            {
                Size = new Size(150, 150),
                Location = new Point(50, 200),
                Image = Image.FromFile("G:\\nejlepsihra\\njelepsihra\\njelepsihra\\bird.png"),
                SizeMode = PictureBoxSizeMode.Zoom,
                Name = "bird"
            };
            Controls.Add(bird);
        }

        private void SpawnPipes()
        {
            for (int i = 0; i < 5; i++)
            {
                int height = random.Next(-300, 0);
                PictureBox pipeUpper = new PictureBox
                {
                    Size = new Size(150, 675),
                    Location = new Point(1000 + i * 500, height),
                    Image = Image.FromFile("G:\\nejlepsihra\\njelepsihra\\njelepsihra\\higherpipes.png"),
                    SizeMode = PictureBoxSizeMode.StretchImage
                };

                PictureBox pipeLower = new PictureBox
                {
                    Size = new Size(150, 675),
                    Location = new Point(1000 + i * 500, height + 1100),
                    Image = Image.FromFile("G:\\nejlepsihra\\njelepsihra\\njelepsihra\\lowerpipes.png"),
                    SizeMode = PictureBoxSizeMode.StretchImage
                };

                pipes.Add(pipeUpper);
                pipes.Add(pipeLower);

                Controls.Add(pipeUpper);
                Controls.Add(pipeLower);
            }
        }

        private void MoveBird()
        {
            if (isJumping)
            {
                birdSpeed = JumpSpeed;
                isJumping = false;
            }

            birdSpeed += Gravity;
            bird.Top += birdSpeed;

            if (bird.Top < 0)
                bird.Top = 0;

            if (bird.Bottom > ClientSize.Height) EndGame();
        }

        private void MovePipes()
        {
            foreach (var pipe in pipes)
            {
                pipe.Left -= PipeSpeed;

                if (pipe.Right < 0)
                {
                    pipe.Left = ClientSize.Width;
                    pipe.Tag = null;
                }
            }
        }

        private void CheckCollisions()
        {
            foreach (var pipe in pipes)
            {
                if (bird.Bounds.IntersectsWith(pipe.Bounds)) EndGame();
            }
        }

        private void UpdateScore()
        {
            foreach (var pipe in pipes)
            {
                if (pipe.Right < bird.Left && pipe.Tag == null)
                {
                    score += 0.5;
                    pipe.Tag = "scored";
                }
            }

            Text = $"Flappy Bird - Score: {score}";
        }

        private void EndGame()
        {
            timer1.Stop();            
            MessageBox.Show($"Game Over! Your Score: {score}", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
            timer1.Start();
            InitializeGame();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            MoveBird();
            MovePipes();
            CheckCollisions();
            UpdateScore();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space) isJumping = true;
        }
    }
}
