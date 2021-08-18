using MSC.Server.Models;
using MSC.Server.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSC.Server.Repositories.Interface
{
    public interface IRankRepository
    {
        /// <summary>
        /// 获取排名
        /// </summary>
        /// <param name="skip">跳过数量</param>
        /// <param name="count">获取数量</param>
        /// <returns></returns>
        public Task<List<RankMessageModel>> GetRank(int skip, int count);
        /// <summary>
        /// 更新排名对象
        /// </summary>
        /// <param name="rank">排名对象</param>
        /// <param name="score">分数</param>
        /// <returns></returns>
        public Task UpdateRank(Rank rank, int score);
    }
}
