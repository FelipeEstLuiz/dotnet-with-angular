using Dapper;
using System.Text;

namespace Application.Infraestructure.Data.Extensions;

public static class DynamicQueryExtensions
{
    public static string MountEqual(this string sql, string column, object value, ref DynamicParameters parameters)
        => MountQuery(sql, column, column, value, "=", ref parameters);

    public static string MountBetween(
        this string sql,
        string column,
        object value1,
        object value2,
        ref DynamicParameters parameters
    )
    {
        sql = sql.MountGreaterOrThan(column, value1, ref parameters);
        return sql.MountLessOrThan(column, value2, ref parameters);
    }

    public static string MountIn(this string sql, string column, object value, ref DynamicParameters parameters)
        => MountQuery(sql, column, $"{column}", value, "IN", ref parameters);

    public static string MountNotIn(this string sql, string column, object value, ref DynamicParameters parameters)
        => MountQuery(sql, column, $"{column}", value, "NOT IN", ref parameters);

    public static string MountGreater(this string sql, string column, object value, ref DynamicParameters parameters)
        => MountQuery(sql, column, column, value, condition: ">", ref parameters);

    public static string MountGreaterOrThan(
        this string sql,
        string column,
        object value,
        ref DynamicParameters parameters
    ) => MountQuery(sql, column, column, value, condition: ">=", ref parameters);

    public static string MountLess(this string sql, string column, object value, ref DynamicParameters parameters)
        => MountQuery(sql, column, column, value, condition: "<", ref parameters);

    public static string MountLessOrThan(this string sql, string column, object value, ref DynamicParameters parameters)
        => MountQuery(sql, column, column, value, condition: "<=", ref parameters);

    public static string MountLike(this string sql, string column, object value, ref DynamicParameters parameters)
    {
        StringBuilder query = new StringBuilder(MountQuery(
            sql,
            column,
            parameter: column, $"%{value}%",
            condition: "LIKE",
            ref parameters
        )).Append(" COLLATE SQL_Latin1_General_CP1_CI_AI ");

        return query.ToString();
    }

    public static string MountParamsServerSide(this string sql, int page, int pageSize, ref DynamicParameters parameters)
    {
        parameters.Add("Page", page);
        parameters.Add("PageSize", pageSize);

        StringBuilder query = new StringBuilder(sql)
            .Append(" OFFSET (@Page / @PageSize) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY");

        return query.ToString();
    }

    public static string MountOrderDesc(this string sql, string[] columns) => MountOrder(sql, columns) + " DESC";

    public static string MountOrderAsc(this string sql, string[] columns) => MountOrder(sql, columns) + " ASC";

    private static string MountOrder(string sql, string[] columns)
    {
        for (int i = 0; i < columns.Length; i++)
        {
            if (i == 0)
                sql += $" ORDER BY {columns[i]}";
            else
                sql += $", {columns[i]}";
        }

        return sql;
    }

    private static string MountQuery(
        string sql,
        string column,
        string parameter,
        object value,
        string condition,
        ref DynamicParameters parameters
    )
    {
        if (value is null) return sql;

        StringBuilder query = new(sql);

        if (sql.Contains("WHERE"))
            query.Append("AND");
        else
            query.Append("WHERE");

        query.Append($" {column} {condition} @{parameter} ");

        parameters.Add(parameter, value);

        return query.ToString();
    }

    public static string BuildDynamicQuery(this DynamicParameters parameters, string query)
    {
        StringBuilder sqlBuilder = new(query);

        if (parameters.ParameterNames.Any())
        {
            if (query.Contains("WHERE"))
                sqlBuilder.Append(" AND ");
            else
                sqlBuilder.Append(" WHERE ");

            // Adiciona as condições com base nos parâmetros
            foreach (string paramName in parameters.ParameterNames)
                sqlBuilder.Append($"{paramName} = @{paramName} AND ");

            // Remove o último "AND"
            sqlBuilder.Remove(sqlBuilder.Length - 5, 5);
        }

        return sqlBuilder.ToString();
    }
}
