using Application.Api.Controllers._Shared;
using Application.Domain.Enums;
using Application.Domain.Model;
using Application.Domain.Util;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Application.Api.Tets;

public class BaseControllerTests
{
    private CommunicationProtocol protocol;

    public BaseControllerTests()
    {
        protocol = new CommunicationProtocol();
        protocol.SetProtocol("API");
    }

    public class TestController(CommunicationProtocol protocol) : BaseController(protocol)
    {
        public IActionResult CallHandlerResponse<T>(HttpStatusCode code, Result<T> result)
            => HandlerResponse(code, result);
    }

    [Fact]
    public void HandlerResponse_Deve_Retornar_200_Se_Sucesso()
    {
        TestController controller = new(protocol);
        Result<string> result = "dados de teste";

        IActionResult response = controller.CallHandlerResponse(HttpStatusCode.OK, result);

        ObjectResult objectResult = Assert.IsType<ObjectResult>(response);
        Assert.Equal(200, objectResult.StatusCode);
        Response responseData = Assert.IsType<Response>(objectResult.Value);
        Assert.Equal("dados de teste", responseData.Data);
        Assert.Equal("API", responseData.Protocol);
    }

    [Fact]
    public void HandlerResponse_Deve_Retornar_400_Se_Failure_Sem_Codigo_Especifico()
    {
        TestController controller = new(protocol);
        Result<string> result = Result<string>.Failure("Erro gen�rico");

        IActionResult response = controller.CallHandlerResponse(HttpStatusCode.OK, result);

        ObjectResult objectResult = Assert.IsType<ObjectResult>(response);
        Assert.Equal(400, objectResult.StatusCode);
        Response responseData = Assert.IsType<Response>(objectResult.Value);
        Assert.Contains("Erro gen�rico", responseData.Errors);
    }

    [Fact]
    public void HandlerResponse_Deve_Retornar_403_Se_Nao_Tem_Permissao()
    {
        TestController controller = new(protocol);
        Result<string> result = Result<string>.Failure("Sem permiss�o", ResponseCodes.USER_NOT_HAVE_PERMISSION);

        IActionResult response = controller.CallHandlerResponse(HttpStatusCode.OK, result);

        ObjectResult objectResult = Assert.IsType<ObjectResult>(response);
        Assert.Equal(403, objectResult.StatusCode);
        Response responseData = Assert.IsType<Response>(objectResult.Value);
        Assert.Contains("Sem permiss�o", responseData.Errors);
    }

    [Theory]
    [InlineData(ResponseCodes.UNAUTHORIZED, 401)]
    [InlineData(ResponseCodes.USER_NOT_HAVE_PERMISSION, 403)]
    [InlineData(ResponseCodes.NOT_FOUND, 404)]
    public void HandlerResponse_Deve_Mapear_ResponseCode_Para_StatusCode_Correto(
        ResponseCodes responseCode,
        int expectedStatusCode
    )
    {
        TestController controller = new(protocol);
        Result<string> result = Result<string>.Failure("erro teste", responseCode);

        IActionResult response = controller.CallHandlerResponse(HttpStatusCode.OK, result);

        ObjectResult objectResult = Assert.IsType<ObjectResult>(response);
        Assert.Equal(expectedStatusCode, objectResult.StatusCode);
    }
}