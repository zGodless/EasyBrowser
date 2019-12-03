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
    public partial class FormURL : Form
    {
        public FormURL()
        {
            InitializeComponent();
            InitEvent();
        }

        //重定url委托
        public delegate void RedirectUrl(string url);

        public event RedirectUrl RedirectEvent;

        public void InitEvent()
        {
            textURL.KeyUp += TextURL_KeyUp;
        }

        private void TextURL_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //执行重定向
                string url = textURL.Text.Trim();
                RedirectEvent?.Invoke(url);
            }
        }
    }
}
