using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HireHive.Api.Areas.Common.Controllers
{
#if DEBUG
    [AllowAnonymous]
#endif
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ApiController : ControllerBase
    {
        #region DI

        protected readonly IMapper Mapper;
        protected readonly ILogger<ApiController> Logger;

        protected ApiController(
            IMapper mapper,
            ILogger<ApiController> logger)
        {
            Mapper = mapper;
            Logger = logger;
        }

        #endregion



        protected string? UserId
        {
            get
            {
                if (User.Identity is { IsAuthenticated: true })
                {
                    return ((ClaimsIdentity)User.Identity).Claims
                        .Where(c => c.Type == ClaimTypes.NameIdentifier)
                        .Select(c => c.Value)
                        .FirstOrDefault();
                }

                return null;
            }
        }



        //Returns the ID of the current authenticated user.
        protected string? GetUserId()
        {
            if (User.Identity is { IsAuthenticated: true })
            {
                return ((ClaimsIdentity)User.Identity).Claims
                    .Where(c => c.Type == ClaimTypes.NameIdentifier)
                    .Select(c => c.Value)
                    .FirstOrDefault();
            }

#if DEBUG
            return "-";
#else
            throw new Exception("Can't find user's username claim.");
#endif
        }

        //Returns the email of the current authenticated user.
        protected string? GetUserEmail()
        {
            if (User.Identity is { IsAuthenticated: true })
            {
                return ((ClaimsIdentity)User.Identity).Claims
                    .Where(c => c.Type == ClaimTypes.Email)
                    .Select(c => c.Value)
                    .FirstOrDefault();
            }

#if DEBUG
            return "-";
#else
            throw new Exception("Can't find user's email claim.");
#endif
        }

        //Returns the role of the current authenticated user.
        protected string GetUserRole()
        {
            if (User.Identity is { IsAuthenticated: true })
            {
                return ((ClaimsIdentity)User.Identity).Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value)
                    .First();
            }

#if DEBUG
            return "-";
#else
            throw new Exception("Can't find user's role claim.");
#endif
        }
    }
}
