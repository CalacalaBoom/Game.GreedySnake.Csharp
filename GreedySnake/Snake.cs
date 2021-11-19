using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;

namespace GreedySnake
{
    class Snake
    {
        //定义全局变量
        public static int Condyle = 0;  //设置骨节大小
        public static int Aspect = 0;   //设置方向
        public static Point[] Place = {new Point(-1,-1),new Point(-1,-1), new Point(-1, -1),
            new Point(-1, -1), new Point(-1, -1), new Point(-1, -1),};  //设置各个骨节的位置
        public static Point Food = new Point(-1,-1); //设置食物所在点
        public static bool ifFood = false;  //是否有食物
        public static bool ifGame = false;  //游戏是否结束
        public static int Field_width = 0;  //场地的宽度
        public static int Field_height = 0;  //场地的高度
        public static Control control;  //记录绘制贪吃蛇的控件
        public static Timer time;  //记录Timer组件
        public static SolidBrush SolidB = new SolidBrush(Color.Red);  //设置贪吃蛇身体的颜色
        public static SolidBrush SolidD = new SolidBrush(Color.LightCoral);  //设置背景颜色
        public static SolidBrush SolidF = new SolidBrush(Color.Blue);  //设置食物颜色
        public static Label label;  //记录Label控件
        public static ArrayList List = new ArrayList();  //实例化ArrayList
        Graphics g;  //实例化Graphics类
        public static Timer timer;

