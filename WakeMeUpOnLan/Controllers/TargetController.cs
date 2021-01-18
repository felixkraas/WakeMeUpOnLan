using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using WakeMeUpOnLan.Core.Models;
using WakeMeUpOnLan.Services;

namespace WakeMeUpOnLan.Controllers {
    [Route( "api/targets" )]
    [ApiController]
    public class TargetController : ControllerBase {
        private readonly WolContext _context;

        public TargetController( WolContext context ) {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WolTarget>>> GetWolTargets() {
            IQueryable<WolTarget> targets = _context.WolTargets.Include( t => t.ApiUsers ).Where( t => t.ApiUsers.Any( au => au.ApiKey == HttpContext.Request.Headers["ApiKey"] ) );
            if( !await targets.AnyAsync() ) {
                return new NoContentResult();
            } else {
                return await _context.WolTargets.ToListAsync();
            }
        }
    }
}
