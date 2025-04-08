using Dapper;
using FluentAssertions;
using Application.Infraestructure.Data.Extensions;

namespace Application.Infraestructure.Data.Tests;

public class DynamicQueryExtensionsTests
{
    [Fact]
    public void MountEqual_Deve_Montar_Where_Com_Parametro()
    {
        string sql = "SELECT * FROM Usuarios";
        DynamicParameters parameters = new();

        string result = sql.MountEqual("Email", "teste@exemplo.com", ref parameters);

        result.Should().Contain("WHERE Email = @Email");
        parameters.ParameterNames.Should().Contain("Email");
        parameters.Get<string>("Email").Should().Be("teste@exemplo.com");
    }

    [Fact]
    public void MountBetween_Deve_Montar_Clausula_Entre_Duas_Datas()
    {
        string sql = "SELECT * FROM Pedidos";
        DynamicParameters parameters = new();

        string result = sql.MountBetween("DataCriacao", new DateTime(2023, 01, 01), new DateTime(2023, 12, 31), ref parameters);

        result.Should().Contain("WHERE DataCriacao >= @DataCriacao");
        result.Should().Contain("AND DataCriacao <= @DataCriacao");
        parameters.ParameterNames.Should().Contain("DataCriacao");
    }

    [Fact]
    public void MountIn_Deve_Montar_IN_Clause()
    {
        string sql = "SELECT * FROM Produtos";
        DynamicParameters parameters = new();

        int[] categorias = [1, 2, 3];

        string result = sql.MountIn("CategoriaId", categorias, ref parameters);

        result.Should().Contain("WHERE CategoriaId IN @CategoriaId");
        parameters.ParameterNames.Should().Contain("CategoriaId");
        parameters.Get<IEnumerable<int>>("CategoriaId").Should().BeEquivalentTo(categorias);
    }

    [Fact]
    public void MountLike_Deve_Montar_Like_Com_Collate()
    {
        string sql = "SELECT * FROM Usuarios";
        DynamicParameters parameters = new();

        string result = sql.MountLike("Nome", "joão", ref parameters);

        result.Should().Contain("WHERE Nome LIKE @Nome");
        result.Should().Contain("COLLATE SQL_Latin1_General_CP1_CI_AI");
        parameters.Get<string>("Nome").Should().Be("%joão%");
    }

    [Fact]
    public void MountOrderAsc_Deve_Montar_OrderBy_Crescente()
    {
        string sql = "SELECT * FROM Tarefas";
        string result = sql.MountOrderAsc(["DataCriacao", "Prioridade"]);

        result.Should().Be("SELECT * FROM Tarefas ORDER BY DataCriacao, Prioridade ASC");
    }


    [Fact]
    public void BuildDynamicQuery_Deve_Adicionar_Parametros_Como_Clausula_Where()
    {
        DynamicParameters parameters = new();
        parameters.Add("Nome", "Carlos");
        parameters.Add("Email", "carlos@email.com");

        string baseQuery = "SELECT * FROM Usuarios";

        string result = parameters.BuildDynamicQuery(baseQuery);

        result.Should().Contain("WHERE Nome = @Nome AND Email = @Email");
    }
}