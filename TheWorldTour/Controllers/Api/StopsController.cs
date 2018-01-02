using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorldTour.Controllers.Web.Services;
using TheWorldTour.Model;
using TheWorldTour.ViewModels;

namespace TheWorldTour.Controllers.Api
{
 
    [Route("/api/trips/{tripName}/stops")]
    public class StopsController : Controller
    {
        private IWorldRepository _repository;
        private ILogger<StopsController> _logger;
        private GeoCordService _geoCordService;

        public StopsController(IWorldRepository repository, ILogger<StopsController> logger
            , GeoCordService geoCordService)
        {
            _repository = repository;  	
            _logger = logger;
            _geoCordService = geoCordService;
        }

        [HttpGet("")]
        public IActionResult Get(string tripName)
        {
            var trip = _repository.GetUserTripByName(tripName, this.User.Identity.Name);

            return Ok(Mapper.Map<IEnumerable<StopViewModel>>(trip.Stops.OrderBy(s => s.Order).ToList()));

        }

    [HttpPost("")]
    public async Task<IActionResult> Post(string tripName, [FromBody]StopViewModel vm)
    {
      try
      {
        // If the VM is valid
        if (ModelState.IsValid)
        {
          var newStop = Mapper.Map<Stop>(vm);

                    //lookup GeoCode
                    var result = await _geoCordService.GetGeoCord(newStop.Name);

                    if (!result.Success)
                    {
                        _logger.LogError("Failed to get Geo Cordinate");
                    }
                    else
                    {
                        newStop.Longitude = result.Longitude;
                        newStop.Latitude = result.Latitude;

                        //Save to DB
                        _repository.AddStop(tripName,User.Identity.Name, newStop);

                        if (await _repository.SaveChangesAsync())
                        {
                            return Created($"/api/trips/{tripName}/stops/{newStop.Name}",
                                Mapper.Map<StopViewModel>(newStop));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save stop {ex.Message}");                
            }

            return BadRequest("Failed to save stop");
        }
    }
}
