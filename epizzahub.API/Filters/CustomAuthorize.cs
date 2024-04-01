using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace epizzahub.API.Filters
{
    public class CustomAuthorize : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string authorization = context.HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authorization))
            {
                context.Result = new UnauthorizedResult();
            }
            else if (authorization.StartsWith("Bearer"))
            {
                string token = authorization.Substring("Bearer ".Length).Trim();
                if (!string.IsNullOrEmpty(token))
                {
                    try
                    {
                        var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
                        string jwtKey = config["Jwt:Key"];
                        string jwtIssuer = config["Jwt:Issuer"];
                        string jwtAudience = config["Jwt:Audience"];

                        SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                        TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = securityKey,
                            ValidateIssuer = true,
                            ValidIssuer = jwtIssuer,
                            ValidateAudience = true,
                            ValidAudience = jwtAudience,
                            ValidateLifetime = true
                        };
                        SecurityToken validatedToken;
                        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                        var user = handler.ValidateToken(token, tokenValidationParameters, out validatedToken);
                        if (!user.Identity.IsAuthenticated)
                        {
                            context.Result = new UnauthorizedResult();
                        }
                    }
                    catch (SecurityTokenValidationException e)
                    {
                        context.Result = new UnauthorizedResult();
                    }
                    catch (Exception ex)
                    {
                        context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                    }
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}