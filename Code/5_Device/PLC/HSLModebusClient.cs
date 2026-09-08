using HslCommunication;
using HslCommunication.ModBus;
using System;
using System.Diagnostics;
using System.Linq;
using Vison_Inspect_System._2_ComPart;

namespace Vison_Inspect_System._5_Device.PLC
{
    public class HSLModebusClient
    {
        private ModbusTcpNet _modbusTcpClient;
        /// <summary>
        /// ModbusTcp客户端对象
        /// </summary>
        public ModbusTcpNet ModbusTcpClient
        {
            get { return _modbusTcpClient; }
            set { _modbusTcpClient = value; }
        }

        private bool _isConnect;
        /// <summary>
        /// 是否连接
        /// </summary>
        public bool IsConnect
        {
            get { return _isConnect; }
            set { _isConnect = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="modbusTcpNet"></param>
        public HSLModebusClient(ModbusTcpNet modbusTcpNet)
        {
            _modbusTcpClient = modbusTcpNet;
        }

        /// <summary>
        /// 连接PLC
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="Port"></param>
        /// <param name="StationNumber"></param>
        /// <returns></returns>
        public string Connect(string IP, int Port, int StationNumber)
        {
            try
            {
                _modbusTcpClient.IpAddress = IP;
                _modbusTcpClient.Port = Port;
                _modbusTcpClient.Station = (byte)StationNumber;
                _modbusTcpClient.ConnectTimeOut = 3000;
                _modbusTcpClient.DataFormat = HslCommunication.Core.DataFormat.CDAB;
                OperateResult con = _modbusTcpClient.ConnectServer();
                this.IsConnect = con.IsSuccess;
                Log.SaveLog(LogType.Operate, $"ModbusTCP 连接{(con.IsSuccess ? "成功" : "失败")}");
                return "";
            }
            catch (Exception ex)
            {
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
                return ex.Message;
            }
        }

        /// <summary>
        /// PLC断开连接
        /// </summary>
        /// <returns></returns>
        public string Disconnect()
        {
            try
            {
                _modbusTcpClient.ConnectClose();
                Log.SaveLog(LogType.Operate, "ModbusTCP 断开连接成功");
                return "";
            }
            catch (Exception ex)
            {
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
                return ex.Message;
            }
        }

        /// <summary>
        /// 读取PLC数据
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value">布尔类型值</param>
        /// <returns></returns>
        public bool ReadValue(string address, out bool value)
        {
            try
            {
                OperateResult<bool> result = _modbusTcpClient.ReadBool(address);
                if (result.IsSuccess)
                {
                    value = result.Content;
                    Log.SaveLog(LogType.Operate, $"Write adress{address} value {value}");
                    return true;
                }
                else
                {
                    Log.SaveLog(LogType.comm, result.Message);
                    value = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
                value = false;
                return false;
            }
        }

        /// <summary>
        /// 读取PLC数据
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value">short类型值</param>
        /// <returns></returns>
        public bool ReadValue(string address, out short value)
        {
            try
            {
                OperateResult<short> result = _modbusTcpClient.ReadInt16(address);
                if (result.IsSuccess)
                {
                    value = result.Content;
                    if (value != 0)
                    {
                        Log.SaveLog(LogType.Operate, $"Read adress{address} value {value}");
                    }
                    return true;
                }
                else
                {
                    value = 0;
                    Log.SaveLog(LogType.comm, result.Message);
                    return false;
                }
            }
            catch (Exception ex)
            {
                value = 0;
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
                return false;
            }
        }

        /// <summary>
        /// 读取PLC数据
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value">int类型值</param>
        /// <returns></returns>
        public bool ReadValue(string address, out int value)
        {
            try
            {
                OperateResult<int> result = _modbusTcpClient.ReadInt32(address);
                if (result.IsSuccess)
                {
                    value = result.Content;
                    if (value != 0)
                    {
                        Log.SaveLog(LogType.Operate, $"Read adress{address} value {value}");
                    }
                    return true;
                }
                else
                {
                    value = 0;
                    Log.SaveLog(LogType.comm, result.Message);
                    return false;
                }
            }
            catch (Exception ex)
            {
                value = 0;
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
                return false;
            }
        }

        /// <summary>
        /// 读取PLC数据
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value">float类型值</param>
        /// <returns></returns>
        public bool ReadValue(string address, out float value)
        {
            try
            {
                OperateResult<float> result = _modbusTcpClient.ReadFloat(address);
                if (result.IsSuccess)
                {
                    value = result.Content;
                    if (value != 0)
                    {
                        Log.SaveLog(LogType.Operate, $"Read adress{address} value {value}");
                    }
                    return true;
                }
                else
                {
                    value = 0;
                    Log.SaveLog(LogType.comm, result.Message);
                    return false;
                }
            }
            catch (Exception ex)
            {
                value = 0;
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
                return false;
            }
        }


        /// <summary>
        /// 读取PLC数据数组
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public bool ReadValue(string address, out short[] value, ushort length)
        {
            try
            {
                OperateResult<short[]> result = _modbusTcpClient.ReadInt16(address, length);
                if (result.IsSuccess)
                {
                    value = result.Content;
                    if (value.Count() != 0)
                    {
                        Log.SaveLog(LogType.Operate, $"Read adress{address} value {string.Join("_", value)}");
                    }
                    return true;
                }
                else
                {
                    value = null;
                    Log.SaveLog(LogType.comm, result.Message);
                    return false;
                }
            }
            catch (Exception ex)
            {
                value = null;
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
                return false;
            }
        }

        /// <summary>
        /// 读取PLC数据数组
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public bool ReadValue(string address, out float[] value, ushort length)
        {
            try
            {
                OperateResult<float[]> result = _modbusTcpClient.ReadFloat(address, length);
                if (result.IsSuccess)
                {
                    value = result.Content;
                    if (value.Count() != 0)
                    {
                        Log.SaveLog(LogType.Operate, $"Read adress{address} value {string.Join("_", value)}");
                    }
                    return true;
                }
                else
                {
                    value = null;
                    Log.SaveLog(LogType.comm, result.Message);
                    return false;
                }
            }
            catch (Exception ex)
            {
                value = null;
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
                return false;
            }
        }

        /// <summary>
        /// 读取PLC数据数组
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public bool ReadValue(string address, out double[] value, ushort length)
        {
            try
            {
                OperateResult<double[]> result = _modbusTcpClient.ReadDouble(address, length);
                if (result.IsSuccess)
                {
                    value = result.Content;
                    if (value.Count() != 0)
                    {
                        Log.SaveLog(LogType.Operate, $"Read adress{address} value {string.Join("_", value)}");
                    }
                    return true;
                }
                else
                {
                    value = null;
                    Log.SaveLog(LogType.comm, result.Message);
                    return false;
                }
            }
            catch (Exception ex)
            {
                value = null;
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
                return false;
            }
        }

        /// <summary>
        /// 读取PLC数据
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value">double类型值</param>
        /// <returns></returns>
        public bool ReadValue(string address, out double value)
        {
            try
            {
                OperateResult<double> result = _modbusTcpClient.ReadDouble(address);
                if (result.IsSuccess)
                {
                    value = result.Content;
                    if (value != 0)
                    {
                        Log.SaveLog(LogType.Operate, $"Read adress{address} value {value}");
                    }
                    return true;
                }
                else
                {
                    value = 0;
                    Log.SaveLog(LogType.comm, result.Message);
                    return false;
                }
            }
            catch (Exception ex)
            {
                value = 0;
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
                return false;
            }
        }

        /// <summary>
        /// 写入PLC数据
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value">布尔值</param>
        /// <returns></returns>
        public bool WriteValue(string address, bool value)
        {
            try
            {
                OperateResult result = _modbusTcpClient.Write(address, value);
                if (result.IsSuccess)
                {
                    Log.SaveLog(LogType.Operate, $"Write adress{address} value {value}");
                    return true;
                }
                else
                {
                    Log.SaveLog(LogType.comm, result.Message);
                    return false;
                }

            }
            catch (Exception ex)
            {
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
                return false;
            }
        }

        /// <summary>
        /// 写入PLC数据
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value">short类型值</param>
        /// <returns></returns>
        public bool WriteValue(string address, short value)
        {
            try
            {
                OperateResult result = _modbusTcpClient.Write(address, value);
                if (result.IsSuccess)
                {
                    Log.SaveLog(LogType.Operate, $"Write adress{address} value {value}");
                    return true;
                }
                else
                {
                    Log.SaveLog(LogType.comm, result.Message);
                    return false;
                }

            }
            catch (Exception ex)
            {
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
                return false;
            }
        }

        /// <summary>
        /// 用于写入心跳
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value">short类型值</param>
        /// <returns></returns>
        public bool WriteHeartValue(string address, short value)
        {
            try
            {
                OperateResult result = _modbusTcpClient.Write(address, value);
                if (result.IsSuccess)
                {
                    return true;
                }
                else
                {
                    Log.SaveLog(LogType.comm, result.Message);
                    return false;
                }

            }
            catch (Exception ex)
            {
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
                return false;
            }
        }

        /// <summary>
        /// 写入PLC数据
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value">int类型值</param>
        /// <returns></returns>
        public bool WriteValue(string address, int value)
        {
            try
            {
                OperateResult result = _modbusTcpClient.Write(address, value);
                if (result.IsSuccess)
                {
                    Log.SaveLog(LogType.Operate, $"Write adress{address} value {value}");
                    return true;
                }
                else
                {
                    Log.SaveLog(LogType.comm, result.Message);
                    return false;
                }

            }
            catch (Exception ex)
            {
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
                return false;
            }
        }

        /// <summary>
        /// 写入PLC数据
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value">float类型值</param>
        /// <returns></returns>
        public bool WriteValue(string address, float value)
        {
            try
            {
                OperateResult result = _modbusTcpClient.Write(address, value);
                if (result.IsSuccess)
                {
                    Log.SaveLog(LogType.Operate, $"Write adress{address} value {value}");
                    return true;
                }
                else
                {
                    Log.SaveLog(LogType.comm, result.Message);
                    return false;
                }

            }
            catch (Exception ex)
            {
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
                return false;
            }
        }
        /// <summary>
        /// 写入PLC数据
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value">double类型值</param>
        /// <returns></returns>
        public bool WriteValue(string address, double value)
        {
            try
            {
                OperateResult result = _modbusTcpClient.Write(address, value);
                if (result.IsSuccess)
                {
                    Log.SaveLog(LogType.Operate, $"Write adress{address} value {value}");
                    return true;
                }
                else
                {
                    Log.SaveLog(LogType.comm, result.Message);
                    return false;
                }

            }
            catch (Exception ex)
            {
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
                return false;
            }
        }
    }
}
