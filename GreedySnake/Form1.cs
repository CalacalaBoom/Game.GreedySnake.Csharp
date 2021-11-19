using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace GreedySnake
{
    public partial class Form1 : Form
    {
        Snake snake = new Snake();
        public static bool ifStart = false;
        public static int snake_W = 1;
        public static int snake_H = 1;
        public static int career = 50;
        public static bool pause = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            ProtractTable(g);
            if (!ifStart)
            {
                Snake.timer = timer1;
                Snake.label = label2;
                snake.Ophidian(panel1, snake_W);
            }
            else
            {
                for (int i = 0; i < Snake.List.Count; i++)
                {
                    e.Graphics.FillRectangle(Snake.SolidB, ((Point)Snake.List[i]).X + 1, 
                        ((Point)Snake.List[i]).Y + 1,snake_W - 1, snake_H - 1);
                }
                //绘制食物
                e.Graphics.FillRectangle(Snake.SolidF, Snake.Food.X + 1, Snake.Food.Y + 1,
                    snake_W - 1, snake_H - 1);
                if (Snake.ifGame)
                    //绘制提示文本
                    e.Graphics.DrawString("Game Over", new Font("宋体", 30, FontStyle.Bold),
                        new SolidBrush(Color.DarkSlateGray), new PointF(150, 130));
            }
        }
        //自定义方法ProtractTable用于绘制游戏场景
        public void ProtractTable(Graphics g)
        {
            for (int i = 0; i <= panel1.Width / snake_W; i++)
            {
                g.DrawLine(new Pen(Color.Black, 1), new Point(i * snake_W, 0), 
                    new Point(i * snake_W, panel1.Height));
            }
            for (int i = 0; i <= panel1.Height / snake_H; i++)
            {
                g.DrawLine(new Pen(Color.Black, 1), new Point(0, i * snake_H),
                    new Point(panel1.Width, i * snake_H));
            }
        }

        private void 开始ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NoviceCortrol(Convert.ToInt32(((ToolStripMenuItem)sender).Tag.ToString()));
            snake.BuildFood();   
            textBox1.Focus();
        }

        public void NoviceCortrol(int n)
        {
            switch (n)
            {
                case 1:  
                    {
                        ifStart = false;
                        Graphics g = panel1.CreateGraphics();
                        //刷新游戏场地
                        g.FillRectangle(Snake.SolidD, 0, 0, panel1.Width, panel1.Height);
                        ProtractTable(g);    
                        ifStart = true;  
                        snake.Ophidian(panel1, snake_W);
                        timer1.Interval = career;  
                        timer1.Start();   
                        pause = true; 
                        label2.Text = "0";
                        break;
                    }
                case 2:   
                    {
                        if (pause)
                        {
                            ifStart = true;   
                            timer1.Stop(); 
                            pause = false;  
                        }
                        else
                        {
                            ifStart = true;  
                            timer1.Start(); 
                            pause = true;  
                        }
                        break;
                    }
                case 3:    
                    {
                        timer1.Stop(); 
                        Application.Exit();  
                        break;
                    }
            }

        }

        private void 初级ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled == false)
            {
                初级ToolStripMenuItem.Checked = false;
                中级ToolStripMenuItem.Checked = false;
                高级ToolStripMenuItem.Checked = false;  
                ((ToolStripMenuItem)sender).Checked = true;
                switch (Convert.ToInt32(((ToolStripMenuItem)sender).Tag.ToString()))
                {
                    case 1:                                          
                        {
                            career = 400;                      
                            break;
                        }
                    case 2: 
                        {
                            career = 200;
                            break;
                        }
                    case 3:       
                        {
                            career = 50;
                            break;
                        }
                }

            }
            textBox1.Focus();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            int tem_n = -1;
            if (e.KeyCode == Keys.Right) 
                tem_n = 0;      
            if (e.KeyCode == Keys.Left) 
                tem_n = 1;        
            if (e.KeyCode == Keys.Up) 
                tem_n = 2;         
            if (e.KeyCode == Keys.Down)  
                tem_n = 3;        
            if (tem_n != -1 && tem_n != Snake.Aspect) 
            {
                if (Snake.ifGame == false)
                {
                    //如果移动的方向不是相反的方向
                    if (!((tem_n == 0 && Snake.Aspect == 1 || tem_n == 1 && 
                        Snake.Aspect == 0) || (tem_n == 2 && Snake.Aspect == 3 
                        || tem_n == 3 && Snake.Aspect == 2)))
                    {
                        Snake.Aspect = tem_n;  
                        snake.SnakeMove(tem_n);   
                    }
                }
            }
            int tem_p = -1;      
            if (e.KeyCode == Keys.F2)  
                tem_p = 1;         
            if (e.KeyCode == Keys.F3)  
                tem_p = 2;        
            if (e.KeyCode == Keys.F4) 
                tem_p = 3;       
            if (tem_p != -1)      
                NoviceCortrol(tem_p); 
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            snake.SnakeMove(Snake.Aspect);
        }
    }
}
