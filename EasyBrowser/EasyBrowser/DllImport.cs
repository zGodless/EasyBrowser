using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EasyBrowser
{
    public static class DllImport
    {

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        /// <summary>
        ///消息发送API
        /// </summary>
        /// <param name="hWnd">信息发往的窗口的句柄</param>
        /// <param name="Msg">消息ID</param>
        /// <param name="wParam">貌似没用</param>
        /// <param name="lParam">传输的数据参数</param>
        /// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(
        IntPtr hWnd,        // 信息发往的窗口的句柄
        int Msg,            // 消息ID
        int wParam,         // 参数1
         int lParam // 参数2   [MarshalAs(UnmanagedType.LPTStr)]StringBuilder lParam
        );
    }
}
