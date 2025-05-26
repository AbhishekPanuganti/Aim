using IMSAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace IMSAPI.Controllers
{
    [Authorize]
    [RoutePrefix("imsnew")]
    public class IMSNewController : Controller
    {
        IMSEntities IMSobj = new IMSEntities();
        IMS_KMPORTALEntities IMSKMobj = new IMS_KMPORTALEntities();
       

        [HttpPost]
        [Route("IMSSaveNew")]
        [ValidateInput(true)]
        public HttpResponseMessage IMSSaveNew(IMSClass IMSC)
        {
            HttpRequestMessage Request = new HttpRequestMessage();
            IMS_Save_Result Result = IMSobj.IMS_Save(IMSC.Id, IMSC.IdeaTitle, IMSC.Description, IMSC.ProcessingStatusId, IMSC.GlobalId, IMSC.CategoryCode, IMSC.EvaluationStatus).FirstOrDefault();
            if (Result == null)
            {
                return  Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            else
            {
                if (Result.Status == "Saved" && Result.Return > 0)
                {
                    foreach (IdeaValue IV in IMSC.IdeaValues)
                    {
                        IMSobj.IMS_SaveIdeaValue(Result.Return, IV.IdeaValueId, IV.IdeaValueText, IMSC.GlobalId);
                    }
                    if (IMSC.IdeaFiles != null)
                    {
                        byte[] FileAttachment;
                        foreach (IdeaFile IF in IMSC.IdeaFiles)
                        {

                            FileAttachment = Encoding.ASCII.GetBytes(IF.Base64string);
                            IMSobj.IMS_SaveAttachment(Result.Return, IMSC.GlobalId, IF.FileName, IF.FileLength, IF.FileType, FileAttachment, IF.Base64string, "Creation");
                        }
                    }
                    if (IMSC.collaborators != null && IMSC.collaborators.Count > 0)
                    {
                        foreach (CollaboratorModel collab in IMSC.collaborators)
                        {
                            insertCollaborators_Result crslt = IMSobj.insertCollaborators(collab.Id, Result.Return, collab.collaborator_globalid, collab.CreatedBy).FirstOrDefault();

                            /////

                            var empcount = IMSobj.getEmployeeByGlobalID(collab.collaborator_globalid).FirstOrDefault();
                            if (empcount == null)
                            {
                                IMS_EmployeeDetailListByGlobalID_Result empdetails = IMSKMobj.IMS_EmployeeDetailListByGlobalID(collab.collaborator_globalid).FirstOrDefault();
                                if (empdetails != null)
                                {
                                    string usernamestr = empdetails.Text.Split('-')[1].ToString();
                                    insertemployeesroleandapprovernew_Result Resultxx = IMSobj.insertemployeesroleandapprovernew(0, usernamestr, empdetails.EMAIL, empdetails.ADUserId, empdetails.USER_ID, 3, null, empdetails.Value, empdetails.locationid, empdetails.Functions,
                                    empdetails.DEPT, empdetails.managerid, empdetails.hodid, empdetails.Functionname, empdetails.deptname, "", "Auto Created On Collaborators entry").FirstOrDefault();

                                }

                            }

                            //////
                        }
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, Result);
                }
                else if (Result.Status == "Updated" && Result.Return > 0)
                {
                    IMSobj.IMS_CloseIdeaValue(Result.Return, IMSC.GlobalId);
                    foreach (IdeaValue IV in IMSC.IdeaValues)
                    {
                        IMSobj.IMS_SaveIdeaValue(Result.Return, IV.IdeaValueId, IV.IdeaValueText, IMSC.GlobalId);
                    }
                    if (IMSC.collaborators != null && IMSC.collaborators.Count > 0)
                    {
                        removeCollaborators_Result rmvclb = IMSobj.removeCollaborators(Result.Return, IMSC.GlobalId).FirstOrDefault();
                        foreach (CollaboratorModel collab in IMSC.collaborators)
                        {
                            insertCollaborators_Result crslt = IMSobj.insertCollaborators(collab.Id, Result.Return, collab.collaborator_globalid, collab.CreatedBy).FirstOrDefault();

                            /////

                            var empcount = IMSobj.getEmployeeByGlobalID(collab.collaborator_globalid).FirstOrDefault();
                            if (empcount == null)
                            {
                                IMS_EmployeeDetailListByGlobalID_Result empdetails = IMSKMobj.IMS_EmployeeDetailListByGlobalID(collab.collaborator_globalid).FirstOrDefault();
                                if (empdetails != null)
                                {
                                    string usernamestr = empdetails.Text.Split('-')[1].ToString();
                                    insertemployeesroleandapprovernew_Result Resultxx = IMSobj.insertemployeesroleandapprovernew(0, usernamestr, empdetails.EMAIL, empdetails.ADUserId, empdetails.USER_ID, 3, null, empdetails.Value, empdetails.locationid, empdetails.Functions,
                                    empdetails.DEPT, empdetails.managerid, empdetails.hodid, empdetails.Functionname, empdetails.deptname, "", "Auto Created On Collaborators entry").FirstOrDefault();

                                }

                            }

                            //////
                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, Result);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, 404);
                }


            }
        }
    }
}