using System.Collections.Generic;
using System.Reflection;
using Easy.Admin.Core.Entities;
using Easy.Admin.Core.Options;
using Yitter.IdGenerator;

namespace SqlSugar;

public static class SqlSugarExtensions
{
    /// <summary>
    /// 添加 SqlSugar 拓展（适用于单个数据库）
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config">数据库连接对象</param>
    /// <param name="buildAction"></param>
    /// <returns></returns>
    public static IServiceCollection AddSqlSugar(this IServiceCollection services, ConnectionConfig config, Action<ISqlSugarClient> buildAction = default)
    {
        return services.AddSqlSugar(new List<ConnectionConfig>()
        {
            config
        }, buildAction);
    }

    /// <summary>
    /// 添加 SqlSugar 拓展（适用于多数据库）
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configs">数据库连接对象集合</param>
    /// <param name="buildAction"></param>
    /// <returns></returns>
    public static IServiceCollection AddSqlSugar(this IServiceCollection services, List<ConnectionConfig> configs, Action<SqlSugarClient> buildAction = default)
    {
        SqlSugarScope sqlSugarScope = new SqlSugarScope(configs.ToList(), buildAction ?? Aop);
        services.AddSingleton<ISqlSugarClient>(sqlSugarScope);//使用单例注入
        services.AddSingleton<ITenant>(sqlSugarScope);
        services.AddUnitOfWork<SqlSugarUnitOfWork>();//事务
        services.AddScoped(typeof(ISqlSugarRepository<>), typeof(SqlSugarRepository<>));//注入泛型仓储

        return services;
    }

    /// <summary>
    /// 添加 SqlSugar 服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddSqlSugar(this IServiceCollection services)
    {
        var options = App.GetOptions<DbConnectionOptions>();
        ICacheService ormCache = new SqlSugarCache();
        options.Connections.ForEach(x =>
        {
            x.MoreSettings = new ConnMoreSettings()
            {
                //所有 增、删 、改 会自动调用.RemoveDataCache()
                IsAutoRemoveDataCache = true
            };
            //配置缓存
            x.ConfigureExternalServices = new ConfigureExternalServices()
            {
                DataInfoCacheService = ormCache,
                EntityService = ((prop, column) =>
                {
                    //如果实体不是主键，并且是可空类型，表列设置为可空(支持string?和string)
                    if (column.IsPrimarykey == false && new NullabilityInfoContext()
                            .Create(prop).WriteState is NullabilityState.Nullable)
                    {
                        column.IsNullable = true;
                    }
                })

            };

        });
        return AddSqlSugar(services, options.Connections, Aop);
    }

    /// <summary>
    /// 拦截器
    /// </summary>
    private static readonly Action<SqlSugarClient> Aop = (db) =>
    {
        db.Aop.DataExecuting = (_, entityInfo) =>
        {
            switch (entityInfo.OperationType)
            {
                //执行insert时
                case DataFilterType.InsertByObject:
                    //自动设置主键
                    if (entityInfo.EntityColumnInfo.IsPrimarykey &&
                        entityInfo.EntityValue is Entity<long> { Id: 0 } entity)
                    {
                        entity.Id = YitIdHelper.NextId();
                        break;
                    }
                    //如果当前实体继承ICreatedTime就设置创建时间
                    if (entityInfo.EntityValue is ICreatedTime createdTime && createdTime.CreatedTime == DateTime.MinValue)
                    {
                        createdTime.CreatedTime = DateTime.Now;
                    }
                    break;
                case DataFilterType.UpdateByObject:
                    if (entityInfo.EntityValue is IUpdatedTime { UpdatedTime: null } updatedTime)
                    {
                        updatedTime.UpdatedTime = DateTime.Now;
                    }
                    break;
            }
        };

        // 文档地址：https://www.donet5.com/Home/Doc?typeId=1204
        db.Aop.OnLogExecuting = (sql, parameters) =>
        {
            var originColor = Console.ForegroundColor;
            if (sql.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                Console.ForegroundColor = ConsoleColor.Green;
            if (sql.StartsWith("UPDATE", StringComparison.OrdinalIgnoreCase) || sql.StartsWith("INSERT", StringComparison.OrdinalIgnoreCase))
                Console.ForegroundColor = ConsoleColor.Yellow;
            if (sql.StartsWith("DELETE", StringComparison.OrdinalIgnoreCase))
                Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("【" + DateTime.Now + "——执行SQL】\r\n" + UtilMethods.GetSqlString(db.CurrentConnectionConfig.DbType, sql, parameters) + "\r\n");
            Console.ForegroundColor = originColor;
            App.PrintToMiniProfiler("SqlSugar", "Info", sql + "\r\n" + db.Utilities.SerializeObject(parameters.ToDictionary(it => it.ParameterName, it => it.Value)));
        };
        db.Aop.OnError = ex =>
        {
            if (ex.Parametres == null) return;
            var originColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            var pars = db.Utilities.SerializeObject(((SugarParameter[])ex.Parametres).ToDictionary(it => it.ParameterName, it => it.Value));
            Console.WriteLine("【" + DateTime.Now + "——错误SQL】\r\n" + UtilMethods.GetSqlString(db.CurrentConnectionConfig.DbType, ex.Sql, (SugarParameter[])ex.Parametres) + "\r\n");
            Console.ForegroundColor = originColor;
            App.PrintToMiniProfiler("SqlSugar", "Error", $"{ex.Message}{Environment.NewLine}{ex.Sql}{pars}{Environment.NewLine}");
        };

        //配置逻辑删除过滤器（查询数据时过滤掉已被标记删除的数据）
        db.QueryFilter.AddTableFilter<ISoftDelete>(it => it.DeleteMark == false);
    };
}