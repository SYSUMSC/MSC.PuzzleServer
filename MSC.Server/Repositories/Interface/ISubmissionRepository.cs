using MSC.Server.Models;
using MSC.Server.Models.Request;
using MSC.Server.Utils;

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
        /// <param name="hasSolved">用户是否已经解出过此谜题</param>
        /// <param name="token">操作取消token</param>
        /// <returns></returns>
        public Task AddSubmission(int puzzleId, string userid, string answer, VerifyResult result, bool hasSolved, CancellationToken token);

        /// <summary>
        /// 根据题目Id获取提交记录
        /// </summary>
        /// <param name="puzzleId">题目Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="skip">跳过数量</param>
        /// <param name="count">获取数量</param>
        /// <param name="token">操作取消token</param>
        /// <returns></returns>
        public Task<List<Submission>> GetSubmissions(CancellationToken token, int skip = 0, int count = 50, int puzzleId = 0, string userId = "All");

        /// <summary>
        /// 获取用户时间线
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="token">操作取消Token</param>
        /// <returns></returns>
        public Task<List<TimeLineModel>> GetTimeLine(string userId, CancellationToken token);

        /// <summary>
        /// 是否已经提交
        /// </summary>
        /// <param name="puzzleId">题目Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="token">操作取消Token</param>
        /// <returns></returns>
        public Task<bool> HasSubmitted(int puzzleId, string userId, CancellationToken token);
    }
}