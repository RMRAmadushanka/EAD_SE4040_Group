using web_server.Collections;

namespace web_server.Repository
{
    public interface ITicketBookingRepository
    {
        Task CreateReservation(string userId, string trainId, TicketBooking newBooking);
        List<TicketBooking> GetBookingsByUserId(string userId);
        Task<bool> UpdateReservationDateAsync(string id, string newReservationDate);
        TicketBooking GetReservationById(string reservationId);
        void CancelReservation(string reservationId);
    }
}
