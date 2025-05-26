using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Configuration;

[assembly: OwinStartup(typeof(IMSAPI.Startup))]

namespace IMSAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            string key = "IMSAPPLICATIONNLLRQSML4756789SECRETKEYaqfgrysdjsfihwiuhiu"; //Secret key which will be used later during validation  
            string iss_usr = ConfigurationManager.AppSettings["ISSUSER"].ToString();
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = iss_usr, //some string, normally web url,  
                        ValidAudience = iss_usr,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                    }
                });
        }
    }
}
