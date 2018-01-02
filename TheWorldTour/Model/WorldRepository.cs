using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorldTour.Model
{
    public class WorldRepository : IWorldRepository
    {
        private WorldContext _context;
        private ILogger<WorldRepository> _logger;

        public WorldRepository( WorldContext context, ILogger<WorldRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void AddStop(string tripName, string username, Stop newStop)
        {
            var trip = GetUserTripByName(tripName, username);
            
            if(trip !=null)
            {
                trip.Stops.Add(newStop);
                _context.Stops.Add(newStop);
            }
        }
		
        public IEnumerable<Trip> GetAllTrips()
        {
            _logger.LogInformation("Trying to get all the trips");
            return _context.Trips.ToList();
        }

        public void AddTrip(Trip trip)
        {
            _context.Add(trip);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public Trip GetTripByName(string tripName)
        {
            return _context.Trips
                .Include( a => a.Stops)
                .Where(a => a.Name == tripName)
                .FirstOrDefault();
        }

        

        public IEnumerable<Trip> GetTripsByUsername(string name)
        {
            return _context.Trips
                .Include(a => a.Stops)
                .Where(a => a.UserName == name)
                .ToList();
        }

        public Trip GetUserTripByName(string tripName, string name)
        {
            return _context.Trips
                .Include(a=> a.Stops)
                .Where(a=> a.Name == tripName && a.UserName == name )
                .FirstOrDefault();
        }
    }
}
