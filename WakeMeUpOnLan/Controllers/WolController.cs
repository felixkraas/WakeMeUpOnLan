using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WakeMeUpOnLan.Core.Models;
using WakeMeUpOnLan.Services;

namespace WakeMeUpOnLan.Controllers {
    [Route( "api/wol" )]
    [ApiController]
    public class WolController : ControllerBase {

        private WolContext _dataContext;

        public WolController( WolContext dataContext ) {
            _dataContext = dataContext;
        }

        [HttpGet( "{id}" )]
        public async Task<IActionResult> StartWolTarget( Guid id ) {
            IActionResult result = new BadRequestResult();

            var target = await _dataContext.WolTargets.Include( t => t.ApiUsers ).FirstOrDefaultAsync( t => t.Id == id );

            if( target != null ) {
                if( target.ApiUsers.Any( au => au.ApiKey == HttpContext.Request.Headers["ApiKey"] ) ) {

                    await LanWaker.WakeOnLan( target.TargetMacAddress );

                    result = new OkResult();
                } else {
                    result = new ForbidResult();
                }
            } else {
                result = new NotFoundObjectResult( id );
            }

            return result;
        }

    }
}
