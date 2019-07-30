using WxPay2017.API.DAL.AuthModels;
using WxPay2017.API.VO;
using WxPay2017.API.VO.Param;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WxPay2017.API.BLL.Identity
{
    public class AccountBLL : IDisposable
    {
        private AuthContext _ctx;

        private UserManager<ApplicationUser> _userManager;

        public AccountBLL()
        {
            _ctx = new AuthContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
            _userManager.UserValidator = new UserValidator<ApplicationUser, string>(_userManager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true,
            };
            // 配置用户锁定默认值
            _userManager.UserLockoutEnabledByDefault = true;
            _userManager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            _userManager.MaxFailedAccessAttemptsBeforeLockout = 5;
            _userManager.UserTokenProvider = TokenProvider.Provider;
            //_userManager.to

        }

        public async Task<IdentityResult> RegisterUser(RegisterUserData userModel)
        {
            ApplicationUser user = new ApplicationUser
            {
                ForeignId = userModel.ForeignId,
                IsRightInfo = userModel.IsRightInfo,
                IdentityNo = userModel.IdentityNo,
                EnrollDate = userModel.EnrollDate,
                OrganizationId = userModel.OrganizationId,
                Email = userModel.Email,
                PhoneNumber = userModel.PhoneNumber,
                FullName = userModel.FullName,
                UserName = userModel.UserName,
                StaffNo = userModel.StaffNo

            };

            var result = await _userManager.CreateAsync(user, userModel.Password);
 
            return result;
        }


       
        public async Task<ApplicationUser> FindUser(string userName, string password)
        {
            ApplicationUser user = await _userManager.FindAsync(userName, password);

            return user;
        }


        public async Task<ApplicationUser> FindUserByName(string userName)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(userName);
            return user;
        }

        public ApplicationUser FindUserByUserNameAndIdentityNo(string userName, string identityno)
        {
            ApplicationUser user =  _userManager.Users.FirstOrDefault(x => x.UserName == userName && x.IdentityNo == identityno);
            return user;
        }


        //public async Task<IdentityUser> FindUserById(string userid)
        //{
        //    IdentityUser user = await _userManager.FindByIdAsync(userid);
        //    return user;
        //}

        public async Task<IdentityResult> ChangePassword(UserNode user)
        {
            IdentityResult result = await _userManager.ChangePasswordAsync(user.ID, user.OldPassword, user.Password);
            return result;
        }

        public async Task<string> GeneratePasswordResetToken(string userid)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(userid);
        }

        //public async Task<string>  GenerateTwoFactorToken(string userid)
        //{ 
        //    return await _userManager.GenerateTwoFactorTokenAsync(userid, "");
        //}


        public async Task<IdentityResult> ResetForgottenPassword(ForgotUserNode model)
        {
            var result = await _userManager.ResetPasswordAsync(model.UserId, model.Token, model.Password);
            return result;
        }

        public async Task<IdentityResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return IdentityResult.Failed("验证不通过，或者验证码已失效！");
            }

            var result = await _userManager.ConfirmEmailAsync(userId, code);
            return result;
        }

        //public async Task<IdentityResult> ConfirmPhoneNumber(string userId, string code)
        //{
        //    if (userId == null || code == null)
        //    {
        //        return IdentityResult.Failed("验证不通过，或者验证码已失效！");
        //    }

        //    var result = await _userManager.VerifyTwoFactorTokenAsync(userId, code);
        //    return result;
        //}


        public void Dispose()
        {
            this._ctx.Dispose();
            this._userManager.Dispose();
        }

    }

    public static class TokenProvider
    {
        //[UsedImplicitly]
        private static DataProtectorTokenProvider<ApplicationUser> _tokenProvider;

        public static DataProtectorTokenProvider<ApplicationUser> Provider
        {
            get
            {
                if (_tokenProvider != null) return _tokenProvider;
                var dataProtectionProvider = new DpapiDataProtectionProvider();
                _tokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create());
                return _tokenProvider;
            }
        }
    }
}
