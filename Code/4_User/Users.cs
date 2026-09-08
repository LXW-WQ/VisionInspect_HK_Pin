using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Vison_Inspect_System
{

    public enum Authroity//枚举权限级别
    {
        Operator,
        Administrator,
        Engineer,
        Null
    }
    [Serializable]
    public class Users
    {
        public int SN { get; set; }//序号
        public string Username { get; set; }//用户名称 
        public string Password { get; set; }//用户密码  
        public Authroity Level { get; set; }//权限级别
    }
    public class userHelper
    {
        /// <summary>
        /// 用户信息序列化
        /// </summary>
        /// <param name="filepath">路径</param>
        /// <param name="userlist">用户信息集合</param>
        /// <returns>成功：True；失败：False</returns>
        public bool SerializeUser(string filepath, List<Users> userlist)
        {
            if (userlist == null || filepath == null)
            {
                return false;
            }
            BinaryFormatter format = new BinaryFormatter();
            using (FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write))
            {
                format.Serialize(fs, userlist);
                return true;
            }
        }
        /// <summary>
        /// 反序列化用户信息文件
        /// </summary>
        /// <param name="filepath">用户信息序列化文件存放路径</param>
        /// <returns>用户信息</returns>
        public List<Users> DeSerializeUser(string filepath)
        {
            try
            {
                BinaryFormatter deformater = new BinaryFormatter();
                using (FileStream defs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                {
                    object o = deformater.Deserialize(defs);
                    return o as List<Users>;
                }
            }
            catch (Exception )
            {
                return null;
            }
        }
        /// <summary>
        /// 检查用户重复性
        /// </summary>
        /// <param name="userlist">用户信息集合</param>
        /// <param name="username">输入的用户名称</param>
        /// <returns></returns>
        public bool CheckContainUser(List<Users> userlist, string username)
        {
            var user = from item in userlist
                       where item.Username == username
                       select item;
            if (user.Count() > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 创建超级用户
        /// </summary>
        /// <param name="path">用户信息序列化路径</param>
        /// <param name="listuser">用户信息集合</param>
        public void CheckSupperUser(string path, List<Users> userlist)
        {
            if (!File.Exists(path))
            {
                Users user = new Users()
                { SN = 0, Username = "WQ", Password = "123", Level = Authroity.Administrator };
                userlist.Add(user);
                SerializeUser(path, userlist);
            }
        }
        /// <summary>
        /// 检查用户登陆
        /// </summary>
        /// <param name="path">用户信息序列化文件路径</param>
        /// <param name="username">用户输入的名称</param>
        /// <param name="password">用户输入的密码</param>
        /// <returns></returns>
        public Users CheckUserLogin(string path, string username, string password)
        {
            if (!File.Exists(path)) return null;
            List<Users> userlist = DeSerializeUser(path);
            var res = from item in userlist where item.Username == username & item.Password == password select item;
            if (res.Count() == 0) return null;
            Users user = new Users();
            foreach (var item in res)
            {

                user.Username = item.Username;
                user.Password = item.Password;
                user.Level = item.Level;
            }
            return user;
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="path"></param>
        /// <param name="listuser"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Adduser(string path, List<Users> listuser, Users user)
        {
            if (user == null) return false;
            if (CheckContainUser(listuser, user.Username)) return false;
            listuser.Add(user);
            SerializeUser(path, listuser);
            return true;
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="path"></param>
        /// <param name="listuser"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool DeleteUser(string path, List<Users> listuser, string username)
        {
            if (listuser == null) return false;
            int index = 0;
            foreach (var item in listuser)
            {
                if (item.Username == username)
                {
                    break;
                }
                index++;
            }
            if (index == 0) return false;
            listuser.RemoveAt(index);
            SerializeUser(path, listuser);
            return true;
        }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="path"></param>
        /// <param name="listuser"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool EditUser(string path, List<Users> listuser, Users user)
        {
            if (listuser == null) return false;
            foreach (var item in listuser)
            {
                if (item.Username == user.Username)
                {
                    item.Password = user.Password;
                    item.Level = user.Level;
                    SerializeUser(path, listuser);
                    return true;
                }
            }
            return false;
        }
    }

}
