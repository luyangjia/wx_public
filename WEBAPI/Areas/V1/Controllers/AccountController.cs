using WxPay2017.API.BLL;
using WxPay2017.API.BLL.Identity;
using WxPay2017.API.DAL.AuthModels;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO;
using WxPay2017.API.VO.Param;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.IO;
namespace WxPay2017.API.WEBAPI.Areas.V1.Controllers
{
    /// <summary>
    /// 账户
    /// </summary>
    [Authorize]
    [RoutePrefix("api")]
    public class AccountController : ApiController
    {
        private AccountBLL _accout = null;
      
        public AccountController()
        {
            _accout = new AccountBLL();
            
        }

        // POST api/Account/Register
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="userModel">用户数据对象</param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("account/register")]
        public async Task<IHttpActionResult> PostRegister(RegisterUserData userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await _accout.RegisterUser(userModel);

            //IHttpActionResult errorResult = GetErrorResult(result);

            //if (errorResult != null)
            //{
            //    return errorResult;
            //}

            return Ok(result);
        }
   
       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _accout.Dispose();
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

     
 
        /// <summary>
        /// 获得有效的uKey秘钥
        /// </summary>
        /// <param name="code">获取秘钥</param>
        /// <returns>密钥</returns>
        [HttpGet]
        [Route("account/GetUkeyCode/{code}")]
        public IHttpActionResult GetUkeyCode(string code)
        {
            string key="";
            if (code == "fjnewcap!123")
            {
                Random random=new Random();
                key = DateTime.Now.ToString() + "!" + (random.Next(89999) + 10000) + "!fjnewcap";
                Encrypt encrypt=new Encrypt();
                key = encrypt.Encrypto(key);
               
               
            }
            return Ok(key);
        }

         /// <summary>
        /// 验证有效的uKey秘钥
        /// </summary>
        /// <param name="code">验证码</param>
        /// <returns>提示信息(Ok、Error)</returns>
        [HttpGet]
        [Route("account/IsNeedPay")]
        public IHttpActionResult IsNeedPay()
        {
            if (BLL.MyConsole.GetAppString("IsNeedPay") == "true")
            {
                return Ok("true");
            }
            else
                return BadRequest("false");
        }
        /// <summary>
        /// 验证有效的uKey秘钥
        /// </summary>
        /// <param name="code">验证码</param>
        /// <returns>提示信息(Ok、Error)</returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("account/CheckUkeyCode")]
        public IHttpActionResult CheckUkeyCode()
        {
            if (BLL.MyConsole.GetAppString("UkeyIsCheck") == "true")
            {
                var path = BLL.MyConsole.GetAppString("UkeyPath");
                string code;
                
                Encrypt encrypt = new Encrypt();
                try
                {
                    StreamReader reader = new StreamReader(path);
                    code = reader.ReadLine();
                    reader.Close();
                    reader.Dispose();
                    var key = encrypt.Decrypto(code);
                    if (key.EndsWith("!fjnewcap"))
                        return Ok("Ok");
                }
                catch
                {
                }
                return BadRequest("Error");
            }
            else
            {
                return Ok("Ok");
            }
            
        }
    }
}
