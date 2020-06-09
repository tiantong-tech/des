using Microsoft.AspNetCore.Mvc;
using Renet.Web;
using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Tiantong.Iot.Api
{
  public class SystemController: BaseController
  {
    private Mail _mail;

    private SystemRepository _systemRepository;

    public SystemController(
      Mail mail,
      SystemRepository systemRepository
    ) {
      _mail = mail;
      _systemRepository = systemRepository;
    }

    [HttpPost]
    [Route("system-password/has")]
    public bool HasPassword()
    {
      return _systemRepository.HasPassword();
    }

    public class SetPasswordParams
    {
      public string password { get; set; }

      public string password_confirmation { get; set; }
    }

    [HttpPost]
    [Route("system-password/set")]
    public object SetPassword([FromBody] SetPasswordParams param)
    {
      _systemRepository.SetPassword(param.password, param.password);

      return SuccessOperation("系统密码设置完成");
    }

    public class ResetPasswordParams
    {
      public string old_password { get; set; }

      public string password { get; set; }

      public string password_confirmation { get; set; }
    }

    [HttpPost]
    [Route("system-password/reset")]
    public object ResetPassword([FromBody] ResetPasswordParams param)
    {
      _systemRepository.ResetPassword(param.old_password, param.password, param.password_confirmation);

      return SuccessOperation("密码重制成功");
    }

    public class CreateEmailVerifyCodeParams
    {
      [EmailAddress(ErrorMessage = "邮箱地址无效")]
      public string email { get; set; }
    }

    [HttpPost]
    [Route("/system-password/send-email-code")]
    public async Task<object> SystemPasswordSendEmail()
    {
      var email = _systemRepository.GetAdminEmail();

      if (email == null) {
        return FailureOperation("请先绑定管理员邮箱");
      }

      var ev = _systemRepository.CreateEmailVerifyCode(email);

      await _mail.SendVerifyCodeAsync(email, ev.verify_code, "密码重置");

      return SuccessOperation("验证码已发送，请在 30 分钟内处理", ev.id);
    }

    public class ResetSystemPasswordParams
    {
      public int email_verify_code_id { get; set; }

      public string verify_code { get; set; }
    }

    [HttpPost]
    [Route("/system-password/unset")]
    public object ResetSystemPassword([FromBody] ResetSystemPasswordParams param)
    {
      var email = _systemRepository.GetAdminEmail();
      _systemRepository.EnsureEmailVerifyCode(param.email_verify_code_id, email, param.verify_code);
      _systemRepository.UnsetAdminPassword();

      return SuccessOperation("密码已解除");
    }

    [HttpPost]
    [Route("email-verify-code/create")]
    public async Task<object> CreateEmailVerifyCode([FromBody] CreateEmailVerifyCodeParams param)
    {
      var ev = _systemRepository.CreateEmailVerifyCode(param.email);

      try {
        await _mail.SendVerifyCodeAsync(ev.email, ev.verify_code, "绑定管理员邮箱");
      } catch (Exception e) {
        Console.WriteLine(e);
        return FailureOperation($"邮件发送失败: {e.Message}");
      }

      return SuccessOperation("邮箱验证码已发送，请在 30 分钟内处理", ev.id);
    }

    public class SetAdminEmailParams
    {
      public int email_verify_code_id { get; set; }

      [EmailAddress(ErrorMessage = "邮箱地址无效")]
      public string email { get; set; }

      public string verify_code { get; set; }
    }

    [HttpPost]
    [Route("admin-email/set")]
    public object SetAdminEmail([FromBody] SetAdminEmailParams param)
    {
      _systemRepository.EnsureEmailVerifyCode(param.email_verify_code_id, param.email, param.verify_code);
      _systemRepository.SetAdminEmail(param.email);

      return SuccessOperation("管理员邮箱已绑定");
    }

    [HttpPost]
    [Route("admin-email/unset")]
    public object ResetAdminEmail([FromBody] SetAdminEmailParams param)
    {
      _systemRepository.EnsureEmailVerifyCode(param.email_verify_code_id, param.email, param.verify_code);
      _systemRepository.UnsetAdminEmail();

      return SuccessOperation("管理员邮箱已解绑");
    }

    [HttpPost]
    [Route("admin-email/get")]
    public string GetAdminEmail()
    {
      return _systemRepository.GetAdminEmail() ?? "";
    }

    public class SystemLockParams
    {
      public string password { get; set; }
    }

    [HttpPost]
    [Route("/system-lock/get")]
    public bool GetIsSystemLocked()
    {
      return _systemRepository.GetIsSystemLocked();
    }

    [HttpPost]
    [Route("/system-lock/lock")]
    public object LockSystem([FromBody] SystemLockParams param)
    {
      _systemRepository.EnsureAdminParams(param.password);
      _systemRepository.LockSystem();

      return SuccessOperation("设备锁定已开启");
    }

    [HttpPost]
    [Route("/system-lock/unlock")]
    public object UnlockSystem([FromBody] SystemLockParams param)
    {
      _systemRepository.EnsureAdminParams(param.password);
      _systemRepository.UnlockSystem();

      return SuccessOperation("设备锁定已关闭");
    }

    [HttpPost]
    [Route("/autorun/get")]
    public object GetIsAutorun()
    {
      return _systemRepository.GetIsAutorun();
    }

    public class SetAutorunParams
    {
      public bool value { get; set; }
    }

    [HttpPost]
    [Route("/autorun/set")]
    public object SetAutorun([FromBody] SetAutorunParams param)
    {
      _systemRepository.SetAutorun(param.value);

      return SuccessOperation("自动运行设置已修改");
    }

  }

}