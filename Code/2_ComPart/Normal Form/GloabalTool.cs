using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Vison_Inspect_System._6_Process;

namespace Vison_Inspect_System._2_ComPart.Normal_Form
{
    internal class GloabalTool
    {
        public static string MainDir = @"C:\LXW\";
        public static string ConfigDir = @"C:\LXW\Config\";
        public static string ProductionCofigDir = @"C:\LXW\Recipe\";
        public static string Path_Equipment_Setting = @"C:\LXW\Config\EquipmentSetting.xml";
        public static string ProductFullName = String.Empty;
        public static EquipmentSettings equipmentSettings = null;
        public static RecipeConfig ProductIni = null;
        public static MYSQL mysql_Insert = null;
        public static MYSQL mysql_Select = null;
        public static string MainTitle = "";
        public static string loginUser = "WQ";
        public static string loginLevel = "开发者";

        public static VisionAlgorithm VisionAlgorithm = new VisionAlgorithm();

        public static Dictionary<CameraType, CameraImage> CamDic = new Dictionary<CameraType, CameraImage>();

        #region xml文档处理类

        /// <summary>
        /// 将对应的Class的类保存为xml文档
        /// </summary>
        /// <param name="obj">类转换为object</param>
        /// <param name="path">要保存的参数路径xml</param>
        /// <param name="MySerializType">数据格式化类型，默认不需要该参数</param>
        //    [ System.Runtime.CompilerServices.Extension]
        public static bool TryToSave<T>(T obj, string path, SerializationFormat MySerializType = System.Data.SerializationFormat.Xml)
        {

            FileInfo SaveFile = new FileInfo(path);
            if (SaveFile.Directory.Exists == false)
            {
                try
                {
                    SaveFile.Directory.Create();
                }
                catch (Exception ex)
                {
                    Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
                    return false;
                }
            }
            else
            {
                try
                {
                    if (SaveFile.Exists)
                    {
                        SaveFile.Delete();
                    }
                }
                catch (Exception ex)
                {
                    Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
                    return false;
                }
            }
            FileStream myFs = new FileStream(SaveFile.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            try
            {
                switch (MySerializType)
                {
                    case SerializationFormat.Xml:
                        XmlSerializer myXS = new XmlSerializer(typeof(T));
                        myXS.Serialize(myFs, obj);
                        break;
                    case SerializationFormat.Binary:
                        BinaryFormatter myBny = new BinaryFormatter();
                        myBny.Serialize(myFs, obj);
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
                myFs.Close();
                return false;
            }
            myFs.Close();
            return true;
        }

        /// <summary>
        /// 从对应的xml文档中将保存的类提取出来
        /// </summary>
        /// <param name="TestObj">读取到对应的类中</param>
        /// <param name="Path">xml的参数路径</param>
        /// <param name="MySerializType">数据格式化类型，默认不需要该参数</param>
        /// <returns>是否成功</returns>
        public static bool ToTryLoad<T>(ref T TestObj, string Path, SerializationFormat MySerializType = SerializationFormat.Xml)
        {
            if (Path == null) return false;
            if (Path == string.Empty) return false;
            FileInfo LoadFile = new FileInfo(Path);
            if (LoadFile.Exists == false)
                return false;
            // SerializationFormat          
            FileStream myReader = new FileStream(LoadFile.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            try
            {
                //Dim objStreamReader As New StreamReader(Path)
                switch (MySerializType)
                {
                    case SerializationFormat.Xml:
                        //Deserialize text file to a new object.
                        XmlSerializer x = new XmlSerializer(typeof(T));
                        TestObj = (T)x.Deserialize(myReader);
                        break;
                    case SerializationFormat.Binary:
                        BinaryFormatter myBny = new BinaryFormatter();
                        TestObj = (T)myBny.Deserialize(myReader);
                        break;
                }
                myReader.Close();
            }
            catch (Exception ex)
            {
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
                myReader.Close();
                return false;
            }
            myReader.Close();
            return true;
        }
        #endregion
        public static void LoadEquipmentConfig()
        {
            try
            {
                if (ToTryLoad<EquipmentSettings>(ref equipmentSettings, Path_Equipment_Setting) == false)
                {
                    STD_IFrameWork.异常信息.Instance.ShowMsgInfor("EquipmentSetting的XML文件没有发现，准备新的文件");
                    STD_IFrameWork.异常信息.Instance.ShowPannel("OK");
                    equipmentSettings = new EquipmentSettings();
                    TryToSave<EquipmentSettings>(equipmentSettings, Path_Equipment_Setting);
                }
            }
            catch (Exception ex)
            {
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
            }
        }
        public static void EquipmentConfigLoad()
        {
            try
            {
                mysql_Insert = new MYSQL(equipmentSettings.DataBaseIP, equipmentSettings.DataBaseUser, equipmentSettings.DataPassWord, equipmentSettings.DataBaseTable, equipmentSettings.DataBasePort);
                mysql_Select = new MYSQL(equipmentSettings.DataBaseIP, equipmentSettings.DataBaseUser, equipmentSettings.DataPassWord, equipmentSettings.DataBaseTable, equipmentSettings.DataBasePort);
            }
            catch (Exception ex)
            {
                Log.SaveError(new StackTrace(new StackFrame(true)), new StackFrame(), ex);
            }
        }
        public static void EquipmentConfigRefresh()
        {

            MainTitle = equipmentSettings.MainTitle;

        }
        public static string GetAsscallToString(short[] DaContent)
        {
            List<byte> databytelist = new List<byte>();
            for (int i = 0; i < DaContent.Length; i++)
            {
                byte byte0 = (byte)(DaContent[i] & 255);
                byte byte1 = (byte)(DaContent[i] >> 8);
                databytelist.Add(byte0);
                databytelist.Add(byte1);
            }
            var result = System.Text.Encoding.ASCII.GetString(databytelist.ToArray());
            return result;
        }


        /// <summary>
        /// 加载配方
        /// </summary>
        /// <param name="recipePath">配方路径</param>
        /// <returns></returns>
        public static (bool, RecipeConfig) LoadRecipe(string recipePath)
        {
            RecipeConfig recipePara = null;
            string filePath = "";
            if (recipePath.Contains(".json"))
            {
                filePath = recipePath;
            }
            else
            {
                filePath = recipePath + "\\.json";
            }
            //加载配方
            string jsonString = File.ReadAllText(filePath);
            //反序列化
            recipePara = JsonConvert.DeserializeObject<RecipeConfig>(jsonString);
            if (recipePara != null)
            {
                return (true, recipePara);
            }
            else
            {
                return (false, recipePara);
            }
        }

        /// <summary>
        /// 保存配方
        /// </summary>
        /// <param name="recipePath">配方路径</param>
        /// <returns></returns>
        public static bool SaveRecipe(string recipePath, RecipeConfig recipePara)
        {
            //序列化
            string jsonString = JsonConvert.SerializeObject(recipePara);
            string filePath = "";
            if (recipePath.Contains(".json"))
            {
                filePath = recipePath;
            }
            else
            {
                filePath = recipePath + "\\.json";
            }
            //保存配方
            File.WriteAllText(filePath, jsonString);
            if (File.Exists(filePath) || !string.IsNullOrEmpty(jsonString))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
