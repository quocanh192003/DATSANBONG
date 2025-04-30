using DATSANBONG.Models;
using DATSANBONG.Models.DTO;
using DATSANBONG.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace DATSANBONG.Controllers
{
    [Route("api/football")]
    [ApiController]
    public class FootballFieldController : Controller
    {
        private readonly IFootballFieldRepository _football;
        
        public FootballFieldController(IFootballFieldRepository football)
        {
            _football = football;
        }

        //ADMIN CONFIRM FOOTBALL
        [HttpPost("create-football")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Chủ Sân", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreateFootBall([FromBody] CreateFootballDTO request)
        {
            
            var response = await _football.CreateFootBall(request);
            return StatusCode((int)response.Status, response);
        }

    }
}
