using DataCollection_System._5_Device.Cam;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Vison_Inspect_System._2_ComPart;
using Vison_Inspect_System._2_ComPart.Normal_Form;
using Vison_Inspect_System._2_ComPart.Statistics;
using Vison_Inspect_System._5_Device.Cam.CamDevice;
using Vison_Inspect_System._5_Device.PLC;
using static Vison_Inspect_System._6_Process.VisionAlgorithm;

namespace Vison_Inspect_System
{
    public partial class MainForm : Form
    {

        /// <summary>
        /// SQL表头枚举
        /// </summary>
        protected enum sqlHead
        {
            日期,
            检测位置1数据1,
            检测位置1数据2,
            检测位置1高度差,
            检测位置1判断结果,
            检测位置2数据1,
            检测位置2数据2,
            检测位置2高度差,
            检测位置2判断结果,
            检测高度差判断总结果,
            铜片位置1数据1,
            铜片位置1数据2,
            铜片位置1平面度,
            铜片位置1平面度结果,
            铜片位置2数据1,
            铜片位置2数据2,
            铜片位置2平面度,
            铜片位置2平面度结果,
            铜片平面度判断总结果,
            多胶视觉检测结果,
            铜片压铸视觉检测结果,
            视觉检测总结果,
            成品总结果
        }

        userHelper help = new userHelper();//实例化用户管理帮助类
        List<Users> listusers = new List<Users>();//实例化一个用户对象

        private bool _closeMainFlag = false; //关闭主窗体标志位

        private double _cycleTime = 0;

        public event Action<string> LoadingMessage;    //加载窗口载入显示信息方法委托事件

        private ManualResetEvent _thrResetEvent = new ManualResetEvent(false);//线程控制事件

        private HSLModebusClient modebusClient = new HSLModebusClient(new HslCommunication.ModBus.ModbusTcpNet());//实例化PLC通讯类

        private Dictionary<sqlHead, string> sqlMessageDic = new Dictionary<sqlHead, string>();//SQL语句字典

        private Dictionary<int, ProductionRecord> statisticsDic = new Dictionary<int, ProductionRecord>();//生产数据统计字典


        ScheduledDataWriter writer = new ScheduledDataWriter(new TimeSpan(8, 0, 0));
        //MYSQL mysql = new MYSQL("localhost", "root", "lxw201314wq", "datacollect_db", 3306);

        public MainForm()
        {
            try
            {
                //先检测加密狗是否存在

                InitializeComponent();
                help.CheckSupperUser("user.SRX", listusers);//检查是否有用户存储文件，无就建立一个超级用户
                Log.logRecordEvent += AppendText;
                #region [加载窗口事件委托]
                new Thread(new ThreadStart(() =>
                {
                    LoadingForm loadingFrm = new LoadingForm(4);//加载最大资源数量
                    LoadingMessage += new Action<string>(loadingFrm.ShowMessage);
                    loadingFrm.ShowDialog();
                })).Start();
                Thread.Sleep(100);
                #endregion
                GloabalTool.LoadEquipmentConfig();
                LoadingMessage("加载系统参数");
                GloabalTool.EquipmentConfigLoad();
                LoadingMessage("加载相关对象实例化");
                modebusClient.Connect(GloabalTool.equipmentSettings.PLCIP, GloabalTool.equipmentSettings.Port, GloabalTool.equipmentSettings.ID);
                LoadingMessage("加载PLC通讯连接");
                GloabalTool.EquipmentConfigRefresh();
                LoadingMessage("加载窗口资源");
                Thread.Sleep(500);
                LoadingMessage("加载完成");

            }
            catch (Exception ex)
            {
                VM.PlatformSDKCS.VmException vmEx = VM.Core.VmSolution.GetVmException(ex);
                if (null != vmEx)
                {
                    MessageBox.Show("未检测到加密狗！");
                    Log.SaveLog(LogType.Error, "未检测到加密狗！");
                    //System.Environment.Exit(System.Environment.ExitCode);
                    this.Dispose();
                    this.Close();
                }
                else
                {
                    Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
                }
            }
        }
        #region 初始化流程

        /// <summary>
        /// 初始化流程
        /// </summary>
        private void Init()
        {
            try
            {
                #region 开启界面显示时间线程
                TimeDisplayTask = new Thread(TimeProcess);
                TimeDisplayTask.IsBackground = true;
                TimeDisplayTask.Start();
                #endregion
                if (System.IO.Directory.Exists(GloabalTool.MainDir) == false)
                {
                    System.IO.Directory.CreateDirectory(GloabalTool.MainDir);
                }
                if (System.IO.Directory.Exists(GloabalTool.ConfigDir) == false)
                {
                    System.IO.Directory.CreateDirectory(GloabalTool.ConfigDir);
                }
                if (System.IO.Directory.Exists(GloabalTool.ProductionCofigDir) == false)
                {
                    System.IO.Directory.CreateDirectory(GloabalTool.ProductionCofigDir);
                }

                this.WindowState = FormWindowState.Maximized;//即最大化打开窗口
                InitDic();
                Ctrtimer.Enabled = true;//控件刷新
                btnLoadRecipe.Enabled = true;
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                var err = InitCam();
                if (err != "")
                {
                    Log.SaveLog(LogType.comm, $"初始化失败：【{err}】");
                }
                else
                {
                    Log.SaveLog(LogType.Operate, $"初始化成功！！！");
                }
            }
            catch (Exception ex)
            {
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
            }
        }

        /// <summary>
        /// 相机相关进行初始化
        /// </summary>
        private string InitCam()
        {
            string err = string.Empty;
            Array values = Enum.GetValues(typeof(CameraType));
            foreach (CameraType camtype in values)
            {
                var camConfig = GloabalTool.equipmentSettings.CamConfigList.Find(r => r.CameraType == camtype);
                if (camConfig == null)
                {
                    err = $"相机初始化失败：err：camConfig is null";
                    return err;
                }
                switch (camtype)
                {
                    case CameraType.Camera1:
                        CameraImage camImg = new CameraImage() { CamDev = new HaiKangCam(), CamConfig = camConfig, Dock = DockStyle.Fill };
                        GloabalTool.CamDic.Add(camtype, camImg);
                        this.CamPannel.Controls.Add(camImg);
                        err = camImg.Init();
                        camImg.NotifiCamOfflineEvent -= CamOffline;
                        camImg.NotifiCamOfflineEvent += CamOffline;
                        break;
                    default:
                        break;
                }
            }
            if (err == "")
            {
                GetCamStatus(true);
            }
            return err;

        }

