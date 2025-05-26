using IMSAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace IMSAPI.Controllers
{
    [Authorize]
    [RoutePrefix("ims")]
    public class IMSController : ApiController
    {
        IMSEntities IMSobj = new IMSEntities();
        IMS_KMPORTALEntities IMSKMobj = new IMS_KMPORTALEntities();

        public string EncodeString(string inputstr)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(HttpUtility.HtmlEncode(inputstr));
                return sb.ToString();
            }
            catch(Exception ex)
            {
                return "";
            }
          
        }

        [HttpPost]
        [Route("IMSSave")]
        public HttpResponseMessage IMSSave(IMSClass IMSC)
        {
            

            IMS_Save_Result Result = IMSobj.IMS_Save(IMSC.Id, IMSC.IdeaTitle, IMSC.Description, IMSC.ProcessingStatusId, IMSC.GlobalId, IMSC.CategoryCode, IMSC.EvaluationStatus).FirstOrDefault();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
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

        [HttpPost]
        [Route("IMSEvaluationSave")]
        public HttpResponseMessage IMSEvaluationSave(IMSClass IMSC)
        {
            List<IMS_SaveEvaluation_Result> Result = IMSobj.IMS_SaveEvaluation(IMSC.Id, IMSC.GlobalId, IMSC.Status, IMSC.actionremarks,IMSC.onbehalfselectionid,IMSC.onbehalfselectionremarks).ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            else
            {

                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
        }
        [HttpPost]
        [Route("IMSSaveIdeaScore")]
        public HttpResponseMessage IMSSaveIdeaScore(IMSClass IMSC)
        {
            List<IMS_SaveIdeaScore_Result> Result = IMSobj.IMS_SaveIdeaScore(IMSC.Id, IMSC.GlobalId, IMSC.EOI_Score, IMSC.CSP_Score, IMSC.actionremarks,IMSC.onbehalfscoringid,IMSC.onbehalfscoringremarks).ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
        }
        [HttpPost]
        [Route("IMSSaveIdeaShortList")]
        public HttpResponseMessage IMSSaveIdeaShortList(IMSClass IMSC)
        {
            List<IMS_SaveIdeaShortList_Result> Result = IMSobj.IMS_SaveIdeaShortList(IMSC.Id, IMSC.GlobalId, IMSC.actionremarks,IMSC.isboosted).ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
        }

        [HttpPost]
        [Route("IMSDropIdeaAtShortList")]
        public HttpResponseMessage IMSDropIdeaAtShortList(IMSClass IMSC)
        {
            List<IMS_DropIdeaAtShortListing_Result> Result = IMSobj.IMS_DropIdeaAtShortListing(IMSC.Id, IMSC.GlobalId, IMSC.actionremarks).ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
        }

        [HttpPost]
        [Route("IMSSaveIdeaWinner")]
        public HttpResponseMessage IMSSaveIdeaWinner(IMSClass IMSC)
        {
            List<IMS_SaveIdeawinner_Result> Result = IMSobj.IMS_SaveIdeawinner(IMSC.Id, IMSC.GlobalId, IMSC.actionremarks).ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
        }
        [HttpPost]
        [Route("IMSDropIdeaAtWinning")]
        public HttpResponseMessage IMSDropIdeaAtWinning(IMSClass IMSC)
        {
            List<IMS_DropIdeaatAwarding_Result> Result = IMSobj.IMS_DropIdeaatAwarding(IMSC.Id, IMSC.GlobalId, IMSC.actionremarks).ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
        }
        [HttpPost]
        [Route("IMS_SaveMonthsSavings")]
        public HttpResponseMessage IMSSaveMonthsSavings(IMSClass IMSC)
        {
            List<IMS_SaveMonthsSavings_Result> Result = IMSobj.IMS_SaveMonthsSavings(IMSC.Id, IMSC.IdeaId, IMSC.TwelveMonthsSavings, IMSC.PaybackPeriod, IMSC.M1, IMSC.M2, IMSC.M3, IMSC.M4, IMSC.M5, IMSC.M6, IMSC.M7, IMSC.M8, IMSC.M9, IMSC.M10, IMSC.M11, IMSC.M12, IMSC.GlobalId).ToList();
            if (Result == null)
            {

                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            else
            {
                if (IMSC.EvaluationFiles != null)
                {
                    byte[] FileAttachment;
                    foreach (IdeaFile IF in IMSC.EvaluationFiles)
                    {

                        FileAttachment = Encoding.ASCII.GetBytes(IF.Base64string);
                        IMSobj.IMS_SaveAttachment(IMSC.IdeaId, IMSC.GlobalId, IF.FileName, IF.FileLength, IF.FileType, FileAttachment, IF.Base64string, "Evaluation");
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
        }

        [HttpPost]
        [Route("IMS_Saveimstracking")]
        public HttpResponseMessage IMS_Saveimstracking(IMSClass IMSC)
        {
            if (IMSC.actualimplementationstartstr != null)
            {
                IMSC.actualimplementationstarts = Convert.ToDateTime(IMSC.actualimplementationstartstr);
            }
            else
            {
                IMSC.actualimplementationstarts = null;
            }
            if (IMSC.actualimplementationendstr != null)
            {
                IMSC.actualimplementationends = Convert.ToDateTime(IMSC.actualimplementationendstr);
            }
            else
            {
                IMSC.actualimplementationends = null;
            }

            if (IMSC.actualsavingstartstr != null)
            {
                IMSC.actualsavingstarts = Convert.ToDateTime(IMSC.actualsavingstartstr);
            }
            else
            {
                IMSC.actualsavingstarts = null;
            }
            List<IMS_Savetracking_v1_Result> Result = IMSobj.IMS_Savetracking_v1(IMSC.Id, IMSC.IdeaId, IMSC.TwelveMonthsSavings, IMSC.PaybackPeriod, IMSC.M1, IMSC.M2, IMSC.M3, IMSC.M4, IMSC.M5, IMSC.M6, IMSC.M7, IMSC.M8, IMSC.M9, IMSC.M10, IMSC.M11, IMSC.M12, IMSC.GlobalId, IMSC.actualimplementationstarts, IMSC.actualimplementationends, IMSC.actualsavingstarts, IMSC.ImplementationPeriod,IMSC.incentiveearned).ToList();
            if (Result == null)
            {

                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            else
            {
                byte[] FileAttachment;
                if (IMSC.TrackingFiles != null)
                {
                    foreach (IdeaFile IF in IMSC.TrackingFiles)
                    {

                        FileAttachment = Encoding.ASCII.GetBytes(IF.Base64string);
                        IMSobj.IMS_SaveAttachment(IMSC.IdeaId, IMSC.GlobalId, IF.FileName, IF.FileLength, IF.FileType, FileAttachment, IF.Base64string, "Tracking");
                    }

                }

                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
        }
        [HttpPost]
        [Route("IMSSaveTeamFormation")]
        public HttpResponseMessage IMSSaveTeamFormation(List<IMSClass> IMSClist)
        {
            int i = 0;
            foreach (IMSClass IMSC in IMSClist)
            {
                if (i == 0)
                {
                    List<IMS_getTeamFormationbyIdeaid_Result> memberlist = IMSobj.IMS_getTeamFormationbyIdeaid(IMSC.IdeaId).ToList();
                    if (memberlist.Count > 0)
                    {
                        int rmv = IMSobj.IMS_freezeTeamFormationbyIdeaid(IMSC.IdeaId, IMSC.GlobalId);
                    }
                }
                List<IMS_SaveTeamFormation_Result> Result = IMSobj.IMS_SaveTeamFormation(IMSC.Id, IMSC.IdeaId, IMSC.Member, IMSC.GlobalId, IMSC.membertype).ToList();

                /////

                var empcount = IMSobj.getEmployeeByGlobalID(IMSC.Member).FirstOrDefault();
                if (empcount == null)
                {
                    IMS_EmployeeDetailListByGlobalID_Result empdetails = IMSKMobj.IMS_EmployeeDetailListByGlobalID(IMSC.Member).FirstOrDefault();
                    if (empdetails != null)
                    {
                        string usernamestr = empdetails.Text.Split('-')[1].ToString();
                        insertemployeesroleandapprovernew_Result Resultxx = IMSobj.insertemployeesroleandapprovernew(0, usernamestr, empdetails.EMAIL, empdetails.ADUserId, empdetails.USER_ID, 3, null, empdetails.Value, empdetails.locationid, empdetails.Functions,
                        empdetails.DEPT, empdetails.managerid, empdetails.hodid, empdetails.Functionname, empdetails.deptname, "", "Auto Created On Team Assignment").FirstOrDefault();

                    }

                }

                //////

                i++;
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Ok");


        }

        [HttpGet]
        [Route("IMSGetIMSList")]
        public HttpResponseMessage IMSGetIMSList(string GlobalId)
        {
            List<IMS_GetIMSList_Result> Result = IMSobj.IMS_GetIMSList(GlobalId).ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }
        [HttpGet]
        [Route("IMS_Categorydll")]
        public HttpResponseMessage IMS_Categorydll()
        {
            List<IMS_Categorydll_Result> Result = IMSobj.IMS_Categorydll().ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }
        [HttpGet]
        [Route("IMS_IdeaValuedll")]
        public HttpResponseMessage IMS_IdeaValuedll()
        {
            List<IMS_IdeaValuedll_Result> Result = IMSobj.IMS_IdeaValuedll().ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }
        [HttpGet]
        [Route("IMSGetEvaluationList")]
        public HttpResponseMessage IMSGetEvaluationList(string GlobalId,string fromdatestr,string todatestr)
        {
            DateTime fromdate = Convert.ToDateTime(fromdatestr);
            DateTime todate = Convert.ToDateTime(todatestr);
            List<IMS_GetEvaluationList_Result> Result = IMSobj.IMS_GetEvaluationList(fromdate,todate,GlobalId).ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }

        [HttpGet]
        [Route("IMSGetEvaluationList_admin")]
        public HttpResponseMessage IMSGetEvaluationList_admin(string GlobalId, string fromdatestr, string todatestr)
        {
            DateTime fromdate = Convert.ToDateTime(fromdatestr);
            DateTime todate = Convert.ToDateTime(todatestr);
            List<IMS_GetEvaluationList_admin_Result> Result = IMSobj.IMS_GetEvaluationList_admin(fromdate, todate,GlobalId).ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }

        [HttpGet]
        [Route("IMSGetIdeaScroreList")]
        public HttpResponseMessage IMS_GetIdeaScroreList(string GlobalId, string fromdatestr, string todatestr)
        {
            DateTime fromdate = Convert.ToDateTime(fromdatestr);
            DateTime todate = Convert.ToDateTime(todatestr);
            List<IMS_GetIdeaScroreList_Result> Result = IMSobj.IMS_GetIdeaScroreList(fromdate, todate,GlobalId).ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }

        [HttpGet]
        [Route("IMSGetIdeaScroreList_admin")]
        public HttpResponseMessage IMS_GetIdeaScroreList_admin(string GlobalId, string fromdatestr, string todatestr)
        {
            DateTime fromdate = Convert.ToDateTime(fromdatestr);
            DateTime todate = Convert.ToDateTime(todatestr);
            List<IMS_GetIdeaScroreList_admin_Result> Result = IMSobj.IMS_GetIdeaScroreList_admin(fromdate, todate,GlobalId).ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }

        [HttpGet]
        [Route("IMSGetIdeaShortingList")]
        public HttpResponseMessage IMS_GetIdeaShortingList(string fromdatestr, string todatestr)
        {
            DateTime fromdate = Convert.ToDateTime(fromdatestr);
            DateTime todate = Convert.ToDateTime(todatestr);
            List<IMS_GetIdeaShortingList_Result> Result = IMSobj.IMS_GetIdeaShortingList(fromdate, todate).ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }
        [HttpGet]
        [Route("IMS_GetIMSWinnersList")]
        public HttpResponseMessage IMSGetIMSWinnersList(string fromdatestr, string todatestr)
        {
            DateTime fromdate = Convert.ToDateTime(fromdatestr);
            DateTime todate = Convert.ToDateTime(todatestr);
            List<IMS_GetIMSWinnersList_Result> Result = IMSobj.IMS_GetIMSWinnersList(fromdate, todate).ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }
        [HttpGet]
        [Route("IMS_GetIMSTrackingList")]
        public HttpResponseMessage IMS_GetIMSTrackingList(string globalid)
        {
            List<IMS_GetIMSTrackList_Result> Result = IMSobj.IMS_GetIMSTrackList(globalid).ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }
        [HttpGet]
        [Route("IMS_GetIMSEvaluationList")]
        public HttpResponseMessage IMSGetIMSEvaluationList(int Id)
        {
            List<IMS_GetIMSEvaluationList_Result> Result = IMSobj.IMS_GetIMSEvaluationList(Id).ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }
        [HttpGet]
        [Route("IMSGetFilesList")]
        public HttpResponseMessage IMSGetFilesList(int IdeaId)
        {
            List<IMS_GetFilesList_Result> Result = IMSobj.IMS_GetFilesList(IdeaId).ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }
        [HttpGet]
        [Route("IMSGetFile")]
        public HttpResponseMessage IMSGetFile(int Id)
        {
            List<IMS_GetFile_Result> Result = IMSobj.IMS_GetFile(Id).ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }

        [HttpGet]
        [Route("IMSGetFilesByModule")]
        public HttpResponseMessage IMSGetFilesByModule(int Id, string module)
        {
            List<IMS_GetFileByModule_Result> Result = IMSobj.IMS_GetFileByModule(Id, module).ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }

        [HttpGet]
        [Route("IMSGetIdeaValues")]
        public HttpResponseMessage IMSGetIdeaValues(int IdeaId)
        {

            List<IMS_GetIdeaValues_Result> Result = IMSobj.IMS_GetIdeaValues(IdeaId).ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }

        [HttpGet]
        [Route("IMSGetIdeaEvaluationbyideaid")]
        public HttpResponseMessage IMSGetIdeaEvaluationbyideaid(int IdeaId)
        {

            List<IMS_GetIdeaEvaluationByIdeaid_Result> Result = IMSobj.IMS_GetIdeaEvaluationByIdeaid(IdeaId).ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result.FirstOrDefault());
        }

        [HttpGet]
        [Route("IMSGetTeamDetailsbyideaid")]
        public HttpResponseMessage IMSGetTeamDetailsbyideaid(int IdeaId)
        {

            List<IMS_getTeamFormationbyIdeaid_Result> memberlist = IMSobj.IMS_getTeamFormationbyIdeaid(IdeaId).ToList();
            if (memberlist == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, memberlist);
        }
        [HttpGet]
        [Route("IMSGetIdeaTrackingbyideaid")]
        public HttpResponseMessage IMSGetIdeaTrackingbyideaid(int IdeaId)
        {

            List<IMS_GetIdeaTrackingByIdeaid_Result> Result = IMSobj.IMS_GetIdeaTrackingByIdeaid(IdeaId).ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result.FirstOrDefault());
        }

        [HttpGet]
        [Route("IMSGetIdeaFullDetails")]
        public HttpResponseMessage IMSGetIdeaFullDetails(int IdeaId)
        {
            IMSClass ideaobj = new IMSClass();

            List<IMS_GetIMSIdeaDetails_Result> Result = IMSobj.IMS_GetIMSIdeaDetails(IdeaId).ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            var returnobj = Result.FirstOrDefault();
            ideaobj.Id = returnobj.Id;
            ideaobj.IdeaCode = returnobj.IdeaCode;
            ideaobj.IdeaTitle = returnobj.IdeaTitle;
            ideaobj.Description = returnobj.Description;
            ideaobj.EvaluationStatus = returnobj.EvaluationStatus;
            ideaobj.EvaluationStatusCode = returnobj.EvaluationStatusCode;
            ideaobj.Createdon = returnobj.Createdon;
            ideaobj.Createdby = returnobj.Createdby;
            ideaobj.Createdbyname = returnobj.Createdbyname;
            ideaobj.CategoryCode = Convert.ToInt32(returnobj.CategoryCode);
            ideaobj.CategoryName = returnobj.CategoryName;
            ideaobj.IdeaValueText = returnobj.IdeaValueText;
            ideaobj.locationname = returnobj.locationname;
            ideaobj.departmentname = returnobj.departmentname;
            ideaobj.AVG_EOI = returnobj.AVG_EOI;
            ideaobj.AVG_CSP = returnobj.AVG_CSP;
            ideaobj.Idea_score = returnobj.Idea_score;
            ideaobj.isboosted = returnobj.isboosted;
            ideaobj.shortlistremarks = returnobj.shortlistremarks;
            ideaobj.awardingremarks = returnobj.awardingremarks;

            ideaobj.IdeaFiles = new List<IdeaFile>();
            List<IMS_GetFileByModule_Result> idmainfiles = IMSobj.IMS_GetFileByModule(returnobj.Id, "Creation").ToList();
            foreach (IMS_GetFileByModule_Result file in idmainfiles)
            {
                var obj = new IdeaFile();
                obj.FileName = file.FileName;
                obj.Base64string = file.Base64string;
                ideaobj.IdeaFiles.Add(obj);
            }

            ideaobj.EvaluationFiles = new List<IdeaFile>();
            List<IMS_GetFileByModule_Result> eval_files = IMSobj.IMS_GetFileByModule(returnobj.Id, "Evaluation").ToList();
            foreach (IMS_GetFileByModule_Result file in eval_files)
            {
                var obj = new IdeaFile();
                obj.FileName = file.FileName;
                obj.Base64string = file.Base64string;
                ideaobj.EvaluationFiles.Add(obj);
            }

            ideaobj.TrackingFiles = new List<IdeaFile>();
            List<IMS_GetFileByModule_Result> track_files = IMSobj.IMS_GetFileByModule(returnobj.Id, "Tracking").ToList();
            foreach (IMS_GetFileByModule_Result file in track_files)
            {
                var obj = new IdeaFile();
                obj.FileName = file.FileName;
                obj.Base64string = file.Base64string;
                ideaobj.TrackingFiles.Add(obj);
            }
            ideaobj.ideavalues = IMSobj.IMS_GetIdeaValues(returnobj.Id).ToList();
            ideaobj.teammembers = IMSobj.IMS_getTeamDetailsIdeaid(returnobj.Id).ToList();
            ideaobj.ideaevaluation = IMSobj.IMS_GetIdeaEvaluationByIdeaid(returnobj.Id).FirstOrDefault();
            if (ideaobj.ideaevaluation == null)
            {
                ideaobj.ideaevaluation = new IMS_GetIdeaEvaluationByIdeaid_Result();
            }
            ideaobj.ideatracking = IMSobj.IMS_GetIdeaTrackingByIdeaid(returnobj.Id).FirstOrDefault();
            if (ideaobj.ideatracking == null)
            {
                ideaobj.ideatracking = new IMS_GetIdeaTrackingByIdeaid_Result();
            }
            ideaobj.collaboratorsview = IMSobj.getCollaborators(returnobj.Id).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, ideaobj);
        }

        [HttpGet]
        [Route("GetEmployeeFullDetailbyGlobalid")]
        public HttpResponseMessage GetEmployeeFullDetailbyGlobalid(string globalid)
        {
            List<IMS_EmployeeFullDetailByGlobalID_Result> EmployeeList = IMSKMobj.IMS_EmployeeFullDetailByGlobalID(globalid).ToList();
            if (EmployeeList == null || EmployeeList.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, EmployeeList);
        }

        [HttpGet]
        [Route("gettopcontributers")]
        public HttpResponseMessage gettopcontributers()
        {
            List<gettopcontributers_Result> Result = IMSobj.gettopcontributers().ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }

        [HttpGet]
        [Route("gettopIdeas")]
        public HttpResponseMessage gettopIdeas()
        {
            List<gettopIdeas_Result> Result = IMSobj.gettopIdeas().ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }
        //IMS Query Methods

        [HttpGet]
        [Route("IMSGetQueryList")]
        public HttpResponseMessage IMSGetQueryList()
        {
            List<IMS_GetQueryList_Result> Result = IMSobj.IMS_GetQueryList().ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }
        [HttpGet]
        [Route("IMSGetQueryListByGlobalId")]
        public HttpResponseMessage IMSGetQueryListByGlobalId(string GlobalId)
        {
            List<IMS_GetQueryListByGlobalId_Result> Result = IMSobj.IMS_GetQueryListByGlobalId(GlobalId).ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }
        [HttpPost]
        [Route("IMSQuerySave")]
        public HttpResponseMessage IMSQuerySave(IMSQuery IMSQ)
        {
            byte[] FileContent;
            if (IMSQ.Base64String != "" || IMSQ.Base64String != null)
            {
                FileContent = Encoding.ASCII.GetBytes(IMSQ.Base64String);
            }
            else
            {
                FileContent = Encoding.ASCII.GetBytes("");
            }
            IMS_QuerySave_Result Result = IMSobj.IMS_QuerySave(IMSQ.Id, IMSQ.QueryText, IMSQ.FileName, IMSQ.FileSize, IMSQ.Filetype, FileContent, IMSQ.Base64String, IMSQ.CreatedBy, IMSQ.UpdatedBy, 0, IMSQ.ResponseText, IMSQ.ResponseBy, IMSQ.Status).FirstOrDefault();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
        }


        [HttpGet]
        [Route("IMSGetCollaboratorsbyideaid")]
        public HttpResponseMessage IMSGetCollaboratorsbyideaid(int IdeaId)
        {

            List<getCollaborators_Result> Result = IMSobj.getCollaborators(IdeaId).ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result.FirstOrDefault());
        }

        [HttpGet]
        [Route("IMS_Locationdll")]
        public HttpResponseMessage IMS_Locationdll()
        {
            List<getLocationnew_Result> Result = IMSobj.getLocationnew().ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }

        [HttpGet]
        [Route("IMS_Deptdll")]
        public HttpResponseMessage IMS_Deptdll()
        {
            List<getDepartmentDetails_Result> Result = IMSobj.getDepartmentDetails().ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }

        [HttpGet]
        [Route("IMS_Evaldll")]
        public HttpResponseMessage IMS_Evaldll()
        {
            List<IMS_EvalStatusdll_Result> Result = IMSobj.IMS_EvalStatusdll().ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }

        [HttpGet]
        [Route("IMS_IdeaReport")]
        public HttpResponseMessage IMS_IdeaReport(string fromdatestr,string todatestr,int locationid,int deptid,int categoryid,int statuscode)
        {
            DateTime fromdate = Convert.ToDateTime(fromdatestr);
            DateTime todate = Convert.ToDateTime(todatestr);

            List<IMS_GetIdeaListReport_v1_Result> Result = IMSobj.IMS_GetIdeaListReport_v1(fromdate,todate,locationid,deptid,categoryid,statuscode).ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }

        [HttpGet]
        [Route("IMS_CCDetailsById")]
        public HttpResponseMessage IMS_CCDetailsById(int id)
        {
           
            List<IMS_GetCCListById_Result> Result = IMSobj.IMS_GetCCListById(id).ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }

        [HttpGet]
        [Route("IMS_Ideasvingdata")]
        public HttpResponseMessage IMS_Ideasvingdata()
        {
            
            List<ims_getsavingsdata_Result> Result = IMSobj.ims_getsavingsdata().ToList();
            if (Result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            return Request.CreateResponse(HttpStatusCode.OK, Result);
        }
    }
}