        //自定义方法Ophidian用于绘制贪吃蛇的起始位置，以及对游戏进行初始化设置 
        public void Ophidian(Control Con,int condyle)
        {
            Field_width = Con.Width;  //获取场地宽度
            Field_height = Con.Height;  //获取场地高度
            Condyle = condyle;  //记录骨节大小
            control = Con;  //记录背景控件
            g = control.CreateGraphics();  //创建背景控件的Graphics类
            SolidD = new SolidBrush(Con.BackColor);  //设置画刷颜色
            for (int i = 0; i < Place.Length; i++)  //绘制贪吃蛇
            {
                Place[i].X = (Place.Length - i - 1) * Condyle;  //设置骨节横坐标位置
                Place[i].Y = (Field_height / 2) - Condyle;  //设置骨节纵坐标位置
                //绘制骨节
                g.FillRectangle(SolidB, Place[i].X + 1, Place[i].Y + 1, Condyle - 1, Condyle - 1);
            }
            List = new ArrayList(Place);
            ifGame = false;
            Aspect = 0;
        }
        //自定义方法SnakeMove()方法通过参数n移动贪吃蛇的位置，并根据蛇的位置，判断是否吃到食物，如果吃到，就重新生成食物
        public void SnakeMove(int n)
        {
            Point tem_Point = new Point(-1,-1);  //定义坐标位置
            switch (n)
            {
                case 0:
                    {
                        tem_Point.X = ((Point)List[0]).X + Condyle;  //右
                        tem_Point.Y = ((Point)List[0]).Y;
                        break;
                    }
                case 1:
                    {
                        tem_Point.X = ((Point)List[0]).X - Condyle;  //左
                        tem_Point.Y = ((Point)List[0]).Y;
                        break;
                    }
                case 2:
                    {
                        tem_Point.X = ((Point)List[0]).Y - Condyle;  //上
                        tem_Point.Y = ((Point)List[0]).X;
                        break;
                    }
                case 3:
                    {
                        tem_Point.X = ((Point)List[0]).Y + Condyle;  //下
                        tem_Point.Y = ((Point)List[0]).X;
                        break;
                    }
            }
            BuildFood();  //生成食物
            if (!EstimateMove(tem_Point))
            {
                Aspect = n;
                if (!GameAborted(tem_Point))
                {
                    ProtractSnake(tem_Point);  //重新绘制蛇身
                    EatFood();  //吃食
                }
                g.FillRectangle(SolidF, Food.X + 1, Food.Y + 1, Condyle - 1, Condyle - 1);  //绘制食物
            }
        }
        //自定义方法BuildFood()在没有食物的情况下生成新的食物
        public void BuildFood()
        {
            if (ifFood == false)  //判断，如果没有食物
            {
                Point tem_p = new Point(-1,-1);
                bool tem_bool = true;  //布尔变量用于记录是否计算出食物的位置
                bool tem_b = false;  //布尔变量用于记录食物是否与蛇身重叠
                while (tem_bool)
                {
                    tem_b = false;
                    tem_p = RectFood();
                    for (int i = 0; i < List.Count; i++)  //遍历整个蛇身
                    {
                        if (((Point)List[i]) == tem_p)  //判断蛇身是否与食物重叠
                        {
                            tem_b = true;  //记录重叠
                            break;
                        }
                    }
                    if (tem_b == false)  //如果没有重叠
                        tem_bool = false;
                }
                Food = tem_p;  //记录食物的显示位置
            }
            ifFood = true;  //有食物
        }
        //自定义方法RectFood()方法随机生成食物的横坐标和纵坐标
        public Point RectFood()
        {
            int tem_W = Field_width / 20;  //获取场地行数
            int tem_H = Field_height / 20;  //获取场地列数
            Random RandW = new Random();
            tem_W = RandW.Next(0, tem_W - 1);  //生成食物的横坐标
            Random RandH = new Random();
            tem_H = RandH.Next(0, tem_H - 1);  //生成食物的纵坐标
            Point tem_P = new Point(tem_W * Condyle, tem_H * Condyle);  //生成食物的显示位置
            return tem_P;
        }
        //自定义方法EstimateMove用于判断贪吃蛇的移动方向是否向反方向移动
        public bool EstimateMove(Point Ep)
        {
            bool tem_bool = false;  //记录贪吃蛇的移动方向是否向反方向移动
            if (Ep.X == ((Point)List[0]).X && Ep.Y == ((Point)List[0]).Y)  
            {
                tem_bool = true;
            }
            return tem_bool;
        }
        //自定义方法GameAborted用于判断当前游戏是否失败
        public bool GameAborted(Point GameP)
        {
            bool tem_b = false;  //游戏是否结束
            bool tem_body = false;  //蛇身是否重叠
            for (int i = 1; i < List.Count; i++)  //遍历所有骨节
            {
                if (((Point)List[0]) == ((Point)List[i]))  //骨节重叠
                {
                    tem_body = true;
                }
            }
            //判断蛇头是否超出场地
            if (GameP.X <= -20 || GameP.X >= control.Width - 1 || GameP.Y >= control.Height - 1 || tem_body)
            {
                //绘制游戏结束提示文本
                g.DrawString("Game Over", new Font("宋体", 30, FontStyle.Bold), 
                    new SolidBrush(Color.DarkSlateGray), new PointF(150, 130));
                ifGame = true;  //游戏结束
                timer.Stop();
                tem_b = true;
            }
            return tem_b;
        }
        //自定义方法ProtractSnake是根据蛇头的下一个移动点，重新绘制移动后的蛇身
        public void ProtractSnake(Point Ep)
        {
            bool tem_bool = false;  //是否清除移动后的蛇身
            List.Insert(0, Ep);   //根据蛇头的移动方向，设置蛇头的位置
            Point tem_point = ((Point)List[List.Count - 1]);  //记录蛇尾的位置
            List.RemoveAt(List.Count - 1);  //移除蛇的尾部
            //使骨节向前移动一位
            for (int i = 0; i < List.Count - 1; i++)
            {
                if (tem_point == ((Point)List[i]))
                    tem_bool = true;
            }
            if (!tem_bool)   //清除贪吃蛇移动前的蛇尾部份
            {
                g.FillRectangle(SolidD, tem_point.X + 1, tem_point.Y + 1, Condyle - 1, Condyle - 1);
            }
            for (int i = 0; i < List.Count; i++)  //重新绘制蛇身
            {
                g.FillRectangle(SolidB, ((Point)List[0]).X + 1,
                    ((Point)List[0]).Y + 1, Condyle - 1, Condyle - 1);
            }
        }
        //自定义方法EatFood的主要功能是判断蛇头位置与食物位置是否相重叠，如果重叠，在新的位置重新生成食物
        public void EatFood()
        {
            if (((Point)List[0]) == Food)  //如果蛇头吃到了食物
            {
                List.Add(List[List.Count - 1]);  //在蛇的尾部添加蛇身
                ifFood = false;  //没有食物
                BuildFood();  //生成食物
                label.Text = Convert.ToString(Convert.ToInt32(label.Text) + 5);  //显示当前分数
            }
        }
    }
}
