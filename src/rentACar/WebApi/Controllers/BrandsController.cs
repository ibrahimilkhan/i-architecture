using Application.Brands.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : BaseController
    {

        [HttpPost]
        public async Task<IActionResult> CreateBrand([FromBody] CreateBrandCommand command)
        {
            var result = await Mediator.Send(command);
            return result is not null
                ? Ok(result)
                : BadRequest("Brand creation failed.");
        }
    }
}