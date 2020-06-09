using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace EasyBrowser
{
    public class MyKey
    {
        public int id { get; set; }
        public HotKey.KeyModifiers keyA { get; set; }
        public Keys keyB { get; set; }
        public operateMode op { get; set; }
        public MyKey()
        {

        }
    }
}
