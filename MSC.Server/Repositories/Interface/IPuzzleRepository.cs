﻿using MSC.Server.Models;
using MSC.Server.Models.Request;
using MSC.Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSC.Server.Repositories.Interface
{
    public interface IPuzzleRepository
    {
        /// <summary>
        /// 添加一个题目对象
        /// </summary>
        /// <param name="puzzle">待添加的题目</param>
        /// <returns></returns>
        public Task<Puzzle> AddPuzzle(PuzzleBase newPuzzle);
        /// <summary>
        /// 获取用户题目数据
        /// </summary>
        /// <param name="id">题目Id</param>
        /// <param name="accessLevel">用户访问权限</param>
        /// <returns>用户题目</returns>
        public Task<UserPuzzleModel> GetUserPuzzle(int id, int accessLevel);
        /// <summary>
        /// 更新一个题目对象
        /// </summary>
        /// <param name="id">题目Id</param>
        /// <param name="puzzle">更新的题目数据</param>
        /// <returns></returns>
        public Task<Puzzle> UpdatePuzzle(int id, PuzzleBase newPuzzle);
        /// <summary>
        /// 删除题目
        /// </summary>
        /// <param name="id">题目Id</param>
        /// <returns></returns>
        public Task<bool> DeletePuzzle(int id);
        /// <summary>
        /// 验证答案
        /// </summary>
        /// <param name="id">题目Id</param>
        /// <param name="answer">答案</param>
        /// <param name="accessLevel">用户访问权限</param>
        /// <returns></returns>
        public Task<VerifyResult> VerifyAnswer(int id, string answer, int accessLevel);
    }
}