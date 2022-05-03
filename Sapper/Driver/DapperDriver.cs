using Dapper;
using Sapper.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sapper.Driver
{

    public class DapperDriver : IDapperDriver
    {
        public IDbConnection Connection { get; private set; }

        public DapperDriver(string connectionString)
        {
            Connection = new SqlConnection(connectionString);
        }

        public async Task<SapperResult<TOutput>> QueryAsync<TInput, TOutput>(TInput model, string sql, CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default, params string[] outputParamNames)
        {
            var param = GetDynamicParamFromTModel(model, outputParamNames);
            IEnumerable<TOutput> res;


            if (param == null)
            {
                res = await Connection.QueryAsync<TOutput>(new CommandDefinition(commandText: sql, commandType: commandType, cancellationToken: cancellationToken));
            }
            else
            {
                res = await Connection.QueryAsync<TOutput>(new CommandDefinition(commandText: sql, parameters: param, commandType: commandType, cancellationToken: cancellationToken));
            }

            var outputParams = ExtractInputParamData(param, outputParamNames);

            return new SapperResult<TOutput>
            {
                OutputParams = outputParams,
                Data = res.ToList()
            };
        }

        public async Task<SapperResult<TOutput>> QueryAsync<TOutput>(string sql, CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default)
        {
            IEnumerable<TOutput> res;
            res = await Connection.QueryAsync<TOutput>(new CommandDefinition(commandText: sql, commandType: commandType, cancellationToken: cancellationToken));

            return new SapperResult<TOutput>
            {
                Data = res.ToList()
            };
        }

        public async Task<SapperResult> CommandAsync<TInput>(TInput model, string sql, CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default, params string[] outputParamNames)
        {
            var param = GetDynamicParamFromTModel(model, outputParamNames);
            int count;

            if (param == null)
            {
                count = await Connection.ExecuteAsync(new CommandDefinition(commandText: sql, commandType: commandType, cancellationToken: cancellationToken));
            }
            else
            {
                count = await Connection.ExecuteAsync(new CommandDefinition(commandText: sql, parameters: param, commandType: commandType, cancellationToken: cancellationToken));
            }


            var outputParams = ExtractInputParamData(param, outputParamNames);

            return new SapperResult
            {
                OutputParams = outputParams,
                ModifiedCount = count
            };
        }

        public async Task<SapperResult> CommandAsync(string sql, CommandType commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default)
        {
            var count = await Connection.ExecuteAsync(new CommandDefinition(commandText: sql, commandType: commandType, cancellationToken: cancellationToken));

            return new SapperResult
            {
                ModifiedCount = count
            };
        }

        private DynamicParameters GetDynamicParamFromTModel<TInput>(TInput model, string[] OutputParamas)
        {
            if (model == null)
            {
                return null;
            }

            var param = new DynamicParameters();
            var propertyList = model.GetType().GetProperties();


            foreach (var item in propertyList)
            {
                if (OutputParamas != null && OutputParamas.Contains(item.Name))
                    param.Add(item.Name, dbType: GetDBTypeByType(item.PropertyType), value: item.GetValue(model, null), direction: ParameterDirection.InputOutput);
                else if (item.PropertyType == typeof(byte[]))
                    param.Add(item.Name, dbType: DbType.Binary, value: item.GetValue(model, null), direction: ParameterDirection.Input, size: -1);
                else
                    param.Add(item.Name, item.GetValue(model, null));
            }

            return param;
        }

        private DbType GetDBTypeByType(Type type)
        {
            return SqlMapper.typeMap[type];
        }

        private void AddProperty(ref ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }

        private dynamic ExtractInputParamData(DynamicParameters param, string[] outputParamNames)
        {
            dynamic outputParams = new ExpandoObject();
            if (param != null && outputParamNames.Length > 0)
            {
                for (int i = 0; i < outputParamNames.Length; i++)
                {
                    AddProperty(outputParams, outputParamNames[i], param.Get<dynamic>(outputParamNames[i]));
                }
            }

            return outputParams;
        }
    }
}
