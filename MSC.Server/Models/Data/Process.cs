using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MSC.Server.Models
{
    [Index(nameof(UserId), nameof(PuzzleId))]
    public class Process
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 初次访问时间
        /// </summary>
        public DateTime FirstAccessTime { get; set; }

        /// <summary>
        /// 谜题完结时间
        /// </summary>
        public DateTime PuzzleSolveTime { get; set; }

        /// <summary>
        /// 谜题得分
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 关卡耗时
        /// </summary>
        [NotMapped]
        public TimeSpan Interval
        {
            get
            {
                return PuzzleSolveTime - FirstAccessTime;
            }
        }

        #region 数据库关系
        /// <summary>
        /// 用户数据库Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public UserInfo User { get; set; }

        /// <summary>
        /// 谜题数据库Id
        /// </summary>
        public int PuzzleId { get; set; }

        /// <summary>
        /// 谜题
        /// </summary>
        public Puzzle Puzzle { get; set; }
        #endregion
    }
}
