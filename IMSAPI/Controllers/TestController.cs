using IMSAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace IMSAPI.Controllers
{
    [RoutePrefix("testpage")]
    public class TestController : ApiController
    {
        TestDBEntities dbobj = new TestDBEntities();

        [HttpGet]
        [Route("Gettestlist")]
        public HttpResponseMessage gettestData()
        {
            List<getTestdata_Result> EmployeeList = dbobj.getTestdata().ToList();
            if (EmployeeList == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, EmployeeList);
           
        }

        [HttpPost]
        [Route("posttestlist")]
        public HttpResponseMessage posttestData(testpage data)
        {
            dbobj.insertupdateemployeetest1(data.id,data.globalid,data.empname,data.action);

            return Request.CreateResponse(HttpStatusCode.OK, "done");
        }
    }
}
