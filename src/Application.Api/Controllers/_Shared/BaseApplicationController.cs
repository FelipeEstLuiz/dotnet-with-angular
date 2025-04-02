using Microsoft.AspNetCore.Mvc;

namespace Application.Api.Controllers._Shared;

[ApiController]
[Route("api/app/v{version:apiVersion}/[controller]")]
[ApiExplorerSettings(GroupName = "Application")]
public class BaseApplicationController(CommunicationProtocol protocol) : BaseController(protocol)
{
}
