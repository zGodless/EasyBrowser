using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyBrowser
{
    public partial class FormNavigate : Form
    {
        public Form1 formMain = null;
        public Point location = new Point(0,0);
        public bool beginMove = false;
        int currentXPosition;
        int currentYPosition;
        public FormNavigate()
        {
            InitializeComponent();
        }

        private void FormNavigate_Load(object sender, EventArgs e)
        {
            Location = location;
            buttonNavigate.MouseUp += ButtonNavigate_MouseUp; ;
            buttonNavigate.MouseDown += ButtonNavigate_MouseDown;
            buttonNavigate.MouseMove += ButtonNavigate_MouseMove; ;
            
        }

        private void ButtonNavigate_MouseMove(object sender, MouseEventArgs e)
        {
            if (beginMove)
            {
                Console.WriteLine(MousePosition.X.ToString() + "; " + MousePosition.Y.ToString() + "; " + currentXPosition.ToString() + "; " + currentYPosition);
                this.Left += MousePosition.X - currentXPosition;//根据鼠标x坐标确定窗体的左边坐标x  
                this.Top += MousePosition.Y - currentYPosition;//根据鼠标的y坐标窗体的顶部，即Y坐标  
                currentXPosition = MousePosition.X;
                currentYPosition = MousePosition.Y;
                formMain.Location = new Point(MousePosition.X + 20, MousePosition.Y);
            }
        }

        private void ButtonNavigate_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                currentXPosition = 0; //设置初始状态  
                currentYPosition = 0;
                beginMove = false;
            }
        }

        private void ButtonNavigate_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                beginMove = true;
                currentXPosition = MousePosition.X;//鼠标的x坐标为当前窗体左上角x坐标  
                currentYPosition = MousePosition.Y;//鼠标的y坐标为当前窗体左上角y坐标  
            }
        }
    }
}
