using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheWorldTour.Model
{
    public interface IWorldRepository
    {
        IEnumerable<Trip> GetAllTrips();
        IEnumerable<Trip> GetTripsByUsername(string name);

        Trip GetTripByName(string tripName);
        Trip GetUserTripByName(string tripName, string name);

        void AddTrip(Trip newTrip);
        void AddStop(string tripName, string username, Stop newStop);

        Task<bool> SaveChangesAsync();
    }
}
