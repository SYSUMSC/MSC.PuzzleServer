namespace MSC.Server.Models.Request
{
    /// <summary>
    /// 用户端题目
    /// </summary>
    public class UserPuzzleModel
    {
        /// <summary>
        /// 题目标题
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 题目内容
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// 已解出人数
        /// </summary>
        public int SolvedCount { get; set; }
    }
}