using IMSAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http;

namespace IMSAPI.Controllers
{
    [RoutePrefix("imsuser")]
    public class UserController : ApiController
    {
        IMSEntities IMSobj = new IMSEntities();
        IMS_KMPORTALEntities IMSKMobj = new IMS_KMPORTALEntities();

        [Route("IMSlogin")]
        public HttpResponseMessage POSTCheckCredential(MUser muser)
        {
            string wholeString = muser.UserName;
            string firstBit = wholeString.Split('@')[0];
            string pass = Encryption_Decryption.EncryptString(muser.Password);
            checkQRLogin_Result chkresult = IMSobj.checkQRLogin(firstBit, pass).FirstOrDefault();
            if (chkresult != null)
            {
                chkresult.token = GetToken(chkresult.adid, chkresult.name);
                return Request.CreateResponse(HttpStatusCode.OK, chkresult);
            }
            Customresponse rslt = getValidEmployeeList(muser);
            getEmployeeByGlobalID_Result ValidEmployeeList = rslt.emp1;
            if (rslt.status == HttpStatusCode.OK)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ValidEmployeeList);

            }
            else if (rslt.status == HttpStatusCode.NotFound)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 404);
            }
            else if (rslt.status == HttpStatusCode.GatewayTimeout)
            {
                return Request.CreateResponse(HttpStatusCode.GatewayTimeout, 504);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, 400);
            }
        }

        [HttpPost]
        [Route("IMSloginfrombasecamp")]
        public HttpResponseMessage IMSloginfrombasecamp(MUser muser)
        {
            getEmployeeByGlobalID_Result t_result = new getEmployeeByGlobalID_Result();
            string gid = Encryption_Decryption.DecryptString(Encryption_Decryption.DecryptString(muser.adid));
            var empcount = IMSobj.getEmployeeByGlobalID(gid).FirstOrDefault();
            int empid = 0;
            string rolenamestr = "imsuser";
            string rolecodestr = "RL_000003";
            if (empcount == null)
            {
                IMS_EmployeeDetailListByGlobalID_Result empdetails = IMSKMobj.IMS_EmployeeDetailListByGlobalID(gid).FirstOrDefault();
                if (empdetails != null)
                {
                    string usernamestr = empdetails.Text.Split('-')[1].ToString();
                    insertemployeesroleandapprovernew_Result Resultxx = IMSobj.insertemployeesroleandapprovernew(0, usernamestr, empdetails.EMAIL, empdetails.ADUserId, empdetails.USER_ID, 3, null, empdetails.Value, empdetails.locationid, empdetails.Functions,
                    empdetails.DEPT, empdetails.managerid, empdetails.hodid, empdetails.Functionname, empdetails.deptname, "", "Auto Created On Team Assignment").FirstOrDefault();
                    empid = Convert.ToInt32(Resultxx.empid);
                    t_result.id = empid;
                    t_result.adid = empdetails.ADUserId;
                    t_result.name = usernamestr;
                    t_result.email = empdetails.EMAIL;
                    t_result.globalid = empdetails.Value;
                    t_result.userid = empdetails.USER_ID;
                    t_result.locationid = empdetails.locationid;
                    t_result.rolename = rolenamestr;
                    t_result.locationname = "";
                    t_result.token = GetToken(empdetails.ADUserId, usernamestr);
                    t_result.rolecode = rolecodestr;

                }
            }
            else
            {
                empid = empcount.id;
                rolenamestr = empcount.rolename;
                rolecodestr = empcount.rolecode;
                t_result.id = empid;
                t_result.adid = empcount.adid;
                t_result.name = empcount.name;
                t_result.email = empcount.email;
                t_result.globalid = empcount.globalid;
                t_result.userid = empcount.userid;
                t_result.locationid = empcount.locationid;
                t_result.rolename = rolenamestr;
                t_result.locationname = empcount.locationname;
                t_result.token = GetToken(empcount.adid, empcount.name); ;
                t_result.rolecode = rolecodestr;
            }



            return Request.CreateResponse(HttpStatusCode.OK, t_result);


        }


        public getEmployeeByGlobalID_Result EmployeeDetailbyGlobalid(string globalid)
        {
            getEmployeeByGlobalID_Result rslt = IMSobj.getEmployeeByGlobalID(globalid).FirstOrDefault();
            if (rslt != null)
            {
                rslt.token = GetToken(rslt.adid, rslt.name);
            }
            return rslt;
        }

        public checkQRLogin_Result checkQRLogin(MUser muser)
        {
            string wholeString = muser.UserName;
            string firstBit = wholeString.Split('@')[0];
            string pass = Encryption_Decryption.EncryptString(muser.Password);
            checkQRLogin_Result chkresult = IMSobj.checkQRLogin(firstBit, pass).FirstOrDefault();
            if (chkresult != null)
            {
                chkresult.token = GetToken(chkresult.adid, chkresult.name);
            }
            return chkresult;
        }


        public Customresponse getValidEmployeeList(MUser muser)
        {
            string ADUserID;
            string ADUserName;
            string ADUserGlobalId;
            string ADuserAccountControl;
            string DomainName = "neulandlabs";
            Customresponse rslt = new Customresponse();
            List<spChkUserAD_Result> emp = new List<spChkUserAD_Result>();
            string result = IsAuthenticatedForms(DomainName, muser.UserName, muser.Password, out ADUserID, out ADUserName, out ADUserGlobalId, out ADuserAccountControl);
            if (result == "true")
            {
                emp = IMSKMobj.spChkUserAD(ADUserGlobalId, muser.Password, "0", ADUserID).ToList();
                if (emp == null || emp.Count == 0)
                {
                    rslt.status = HttpStatusCode.BadRequest;
                    rslt.emp2 = null;
                    return rslt;
                }
                else
                {
                    var empobj = emp.FirstOrDefault();
                    var empcount = IMSobj.getEmployeeByGlobalID(empobj.GLOBALID).FirstOrDefault();
                    int empid = 0;
                    string rolenamestr = "imsuser";
                    string rolecodestr = "RL_000003";
                    if (empcount == null)
                    {
                        IMS_EmployeeDetailListByGlobalID_Result empdetails = IMSKMobj.IMS_EmployeeDetailListByGlobalID(empobj.GLOBALID).FirstOrDefault();
                        if (empdetails == null)
                        {
                            insertemployeesroleandapprovernew_Result Result = IMSobj.insertemployeesroleandapprovernew(0, empobj.USER_NAME, empobj.Email, empobj.aduserid, empobj.USER_ID, 3, null, empobj.GLOBALID, empobj.LOCATION, empobj.Functions,
                             empobj.DEPT, "", "", "", "", "", "Auto Created On logged In").FirstOrDefault();
                            empid = Convert.ToInt32(Result.empid);
                        }
                        else
                        {
                            insertemployeesroleandapprovernew_Result Result = IMSobj.insertemployeesroleandapprovernew(0, empobj.USER_NAME, empobj.Email, empobj.aduserid, empobj.USER_ID, 3, null, empobj.GLOBALID, empobj.LOCATION, empobj.Functions,
                            empobj.DEPT, empdetails.managerid, empdetails.hodid, empdetails.Functionname, empdetails.deptname, "", "Auto Created On logged In").FirstOrDefault();
                            empid = Convert.ToInt32(Result.empid);
                        }




                    }
                    else
                    {
                        empid = empcount.id;
                        rolenamestr = empcount.rolename;
                        rolecodestr = empcount.rolecode;
                    }
                    getEmployeeByGlobalID_Result t_result = new getEmployeeByGlobalID_Result();
                    t_result.id = empid;
                    t_result.adid = empobj.aduserid;
                    t_result.name = empobj.USER_NAME;
                    t_result.email = empobj.Email;
                    t_result.globalid = empobj.GLOBALID;
                    t_result.userid = empobj.USER_ID;
                    t_result.locationid = empobj.LOCATION;
                    t_result.rolename = rolenamestr;
                    t_result.locationname = empobj.LOCATION_TEXT;
                    t_result.token = GetToken(empobj.aduserid, empobj.USER_NAME);
                    t_result.rolecode = rolecodestr;


                    rslt.emp1 = t_result;
                    rslt.status = HttpStatusCode.OK;
                }


                return rslt;
            }
            else if (result == "locked")
            {
                rslt.status = HttpStatusCode.GatewayTimeout;
                rslt.emp1 = null;
                return rslt;
            }
            else
            {

                rslt.status = HttpStatusCode.BadRequest;
                rslt.emp1 = null;
                return rslt;
            }

        }




        public string GetToken(string adid, string username)
        {
            string key = "IMSAPPLICATIONNLLRQSML4756789SECRETKEYaqfgrysdjsfihwiuhiu"; //Secret key which will be used later during validation  
            string iss_usr = ConfigurationManager.AppSettings["ISSUSER"].ToString();
            var issuer = iss_usr;  //normally this will be your site URL    

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            permClaims.Add(new Claim("valid", "1"));
            permClaims.Add(new Claim("adid", adid));
            permClaims.Add(new Claim("username", username));

            //Create Security Token object by giving required parameters    
            var token = new JwtSecurityToken(issuer, //Issure    
                            issuer,  //Audience    
                            permClaims,
                            expires: DateTime.Now.AddHours(4),
                            signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public void Unlock(string userDn)
        {
            try
            {
                DirectoryEntry uEntry = new DirectoryEntry(userDn);
                uEntry.Properties["LockOutTime"].Value = 0; //unlock account

                uEntry.CommitChanges(); //may not be needed but adding it anyways

                uEntry.Close();
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException E)
            {
                //DoSomethingWith --> E.Message.ToString();

            }
        }

        public bool isUserlocked(string username)
        {
            // set up domain context
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain);

            // find a user
            UserPrincipal user = UserPrincipal.FindByIdentity(ctx, username);

            if (user != null)
            {
                string displayName = user.DisplayName;

                if (user.IsAccountLockedOut())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private string IsAuthenticatedForms(string DomainName, string UserName, string Password, out string ADUserID, out string ADUserName, out string ADUserGlobalId, out string ADuserAccountControl)
        {

            ADUserID = "";
            ADUserName = "";
            ADUserGlobalId = "";
            ADuserAccountControl = "";

            string adPath = "LDAP://neulandlabs.com";

            string wholeString = UserName;
            string firstBit = wholeString.Split('@')[0];
            UserName = firstBit;

            string domainAndUsername = DomainName + @"\" + UserName;

            bool islocked1 = isUserlocked(UserName);
            if (islocked1)
            {
                return "locked";
            }

            System.DirectoryServices.DirectoryEntry entry = new DirectoryEntry(adPath, domainAndUsername, Password);
            //bool islocked = Convert.ToBoolean(entry.InvokeGet("IsAccountLocked"));
            try
            {
                //Bind to the native AdsObject to force authentication.
                object obj = entry.NativeObject;

                //if (entry.SchemaClassName == "User")
                //{


                //}            

                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(SAMAccountName=" + UserName + ")";

                //UserId
                search.PropertiesToLoad.Add("SAMAccountName");

                //CN or Display Name
                search.PropertiesToLoad.Add("cn");

                //GlobalID
                search.PropertiesToLoad.Add("Description");

                //status
                search.PropertiesToLoad.Add("userAccountControl");

                search.PropertiesToLoad.Add("lockoutTime");


                SearchResult result = search.FindOne();

                //string outtime = result.Properties["lockoutTime"][0].ToString();
                //if (outtime != "0")
                //{

                //    long value1 = (long)result.Properties["lockoutTime"][0];
                //    DateTime pwdLastSet1 = DateTime.FromFileTime(value1);
                //    DateTime currnt = DateTime.Now;
                //    TimeSpan span = currnt.Subtract(pwdLastSet1);
                //    double mins = span.TotalMinutes;
                //    if(mins <= 5)
                //    {
                //        return "locked";
                //    }
                //    else{

                //    }
                //}




                if (null == result)
                {
                    return "false";
                }
                else
                {
                    //Update the new path to the user in the directory.
                    //Response.Write(result.Path);
                    //Session["ADUserID"] = string.Empty;
                    //Session["ADUserName"] = string.Empty;
                    //Session["ADUserGlobalId"] = string.Empty;
                    //Session["ADuserAccountControl"] = string.Empty;

                    //ADUser UserId
                    //Session["ADUserID"] = result.Properties["SAMAccountName"][0];
                    ADUserID = Convert.ToString(result.Properties["SAMAccountName"][0]);

                    //AD UserName
                    // Session["ADUserName"] = result.Properties["cn"][0];
                    ADUserName = Convert.ToString(result.Properties["cn"][0]);

                    //AD GlobalID
                    // Session["ADUserGlobalId"] = "000" + result.Properties["Description"][0];
                    ADUserGlobalId = Convert.ToString("000" + result.Properties["Description"][0]);

                    //AD ENABLE/DISABLE Status Flag
                    //  Session["ADuserAccountControl"] = Convert.ToString(result.Properties["userAccountControl"][0]);
                    ADuserAccountControl = Convert.ToString(result.Properties["userAccountControl"][0]);


                }
            }
            catch (Exception ex)
            {
                return "false";
                //throw new Exception("Error authenticating user. " + ex.Message);
            }
            return "true";
        }
    }
}
