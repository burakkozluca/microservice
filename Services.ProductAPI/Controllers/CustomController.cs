using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Services.ProductAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomController : ControllerBase
{
    [NonAction]
    public IActionResult CreateActionResult<T> (ServiceResult<T> result)
    {
        if(result.Status == HttpStatusCode.NoContent)
        {
            return NoContent();


        };

            
        if (result.Status == HttpStatusCode.Created)
        {
            return Created(result.UrlAsCreated , result);

        }

        return new ObjectResult(result) { StatusCode = result.Status.GetHashCode()};
    }

    [NonAction]
    public IActionResult CreateActionResult(ServiceResult result)
    {
        if (result.Status == HttpStatusCode.NoContent)
        {
            return NoContent();


        }
        //data yok ama hata olursa errormessage yazdırabiliriz 
        return new ObjectResult(result) { StatusCode = result.Status.GetHashCode() };
    }

}