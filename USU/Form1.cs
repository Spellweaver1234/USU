using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace USU
{
    public partial class Form1 : Form
    {
        // условие: картинка-цель и картинка-указатель одного размера 100х100
        public Bitmap HandlerTexture = Resource1.Handler;
        public Bitmap TargetTexture = Resource1.Target;

        private Point targetPosition = new Point(100, 100);
        private Point direction = Point.Empty;

        // половинные размеры, коэффициент скорости, счёт
        private int halfWidth=50, halfHeight=50;
        private int score = 0;
        private int speedCoefficient = 2;

        public Form1()
        {
            // половиннные размеры для обработки границ
            //halfWidth = TargetTexture.Width / 2;
            //halfHeight = TargetTexture.Height / 2;

            InitializeComponent();

            // исключаем мерцание формы
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint, true);
            UpdateStyles();
        }

        // событие при перерисовке формы
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // новое положение Картинки-цели с учётом направления и скорости
            targetPosition.X += direction.X * speedCoefficient;
            targetPosition.Y += direction.Y * speedCoefficient;

            // обработка достижения краёв области Картинкой-целью (смена направления движения) по X
            if (targetPosition.X - halfWidth < 0 || targetPosition.X + halfWidth > Width)
            {
                direction.X *= -1;
                timer2.Interval = 500;
            }

            // обработка достижения краёв области Картинкой-целью (смена направления движения) по Y
            if (targetPosition.Y - halfHeight < 0 || targetPosition.Y + halfHeight > Height)
            {
                direction.Y *= -1;
                timer2.Interval = 500;
            }

            // вычисление дистанции до центра картинки-цели
            var localPosition = this.PointToClient(Cursor.Position);
            var between = new Point(localPosition.X - targetPosition.X, localPosition.Y - targetPosition.Y);
            float distance = (float)Math.Sqrt(Math.Pow(between.X, 2) + Math.Pow(between.Y, 2));

            // если близко к центру картинки-цели добавляем баллы
            if (distance < 20)
            {
                AddScore(1);
            }

            // создание границ для картинки-цели и картинки-указателя
            var handleRect = new Rectangle(localPosition.X - halfWidth, localPosition.Y - halfHeight, HandlerTexture.Width, HandlerTexture.Height);
            var targetRect = new Rectangle(targetPosition.X - halfWidth, targetPosition.Y - halfHeight, TargetTexture.Width, TargetTexture.Height);

            // отрисовка картинки-цели и картинки-указателя
            Graphics g = e.Graphics;
            g.DrawImage(HandlerTexture, handleRect);
            g.DrawImage(TargetTexture, targetRect);
        }

        // таймер на 25 мс перерисовывает форму
        private void timer1_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        // таймер с рандомным интервалом для рандомного изменения направления картинки-цели
        private void timer2_Tick(object sender, EventArgs e)
        {
            Random r = new Random();
            timer2.Interval = r.Next(250, 1000);
            direction.X = r.Next(-1, 2);
            direction.Y = r.Next(-1, 2);
        }

        // метод добавления очков и вывода их на экран
        private void AddScore(int sc)
        {
            score += sc;
            scoreLabel.Text = score.ToString();
        }
    }
}