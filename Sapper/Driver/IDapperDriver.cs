using Sapper.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sapper.Driver
{
    public interface IDapperDriver
    {
        IDbConnection Connection { get; }
        Task<SapperResult> CommandAsync(string sql, CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default);
        Task<SapperResult> CommandAsync<TInput>(TInput model, string sql, CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default, params string[] outputParamNames);
        Task<SapperResult<TOutput>> QueryAsync<TInput, TOutput>(TInput? model, string sql, CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default, params string[] outputParamNames);
        Task<SapperResult<TOutput>> QueryAsync<TOutput>(string sql, CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default);
    }
}
