using MSC.Server.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSC.Server.Utils
{
    /// <summary>
    /// 验证结果
    /// </summary>
    public record VerifyResult(AnswerResult Result = AnswerResult.WrongAnswer, int Score = 0);

    #region 请求响应
    public record RequestResponse(string Title, int Status = 400);
    public record PuzzleResponse(int Id, int Status = 200);
    #endregion
}