        private void dgInit()
        {
            dgData.Rows.Add();
            dgData.Rows.Add();
            dgData.Rows.Add();
            dgData.Rows.Add();
            dgData.ClearSelection();
            dgStatistics.ClearSelection();

        }
        private void InitDic()
        {
            dgInit();
            for (int i = 0; i < Enum.GetValues(typeof(sqlHead)).Length; i++)
            {
                sqlMessageDic.Add((sqlHead)i, "");
            }
            statisticsDic.Clear();
            statisticsDic.Add(1, new ProductionRecord() { SN = 1, Timestamp = "08:00-09:00" });
            statisticsDic.Add(2, new ProductionRecord() { SN = 2, Timestamp = "09:00-10:00" });
            statisticsDic.Add(3, new ProductionRecord() { SN = 3, Timestamp = "10:00-11:00" });
            statisticsDic.Add(4, new ProductionRecord() { SN = 4, Timestamp = "11:00-12:00" });
            statisticsDic.Add(5, new ProductionRecord() { SN = 5, Timestamp = "12:00-13:00" });
            statisticsDic.Add(6, new ProductionRecord() { SN = 6, Timestamp = "13:00-14:00" });
            statisticsDic.Add(7, new ProductionRecord() { SN = 7, Timestamp = "14:00-15:00" });
            statisticsDic.Add(8, new ProductionRecord() { SN = 8, Timestamp = "15:00-16:00" });
            statisticsDic.Add(9, new ProductionRecord() { SN = 9, Timestamp = "16:00-17:00" });
            statisticsDic.Add(10, new ProductionRecord() { SN = 10, Timestamp = "17:00-18:00" });
            statisticsDic.Add(11, new ProductionRecord() { SN = 11, Timestamp = "18:00-19:00" });
            statisticsDic.Add(12, new ProductionRecord() { SN = 12, Timestamp = "19:00-20:00" });
            statisticsDic.Add(13, new ProductionRecord() { SN = 13, Timestamp = "20:00-21:00" });
            statisticsDic.Add(14, new ProductionRecord() { SN = 14, Timestamp = "21:00-22:00" });
            statisticsDic.Add(15, new ProductionRecord() { SN = 15, Timestamp = "22:00-23:00" });
            statisticsDic.Add(16, new ProductionRecord() { SN = 16, Timestamp = "23:00-24:00" });
            statisticsDic.Add(17, new ProductionRecord() { SN = 17, Timestamp = "00:00-01:00" });
            statisticsDic.Add(18, new ProductionRecord() { SN = 18, Timestamp = "01:00-02:00" });
            statisticsDic.Add(19, new ProductionRecord() { SN = 19, Timestamp = "02:00-03:00" });
            statisticsDic.Add(20, new ProductionRecord() { SN = 20, Timestamp = "03:00-04:00" });
            statisticsDic.Add(21, new ProductionRecord() { SN = 21, Timestamp = "04:00-05:00" });
            statisticsDic.Add(22, new ProductionRecord() { SN = 22, Timestamp = "05:00-06:00" });
            statisticsDic.Add(23, new ProductionRecord() { SN = 23, Timestamp = "06:00-07:00" });
            statisticsDic.Add(24, new ProductionRecord() { SN = 24, Timestamp = "07:00-08:00" });

            string selectSql = $"SELECT * FROM statisticstable2";
            DataTable dt = GloabalTool.mysql_Insert.ExecSQLQuery(selectSql, null);
            bindingSource1.DataSource = dt;
            dgStatistics.DataSource = bindingSource1;

            writer.OnDataClearEvent -= Writer_OnDataClearEvent;
            writer.OnDataClearEvent += Writer_OnDataClearEvent;
            writer.ScheduleNextRun();
        }

        private void Writer_OnDataClearEvent()
        {
            try
            {
                //对数据统计界面进行更新
                UpdateStatisticsFrm();
            }
            catch (Exception ex)
            {
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
            }
        }

        #endregion

