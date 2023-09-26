using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBrowser
{
    internal class QueueManager
    {
        private const int MaxHistorySize = 1000;
        public Queue<string> mainQueue;//存储队列
        public List<string> reverseQueue //倒序数组
        {
            get { 
                return mainQueue.Reverse().ToList();
            } 
        }
        private string filePath;

        public QueueManager(string path)
        {
            mainQueue = new Queue<string>();
            filePath = path;
            LoadQueue();
        }

        public void AddQueue(string item)
        {
            if (mainQueue.Count >= MaxHistorySize)
            {
                mainQueue.Dequeue(); // 删除最早的记录
            }
            mainQueue.Enqueue(item); // 添加新的记录

            SaveQueue();
        }

        public Queue<string> GetMainQueue()
        {
            return mainQueue;
        }

        public List<string> GetReverseQueue()
        {
            return reverseQueue;
        }

        private void LoadQueue()
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        mainQueue.Enqueue(line);
                    }
                }
            }
        }

        private void SaveQueue()
        {
            File.WriteAllLines(filePath, mainQueue.ToArray());
        }
    }

}
