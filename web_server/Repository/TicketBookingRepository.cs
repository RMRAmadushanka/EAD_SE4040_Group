using MongoDB.Bson;
using MongoDB.Driver;
using web_server.Repository;
using WEB_SERVER.Collections;

namespace web_server.Repository
{
    public class TicketBookingRepository : ITicketBookingRepository
    {
        private readonly IMongoCollection<TicketBooking> _mongoTicketCollection;
        private readonly ITrainsRepository _trainsRepository;
        private readonly IUserRepository _userRepository;

        public TicketBookingRepository(IMongoDatabase mongoDatabase, ITrainsRepository trainsRepository, IUserRepository userRepository)
        {
            _mongoTicketCollection = mongoDatabase.GetCollection<TicketBooking>("Ticket");
            _trainsRepository = trainsRepository;
            _userRepository = userRepository;
        }

        public async Task CreateReservation(string userId, string trainId, TicketBooking newBooking)
        {
            // Retrieve train details based on the trainId
            var trainDetails = await _trainsRepository.GetTrainScheduleById(trainId);

            if (trainDetails == null)
            {
                throw new Exception("Train details not found.");
            }

            // Retrieve user information based on the userId
            var userInfo = await _userRepository.GetUserById(userId);

            if (userInfo == null)
            {
                throw new Exception("User information not found.");
            }
            //Add train Id to user



            // Check if the user has already made 4 reservations
            var userReservations = await _userRepository.GetTicketBookingCountForUser(userId);
            Console.WriteLine($"Count of reserva: {userReservations}");
            if (userReservations >= 4)
            {
                throw new Exception("Maximum 4 reservations per user are allowed.");
            }

            // Check if the reservation date is within 30 days from the booking date
            //if ((newBooking.ReservationDate - DateTime.UtcNow).Days > 30)
            //{
            //  throw new Exception("Reservation date must be within 30 days from the booking date.");
            // }

            // Set the booking details
            newBooking.ReservationDate = newBooking.ReservationDate;
            newBooking.TrainName = trainDetails.TrainName;
            newBooking.UserId = userId;
            newBooking.UserNIC = userInfo.NIC;
            newBooking.UserName = userInfo.Username;
            newBooking.TrainName = trainDetails.TrainName;
            newBooking.TrainId = trainDetails.Id;
            newBooking.BookingStatus = "Pending";
            newBooking.CreatedDate = DateTime.UtcNow;

            // Insert the new booking into the MongoDB collection
            await _mongoTicketCollection.InsertOneAsync(newBooking);
            await _userRepository.AddTrainIdToUser(userId, trainId);
        }


        public List<TicketBooking> GetBookingsByUserId(string userId)
        {
            var filter = Builders<TicketBooking>.Filter.Eq(x => x.UserId, userId);
            return _mongoTicketCollection.Find(filter).ToList();
        }


        public async Task<bool> UpdateReservationDateAsync(string id, string newReservationDate)
        {
            try
            {
                var filter = Builders<TicketBooking>.Filter.Eq("Id", id);
                var update = Builders<TicketBooking>.Update.Set("ReservationDate", newReservationDate);

                var result = await _mongoTicketCollection.UpdateOneAsync(filter, update);

                return result.ModifiedCount == 1;
            }
            catch (Exception)
            {
                // Handle exceptions appropriately.
                return false;
            }
        }

    }
}
