
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vison_Inspect_System._2_ComPart
{
    internal enum LogType
    {
        comm,
        Error,
        Data,
        Operate
    }
    class Log
    {
        public static event Action<string,Color> logRecordEvent;
        /// <summary>
        /// 通讯文件锁，防止文件冲突
        /// </summary>
        private static object objComm = new object();
        /// <summary>
        /// 异常文件锁，防止文件冲突
        /// </summary>
        private static object objError = new object();
        /// <summary>
        /// 数据信息锁，防止文件冲突
        /// </summary>
        private static object objData = new object();
        /// <summary>
        /// 操作信息锁，防止文件冲突
        /// </summary>
        private static object objOperate = new object();
        internal static void SaveLog(LogType logType, string message)
        {
            try
            {
                switch (logType)
                {
                    case LogType.comm:
                        lock (objComm)
                        {
                            DateTime now = DateTime.Now;
                            string filePath = string.Format(@"{0}\Log\Comm\{1}\", System.Windows.Forms.Application.StartupPath, now.ToString("yyyy-MM-dd"));
                            string fileName = now.ToString("HH时") + ".txt";//每小时创建一个txt，防止通讯数据交换频率高，数据量大，文本文件过大
                            if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
                            if (!File.Exists(filePath + fileName)) File.Create(filePath + fileName).Close();
                            File.AppendAllText(filePath + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss      ") + message + Environment.NewLine);
                            logRecordEvent($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}:{message}{Environment.NewLine}", Color.Red);
                        }
                        break;
                    case LogType.Operate:
                        lock (objOperate)
                        {
                            DateTime now = DateTime.Now;
                            string filePath = string.Format(@"{0}\Log\Operate\{1}\", System.Windows.Forms.Application.StartupPath, now.ToString("yyyy-MM-dd"));
                            string fileName = now.ToString("HH时") + ".txt";
                            if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
                            if (!File.Exists(filePath + fileName)) File.Create(filePath + fileName).Close();
                            File.AppendAllText(filePath + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss      ") + message + Environment.NewLine);
                            logRecordEvent($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}:{message}{Environment.NewLine}", Color.Black);
                        }
                        break;
                    case LogType.Data:
                        lock (objData)
                        {
                            DateTime now = DateTime.Now;
                            string filePath = string.Format(@"{0}\Log\Data\{1}\", System.Windows.Forms.Application.StartupPath, now.ToString("yyyy-MM-dd"));
                            string fileName = now.ToString("HH时") + ".txt";
                            if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
                            if (!File.Exists(filePath + fileName)) File.Create(filePath + fileName).Close();
                            File.AppendAllText(filePath + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss      ") + message + Environment.NewLine);
                            logRecordEvent($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}:{message}{Environment.NewLine}", Color.Black);
                            //此处待加
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("存储日志文件异常\r\n" + ex.ToString(), "Error");
            }
        }
        internal static void SaveError(StackTrace tmpST, StackFrame tmpSF, Exception ex)
        {
            try
            {
                lock (objError)
                {
                    tmpSF = tmpST.GetFrame(0);
                    string rowNo = "";
                    if (ex.ToString().Contains("行号") || ex.ToString().Contains("line"))
                    {
                        string temp = ex.ToString();
                        if (ex.ToString().Contains("行号"))
                        {
                            int index = temp.LastIndexOf(" ");
                            rowNo = temp.Substring(index + 1, temp.Length - index - 1);
                        }
                        else
                        {
                            rowNo = ex.ToString().Split(new string[] { "line" }, StringSplitOptions.RemoveEmptyEntries)[1];
                        }
                    }
                    else
                    {
                        rowNo = "未知";
                    }
                    string data = "----------     " + DateTime.Now.ToString() + "     -----------------------------------------------------" + Environment.NewLine +
                                  "原始数据：" + ex.ToString() + Environment.NewLine +
                                  "出错文件：" + tmpSF.GetFileName() + Environment.NewLine +
                                  "出错函数：" + tmpSF.GetMethod().Name + Environment.NewLine +
                                  "出错行号：" + rowNo + Environment.NewLine +
                                  "出错列号：" + tmpSF.GetFileColumnNumber() + Environment.NewLine +
                                  "出错信息：" + ex.Message.ToString() + Environment.NewLine + Environment.NewLine;

                    data += Environment.NewLine;
                    string ErrorPath = System.Windows.Forms.Application.StartupPath + "\\Log\\Error";
                    if (!Directory.Exists(ErrorPath))
                    {
                        Directory.CreateDirectory(ErrorPath);
                    }
                    string path = ErrorPath + "\\" + DateTime.Now.ToShortDateString().Replace("/", "-") + ".txt";
                    if (!File.Exists(path))
                    {
                        File.Create(path).Close();
                    }
                    File.AppendAllText(path, data);
                    System.Windows.Forms.MessageBox.Show(ex.ToString(), "提示：");
                }
            }
            catch (Exception es)
            {
                System.Windows.Forms.MessageBox.Show("存储错误日志文件异常\r\n" + es.ToString(), "提示：");
            }

        }
    }
}
