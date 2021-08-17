using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSC.Server.Models
{
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
                if (SolvedCount <= AwardCount)
                    return OriginalScore - SolvedCount;
                if (SolvedCount > ExpectMaxCount)
                    return MinScore;
                var range = OriginalScore - AwardCount - MinScore;
                return (int)(OriginalScore - AwardCount 
                    - Math.Floor( range * (SolvedCount - AwardCount)/(float)(ExpectMaxCount - AwardCount)));
            }
        }

        #region 数据库关系
        public List<Process> Processes { get; set; } = new();

        public List<Submission> Submissions { get; set; } = new();
        #endregion

        public void Update(PuzzleBase puzzle)
        {
            foreach (var item in typeof(PuzzleBase).GetProperties())
                item.SetValue(this, item.GetValue(puzzle));
        }

        public Puzzle() : base() { }
        public Puzzle(PuzzleBase puzzle) : base() => Update(puzzle);
    }
}