        #region [更新时间标识]
        Thread TimeDisplayTask;
        /// <summary>
        /// System时间显示
        /// </summary>
        private void TimeProcess()
        {
            while (!_closeMainFlag)
            {
                try
                {
                    labTime.Invoke(new MethodInvoker(() => labTime.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
                    Thread.Sleep(1000);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        #endregion

        #region 最小化、最大化、关闭窗口按钮事件
        /// <summary>
        /// 窗口最小化按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripBtnMinForm_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        /// <summary>
        /// 窗口最大化按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripBtnMaxForm_Click(object sender, EventArgs e)
        {
            FormMaxMinOperation();
        }
        /// <summary>
        /// 窗口最大化最小化操作
        /// </summary>
        private void FormMaxMinOperation()
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.MaximizedBounds = Screen.PrimaryScreen.WorkingArea;
                this.WindowState = FormWindowState.Maximized;
                this.ToolStripBtnMaxForm.Image = Vison_Inspect_System.Properties.Resources.ReducingDownForm;
                this.ToolStripBtnMaxForm.ToolTipText = "向下还原";
            }
            else if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                this.ToolStripBtnMaxForm.ToolTipText = "最大化窗口";
                this.ToolStripBtnMaxForm.Image = Vison_Inspect_System.Properties.Resources.MaxForm;
            }
        }

        /// <summary>
        /// 关闭窗口按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripBtnCloseForm_Click(object sender, EventArgs e)
        {
            try
            {
                //询问是否关闭窗体
                DialogResult dialogresult = MessageBox.Show("是否退出？是：退出；否：取消退出", "界面关闭提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                //关闭窗体判断
                if (dialogresult == DialogResult.Yes)
                {
                    bool flag = _runflag;
                    _runflag = false;
                    _closeMainFlag = true;//设置关闭标志位为true
                    if (flag)
                    {
                        TimeDisplayTask.Abort();
                        Thread.Sleep(1500);
                    }
                    modebusClient.Disconnect();//关闭PLC通讯
                    if (CloseCam() != "")
                    {
                        GetCamStatus(false);
                        MessageBox.Show("相机关闭失败");
                    }
                    GloabalTool.VisionAlgorithm.Dispose();
                    writer.Stop();
                    this.Close();
                    this.Dispose();
                    //Application.Exit();
                }
            }
            catch
            {
                //System.Environment.Exit(System.Environment.ExitCode);
                writer.Stop();
                GloabalTool.VisionAlgorithm.Dispose();
                this.Close();
                this.Dispose();
            }
        }

        #endregion
        #region [窗体顶部拖动]
        private static bool IsDrag = false;
        private int enterX;
        private int enterY;
        private void SetForm_MouseDown(object sender, MouseEventArgs e)
        {
            IsDrag = true;
            enterX = e.Location.X;
            enterY = e.Location.Y;
        }
        private void SetForm_MouseUp(object sender, MouseEventArgs e)
        {
            IsDrag = false;
            enterX = 0;
            enterY = 0;
        }
        private void SetForm_MouseLeave(object sender, EventArgs e)
        {
            IsDrag = false;
            enterX = 0;
            enterY = 0;
        }
        private void SetForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDrag)
            {
                Left += e.Location.X - enterX;
                Top += e.Location.Y - enterY;
            }
        }
        #endregion
        #region [窗体顶部双击放大和缩小]
        private void LabTitle_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            FormMaxMinOperation();
        }
        private void PictureBoxLog_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            FormMaxMinOperation();
        }
        #endregion
        #region  [窗体缩放]
        private const int WM_NCHITTEST = 0x0084; //鼠标在窗体客户区（除标题栏和边框以外的部分）时发送的信息
        const int HTLEFT = 10;  //左变
        const int HTRIGHT = 11;  //右边
        const int HTTOP = 12;
        const int HTTOPLEFT = 13;  //左上
        const int HTTOPRIGHT = 14; //右上
        const int HTBOTTOM = 15;  //下
        const int HTBOTTOMLEFT = 0x10;  //左下
        const int HTBOTTOMRIGHT = 17;  //右下
        System.Drawing.Point vPoint = System.Drawing.Point.Empty;
        //自定义边框拉伸
        protected override void WndProc(ref Message m)
        {

            try
            {
                base.WndProc(ref m);
                switch (m.Msg)
                {
                    case WM_NCHITTEST:
                        vPoint = new System.Drawing.Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 16 & 0xFFFF);
                        vPoint = PointToClient(vPoint);
                        if (vPoint.X <= 5)
                            if (vPoint.Y <= 5)
                                m.Result = (IntPtr)HTTOPLEFT;  //左上
                            else if (vPoint.Y >= this.ClientSize.Height - 5)
                                m.Result = (IntPtr)HTBOTTOMLEFT; //左下
                            else
                                m.Result = (IntPtr)HTLEFT;  //左边
                        else if (vPoint.X >= this.ClientSize.Width - 5)
                            if (vPoint.Y <= 5)
                                m.Result = (IntPtr)HTTOPRIGHT;  //右上
                            else if (vPoint.Y >= this.ClientSize.Height - 5)
                                m.Result = (IntPtr)HTBOTTOMRIGHT;  //右下
                            else
                                m.Result = (IntPtr)HTRIGHT;  //右
                        else if (vPoint.Y <= 5)
                            m.Result = (IntPtr)HTTOP;  //上
                        else if (vPoint.Y >= this.ClientSize.Height - 5)
                            m.Result = (IntPtr)HTBOTTOM; //下

                        else
                        {
                            base.WndProc(ref m);//如果去掉这一行代码,窗体将失去MouseMove..等事件
                            System.Drawing.Point lpint = new System.Drawing.Point((int)m.LParam);//可以得到鼠标坐标,这样就可以决定怎么处理这个消息了,是移动窗体,还是缩放,以及向哪向的缩放

                            m.Result = (IntPtr)0x2;//托动HTCAPTION=2 <0x2>
                        }
                        break;
                }
            }
            catch
            {
            }
        }


        #endregion
        #region UserLogin FUNC
        /// <summary>
        /// 底部用户登陆信息展示
        /// </summary>
        /// <param name="user">用户信息</param>
        public void Userlogin(Users user)
        {
            UserNametoolStripLabel.Text = user.Username;
            UserLeveltoolStripLabel.Text = user.Level.ToString();
            GloabalTool.loginUser = user.Username;
            GloabalTool.loginLevel = user.Level.ToString();
            CurrentInfo.authority = user.Level;
            CurrentInfo.LoginOut = false;
        }
        /// <summary>
        /// 底部用户注销提示
        /// </summary>
        public void Userlogout()
        {
            Log.SaveLog(LogType.Operate, $"用户:{GloabalTool.loginUser};用户类型:{GloabalTool.loginLevel}:注销用户");
            CurrentInfo.authority = Authroity.Null;
            Action<string> TooltripSetLabText = (labText) =>
            {
                if (this.toolStripBottomInfo.InvokeRequired)
                {
                    this.toolStripBottomInfo.Invoke(new MethodInvoker(() => { this.UserNametoolStripLabel.Text = labText; this.UserLeveltoolStripLabel.Text = labText; }));
                }
                else
                {
                    this.UserNametoolStripLabel.Text = labText;
                    this.UserLeveltoolStripLabel.Text = labText;
                }
            };
            TooltripSetLabText("Null");
            CurrentInfo.LoginOut = true;

        }
        /// <summary>
        /// 检查是不是管理员等级
        /// </summary>
        /// <returns></returns>
        private bool ChekAdmin()
        {
            if (CurrentInfo.authority == Authroity.Administrator)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 检查是不是管理员或工程师等级
        /// </summary>
        /// <returns></returns>
        private bool ChekAdminAndEng()
        {
            if (CurrentInfo.authority == Authroity.Administrator | CurrentInfo.authority == Authroity.Engineer)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 相关菜单事件
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            Init();
        }

        /// <summary>
        /// 用户登录事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 用户登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentInfo.authority == Authroity.Null)
            {
                UserLoginFrm userloginfrm = new UserLoginFrm();
                userloginfrm.sendlogin += Userlogin;
                userloginfrm.ShowDialog();
                Log.SaveLog(LogType.Operate, $"用户:{GloabalTool.loginUser};用户类型:{GloabalTool.loginLevel}:登录成功");
            }
            else
            {
                Userlogout();
            }

        }

        /// <summary>
        /// 用户注销事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 用户注销ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Userlogout();
        }

        /// <summary>
        /// 用户管理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 用户管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ChekAdminAndEng())
            {
                MessageBox.Show("当前用户无此权限", "拒绝访问", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            UserMangagement usermagementfrm = new UserMangagement();
            usermagementfrm.ShowDialog();
        }

        /// <summary>
        /// 数据库查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 数据库查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ChekAdminAndEng())
            {
                MessageBox.Show("当前用户无此权限", "拒绝访问", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DataSelectForm dataSelectForm = new DataSelectForm();
            dataSelectForm.Show();
        }

        /// <summary>
        /// 系统参数设置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 系统参数设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ChekAdminAndEng())
            {
                MessageBox.Show("当前用户无此权限", "拒绝访问", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ParameterForm parameter = new ParameterForm("设备硬件配置", GloabalTool.equipmentSettings, GloabalTool.Path_Equipment_Setting);
            parameter.Show();
        }

        /// <summary>
        /// 配方参数设置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 配方参数设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (!ChekAdminAndEng())
            //{
            //    MessageBox.Show("当前用户无此权限", "拒绝访问", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            VisonSetting visonSetting = new VisonSetting(GloabalTool.ProductFullName);
            visonSetting.ShowDialog();
        }


        private void 班次数据统计查看ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (!ChekAdminAndEng())
            {
                MessageBox.Show("当前用户无此权限", "拒绝访问", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ShiftStatisticsFrm shiftStatisticsFrm = new ShiftStatisticsFrm();
            shiftStatisticsFrm.Show();

        }



        /// <summary>
        /// 客户端Modbus通讯测试事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void modBus客户端测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("此功能屏蔽");
        }

        /// <summary>
        /// 关于事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripBtnHelp_Click(object sender, EventArgs e)
        {
            About aboutFrm = new About();
            aboutFrm.ShowDialog();
        }

        /// <summary>
        /// 底部状态栏显示刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ctrtimer_Tick(object sender, EventArgs e)
        {
            CycleTimetoolStripLabel.Text = _cycleTime.ToString() + "ms";
            if (labTitle.Text != GloabalTool.MainTitle) labTitle.Text = GloabalTool.MainTitle;
        }



        #endregion

        #region 按钮相关事件
        /// <summary>
        /// 加载配方按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadRecipe_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.InitialDirectory = GloabalTool.ProductionCofigDir;
            dialog.Filter = "Program|*.json";
            dialog.Title = "选择配方";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string recipeFullName = dialog.FileName;
                if (recipeFullName == "" || !File.Exists(recipeFullName))
                {
                    MessageBox.Show("空配方无法加载、请新建或打开现有配方");
                    Log.SaveLog(LogType.comm, $"空配方无法加载");
                    return;
                }
                else
                {
                    GloabalTool.ProductFullName = recipeFullName;

                    GloabalTool.ProductIni = new RecipeConfig();



                    if (GloabalTool.LoadRecipe(recipeFullName).Item1 == false)
                    {
                        MessageBox.Show("配方加载失败、请新建或打开正确配方");
                        Log.SaveLog(LogType.comm, $"配方加载失败");
                        return;
                    }
                    else
                    {
                        GloabalTool.ProductIni = GloabalTool.LoadRecipe(recipeFullName).Item2;
                        if (GloabalTool.ProductIni == null)
                        {
                            MessageBox.Show($"配方参数为null！");
                            Log.SaveLog(LogType.comm, $"配方参数为null！");
                            return;
                        }
                        var err = LoadRecipeConfig(GloabalTool.ProductIni);
                        if (err != "")
                        {
                            MessageBox.Show($"配方参数加载失败 ！ err：{err}");
                            Log.SaveLog(LogType.Operate, $"配方参数加载失败 ！ err：{err}");
                            return;
                        }

                    }
                    // 获取文件名（不包含扩展名）
                    string fileNameNoExt = Path.GetFileNameWithoutExtension(GloabalTool.ProductFullName);
                    labRecipe.Text = fileNameNoExt;
                    MessageBox.Show("配方加载成功");
                    Log.SaveLog(LogType.Operate, $"【{fileNameNoExt}】配方加载成功！！！");
                }
            }
        }

        /// <summary>
        /// 加载相关配方参数
        /// </summary>
        /// <returns></returns>
        private string LoadRecipeConfig(RecipeConfig recipeConfig)
        {
            var err = string.Empty;

            foreach (var cam in GloabalTool.CamDic)
            {
                var flag = cam.Value.CamDev.SetExposureTime(recipeConfig.Exposure);
                flag &= cam.Value.CamDev.SetGain(recipeConfig.GainValue);
                if (!flag)
                {
                    Log.SaveLog(LogType.comm, $"相机【{cam.Key.ToString()}】曝光或增益参数写入失败！");
                }
            }
            err = GloabalTool.VisionAlgorithm.LoadProject(GloabalTool.ProductIni.VisionProjectPath);

            if (err != "")
            {
                err = $"视觉方案加载失败！Err:{err}";

                Log.SaveLog(LogType.comm, err);
            }
            return err;
        }


        /// <summary>
        /// 开始按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            Log.SaveLog(LogType.Operate, $"启动视觉检测");
            if (GloabalTool.ProductIni == null)
            {
                MessageBox.Show("请先加载配方、再启动");
                Log.SaveLog(LogType.comm, $"请先加载配方、再启动");
                return;
            }
            if (modebusClient.WriteHeartValue("7010", (short)(0)))
            {
                Log.SaveLog(LogType.Operate, $"PLC连接成功");
            }
            else
            {
                MessageBox.Show("PLC连接失败");
                Log.SaveLog(LogType.comm, $"PLC连接失败");
                return;
            }
            _runflag = true;
            _thrResetEvent.Reset();
            ThreadStart pts = new ThreadStart(DoActionVision);//开启相机检测线程
            Thread thr = new Thread(pts);
            thr.IsBackground = true;
            thr.Start();
            var timeout = 2000;
            var flag = _thrResetEvent.WaitOne(timeout);
            Log.SaveLog(LogType.Operate, $"相机检测线程开启成功");
            btnctr(true);
        }

        /// <summary>
        /// 停止按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnStop_Click(object sender, EventArgs e)
        {
            Log.SaveLog(LogType.Operate, $"停止视觉检测");
            _runflag = false;
            btnctr(false);
        }


        #endregion

        /// <summary>
        /// 按钮控制
        /// </summary>
        /// <param name="isStart">是否是开始</param>
        private void btnctr(bool isStart)
        {
            if (isStart)
            {
                btnLoadRecipe.Enabled = false;
                btnStart.Enabled = false;
                btnStop.Enabled = true;
                系统参数设置ToolStripMenuItem.Enabled = false;
                配方参数设置ToolStripMenuItem.Enabled = false;
            }
            else
            {
                btnLoadRecipe.Enabled = true;
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                配方参数设置ToolStripMenuItem.Enabled = true;
                系统参数设置ToolStripMenuItem.Enabled = true;
            }
        }

        /// <summary>
        /// 相机连接状态
        /// </summary>
        /// <param name="camstatus"></param>
        private void GetCamStatus(bool camstatus)
        {
            try
            {
                //状态信息操作
                this.Invoke(new MethodInvoker(() =>
                {
                    if (camstatus)
                    {
                        if (ToolLabIsConnectCam.Text != "在线")
                        {
                            ToolLabIsConnectCam.Text = "在线";
                            ToolLabIsConnectCam.Image = Vison_Inspect_System.Properties.Resources.Connect;
                            ToolLabIsConnectCam.ForeColor = Color.LimeGreen;
                        }
                    }
                    else
                    {
                        if (ToolLabIsConnectCam.Text != "离线")
                        {
                            ToolLabIsConnectCam.Text = "离线";
                            ToolLabIsConnectCam.Image = Vison_Inspect_System.Properties.Resources.DisConnect;
                            ToolLabIsConnectCam.ForeColor = Color.Red;
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
            }
        }

        /// <summary>
        /// 与相机的掉线事件绑定
        /// </summary>
        /// <param name="isOffline"></param>
        private void CamOffline(bool isOffline)
        {
            GetCamStatus(!isOffline);
        }

        private bool _runflag = false;
        private bool insertthr = false;
        private void DoActionVision()
        {
            try
            {
                //1、7000：//触发信号（1：触发拍照；0：无触发）
                //2、7001：//视觉检测结果（1：OK；2：NG）
                //3、7010：//心跳信号
                DateTime StartTime;
                insertthr = true;
                bool plcHeart = false;
                while (_runflag)
                {
                    var heartFlag = modebusClient.WriteHeartValue("7010", (short)(plcHeart ? 0 : 1));//发送心跳信号
                    if (!heartFlag)
                    {
                        Log.SaveLog(LogType.comm, $"与PLC连接异常、线程中断");
                        _runflag = false;
                        btnctr(false);
                        break;
                    }
                    plcHeart = !plcHeart;//心跳信号翻转
                    bool inspectTotalRes = false;//视觉检测结果
                    StartTime = DateTime.Now;
                    modebusClient.ReadValue("x=3;7000", out short trigVisionSnap);//读取触发信号
                    modebusClient.ReadValue("x=3;7006", out short trigDataGet);//读取数据获取信号
                    if (trigVisionSnap == 1)
                    {
                        modebusClient.WriteValue("7000", (short)0);//将触发信号清零
                        //执行视觉算法处理
                        if (GloabalTool.CamDic.ContainsKey(CameraType.Camera1))
                        {
                            var cam = GloabalTool.CamDic[CameraType.Camera1];
                            if (cam.CamDev != null && cam.CamConnectStatus)
                            {
                                ImageStream image = cam.CamDev.SofwareTrigger();//软触发拍照
                                if (image != null)
                                {
                                    //执行视觉检测
                                    var result = cam.TriggerOnceInspection(cam.CamConfig, image);
                                    if (result.Item1 != "")
                                    {
                                        Log.SaveLog(LogType.Error, $"视觉检测异常，错误信息：{result.Item1}");
                                    }
                                    else
                                    {
                                        if (result.Item2 != null)
                                        {
                                            inspectTotalRes = result.Item2.InspectTotalStauts;
                                            if (cam.CamConfig.IsSaveImage && !inspectTotalRes)
                                            {
                                                if (!Directory.Exists(cam.CamConfig.SavePicturePath))
                                                {
                                                    Directory.CreateDirectory(cam.CamConfig.SavePicturePath);
                                                }
                                                cam.SaveImg(cam.CamConfig);
                                            }
                                            sqlMessageDic[sqlHead.视觉检测总结果] = inspectTotalRes ? "OK" : "NG";
                                            sqlMessageDic[sqlHead.多胶视觉检测结果] = result.Item2.ExcessEpoxyInspectStatus ? "OK" : "NG";
                                            sqlMessageDic[sqlHead.铜片压铸视觉检测结果] = result.Item2.PressureInspectStatus ? "OK" : "NG";
                                            //InsertVisionSql(result.Item2);
                                            Log.SaveLog(LogType.Operate, $"视觉检测成功，结果：{(inspectTotalRes ? "OK" : "NG")}");
                                        }
                                        else
                                        {
                                            sqlMessageDic[sqlHead.视觉检测总结果] = "NG";
                                            sqlMessageDic[sqlHead.多胶视觉检测结果] = "NG";
                                            sqlMessageDic[sqlHead.铜片压铸视觉检测结果] = "NG";
                                            Log.SaveLog(LogType.comm, $"视觉检测失败，错误信息：VisonMaster返回的对象为null");
                                        }
                                    }
                                }
                                else
                                {
                                    Log.SaveLog(LogType.comm, "获取图像失败");
                                    sqlMessageDic[sqlHead.视觉检测总结果] = "NG";
                                    sqlMessageDic[sqlHead.多胶视觉检测结果] = "NG";
                                    sqlMessageDic[sqlHead.铜片压铸视觉检测结果] = "NG";
                                }
                            }
                            else
                            {
                                Log.SaveLog(LogType.comm, "相机未连接或连接失败");
                                sqlMessageDic[sqlHead.视觉检测总结果] = "NG";
                                sqlMessageDic[sqlHead.多胶视觉检测结果] = "NG";
                                sqlMessageDic[sqlHead.铜片压铸视觉检测结果] = "NG";
                            }
                        }
                        modebusClient.WriteValue("7001", (short)(inspectTotalRes ? 1 : 2));//视觉检测结果（1：OK；2：NG）

                        labCamResult.Invoke(new MethodInvoker(() =>
                        {
                            labCamResult.Text = inspectTotalRes ? "视觉检测OK" : "视觉检测NG";
                            labCamResult.BackColor = inspectTotalRes ? Color.LimeGreen : Color.Red;
                        }));
                    }
                    if (trigDataGet == 1)
                    {
                        bool DataGetSuccess = true;
                        modebusClient.WriteValue("7006", (short)(0));//将数据获取信号清零
                        //执行高度相关PLC数据读取
                        modebusClient.ReadValue("x=3;14288", out float[] lugDatas1, 2);//检测位置2个点的数据（汇川PLC地址R2000）
                        modebusClient.ReadValue("x=3;14308", out float[] lugDatas2, 2);//检测位置2个点的数据（汇川PLC地址R2020）
                        modebusClient.ReadValue("x=3;14328", out float[] lugDiffenceValues, 2);//检测位置两点之间的偏差值（汇川PLC地址R2040）
                        modebusClient.ReadValue("x=3;14348", out short[] lugResArray, 2);//检测2个位置比较结果（汇川PLC地址R2060）
                        List<float> lugDatas = new List<float>();
                        for (int i = 0; i < lugDatas1.Length; i++)
                        {
                            lugDatas.Add(lugDatas1[i]);
                            lugDatas.Add(lugDatas2[i]);
                        }
                        var err = UpdateForm(lugDatas.ToArray(), lugDiffenceValues, lugResArray,true);
                        if (err != "")
                        {
                            Log.SaveLog(LogType.Error, $"高度差数据读取错误：{err}");
                            DataGetSuccess = false;
                        }

                        //执行铜片相关的PLC数据读取
                        modebusClient.ReadValue("x=3;14388", out float[] flateDatas1, 2);//铜片位置1的2个点的数据
                        modebusClient.ReadValue("x=3;14408", out float[] flateDatas2, 2);//铜片位置2的2个点的数据
                        modebusClient.ReadValue("x=3;14428", out float[] flateDiffenceValues, 2);//铜片两个位置的平面度数据
                        modebusClient.ReadValue("x=3;14448", out short[] flateResArray, 2);//铜片两个位置的平面度结果
                        List<float> flateDatas = new List<float>();
                        for (int i = 0; i < flateDatas1.Length; i++)
                        {
                            flateDatas.Add(flateDatas1[i]);
                            flateDatas.Add(flateDatas2[i]);
                        }
                        err = UpdateForm(flateDatas.ToArray(), flateDiffenceValues, flateResArray, false);
                        if (err != "")
                        {
                            Log.SaveLog(LogType.Error, $"铜片平面度数据读取错误：{err}");
                            DataGetSuccess = false;
                        }
                        bool insertsqlFlag = InsertSql();
                        if (!insertsqlFlag)
                        {
                            Log.SaveLog(LogType.Error, $"插入数据库错误：{err}");
                            DataGetSuccess = false;
                        }
                        //将折弯数据保存到数据库----------------------------------------待处理
                        modebusClient.WriteValue("7106", (short)(DataGetSuccess ? 1 : 2));//数据写入完成（1：写入成功；2：写入失败）
                        sqlMessageDic.Clear();
                        for (int i = 0; i < Enum.GetValues(typeof(sqlHead)).Length; i++)
                        {
                            sqlMessageDic.Add((sqlHead)i, "");
                        }
                    }

                    _cycleTime = Math.Round(DateTime.Now.Subtract(StartTime).TotalMilliseconds, 2);
                    if (insertthr)
                    {
                        _thrResetEvent.Set();
                        insertthr = false;
                    }
                    Thread.Sleep(50);
                }
            }
            catch (Exception ex)
            {
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
            }
        }

        private bool InsertDataToMysql(string insertSql)
        {
            try
            {
                bool result = false;
                this.Invoke(new MethodInvoker(() =>
                {

                    if (GloabalTool.mysql_Insert.ExecSQL(insertSql) > 0)
                    {
                        if (toolStripLabelSqlIsConnect.Text != "写入成功")
                        {
                            toolStripLabelSqlIsConnect.Text = "写入成功";
                            toolStripLabelSqlIsConnect.ForeColor = Color.LimeGreen;

                        }
                        result = true;
                    }
                    else
                    {
                        if (toolStripLabelSqlIsConnect.Text != "写入失败")
                        {
                            toolStripLabelSqlIsConnect.Text = "写入失败";
                            toolStripLabelSqlIsConnect.ForeColor = Color.Red;
                            result = false;
                        }
                    }
                }));
                return result;
            }
            catch (Exception ex)
            {
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
                return false;
            }
        }


        /// <summary>
        /// 界面信息日志刷新
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="color"></param>
        private void AppendText(string msg, Color color)
        {
            if (this.richTextBox1.IsHandleCreated)
            {
                this.richTextBox1.Invoke(new Action(() =>
                {
                    this.richTextBox1.SelectionStart = this.richTextBox1.TextLength;
                    this.richTextBox1.SelectionLength = 0;

                    this.richTextBox1.SelectionColor = color;
                    this.richTextBox1.AppendText(msg);
                    this.richTextBox1.SelectionColor = this.richTextBox1.ForeColor;

                    this.richTextBox1.ScrollToCaret();
                }));
            }
        }

        /// <summary>
        /// 日志内容变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (this.richTextBox1.Lines.Length > 200)
            {
                int linesToRemove = this.richTextBox1.Lines.Length - 200;
                int charIndex = this.richTextBox1.GetFirstCharIndexFromLine(linesToRemove);
                this.richTextBox1.Select(0, charIndex);
                this.richTextBox1.SelectedText = string.Empty;
                this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
                this.richTextBox1.ScrollToCaret();
            }
        }

        /// <summary>
        /// 关闭相机连接
        /// </summary>
        /// <returns></returns>
        private string CloseCam()
        {
            var err = string.Empty;
            foreach (var cam in GloabalTool.CamDic)
            {
                if (cam.Value.CamDev != null)
                {
                    err = cam.Value.CamDev.DisconnectCamera();
                    if (err != "")
                    {
                        break;
                    }
                }
            }
            return err;
        }

        private string InsertVisionSql(InspectionResult result)
        {
            var err = string.Empty;
            string[] headInfos = new string[4] { "日期", "检测结果", "AOI检测结果", "总结果" };
            string[] mesglist = new string[4]
            {
                string.Format($"'{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}'"),
                string.Format($"'{(result.InspectTotalStauts?"OK":"NG")}'"),
                string.Format($"'{(result.AOIInspectStatus?"OK":"NG")}'"),
                string.Format($"'{((result.AOIInspectStatus&&result.InspectTotalStauts)?"OK":"NG")}'"),
            };
            string str1Head = string.Join(",", headInfos);
            string str2Mesg = string.Join(",", mesglist);
            string InsertSql1 = $"INSERT INTO ProductInspectTable2({str1Head})VALUES({str2Mesg})";
            var insertRes = InsertDataToMysql(InsertSql1);
            Log.SaveLog(LogType.Data, $"插入数据库语句【{InsertSql1}】{(insertRes ? "成功" : "失败")}");
            return "";
        }

        private string UpdateForm(float[] datas, float[] diffenceValues, short[] resArray,bool isHeight)
        {
            var err = string.Empty;
            string st = isHeight ? "高度" : "平面度";
            if (datas.Length != 4)
            {
                err = $"平面的点位数据长度不正确、请确认PLC端数据发送是否正常！！！";
                return err;
            }
            if (diffenceValues.Length != 2)
            {
                err = $"平面的点偏差数据长度不正确、请确认PLC端数据发送是否正常！！！";
                return err;
            }
            if (resArray.Length != 2)
            {
                err = $"平面的点判断结果数据长度不正确、请确认PLC端数据发送是否正常！！！";
                return err;
            }
            Dictionary<string, string> dataDic = new Dictionary<string, string>();
            DataGridView dataGrid = isHeight ? dgData : dgFlatness;
            dataGrid.Invoke(new MethodInvoker(() =>
            {
                for (int i = 0; i < 2; i++)
                {
                    int a = (i * 2);
                    int b = a + 1;
                    bool rowres = resArray[i] == 1;
                    string resStr = rowres ? "OK" : "NG";
                    dataGrid.Rows[i].Cells[0].Value = (i + 1);
                    dataGrid.Rows[i].Cells[1].Value = datas[a].ToString("F3");
                    dataGrid.Rows[i].Cells[2].Value = datas[b].ToString("F3");
                    dataGrid.Rows[i].Cells[3].Value = diffenceValues[i].ToString("F3");
                    dataGrid.Rows[i].Cells[4].Value = resStr;
                    if(isHeight)
                    {
                        dataDic.Add($"检测位置{i + 1}数据1", datas[a].ToString("F3"));
                        dataDic.Add($"检测位置{i + 1}数据2", datas[b].ToString("F3"));
                        dataDic.Add($"检测位置{i + 1}高度差", diffenceValues[i].ToString("F3"));
                        dataDic.Add($"检测位置{i + 1}判断结果", resStr);
                    }
                    else
                    {
                        dataDic.Add($"铜片位置{i + 1}数据1", datas[a].ToString("F3"));
                        dataDic.Add($"铜片位置{i + 1}数据2", datas[b].ToString("F3"));
                        dataDic.Add($"铜片位置{i + 1}平面度", diffenceValues[i].ToString("F3"));
                        dataDic.Add($"铜片位置{i + 1}平面度结果", resStr);
                    }
                    if (rowres)
                    {
                        dataGrid.Rows[i].DefaultCellStyle.BackColor = Color.LimeGreen;
                    }
                    else
                    {
                        dataGrid.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    }
                }
                foreach (var item in dataDic)
                {
                    string str = item.Key.ToString();
                    sqlHead sqlHeadenum;
                    if (Enum.TryParse<sqlHead>(str, out sqlHeadenum))
                    {
                        if (sqlMessageDic.ContainsKey(sqlHeadenum))
                        {
                            sqlMessageDic[sqlHeadenum] = item.Value;
                        }
                    }
                }

            }));
            bool totalRes = !resArray.Contains((short)2);
            if(isHeight)
            {
                sqlMessageDic[sqlHead.检测高度差判断总结果] = totalRes ? "OK" : "NG";
            }
            else
            {
                sqlMessageDic[sqlHead.铜片平面度判断总结果] = totalRes ? "OK" : "NG";
            }
            Label labRes = isHeight ? labDataRes : labFlatness;
            string title = isHeight ? "高度差检测" : "平面度检测";
            labRes.Invoke(new MethodInvoker(() =>
            {
                labRes.Text = title + (totalRes ? "OK" : "NG");
                labRes.BackColor = totalRes ? Color.LimeGreen : Color.Red;
            }));
            return err;
        }

        private bool InsertSql()
        {
            var err = string.Empty;
            sqlMessageDic[sqlHead.日期] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            bool totalRes = (sqlMessageDic[sqlHead.检测高度差判断总结果] == "OK");
            totalRes &= (sqlMessageDic[sqlHead.铜片平面度判断总结果] == "OK");
            totalRes &= sqlMessageDic[sqlHead.视觉检测总结果] == "OK";
            sqlMessageDic[sqlHead.成品总结果] = totalRes ? "OK" : "NG";
            List<string> Headlist = new List<string>();
            List<string> mesglist = new List<string>();
            foreach (var item in sqlMessageDic)
            {
                Headlist.Add(item.Key.ToString());
                mesglist.Add(string.Format($"'{item.Value}'"));
            }
            string str1Head = string.Join(",", Headlist);
            string str2Mesg = string.Join(",", mesglist);
            string InsertSql1 = $"INSERT INTO ProductInspectTable2({str1Head})VALUES({str2Mesg})";
            var insertRes = InsertDataToMysql(InsertSql1);
            Log.SaveLog(LogType.Data, $"插入数据库语句【{InsertSql1}】{(insertRes ? "成功" : "失败")}");
            //更新统计表
            UpdateStatisticsSql();
            return insertRes;
        }

        //更新统计数据库
        private string UpdateStatisticsSql()
        {
            var err = string.Empty;
            //更新数据库中统计表
            //获取当前时间段
            int timeSlot = GetTimeSlot(DateTime.Now);
            if (timeSlot > 24 || timeSlot < 1)
            {
                return "错误的时间段";
            }
            //1、读取当前统计数据
            string selectSql = $"SELECT * FROM statisticstable2 WHERE 序号 ={timeSlot}";
            DataTable dt = GloabalTool.mysql_Insert.ExecSQLQuery(selectSql, null);
            bool totalRes = sqlMessageDic[sqlHead.成品总结果] == "OK";
            bool HeightRes = sqlMessageDic[sqlHead.检测高度差判断总结果] == "OK";
            bool plateRes= sqlMessageDic[sqlHead.铜片平面度判断总结果] == "OK";
            bool expoxyVisionRes = sqlMessageDic[sqlHead.多胶视觉检测结果] == "OK";
            bool pressureVisonRes = sqlMessageDic[sqlHead.铜片压铸视觉检测结果] == "OK";
            statisticsDic[timeSlot].TotalCount = Convert.ToInt32(dt.Rows[0]["总数"]) + 1;
            statisticsDic[timeSlot].TotalNGCount = Convert.ToInt32(dt.Rows[0]["NG数"]) + (!totalRes ? 1 : 0);
            statisticsDic[timeSlot].TotalOKCount = Convert.ToInt32(dt.Rows[0]["OK数"]) + (totalRes ? 1 : 0);
            statisticsDic[timeSlot].HeightNGCount = Convert.ToInt32(dt.Rows[0]["高度差检测NG数"]) + (!HeightRes ? 1 : 0);
            statisticsDic[timeSlot].HeightOKCount = Convert.ToInt32(dt.Rows[0]["高度差检测OK数"]) + (HeightRes ? 1 : 0);
            statisticsDic[timeSlot].PlateNGCount = Convert.ToInt32(dt.Rows[0]["铜片平面度检测NG数"]) + (!plateRes ? 1 : 0);
            statisticsDic[timeSlot].PlateOKCount = Convert.ToInt32(dt.Rows[0]["铜片平面度检测OK数"]) + (plateRes ? 1 : 0);
            statisticsDic[timeSlot].ExcessExpoxyVisionNGCount = Convert.ToInt32(dt.Rows[0]["多胶视觉检NG数"]) + (!expoxyVisionRes ? 1 : 0);
            statisticsDic[timeSlot].ExcessExpoxyVisionOKCount = Convert.ToInt32(dt.Rows[0]["多胶视觉检OK数"]) + (expoxyVisionRes ? 1 : 0);
            statisticsDic[timeSlot].PressureVisionNGCount = Convert.ToInt32(dt.Rows[0]["铜片压铸视觉检NG数"]) + (!pressureVisonRes ? 1 : 0);
            statisticsDic[timeSlot].PressureVisionOKCount = Convert.ToInt32(dt.Rows[0]["铜片压铸视觉检OK数"]) + (pressureVisonRes ? 1 : 0);


            string[] headInfos = new string[12] {"时间段", "总数", "NG数","OK数",
                "高度差检测NG数", "高度差检测OK数","铜片平面度检测NG数","铜片平面度检测OK数", "多胶视觉检NG数","多胶视觉检OK数","铜片压铸视觉检NG数","铜片压铸视觉检OK数"};
            string[] mesglist = new string[12]
            {
                string.Format($"{statisticsDic[timeSlot].Timestamp}"),
                string.Format($"{statisticsDic[timeSlot].TotalCount}"),
                string.Format($"{statisticsDic[timeSlot].TotalNGCount}"),
                string.Format($"{statisticsDic[timeSlot].TotalOKCount}"),
                string.Format($"{statisticsDic[timeSlot].HeightNGCount}"),
                string.Format($"{statisticsDic[timeSlot].HeightOKCount}"),
                string.Format($"{statisticsDic[timeSlot].PlateNGCount}"),
                string.Format($"{statisticsDic[timeSlot].PlateOKCount}"),
                string.Format($"{statisticsDic[timeSlot].ExcessExpoxyVisionNGCount}"),
                string.Format($"{statisticsDic[timeSlot].ExcessExpoxyVisionOKCount}"),
                string.Format($"{statisticsDic[timeSlot].PressureVisionNGCount}"),
                string.Format($"{statisticsDic[timeSlot].PressureVisionOKCount}")
            };
            List<string> updateInfoList = new List<string>();
            for (int i = 0; i < headInfos.Length; i++)
            {
                updateInfoList.Add($"{headInfos[i]}='{mesglist[i]}'");
            }
            string strUpdateMesg = string.Join(",", updateInfoList);
            string UpDateSql1 = $"UPDATE statisticstable2 SET {strUpdateMesg} WHERE 序号 ={timeSlot}";
            int nRet = GloabalTool.mysql_Insert.ExecSQL(UpDateSql1);
            Log.SaveLog(LogType.Data, $"更新数据库语句【{UpDateSql1}】{(nRet > 0 ? "成功" : "失败")}");
            UpdateStatisticsFrm();
            return err;
        }
        //更新界面
        private string UpdateStatisticsFrm()
        {
            var err = string.Empty;
            string selectSql = $"SELECT * FROM statisticstable2";
            DataTable dt = GloabalTool.mysql_Insert.ExecSQLQuery(selectSql, null);
            if (dt.Rows.Count == 24)
            {
                for (int i = 1; i <= 24; i++)
                {
                    statisticsDic[i].TotalCount = Convert.ToInt32(dt.Rows[i - 1]["总数"]);
                    statisticsDic[i].TotalNGCount = Convert.ToInt32(dt.Rows[i - 1]["NG数"]);
                    statisticsDic[i].TotalOKCount = Convert.ToInt32(dt.Rows[i - 1]["OK数"]);
                }
            }
            int timeSlot = GetTimeSlot(DateTime.Now);
            string dateDay = DateTime.Now.ToString("MM/dd");
            string datetimeStr = $"{dateDay}/" + (timeSlot >= 12 ? "08:00-20:00" : "20:00-08:00");
            int totalCount = 0;
            int totalOkCount = 0;
            int totalNgCount = 0;
            if (timeSlot <= 12)
            {
                for (int i = 1; i <= 12; i++)
                {
                    totalCount += statisticsDic[i].TotalCount;
                    totalOkCount += statisticsDic[i].TotalOKCount;
                    totalNgCount += statisticsDic[i].TotalNGCount;
                }
            }
            else
            {
                for (int i = 13; i <= 24; i++)
                {
                    totalCount += statisticsDic[i].TotalCount;
                    totalOkCount += statisticsDic[i].TotalOKCount;
                    totalNgCount += statisticsDic[i].TotalNGCount;
                }

            }
            this.Invoke(new MethodInvoker(() =>
            {
                dgTotal.Rows[0].Cells[0].Value = datetimeStr;
                dgTotal.Rows[0].Cells[1].Value = totalCount;
                dgTotal.Rows[0].Cells[2].Value = totalNgCount;
                dgTotal.Rows[0].Cells[3].Value = totalOkCount;
                bindingSource1.DataSource = dt;
                dgStatistics.DataSource = bindingSource1;

            }));
            return err;
        }


        // 判断时间是否在指定时间段内
        public static int GetTimeSlot(DateTime time)
        {
            int currentHour = time.Hour;
            int currentTimeSlot = 0;
            if (currentHour - 7 > 0)
            {
                currentTimeSlot = currentHour - 7;
            }
            else
            {
                currentTimeSlot = currentHour + 17;
            }
            return currentTimeSlot;
        }


    }
}
