using BranchStringCpmLCs;
using GlobalVariableModuleCs;
using ImageSourceModuleCs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Vison_Inspect_System._2_ComPart;
using Vison_Inspect_System._5_Device.Cam;
using VM.Core;
using VM.PlatformSDKCS;

namespace Vison_Inspect_System._6_Process
{
    /// <summary>
    /// Vison Master视觉算法处理类
    /// </summary>
    public class VisionAlgorithm : IDisposable
    {

        public GlobalVariableModuleCs.GlobalVariableModuleTool GlobalVarTool = new GlobalVariableModuleCs.GlobalVariableModuleTool();
        private Dictionary<CameraType, bool> IsVmRunEnd = new Dictionary<CameraType, bool>();
        /// <summary>
        /// 获取VM processId
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private string GetProcessId(CameraConfig config)
        {
            string processId = string.Empty;
            switch (config.CameraType)
            {
                case CameraType.Camera1:
                    {
                        processId = "Cam1";
                        break;
                    }
                default:
                    break;
            }
            return processId;
        }

        /// <summary>
        /// 获取分支字符调试模式的VisionMaster工具
        /// </summary>
        /// <param name="config"></param>
        /// <param name="branchStringCpmLTool"></param>
        /// <returns></returns>
        public bool GetVisionMasterTool(CameraConfig config, out BranchStringCpmLTool branchStringCpmLTool)
        {
            string processId = GetProcessId(config);

            if (processId == string.Empty)
            {
                branchStringCpmLTool = null;
                return false;
            }
            branchStringCpmLTool = VmSolution.Instance[$"{processId}.Branch String1"] as BranchStringCpmLTool;
            return true;
        }

        /// <summary>
        /// 关闭分支字符调试模式
        /// </summary>
        public void DisableDebugMode(CameraConfig config)
        {
            if (GetVisionMasterTool(config, out BranchStringCpmLTool branchStringCpmLTool))
            {
                List<BranchStringItemParam> itemList = branchStringCpmLTool?.ModuParams.GetBranchStrItemParamList();

                if (itemList != null)
                {
                    foreach (var item in itemList)
                    {
                        item.IsDebugMode = false;
                    }
                }
                Log.SaveLog(LogType.Operate, "关闭VisonMaster里的调试模式");
            }
        }

        /// <summary>
        /// 设置全局参数
        /// </summary>
        /// <param name="ParameterName"></param>
        /// <param name="ParameterValue"></param>
        public void SetGlobalParameter(string ParameterName, string ParameterValue)
        {
            try
            {
                GlobalVarTool.SetGlobalVar(ParameterName, ParameterValue);
            }
            catch
            { }
        }

        /// <summary>
        /// 获取全局参数值
        /// </summary>
        /// <param name="ParameterName"></param>
        /// <param name="ParameterValue"></param>
        public void GetGlobalParameter(string ParameterName, ref string ParameterValue)
        {
            List<GlobalVarInfo> tmp = GlobalVarTool.GetAllGlobalVar();
            try
            {
                ParameterValue = tmp.Find(e => e.strValueName == ParameterName).strValue;
            }
            catch
            { }
        }

        /// <summary>
        /// 加载视觉方案
        /// </summary>
        /// <param name="ProjectPath"></param>
        public string LoadProject(string ProjectPath)
        {
            try
            {
                VmSolution.Instance.CloseSolution();
            }
            catch
            { }
            try
            {
                if (System.IO.File.Exists(ProjectPath) == false)
                {
                    Log.SaveLog(LogType.comm, "视觉方案路径不存在");
                    return "视觉方案路径不存在！！！";
                }
                VmSolution.Load(ProjectPath, "");
                VmSolution.Instance.DisableModulesCallback();
                VmSolution.Instance.SetModuleResultBuffer(false, 5);
                IsVmRunEnd.Clear();
                for (int i = 0; i < Enum.GetNames(typeof(CameraType)).Length; i++)
                    IsVmRunEnd.Add((CameraType)Enum.Parse(typeof(CameraType), Enum.GetNames(typeof(CameraType))[i]), false);
                VmSolution.OnWorkStatusEvent += VmSolution_OnWorkStatusEvent;
                Log.SaveLog(LogType.Operate, "视觉方案载入成功");
                return "";
            }
            catch (Exception ex)
            {
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
                return $"ERR  EXP:{ex}";
            }
        }

