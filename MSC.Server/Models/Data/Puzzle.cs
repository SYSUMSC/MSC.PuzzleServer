using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSC.Server.Models;

[Index(nameof(AccessLevel))]
public class Puzzle : PuzzleBase
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// 当前题目分值
    /// </summary>
    [NotMapped]
    public int CurrentScore
    {
        get
        {
            if (AcceptedCount <= AwardCount)
                return OriginalScore - AcceptedCount;
            if (AcceptedCount > ExpectMaxCount)
                return MinScore;
            var range = OriginalScore - AwardCount - MinScore;
            return (int)(OriginalScore - AwardCount
                - Math.Floor(range * (AcceptedCount - AwardCount) / (float)(ExpectMaxCount - AwardCount)));
        }
    }

    #region 数据库关系

    public List<Submission> Submissions { get; set; } = new();

    #endregion 数据库关系

    public void Update(PuzzleBase puzzle)
    {
        foreach (var item in typeof(PuzzleBase).GetProperties())
            item.SetValue(this, item.GetValue(puzzle));

        Answer = Answer.Trim();
    }

    public Puzzle() : base()
    {
    }

    public Puzzle(PuzzleBase puzzle) : base() => Update(puzzle);
}
