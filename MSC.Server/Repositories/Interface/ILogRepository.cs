﻿using MSC.Server.Models;

namespace MSC.Server.Repositories.Interface
{
    public interface ILogRepository
    {
        /// <summary>
        /// 获取指定数量和等级的日志
        /// </summary>
        /// <param name="skip">跳过数量</param>
        /// <param name="count">数量</param>
        /// <param name="level">等级</param>
        /// <param name="token">操作取消token</param>
        /// <returns>不超过指定数量的日志</returns>
        public Task<List<LogMessageModel>?> GetLogs(int skip, int count, string level, CancellationToken token);
    }
}