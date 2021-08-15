# User API

以下API均使用POST请求。

## Register

- PageUrl: `/Account/Register`
- APIUrl: `/api/account/register`
- Params: RegisterModel
    - UserName: 用户名，必须，6-25字符，大写唯一
    - Password: 密码，必须，大于6字符
    - Email: 邮箱，必须，用于接收确认邮件
    - GToken: Google Recaptcha v3 token
- Result: Json
    - status:
        - Fail: 请求失败, 错误信息见`msg`字段
        - OK: 注册成功
- Note: 邮件中URL形式为`/Account/Verify?token=xxx&email=xxx`其中参数均为base64编码，访问此页面显示邮件

## Register Verify

- PageUrl: `/Account/Verify?token=xxx&email=xxx`
- APIUrl: `/api/account/verify`
- Params: AccountVerifyModel
    - Email: 邮箱，必须，用于接收确认邮件
    - Token: 验证Token，必须
- Result: Json
    - status:
        - Fail: 请求失败, 错误信息见`msg`字段
        - OK: 验证成功，显示几秒的提示，重定向到主页面
- Note: token与email两个URL参数为base64编码，直接传入API即可

## Login

- PageUrl: `/Account/Login`
- APIUrl: `/api/account/login`
- Params: LoginModel
    - UserName: 用户名，必须，6-25字符，大写唯一
    - Password: 密码，必须，大于6字符
    - GToken: Google Recaptcha v3 token
- Result: Json
    - status:
        - Fail: 登录失败
        - OK: 登录成功

## Account Recovery

- PageUrl: `/Account/Recovery`
- APIUrl: `/api/account/recovery`
- Params: RecoveryModel
    - Email: 邮箱，必须
    - GToken: Google Recaptcha v3 token
- Result: Json
    - status:
        - Fail: 请求失败, 错误信息见`msg`字段
        - OK: 密码重置邮件发送成功
- Note: 邮件中URL形式为`/Account/PasswordReset?token=xxx&email=xxx`其中参数均为base64编码

## Password Reset

- PageUrl: `/Account/PasswordReset?token=xxx&email=xxx`
- APIUrl: `/api/account/pwdreset`
- Params: PasswordResetModel
    - Password: 密码，必须，大于6字符
    - Email: 邮箱，必须，从URL参数获取
    - RToken: 密码重置token，必须，从URL参数获取
    - GToken: Google Recaptcha v3 token
- Result: Json
    - status:
        - Fail: 登录失败
        - OK: 登录成功
- Note: token与email两个URL参数为base64编码，直接传入API即可

## Logout

- APIUrl: `/api/account/logout`
- Params: None
- Result: Json
    - status:
        - OK: 请求成功
- Note: 已登录状态请求后登出

## Update Profile

- PageUrl: `/Profile`
- APIUrl: `/api/account/update`
- Params: ProfileUpdateModel
    - UserName: 用户名，必须，6-25字符，大写唯一
    - Des: 个人简介、描述
- Result: Json
    - status:
        - Fail: 请求失败, 错误信息见`msg`字段
        - OK: 请求成功

## Change Password

- PageUrl: `/Profile`
- APIUrl: `/api/account/changepwd`
- Params: PasswordChangeModel
    - Old: 旧密码，必须，大于6字符
    - New: 新密码，必须，大于6字符
- Result: Json
    - status:
        - Fail: 请求失败, 错误信息见`msg`字段
        - OK: 请求成功

## Change Email

- PageUrl: `/Profile`
- APIUrl: `/api/account/changemail`
- Params: MailChangeModel
    - NewMail: 新邮箱，必须
- Result: Json
    - status:
        - Fail: 请求失败, 错误信息见`msg`字段
        - OK: 邮箱更改邮件发送成功
- Note: 邮件中URL形式为`/Account/ChangeMail?token=xxx&email=xxx`其中参数均为base64编码

## Change Email Confirm

- PageUrl: `/Account/ChangeMail?token=xxx&email=xxx`
- APIUrl: `/api/account/mailconfirm`
- Params: AccountVerifyModel
    - Email: 邮箱，必须，用于接收确认邮件
    - Token: 验证Token，必须
- Result: Json
    - status:
        - Fail: 请求失败, 错误信息见`msg`字段
        - OK: 邮箱更改成功，显示几秒的提示，重定向到主页面
