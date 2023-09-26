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
    public partial class FormTag : Form
    {
        private static string tagFilePath = Path.Combine(Application.StartupPath, "tag.txt");

        public string ChoosedTag { get; set; }   //选择的行
        public FormTag()
        {
            InitializeComponent();
            InitEvent();
        }
        public void InitEvent()
        {
            Load += FormTag_Load;
            dataGridTag.DoubleClick += dataGridTag_DoubleClick;
            dataGridTag.CellClick += DataGridTag_CellClick;
        }

        private void DataGridTag_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridTag.Columns[e.ColumnIndex].Name == "Column3")
            {
                if (MessageBox.Show("是否删除书签？", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    // 读取文件内容到列表中
                    List<string> lines = new List<string>(File.ReadAllLines(tagFilePath));

                    // 删除指定行
                    if (e.RowIndex >= 0 && e.RowIndex <= lines.Count - 1)
                    {
                        lines.RemoveAt(e.RowIndex);
                    }
                    else
                    {
                        MessageBox.Show("行号超出范围。");
                        return;
                    }
                    // 将更新后的内容写回文件
                    File.WriteAllLines(tagFilePath, lines);
                    //重新加载
                    LoadTagList();
                }
            }
        }

        private void dataGridTag_DoubleClick(object sender, EventArgs e)
        {
            var item = dataGridTag.CurrentRow.DataBoundItem as DataModel;
            if (item != null)
            {
                ChoosedTag = item.TextString;
                DialogResult = DialogResult.OK;
            }
        }

        private void FormTag_Load(object sender, EventArgs e)
        {
            LoadTagList();
        }

        private void LoadTagList()
        {
            QueueManager queueManager = new QueueManager(tagFilePath);
            if (queueManager.mainQueue.Count > 0)
            {
                var tagList = new List<DataModel>();
                var queueToArray = queueManager.mainQueue.ToArray();
                for (int i = 0; i < queueToArray.Length; i++)
                {
                    tagList.Add(new DataModel
                    {
                        Index = i,
                        TextString = queueToArray[i]
                    });
                }
                dataGridTag.DataSource = tagList;
            }
        }
    }
}
