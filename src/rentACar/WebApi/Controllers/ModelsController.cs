using Application.Features.Models.Queries.GetList;
using Core.Application.Requests;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;


[Route("api/[controller]")]
[ApiController]
public class ModelsController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListModelQuery getListBrandQuery = new() { PageRequest = pageRequest };
        var response = await Mediator.Send(getListBrandQuery);
        return Ok(response);
    }

    [HttpGet("GetList/ByDynamic")]
    public async Task<IActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest)
    {
        GetListModelQuery getListBrandQuery = new() { PageRequest = pageRequest };
        var response = await Mediator.Send(getListBrandQuery);
        return Ok(response);
    }
}