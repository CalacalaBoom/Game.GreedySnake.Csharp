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

    }
}
