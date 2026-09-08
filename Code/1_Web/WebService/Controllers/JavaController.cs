using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Vison_Inspect_System._1_Web.WebService.Models;

namespace Vison_Inspect_System._1_Web.WebService.Controllers
{
    [RoutePrefix("api/Java")]
    public class JavaController : ApiController
    {
        #region [Post]登录用户获得通讯密钥
        [HttpPost, Route("PostLogin")]//Http://localhost:5000/api/Java/PostLogin
        public IHttpActionResult PostLogin([FromBody] dynamic ObJsonStr)
        {
            ApiResponseInfo apiResponseInfo = new ApiResponseInfo();
            try
            {
                dynamic DeSerialObJsonStr = JsonConvert.DeserializeObject(Convert.ToString(ObJsonStr));
                if (DeSerialObJsonStr.username == "Ronstein" & DeSerialObJsonStr.password == "software123")
                {
                    List<object> Token = new List<object>();
                    apiResponseInfo.IsSuccess = true;
                    apiResponseInfo.Code = 200;
                    apiResponseInfo.Message = "用户登录成功";
                    string PubKey = string.Format("username:{0},password:{1},CreateTime:{2},WangQian", DeSerialObJsonStr.username, DeSerialObJsonStr.password, DateTime.Now.ToString("yy-MM-dd HH:mm:ss"));
                    string Key = "Token:" + DesEncryption.EncryptString(PubKey, "Ronstein");
                    Token.Add(Key);
                    apiResponseInfo.Content = Token;
                }
                else
                {
                    apiResponseInfo.IsSuccess = false;
                    apiResponseInfo.Code = 610;
                    apiResponseInfo.Message = "用户名与密码不正确，登录失败";
                }
            }
            catch (Exception ex)
            {
                apiResponseInfo.IsSuccess = false;
                apiResponseInfo.Code = 500;
                apiResponseInfo.Message = "无用户登录信息，登录线程异常：异常信息：user:和password：的" + ex.Message;
            }
            return Json(apiResponseInfo);
        } 
        #endregion

        #region [get]获取PC时钟接口
        [HttpGet, Route("GetPCDateTime")]//Http://localhost:5000/api/Java/GetPCDateTime
        public IHttpActionResult GetPCDateTime()
        {
            ApiResponseInfo apiResponseInfo = new ApiResponseInfo();
            try
            {
                if (CheckToken(ref apiResponseInfo))
                {
                    List<object> datetime = new List<object>();
                    apiResponseInfo.IsSuccess = true;
                    apiResponseInfo.Code = 200;
                    apiResponseInfo.Message = "获取PC时间成功";
                    datetime.Add(DateTime.Now.ToString("yy-MM-dd HH:mm:ss"));
                    apiResponseInfo.Content = datetime;
                }
            }
            catch (Exception ex)
            {
                apiResponseInfo.IsSuccess = false;
                apiResponseInfo.Code = 500;
                apiResponseInfo.Message = "获取PC时间异常，异常信息：" + ex.Message;
            }
            return Json(apiResponseInfo);
        }
        #endregion






        #region 检查密钥方法
        /// <summary>
        /// 检查密钥
        /// </summary>
        /// <param name="apiResponseInfo">信息响应对象</param>
        /// <returns>密钥是否解读成功</returns>
        public bool CheckToken(ref ApiResponseInfo apiResponseInfo)
        {
            if (ActionContext.Request.Headers.Contains("Token"))
            {
                if (string.IsNullOrEmpty(ActionContext.Request.Headers.GetValues("Token").First()))
                {
                    apiResponseInfo.IsSuccess = false;
                    apiResponseInfo.Code = 401;
                    apiResponseInfo.Message = "秘钥为Null，授权不通过";
                    return false;
                }
            }
            else
            {
                apiResponseInfo.IsSuccess = false;
                apiResponseInfo.Code = 401;
                apiResponseInfo.Message = "未携带秘钥，授权不通过";
                return false;
            }
            string DesToken = DesEncryption.DecryptString(ActionContext.Request.Headers.GetValues("Token").First(), "Ronstein");
            if (string.IsNullOrEmpty(DesToken))
            {
                apiResponseInfo.IsSuccess = false;
                apiResponseInfo.Code = 401;
                apiResponseInfo.Message = "秘钥无效，授权不通过";
                return false;
            }
            return true;
        }
        #endregion
    }
}
