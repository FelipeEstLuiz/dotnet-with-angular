using Dapper;
using System.Data;

namespace Application.Infraestructure.Data.Extensions;

public static class DynamicParametersExtensions
{
    public static DynamicParameters SetDataInicialAndFinal(
        this DynamicParameters parameters,
        DateTime? dataInicial,
        DateTime? dataFinal
    ) => parameters.SetParameter("DataInicial", dataInicial).SetParameter("DataFinal", dataFinal);

    public static DynamicParameters SetParameter(
        this DynamicParameters parameters,
        string column,
        bool? param,
        bool insereNull = false
    )
    {
        if (param.HasValue || insereNull)
            parameters.Add(column, value: param, dbType: DbType.Boolean, direction: ParameterDirection.Input);

        return parameters;
    }

    public static DynamicParameters SetOutputParameter(this DynamicParameters parameters, string column, DbType dbType)
    {
        parameters.Add(column, dbType: dbType, direction: ParameterDirection.Output);
        return parameters;
    }

    public static DynamicParameters SetParameter(
        this DynamicParameters parameters,
        string column,
        decimal? param,
        bool insereNull = false
    )
    {
        if (param.HasValue || insereNull)
            parameters.Add(column, value: param, dbType: DbType.Decimal, direction: ParameterDirection.Input);

        return parameters;
    }

    public static DynamicParameters SetParameter(
        this DynamicParameters parameters,
        string column,
        DateTime? param,
        bool insereNull = false
    )
    {
        if (param.HasValue || insereNull)
            parameters.Add(column, value: param, dbType: DbType.DateTime, direction: ParameterDirection.Input);

        return parameters;
    }

    public static DynamicParameters SetParameter(
        this DynamicParameters parameters,
        string column,
        int? param,
        bool insereNull = false
    )
    {
        if (param.HasValue || insereNull)
            parameters.Add(column, value: param, dbType: DbType.Int32, direction: ParameterDirection.Input);

        return parameters;
    }

    public static DynamicParameters SetParameter(
        this DynamicParameters parameters,
        string column,
        Guid? param,
        bool insereNull = false
    )
    {
        if (param.HasValue || insereNull)
            parameters.Add(column, value: param, dbType: DbType.Guid, direction: ParameterDirection.Input);

        return parameters;
    }

    public static DynamicParameters SetParameter(
        this DynamicParameters parameters,
        string column,
        long? param,
        bool insereNull = false
    )
    {
        if (param.HasValue || insereNull)
            parameters.Add(column, value: param, dbType: DbType.Int64, direction: ParameterDirection.Input);

        return parameters;
    }

    public static DynamicParameters SetParameter(
        this DynamicParameters parameters,
        string column,
        string param,
        bool insereNull = false
    )
    {
        if (param is not null && !string.IsNullOrWhiteSpace(param) || insereNull)
            parameters.Add(column, value: param, dbType: DbType.String, direction: ParameterDirection.Input);

        return parameters;
    }

    public static DynamicParameters SetParameter(
        this DynamicParameters parameters,
        string name,
        IEnumerable<int> value
    )
    {
        if (value is null || !value.Any()) return parameters;

        DataTable table = new();
        string lista = "dbo.ListaInt";

        table.Columns.Add("Item", typeof(int));

        foreach (int item in value)
            table.Rows.Add(item);

        parameters.Add(name, value: table.AsTableValuedParameter(lista), direction: ParameterDirection.Input);

        return parameters;
    }

    public static DynamicParameters SetParameter(
        this DynamicParameters parameters,
        string name,
        IEnumerable<string> value
    )
    {
        if (value is null || !value.Any()) return parameters;

        DataTable table = new();
        string lista = "dbo.ListaString";

        table.Columns.Add("Item", typeof(string));

        foreach (string item in value)
            table.Rows.Add(item);

        parameters.Add(name, value: table.AsTableValuedParameter(lista), direction: ParameterDirection.Input);

        return parameters;
    }

    public static DynamicParameters SetParameter(
        this DynamicParameters parameters,
        string name,
        IEnumerable<Guid> value
    )
    {
        if (value is null || !value.Any()) return parameters;

        DataTable table = new();
        string lista = "dbo.ListaUniqueidentifier";

        table.Columns.Add("Item", typeof(Guid));

        foreach (Guid item in value)
            table.Rows.Add(item);

        parameters.Add(name, value: table.AsTableValuedParameter(lista), direction: ParameterDirection.Input);

        return parameters;
    }

    public static DynamicParameters SetParameterListaLong(
        this DynamicParameters parameters,
        string name,
        IEnumerable<long> value
    )
    {
        if (value is null || !value.Any()) return parameters;

        DataTable table = new();
        string lista = "dbo.ListaBigInt";

        table.Columns.Add("Item", typeof(long));

        foreach (long item in value)
            table.Rows.Add(item);

        parameters.Add(name, value: table.AsTableValuedParameter(lista), direction: ParameterDirection.Input);

        return parameters;
    }

    public static DynamicParameters SetParameterDataInicio(
        this DynamicParameters parameters,
        string column,
        DateTime? param
    )
    {
        if (param.HasValue)
        {
            DateTime novaDataInicio = new(
                param.Value.Year,
                param.Value.Month,
                param.Value.Day,
                0,
                0,
                0
            );

            parameters.SetParameter(column, novaDataInicio);
        }

        return parameters;
    }

    public static DynamicParameters SetParameterDataFim(
        this DynamicParameters parameters,
        string column,
        DateTime? param
    )
    {
        if (param.HasValue)
        {
            DateTime novaDataFim = new(
                param.Value.Year,
                param.Value.Month,
                param.Value.Day,
                23,
                59,
                59
            );

            parameters.SetParameter(column, novaDataFim);
        }

        return parameters;
    }

    public static DynamicParameters SetParameter<TEnum>(
        this DynamicParameters parameters,
        string column,
        TEnum? param,
        bool insereNull = false
    ) where TEnum : struct, Enum
    {
        if (param.HasValue)
            return parameters.SetParameter(column, param.Value);
        else if (insereNull)
            parameters.Add(column, value: null, dbType: DbType.Int32, direction: ParameterDirection.Input);

        return parameters;
    }

    public static DynamicParameters SetParameter<TEnum>(
        this DynamicParameters parameters,
        string column,
        TEnum param
    ) where TEnum : struct, Enum
    {
        parameters.Add(column, value: Convert.ToInt32(param), dbType: DbType.Int32, direction: ParameterDirection.Input);

        return parameters;
    }

    public static DynamicParameters SetParameter<TEnum>(
        this DynamicParameters parameters,
        string name,
        IEnumerable<TEnum> value
    ) where TEnum : struct, Enum
    {
        if (value is null || !value.Any()) return parameters;

        DataTable table = new();
        string lista = "dbo.ListaInt";

        table.Columns.Add("Item", typeof(int));

        foreach (TEnum item in value)
            table.Rows.Add(Convert.ToInt32(item));

        parameters.Add(name, value: table.AsTableValuedParameter(lista), direction: ParameterDirection.Input);

        return parameters;
    }
}