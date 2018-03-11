using System;
using System.Web.Http;
using WAES.BusinessLogic;
using WAES.Shared;

namespace WAES.WebApi.SelfHost
{
    [RoutePrefix("v1/diff")]
    public class DiffController : ApiController
    {
        private readonly IByteDiffClient _byteDiffClient;

        public DiffController(IByteDiffClient byteDiffClient)
        {
            _byteDiffClient = byteDiffClient;
        }

        [Route("{id:int}/Left")]
        [HttpPost]
        public IHttpActionResult Left(int id, [FromBody] LeftRequest request)
        {
            if (request.Base64 == null) throw new ArgumentNullException(DebugAndErrorMessages.Base64);
            _byteDiffClient.Upsert(id, request.Base64, Side.Left);
            return Ok();
        }

        [Route("{id:int}/Right")]
        [HttpPost]
        public IHttpActionResult Right(int id, [FromBody] RightRequest request)
        {
            if (request.Base64 == null) throw new ArgumentNullException(DebugAndErrorMessages.Base64);
            _byteDiffClient.Upsert(id, request.Base64, Side.Right);
            return Ok();
        }

        [Route("{id:int}")]
        [HttpGet]
        public IHttpActionResult Diff(int id)
        {
            try
            {
                var result = _byteDiffClient.GetDiffResult(id);
                return Ok(result);
            }
            catch (ArgumentNullException)
            {
                throw new IndexOutOfRangeException(DebugAndErrorMessages.NoValue);
            }
        }
    }
}