        /// <summary>
        /// 获取视觉方案流程执行状态
        /// </summary>
        /// <param name="workStatusInfo"></param>
        private void VmSolution_OnWorkStatusEvent(ImvsSdkDefine.IMVS_MODULE_WORK_STAUS workStatusInfo)
        {
            if (workStatusInfo.nWorkStatus == 0 && workStatusInfo.nProcessID == 10000)
            {
                IsVmRunEnd[CameraType.Camera1] = true;
            }
        }

        /// <summary>
        /// 执行视觉检测
        /// </summary>
        /// <param name="cameraType"></param>
        /// <param name="SavePath"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        public InspectionResult RunInspection(ImageBaseData imageBase, CameraConfig camConfig, int Index = 0)
        {
            DateTime startTime = DateTime.Now;
            InspectionResult tmp = new InspectionResult();
            try
            {
                string processID = "";
                DisableDebugMode(camConfig);
                CameraType cameraType = camConfig.CameraType;

                switch (cameraType)
                {
                    case CameraType.Camera1:
                        {
                            processID = "Cam1";
                            break;
                        }
                    default:
                        {
                            return new InspectionResult();
                        }
                }

                ImageSourceModuleTool imageSourcTool = (ImageSourceModuleTool)VmSolution.Instance[processID + ".图像源1"];
                if (imageSourcTool.ModuParams.ImageSourceType == ImageSourceParam.ImageSourceTypeEnum.LocalImage || imageSourcTool.ModuParams.ImageSourceType == ImageSourceParam.ImageSourceTypeEnum.Camera)
                {
                    imageSourcTool.ModuParams.ImageSourceType = ImageSourceParam.ImageSourceTypeEnum.SDK;
                    Log.SaveLog(LogType.Operate, "将VisonMaster图像由本地模式转换成SDK资源获取");
                }
                VmProcedure procedure = (VmProcedure)VmSolution.Instance[processID];
                DateTime StartTime = DateTime.Now;
                Stopwatch sw = new Stopwatch();
                sw.Restart();

                //在图像源模块中设置图像数据
                imageSourcTool.SetImageData(imageBase);


                if (procedure == null)
                {
                    Log.SaveLog(LogType.comm, string.Format("VisionMaster process refresh exception"));
                }
                try
                {
                    procedure.Run();
                }
                catch (VmException ex)
                {
                    Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
                }
                DateTime StartTime1 = DateTime.Now;
                while (true)
                {
                    if (IsVmRunEnd[cameraType])
                    {
                        IsVmRunEnd[cameraType] = false;
                        break;
                    }
                    if (DateTime.Now.Subtract(StartTime1).TotalSeconds > 5)
                    {
                        IsVmRunEnd[cameraType] = false;
                        break;
                    }
                }

                List<GlobalVarInfo> globalParam = GlobalVarTool.GetAllGlobalVar();
                switch (cameraType)
                {
                    case CameraType.Camera1:
                        {
                            string runStatus = "0";
                            string pin1Dis1 = "0";
                            string pin1Dis1Res = "0";
                            string pin1Dis2 = "0";
                            string pin1Dis2Res = "0";
                            string pin2Dis1 = "0";
                            string pin2Dis1Res = "0";
                            string pin2Dis2 = "0";
                            string pin2Dis2Res = "0";
                            string pin1Pin2Dis = "0";
                            string pin1Pin2DisRes = "0";
                            GetGlobalParameter("TotalRes", ref runStatus);
                            GetGlobalParameter("Pin1Dis1", ref pin1Dis1);
                            GetGlobalParameter("Pin1Dis1Res", ref pin1Dis1Res);
                            GetGlobalParameter("Pin1Dis2", ref pin1Dis2);
                            GetGlobalParameter("Pin1Dis2Res", ref pin1Dis2Res);
                            GetGlobalParameter("Pin2Dis1", ref pin2Dis1);
                            GetGlobalParameter("Pin2Dis1Res", ref pin2Dis1Res);
                            GetGlobalParameter("Pin2Dis2", ref pin2Dis2);
                            GetGlobalParameter("Pin2Dis2Res", ref pin2Dis2Res);
                            GetGlobalParameter("Pin1Pin2Dis", ref pin1Pin2Dis);
                            GetGlobalParameter("Pin1Pin2DisRes", ref pin1Pin2DisRes);
                            tmp.InspectTotalStauts = runStatus == "0" ? false : true;
                            tmp.Pin1Dis1 =Convert.ToDouble(pin1Dis1);
                            tmp.Pin1Dis1Res = pin1Dis1Res == "0" ? false : true;
                            tmp.Pin1Dis2 = Convert.ToDouble(pin1Dis2);
                            tmp.Pin1Dis2Res = pin1Dis2Res == "0" ? false : true;
                            tmp.Pin2Dis1 = Convert.ToDouble(pin2Dis1);
                            tmp.Pin2Dis1Res = pin2Dis1Res == "0" ? false : true;
                            tmp.Pin2Dis2 = Convert.ToDouble(pin2Dis2);
                            tmp.Pin2Dis2Res = pin2Dis2Res == "0" ? false : true;
                            tmp.Pin1Pin2Dis = Convert.ToDouble(pin1Pin2Dis);
                            tmp.Pin1Pin2DisRes = pin1Pin2DisRes == "0" ? false : true;
                            break;
                        }
                    default:
                        {
                            return new InspectionResult();
                        }
                }
                Log.SaveLog(LogType.Operate, $"算法执行花费了【{DateTime.Now.Subtract(startTime).TotalMilliseconds.ToString("0.00")}ms】");
            }
            catch (Exception ex)
            {
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
            }
            return tmp;
        }

