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
        Task<SapperResult> CommandAsync(string sql, CancellationToken cancellationToken = default, CommandType commandType = CommandType.StoredProcedure);
        Task<SapperResult> CommandAsync<TInput>(TInput model, string sql, CancellationToken cancellationToken = default, CommandType commandType = CommandType.StoredProcedure, params string[] outputParamNames);
        Task<SapperResult<TOutput>> QueryAsync<TInput, TOutput>(TInput? model, string sql, CancellationToken cancellationToken = default, CommandType commandType = CommandType.StoredProcedure, params string[] outputParamNames);
        Task<SapperResult<TOutput>> QueryAsync<TOutput>(string sql, CancellationToken cancellationToken = default, CommandType commandType = CommandType.StoredProcedure);
    }
}
