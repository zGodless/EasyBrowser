using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBrowser
{
    /// <summary>
    /// 操作模式枚举
    /// </summary>
    public enum operateMode
    {
        [Description("上一页")]
        notExist = -1,
        [Description("上一页")]
        prePage = 1,
        [Description("下一页")]
        nextPage = 2,
        [Description("关闭")]
        close = 3,
        [Description("搜索")]
        search = 4,
        [Description("减透明度")]
        reduceOpacity = 5,
        [Description("加透明度")]
        addOpacity = 6,
        [Description("窗体拖动：上")]
        formUp = 7,
        [Description("窗体拖动：下")]
        formDown = 8,
        [Description("窗体拖动：左")]
        formLeft = 9,
        [Description("窗体拖动：右")]
        formRight = 10
    }


    /// <summary>
    /// 鼠标拖动方向
    /// </summary>
    public enum MouseDirection
    {
        Herizontal,//水平方向拖动，只改变窗体的宽度  

        Vertical,//垂直方向拖动，只改变窗体的高度  

        Declining,//倾斜方向，同时改变窗体的宽度和高度

        None//不做标志，即不拖动窗体改变大小 
    }
}
