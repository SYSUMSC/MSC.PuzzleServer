﻿using MSC.Server.Models;
using MSC.Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSC.Server.Repositories.Interface
{
    public interface ISubmissionRepository
    {
        /// <summary>
        /// 添加提交记录
        /// </summary>
        /// <param name="puzzleId">题目Id</param>
        /// <param name="userid">用户Id</param>
        /// <param name="answer">答案</param>
        /// <param name="result">验证结果</param>
        /// <returns></returns>
        public Task AddSubmission(int puzzleId, string userid, string answer, VerifyResult result);
        /// <summary>
        /// 根据题目Id获取提交记录
        /// </summary>
        /// <param name="puzzleId">题目Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="skip">跳过数量</param>
        /// <param name="count">获取数量</param>
        /// <returns></returns>
        public Task<List<Submission>> GetSubmissions(int skip = 0, int count = 50, int puzzleId = 0, string userId = "All");
    }
}