using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheWorldTour.Model;
using TheWorldTour.ViewModels;

namespace TheWorldTour.Controllers.Api
{
    [Route("api/trips")]
    [Authorize] 
    public class TripsController : Controller
    {
        
        private ILogger<TripsController> _logger;
		private IWorldRepository _repository;

        public TripsController(IWorldRepository repository, ILogger<TripsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {
                var results = _repository.GetTripsByUsername(User.Identity.Name);
                return Ok(Mapper.Map<IEnumerable<TripViewModel>>(results));
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return BadRequest("Error getting all trips");
            }
        }

        [HttpPost("")]
        public async  Task<IActionResult> Post([FromBody]TripViewModel theTrip)
        {
            if(ModelState.IsValid)
            {
                var newTrip = Mapper.Map<Trip>(theTrip);

                newTrip.UserName = User.Identity.Name;

                _repository.AddTrip(newTrip);

                if(await _repository.SaveChangesAsync())
                {
                    return Created($"api/trips/{theTrip.Name}", Mapper.Map<TripViewModel>(newTrip));
                }      
            }
            return BadRequest("Failed to save trip to DataBase");
        }
    }
}
