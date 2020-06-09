using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using EasyAndLazy;

namespace EasyBrowser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitBrowser();
            InitEvent();
            Init();
        }
        public Form1(IntPtr Handle)
        {
            SendToHandle = Handle;
            InitializeComponent();
            InitBrowser();
            InitEvent();
            Init();
        }
        #region 属性

        public string[] StoryText { get; set; }   //放置当前文本
        public StreamReader HoleReader { get; set; }    //文件流
        public int CurIndex { get; set; }   //当前阅读行
        private INIClass ini { get; set; }      //配置类
        private string section { get; set; }    //当前配置项

        //用于拖动窗口
        bool beginMove = false;//初始化鼠标位置  
        int currentXPosition;
        int currentYPosition;

        //记录当前文本框宽度
        private int textWide { get; set; }

        //热键集合
        List<MyKey> keyList = new List<MyKey>();

        //内嵌浏览器对象
        public ChromiumWebBrowser browser;


        //URL窗体
        public FormURL FormUrl;

        bool isMouseDown = false; //表示鼠标当前是否处于按下状态，初始值为否 
        MouseDirection direction = MouseDirection.None;//表示拖动的方向，起始为None，表示不拖动
        Point mouseOff;//鼠标移动位置变量
        /// <summary>
        /// 接收窗口的句柄
        /// </summary>
        private IntPtr SendToHandle;
        public const int USER = 0x500;
        /// <summary>
        /// 自定义的消息ID
        /// </summary>
        public const int MYMESSAGE = USER + 1;
        #endregion
        #region 初始化
        public void InitBrowser()
        {
            Cef.Initialize(new CefSettings());
            IRequestContext requestContext = new RequestContext();
            browser = new ChromiumWebBrowser("http://www.baidu.com", requestContext);
            Font font = new Font("微软雅黑", 5.5f);
            this.Controls.Add(browser);
            browser.Font = font;
            browser.Dock = DockStyle.Fill;
            browser.LifeSpanHandler = new OpenPageSelf();//实现同一窗口打开链接
            browser.LoadingStateChanged += LoadingStateChangeds;
            
        }
        public void Init()
        {
            if (keyList == null)
            {
                keyList = new List<MyKey>();
            }

            keyList.AddRange(new[]
            {
                //关闭
                new MyKey{ id = 102,  keyA = HotKey.KeyModifiers.Alt, keyB = Keys.Q, op = operateMode.close },
                new MyKey{ id = 1021, keyA = HotKey.KeyModifiers.Alt, keyB = Keys.O, op = operateMode.close },
                new MyKey{ id = 1022, keyA = HotKey.KeyModifiers.Ctrl, keyB = Keys.O, op = operateMode.close },
                //搜索
                new MyKey{ id = 103,  keyA = HotKey.KeyModifiers.Alt, keyB = Keys.F, op = operateMode.search },
                //透明度
                new MyKey{ id = 105,  keyA = HotKey.KeyModifiers.Alt, keyB = Keys.Up, op = operateMode.addOpacity },
                new MyKey{ id = 106,  keyA = HotKey.KeyModifiers.Alt, keyB = Keys.Down, op = operateMode.reduceOpacity },
                //窗体移动
                new MyKey{ id = 1071,  keyA = HotKey.KeyModifiers.Ctrl, keyB = Keys.Up, op = operateMode.formUp },
                new MyKey{ id = 1072, keyA = HotKey.KeyModifiers.Ctrl, keyB = Keys.Down, op = operateMode.formDown },
                new MyKey{ id = 1073, keyA = HotKey.KeyModifiers.Ctrl, keyB = Keys.Left, op = operateMode.formLeft },
                new MyKey{ id = 1074, keyA = HotKey.KeyModifiers.Ctrl, keyB = Keys.Right, op = operateMode.formRight },
                //前进后退
                new MyKey{ id = 1041,  keyA = HotKey.KeyModifiers.Alt, keyB = Keys.S, op = operateMode.prePage },
                new MyKey{ id = 1042,  keyA = HotKey.KeyModifiers.Alt, keyB = Keys.D, op = operateMode.nextPage },

            });
            //注册热键
            keyList.ForEach(m => HotKey.RegisterHotKey(Handle, m.id, m.keyA, m.keyB));

            //窗体置顶层
            DllImport.SetWindowPos(this.Handle, -1, 0, 0, 0, 0, 1 | 2);

        }


        public void InitEvent()
        {
            Load += Form1_Load;

            //鼠标点击事件
            simpleButton1.MouseDown += btn_MouseDown;
            simpleButton1.MouseMove += btn_MouseMove;
            simpleButton1.MouseUp += btn_MouseUp;
            this.MouseDown += Form1_MouseDown1;
            MouseUp += Form1_MouseUp;
            MouseMove += Form1_MouseMove;
        }

        #endregion
        #region 事件

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            string inipath = Application.StartupPath + @"\indexConfig.ini";     //读取配置文件获取上次阅读行
            ini = new INIClass(inipath);
            section = "browser";
            if (!ini.ExistINIFile())    //不存在ini文件则新建并初始化
            {
                FileStream filest = new FileStream(inipath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                filest.Flush();
                filest.Close();
                ini.IniWriteValue(section, "Opacity", "0.4");
                ini.IniWriteValue(section, "LocationX", "50");
                ini.IniWriteValue(section, "LocationY", "1020");
            }
            if (string.IsNullOrEmpty(ini.IniReadValue(section, "Opacity")))
            {
                ini.IniWriteValue(section, "Opacity", "0.4");
            }
            if (string.IsNullOrEmpty(ini.IniReadValue(section, "LocationX")))
            {
                ini.IniWriteValue(section, "LocationX", "50");
            }
            if (string.IsNullOrEmpty(ini.IniReadValue(section, "LocationY")))
            {
                ini.IniWriteValue(section, "LocationY", "1020");
            }
            Opacity = Convert.ToDouble(ini.IniReadValue(section, "Opacity"));     //获取上次透明度
            int x = Convert.ToInt32(ini.IniReadValue(section, "LocationX"));     //获取上次x坐标
            int y = Convert.ToInt32(ini.IniReadValue(section, "LocationY"));     //获取上次y坐标
            Location = new Point(x, y);//定位
        }

        private void btn_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                currentXPosition = 0; //设置初始状态  
                currentYPosition = 0;
                beginMove = false;
            }
        }

        private void btn_MouseMove(object sender, MouseEventArgs e)
        {
            if (beginMove)
            {
                Console.WriteLine(MousePosition.X.ToString() + "; " + MousePosition.Y.ToString() + "; " + currentXPosition.ToString() + "; " + currentYPosition);
                this.Left += MousePosition.X - currentXPosition;//根据鼠标x坐标确定窗体的左边坐标x  
                this.Top += MousePosition.Y - currentYPosition;//根据鼠标的y坐标窗体的顶部，即Y坐标  
                currentXPosition = MousePosition.X;
                currentYPosition = MousePosition.Y;
            } 
        }

        private void btn_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                beginMove = true;
                currentXPosition = MousePosition.X;//鼠标的x坐标为当前窗体左上角x坐标  
                currentYPosition = MousePosition.Y;//鼠标的y坐标为当前窗体左上角y坐标  
            }
        }


        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //鼠标移动到边缘，改变鼠标的图标
            if (e.Location.X >= this.Width - 5)
            {
                this.Cursor = Cursors.SizeWE;
                direction = MouseDirection.Herizontal;
            }
            else if (e.Location.Y >= this.Height - 5)
            {
                this.Cursor = Cursors.SizeNS;
                direction = MouseDirection.Vertical;
            }
            //否则，以外的窗体区域，鼠标星座均为单向箭头（默认）             
            else
            {
                this.Cursor = Cursors.Arrow;

            }
            if (e.Location.X >= (this.Width + this.Left + 10) || (e.Location.Y > this.Height + this.Top + 10))
            {
                isMouseDown = false;
            }

            //设定好方向后，调用下面方法，改变窗体大小  
            ResizeWindow();
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            direction = MouseDirection.None;
        }

        private void Form1_MouseDown1(object sender, MouseEventArgs e)
        {
            mouseOff = new Point(-e.X, -e.Y); //记录鼠标位置
                                              //当鼠标的位置处于边缘时，允许进行改变大小。

            if (e.Location.X >= this.Width - 10 && e.Location.Y > this.Height - 10)
            {
                isMouseDown = true;
            }
            else if (e.Location.X >= this.Width - 5)
            {
                isMouseDown = true;
            }
            else if (e.Location.Y >= this.Height - 5)
            {
                isMouseDown = true;
            }
            else
            {
                this.Cursor = Cursors.Arrow;//改变鼠标样式为原样
                isMouseDown = false;
                //SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);//鼠标移动事件
            }
        }

        #endregion

        #region 方法
        /// <summary>

        /// 加载文本
        /// </summary>
        /// <param name="filePath">加载路径</param>
        private void LoadText(string filePath)
        {
            StoryText = new string[300000];
            Encoding ed = EncodingType.GetType(filePath);
            HoleReader = new StreamReader(filePath, ed);     //读取文本
            string line = "";
            for (int i = 1; (line = HoleReader.ReadLine()) != null; i++)
            {
                if (line == "")
                {
                    i--;
                    continue;
                }
                int j = 1;
                if (line.Length > textWide) //换行
                {
                    int len = line.Length;
                    StoryText[i] = line.Substring(0, textWide);
                    while (len - j * textWide > 0)
                    {
                        int leftWord = len - j * textWide;
                        if (leftWord > textWide)
                        {
                            StoryText[i + j] = line.Substring(j * textWide, textWide);
                        }
                        else
                        {
                            StoryText[i + j] = line.Substring(j * textWide, leftWord);
                        }
                        j++;
                    }
                    i += j - 1;
                }
                else
                {
                    StoryText[i] = line;
                }
            }
        }

        private void FormClose()
        {
            //卸载快捷键
            keyList.ForEach(m => HotKey.UnregisterHotKey(Handle, m.id));

            //记录当前坐标
            ini.IniWriteValue(section, "LocationX", Location.X.ToString());
            ini.IniWriteValue(section, "LocationY", Location.Y.ToString());
            //记录当前透明度
            ini.IniWriteValue(section, "Opacity", Opacity.ToString());
            Close();
        }
        //重写WndProc()方法，通过监视系统消息，来调用过程
        protected override void WndProc(ref Message m)//监视Windows消息
        {
            const int WM_HOTKEY = 0x0312;
            //按快捷键 
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    RECT currentRect = new RECT();
                    int id = m.WParam.ToInt32();
                    operateMode op = keyList.Find(k => k.id == id) == null ? operateMode.notExist : keyList.Find(k => k.id == id).op;
                    switch (op)
                    {
                        case operateMode.notExist: break;
                       
                        //关闭
                        case operateMode.close:
                            FormClose();
                            break;
                        //URL 
                        case operateMode.search:
                            if (FormUrl == null || FormUrl.IsDisposed)
                            {
                                FormUrl = new FormURL();
                                //重定向
                                FormUrl.RedirectEvent += FormUrl_RedirectEvent;
                                if (FormUrl.ShowDialog() == DialogResult.OK)
                                {
                                }
                            }
                            else
                            {
                                FormUrl.Dispose();
                                FormUrl.Close();//已打开，关闭
                            }
                            break;
                        //减透明度
                        case operateMode.reduceOpacity:
                            if (Opacity > 0.01f)
                            {
                                Opacity -= 0.01;
                            }
                            break;
                        //加透明度
                        case operateMode.addOpacity:
                            if (Opacity < 0.99f)
                            {
                                Opacity += 0.01;
                            }
                            break;
                        //窗体移动
                        case operateMode.formUp:
                            DllImport.GetWindowRect(Handle, ref currentRect);
                            Top--;
                            break;
                        case operateMode.formDown:
                            currentRect = new RECT();
                            DllImport.GetWindowRect(Handle, ref currentRect);
                            Top++;
                            break;
                        case operateMode.formLeft:
                            currentRect = new RECT();
                            DllImport.GetWindowRect(Handle, ref currentRect);
                            Left--;
                            break;
                        case operateMode.formRight:
                            currentRect = new RECT();
                            DllImport.GetWindowRect(Handle, ref currentRect);
                            Left++;
                            break;
                        //前进后退
                        case operateMode.nextPage:
                            browser.Forward();
                            break;
                        case operateMode.prePage:
                            browser.Back();
                            break;
                    }
                    break;

            }
            base.WndProc(ref m);
        }

        //重定向URL
        private void FormUrl_RedirectEvent(string url)
        {
            browser.Load(url);
            FormUrl.Dispose();
            FormUrl.Close();//已打开，关闭
        }

        //加载状态
        private void LoadingStateChangeds(object sender, EventArgs e)
        {

        }

        private void ResizeWindow()
        {

            if (!isMouseDown)
                return;

            if (direction == MouseDirection.Herizontal)
            {
                this.Cursor = Cursors.SizeWE;
                this.Width = MousePosition.X - this.Left + 5;//改变宽度
            }
            else if (direction == MouseDirection.Vertical)
            {
                this.Cursor = Cursors.SizeNS;
                this.Height = MousePosition.Y - this.Top + 5;//改变高度
            }
            //鼠标不在窗口右和下边缘，把鼠标打回原型
            else
            {
                this.Cursor = Cursors.Arrow;
                isMouseDown = false;
            }
        }

        #endregion

    }
}
