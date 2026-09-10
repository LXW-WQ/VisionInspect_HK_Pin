using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vison_Inspect_System._2_ComPart.Normal_Form;

namespace Vison_Inspect_System._2_ComPart.Statistics
{
    public class ScheduledDataWriter
    {
        private readonly System.Threading.Timer _timer;
        private readonly TimeSpan _scheduledTime; // 目标执行时间（如 08:00）
        public Action OnDataClearEvent; // 数据写入完成后的回调

        public ScheduledDataWriter(TimeSpan scheduledTime)
        {
            _scheduledTime = scheduledTime;
            _timer = new System.Threading.Timer(ExecuteScheduledTask, null, Timeout.Infinite, Timeout.Infinite);
        }

        public void ScheduleNextRun()
        {
            var now = DateTime.Now;
            // 计算今天的执行时间
            var scheduledRunTime = now.Date.Add(_scheduledTime);

            // 如果今天的时间已经过了，就安排到明天
            if (now > scheduledRunTime)
            {
                scheduledRunTime = scheduledRunTime.AddDays(1);
            }

            var delay = scheduledRunTime - now;

            Log.SaveLog(LogType.Data, $"[{DateTime.Now:HH:mm:ss}] 下一次数据刷新安排在: {scheduledRunTime:yyyy-MM-dd HH:mm:ss}");


            // 设置定时器在指定延迟后执行一次
            _timer.Change(delay, Timeout.InfiniteTimeSpan);
        }

        private async void ExecuteScheduledTask(object state)
        {
            try
            {

                // 在这里执行您的数据写入逻辑
                await WriteDataToDatabase();

            }
            catch (Exception ex)
            {
                Log.SaveLog(LogType.Operate, $"[{DateTime.Now:HH:mm:ss}] 数据写入失败: {ex.Message}");
            }
            finally
            {
                // 重新安排下一次执行
                ScheduleNextRun();
            }
        }

        private async Task WriteDataToDatabase()
        {

            // 模拟异步数据库操作
            DialogResult diag = MessageBox.Show("请确定是否进行班次数据清零操作？？");
            if (diag == DialogResult.Yes)
            {
                string[] headInfos = new string[7] {"总数", "NG数","OK数",
                "高度差检测NG数", "高度差检测OK数", "多胶视觉检NG数","多胶视觉检OK数",
                };
                string[] mesglist = new string[7]
                {
                string.Format($"{0}"),
                string.Format($"{0}"),
                string.Format($"{0}"),
                string.Format($"{0}"),
                string.Format($"{0}"),
                string.Format($"{0}"),
                string.Format($"{0}"),

                };
                List<string> updateInfoList = new List<string>();
                for (int i = 0; i < headInfos.Length; i++)
                {
                    updateInfoList.Add($"{headInfos[i]}='{mesglist[i]}'");
                }
                string strUpdateMesg = string.Join(",", updateInfoList);
                for (int i = 1; i <= 24; i++)
                {
                    string UpDateSql1 = $"UPDATE statisticstable SET {strUpdateMesg} WHERE 序号={i}";
                    int nRet = GloabalTool.mysql_Insert.ExecSQL(UpDateSql1);
                    Log.SaveLog(LogType.Data, $"更新数据库语句【{UpDateSql1}】{(nRet > 0 ? "成功" : "失败")}");
                }
                OnDataClearEvent();
                Log.SaveLog(LogType.Data, $"[{DateTime.Now:HH:mm:ss}] 班次数据清零操作完成");
            }
            else
            {
                Log.SaveLog(LogType.Data, $"[{DateTime.Now:HH:mm:ss}] 班次数据清零操作取消");
            }

        }

        public void Stop()
        {
            _timer?.Dispose();
        }
    }
}
