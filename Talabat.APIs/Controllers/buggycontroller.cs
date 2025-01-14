using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers
{
    public class buggycontroller : BaseApiController
    {
        private readonly StoreContext _store;

        public buggycontroller(StoreContext store)
        {
            _store = store;
        }

        [HttpGet("notfound")]
        public ActionResult GetNotFound()
        {
            var product = _store.Products.Find(100);
            if (product == null) return NotFound(new ApiResponse(404));
            return Ok(product);
        }
        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var product = _store.Products.Find(100);
            var st = product.ToString();
            return Ok(product);
        }
        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("badrequest/{id}")]
        public ActionResult GetBadRequestValidation(int id)
        {
            return BadRequest();
        }

        [HttpGet("unauthorize")]
        public ActionResult GetUnAuthorzeError()
        {
            return Unauthorized(new ApiResponse(401));
        }



    }
}
