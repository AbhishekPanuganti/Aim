using IMSAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IMSAPI.Controllers
{
    [Authorize]
    [RoutePrefix("imsadmin")]
    public class IMSAdminController : ApiController
    {

        IMSEntities IMSobj = new IMSEntities();
        IMS_KMPORTALEntities IMSKMobj = new IMS_KMPORTALEntities();
        Customresponse custresponse = new Customresponse();



        [Route("GetEmployeeDetailList")]
        public HttpResponseMessage GetEmployeeDetailList()
        {
            List<IMS_EmployeeDetailList_Result> EmployeeList = IMSKMobj.IMS_EmployeeDetailList().ToList();
            if (EmployeeList == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, EmployeeList);
        }

        [Route("GetHODList")]
        public HttpResponseMessage GetHODList()
        {
            List<IMS_GetHODS_Result> hodlist = IMSKMobj.IMS_GetHODS().ToList();
            if (hodlist == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, hodlist);
        }

        [Route("GetHODReporteesList")]
        public HttpResponseMessage GetHODReporteesList(string hodgid)
        {
            List<IMS_GetHODReportees_Result> dtlist = IMSKMobj.IMS_GetHODReportees(hodgid).ToList();
            if (dtlist == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, dtlist);
        }

        [Route("employeesmenu")]
        public HttpResponseMessage Getemployeesmenu(int userid)
        {
            List<getusermenu_Result> menuList = IMSobj.getusermenu(userid).ToList();
            if (menuList == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, menuList);
        }



        [Route("settings")]
        public HttpResponseMessage GetSettings()
        {
            getSettings_Result stList = IMSobj.getSettings().FirstOrDefault();
            if (stList == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, stList);
        }



        [Route("employeelistforaudit")]
        public HttpResponseMessage GetEmployeesdata()
        {
            List<getEmployeesdata_Result> menuList = IMSobj.getEmployeesdata().ToList();
            if (menuList == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, menuList);
        }

        [Route("locations")]
        public HttpResponseMessage GetLocationDetailsList(int code)
        {
            List<getLocationDetails_Result> menuList = IMSobj.getLocationDetails(code).ToList();
            if (menuList == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, menuList);
        }

        [Route("locationnew")]
        public HttpResponseMessage GetLocationList()
        {
            List<getLocationnew_Result> menuList = IMSobj.getLocationnew().ToList();
            if (menuList == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, menuList);
        }



        [Route("departments")]
        public HttpResponseMessage getDepartmentDetails()
        {

            List<getDepartmentDetails_Result> menuList = IMSobj.getDepartmentDetails().ToList();
            if (menuList == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, menuList);
        }

        [Route("menu")]
        public HttpResponseMessage GetMenuList()
        {
            List<getMenulist_Result> menuList = IMSobj.getMenulist().ToList();
            if (menuList == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, menuList);
        }

        [Route("systeminfo")]
        public HttpResponseMessage GetSyatemInfo(DateTime fromdate, DateTime todate, int userid, int locationid, int deptid)
        {
            todate = todate.AddDays(1);
            List<getsystemlog_Result> objist = IMSobj.getsystemlog(fromdate, todate, userid, locationid, deptid).ToList();
            if (objist == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, objist);
        }

        [Route("errorlog")]
        public HttpResponseMessage GetErrorLog(DateTime fromdate, DateTime todate, int userid, int locationid, int deptid)
        {
            List<geterrorlog_Result> objist = IMSobj.geterrorlog(fromdate, todate, userid, locationid, deptid).ToList();
            if (objist == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, objist);
        }




        [Route("permission")]
        public HttpResponseMessage GetPermission()
        {
            List<getPermissions_Result> menuList = IMSobj.getPermissions().ToList();
            if (menuList == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, menuList);
        }

        [Route("role")]
        public HttpResponseMessage GetRoleList()
        {
            List<getRolelist_Result> roleList = IMSobj.getRolelist().ToList();
            if (roleList == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, roleList);
        }


        public HttpResponseMessage GetRoleWiseMenuList()
        {
            List<getRoleWiseMenu_Result> uwrlist = IMSobj.getRoleWiseMenu().ToList();
            if (uwrlist == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, uwrlist);
        }

        [Route("employeesuwr")]
        public HttpResponseMessage GetEmployeeUWR()
        {
            List<getUserWiseRole_Result> uwrlist = IMSobj.getUserWiseRole().ToList();
            if (uwrlist == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, uwrlist);
        }


        [Route("GetEmployeeDetailbyGlobalid")]
        public HttpResponseMessage GetEmployeeDetailbyGlobalid(string globalid)
        {
            List<getEmployeeByGlobalID_Result> EmployeeList = IMSobj.getEmployeeByGlobalID(globalid).ToList();
            if (EmployeeList == null || EmployeeList.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, EmployeeList);
        }

        [HttpPost]
        [Route("savemenu")]
        public HttpResponseMessage saveMenu(getMenulist_Result menu)
        {
            string clickmode = "A";
            int user = (int)menu.createdby;
            if (menu.id > 0)
            {
                user = (int)menu.updatedby;
                clickmode = "E";
            }
            ObjectParameter returnId = new ObjectParameter("return", typeof(int));
            int response = IMSobj.insertupdatemenu(menu.id, menu.name, menu.menuicon, menu.menuurl, menu.parentid, user, clickmode, "", menu.remarks, returnId);
            if (response > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "ok");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "error");
            }


        }




        [Route("insertsystemlog")]
        public HttpResponseMessage insertSystemlog(getsystemlog_Result obj)
        {
            return Request.CreateResponse(HttpStatusCode.OK, "ok");
            //try
            //{
            //    int res = IMSobj.insertsysteminfo(obj.userid, obj.module, obj.activity);
            //    if (res > 0)
            //    {
            //        custresponse.status = HttpStatusCode.OK;
            //    }
            //    else
            //    {
            //        custresponse.status = HttpStatusCode.BadRequest;
            //    }


            //}
            //catch (Exception ex)
            //{
            //    int errsave = IMSobj.inserterrorlog(obj.userid, "System Log", ex.Message.ToString(), "Insertion");
            //}

            //if (custresponse.status == HttpStatusCode.OK)
            //{
            //    return Request.CreateResponse(HttpStatusCode.OK, "ok");
            //}
            //else if (custresponse.status == HttpStatusCode.Found)
            //{
            //    return Request.CreateResponse(HttpStatusCode.Found, "Found");
            //}
            //else
            //{
            //    return Request.CreateResponse(HttpStatusCode.BadRequest, "error");
            //}

        }



        [HttpPost]
        [Route("saverole")]
        public HttpResponseMessage saveRole(getRolelist_Result role)
        {
            string clickmode = "A";
            int user = (int)role.createdby;
            if (role.id > 0)
            {
                user = (int)role.updatedby;
                clickmode = "E";
            }
            ObjectParameter returnId = new ObjectParameter("return", typeof(int));
            int response = IMSobj.insertupdaterole(role.id, role.name, user, clickmode, "", role.remarks, returnId);
            if (response > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "ok");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "error");
            }

        }



        [HttpPost]
        [Route("saverolewisemenu")]
        public HttpResponseMessage saveRoleWiseMenu(List<getRoleWiseMenu_Result> roles)
        {
            int roleid = roles[0].roleid;
            string remarks = roles[0].remarks;
            int user = (int)roles[0].createdby;
            List<getRoleWiseMenuByRoleID_Result> existingRows = IMSobj.getRoleWiseMenuByRoleID(roleid).ToList();
            foreach (getRoleWiseMenuByRoleID_Result rows in existingRows)
            {

                ObjectParameter delreturnId = new ObjectParameter("return", typeof(int));
                int deleteresponse = IMSobj.removeRoleWiseMenuByID(rows.id, rows.createdby, "", remarks, delreturnId);

            }

            foreach (getRoleWiseMenu_Result role in roles)
            {

                ObjectParameter returnId = new ObjectParameter("return", typeof(int));
                int response = IMSobj.insertrolewisemenu(role.menuid, role.roleid, role.permissionid, user, "", remarks, returnId);
                if (response > 0)
                {
                    custresponse.status = HttpStatusCode.OK;
                }
                else
                {
                    custresponse.status = HttpStatusCode.BadRequest;
                }

            }
            if (custresponse.status == HttpStatusCode.OK)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "ok");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "error");
            }

        }

        [HttpPost]
        [Route("saveuserwiserole")]
        public HttpResponseMessage SaveUserWiseRole(List<UserRoleMatrix> roles)
        {
            UserRoleMatrix role = roles.FirstOrDefault();
            insertemployeesroleandapprovernew_Result Result = IMSobj.insertemployeesroleandapprovernew(role.Id, role.name, role.email, role.adid, role.userid, role.roleid, role.createdby, role.globalid, role.locationid, role.departmentid,
                    role.subdepartmentid, role.managerid, role.hodid, role.departmentname, role.subdepartmentname, role.ipaddress, role.remarks).FirstOrDefault();
            if (Result != null)
            {

                return Request.CreateResponse(HttpStatusCode.OK, "ok");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "error");
            }




        }



    }
}