        /// <summary>
        /// 检测结果类
        /// </summary>
        public class InspectionResult
        {
            /// <summary>
            /// 视觉检测总结果
            /// </summary>
            public bool InspectTotalStauts = false;

            /// <summary>
            /// Pin针1距离检测1
            /// </summary>
            public double Pin1Dis1 = 0.000f;
            /// <summary>
            /// Pin针1距离检测1结果
            /// </summary>
            public bool Pin1Dis1Res = false;

            /// <summary>
            /// Pin针1距离检测2
            /// </summary>
            public double Pin1Dis2 = 0.000f;
            /// <summary>
            /// Pin针1距离检测2结果
            /// </summary>
            public bool Pin1Dis2Res = false;

            /// <summary>
            /// Pin针2距离检测1
            /// </summary>
            public double Pin2Dis1 = 0.000f;
            /// <summary>
            /// Pin针2距离检测1结果
            /// </summary>
            public bool Pin2Dis1Res = false;

            /// <summary>
            /// Pin针2距离检测2
            /// </summary>
            public double Pin2Dis2 = 0.000f;
            /// <summary>
            /// Pin针2距离检测2结果
            /// </summary>
            public bool Pin2Dis2Res = false;
            /// <summary>
            /// Pin针1和Pin针2之间的距离
            /// </summary>
            public double Pin1Pin2Dis = 0.000f;
            /// <summary>
            /// Pin针1和Pin针2之间的距离结果
            /// </summary>
            public bool Pin1Pin2DisRes = false;
            /// <summary>
            /// 检测耗时
            /// </summary>
            public string CostTime { set; get; }

        }

        public void Dispose()
        {
            try
            {
                VmSolution.Instance?.Dispose();
            }
            catch
            { }
        }
    }
}
