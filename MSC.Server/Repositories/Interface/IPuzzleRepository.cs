using MSC.Server.Models;
using MSC.Server.Models.Request;
using MSC.Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MSC.Server.Repositories.Interface
{
    public interface IPuzzleRepository
    {
        /// <summary>
        /// 添加一个题目对象
        /// </summary>
        /// <param name="newPuzzle">待添加的题目</param>
        /// <param name="token">操作取消token</param>
        /// <returns></returns>
        public Task<Puzzle> AddPuzzle(PuzzleBase newPuzzle, CancellationToken token);
        /// <summary>
        /// 获取用户题目数据
        /// </summary>
        /// <param name="id">题目Id</param>
        /// <param name="accessLevel">用户访问权限</param>
        /// <param name="token">操作取消token</param>
        /// <returns>用户题目</returns>
        public Task<UserPuzzleModel> GetUserPuzzle(int id, int accessLevel, CancellationToken token);
        /// <summary>
        /// 更新一个题目对象
        /// </summary>
        /// <param name="id">题目Id</param>
        /// <param name="newPuzzle">更新的题目数据</param>
        /// <param name="token">操作取消token</param>
        /// <returns></returns>
        public Task<Puzzle> UpdatePuzzle(int id, PuzzleBase newPuzzle, CancellationToken token);
        /// <summary>
        /// 删除题目
        /// </summary>
        /// <param name="id">题目Id</param>
        /// <param name="token">操作取消token</param>
        /// <returns></returns>
        public Task<(bool result, string title)> DeletePuzzle(int id, CancellationToken token);
        /// <summary>
        /// 验证答案
        /// </summary>
        /// <param name="id">题目Id</param>
        /// <param name="answer">答案</param>
        /// <param name="accessLevel">用户访问权限</param>
        /// <param name="token">操作取消token</param>
        /// <returns></returns>
        public Task<VerifyResult> VerifyAnswer(int id, string answer, int accessLevel, CancellationToken token);
    }
}
