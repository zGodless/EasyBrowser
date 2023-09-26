using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyBrowser
{
    public partial class FormHistory : Form
    {
        private static string historyFilePath = Path.Combine(Application.StartupPath, "history.txt");

        public string ChoosedHistory { get; set; }   //选择的行
        public FormHistory()
        {
            InitializeComponent();
            InitEvent();
        }
        public void InitEvent()
        {
            Load += FormHistory_Load;
            dataGridHistory.DoubleClick += DataGridHistory_DoubleClick;
        }

        private void DataGridHistory_DoubleClick(object sender, EventArgs e)
        {
            var item = dataGridHistory.CurrentRow.DataBoundItem as DataModel;
            if (item != null)
            {
                ChoosedHistory = item.TextString;
                DialogResult = DialogResult.OK;
            }
        }

        private void FormHistory_Load(object sender, EventArgs e)
        {
            QueueManager queueManager = new QueueManager(historyFilePath);
            if(queueManager.reverseQueue.Count > 0)
            {
                var historyList = new List<DataModel>();
                var queueToArray = queueManager.reverseQueue.ToArray();
                for (int i = 0; i < queueToArray.Length; i++)
                {
                    historyList.Add(new DataModel
                    {
                        Index = i,
                        TextString = queueToArray[i]
                    });
                }
                dataGridHistory.DataSource = historyList;
            }
        }
    }
}
