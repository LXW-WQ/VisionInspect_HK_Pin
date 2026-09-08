using System.Collections.Generic;
using System.ComponentModel;
using Vison_Inspect_System._5_Device.Cam;

namespace Vison_Inspect_System._2_ComPart
{
    public class EquipmentSettings
    {
        private string _PLCIP = "127.0.0.1";
        [Category("01.PLC通讯配置")]
        [DisplayName("PLC的IP地址")]
        public string PLCIP
        {
            get { return _PLCIP; }
            set { _PLCIP = value; }
        }

        private int _Port = 502;
        [Category("01.PLC通讯配置")]
        [DisplayName("PLC通讯端口")]
        public int Port
        {
            get { return _Port; }
            set { _Port = value; }
        }
        private short _ID = 1;
        [Category("01.PLC通讯配置")]
        [DisplayName("PLC的站号")]
        public short ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _MainTitle = "轴 位 置 度 视 觉 采 集 系 统";
        [Category("02.主界面显示参数配置")]
        [DisplayName("界面标题配置")]
        public string MainTitle
        {
            get { return _MainTitle; }
            set { _MainTitle = value; }
        }
        private string _DataBaseIP = "localhost";
        [Category("03.数据库参数配置")]
        [DisplayName("服务器地址")]
        public string DataBaseIP
        {
            get { return _DataBaseIP; }
            set { _DataBaseIP = value; }
        }

        private string _DataBaseUser = "root";
        [Category("03.数据库参数配置")]
        [DisplayName("数据库用户名")]
        public string DataBaseUser
        {
            get { return _DataBaseUser; }
            set { _DataBaseUser = value; }
        }
        private string _DataPassWord = "lxw201314wq";
        [Category("03.数据库参数配置")]
        [DisplayName("数据库密码")]
        public string DataPassWord
        {
            get { return _DataPassWord; }
            set { _DataPassWord = value; }
        }
        private int _DataBasePort = 3306;
        [Category("03.数据库参数配置")]
        [DisplayName("数据库端口")]
        public int DataBasePort
        {
            get { return _DataBasePort; }
            set { _DataBasePort = value; }
        }
        private string _DataBaseTable = "datacollect_db";
        [Category("03.数据库参数配置")]
        [DisplayName("数据表名称")]
        public string DataBaseTable
        {
            get { return _DataBaseTable; }
            set { _DataBaseTable = value; }
        }

        private List<CameraConfig> _camConfigList = new List<CameraConfig>();
        [Category("04.相机设置")]
        [Browsable(true)]
        [DisplayName("相机相关参数")]
        public List<CameraConfig> CamConfigList
        {
            get { return _camConfigList; }
            set { _camConfigList = value; }
        }


    }
}
