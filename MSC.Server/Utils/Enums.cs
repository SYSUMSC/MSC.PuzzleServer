using System;

namespace MSC.Server
{
    /// <summary>
    /// 用户权限枚举
    /// </summary>
    public enum Privilege
    {
        /// <summary>
        /// 小黑屋用户权限
        /// </summary>
        BannedUser = 0,
        /// <summary>
        /// 常规用户权限
        /// </summary>
        User = 1,
        /// <summary>
        /// 监控者权限，可查询提交日志
        /// </summary>
        Monitor = 2,
        /// <summary>
        /// 管理员权限，可查看系统日志
        /// </summary>
        Admin = 3,
    }

    /// <summary>
    /// 任务执行状态
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// 任务正在进行
        /// </summary>
        Pending = -1,
        /// <summary>
        /// 任务成功完成
        /// </summary>
        Success = 0,
        /// <summary>
        /// 任务执行失败
        /// </summary>
        Fail = 1,
        /// <summary>
        /// 任务遇到重复错误
        /// </summary>
        Duplicate = 2,
        /// <summary>
        /// 任务处理被拒绝
        /// </summary>
        Denied = 3,
    }
}
