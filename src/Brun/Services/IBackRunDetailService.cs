using Brun.Models;
using System.Threading.Tasks;

namespace Brun.Services
{
    /// <summary>
    /// BackRun运行历史相关服务
    /// </summary>
    public interface IBackRunDetailService
    {
        /// <summary>
        /// 获取指定BackRunId的运行数量信息
        /// </summary>
        /// <param name="backRunId"></param>
        /// <returns></returns>
        Task<BackRunContextNumberModel> GetBackRunDetailNumber(string backRunId);
    }
}