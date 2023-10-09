using MongoDB.Driver;
using web_server.Collections;
using web_server.Repository;
using WEB_SERVER.Collections;

namespace WEB_SERVER.Repository
{
    public class TrainsRepository : ITrainsRepository
    {
        private readonly IMongoCollection<Train> _mongoTrainCollection;
        private readonly IMongoCollection<TicketBooking> _mongoTicketCollection;
        public TrainsRepository(IMongoDatabase mongoDatabase)
        {
            _mongoTrainCollection = mongoDatabase.GetCollection<Train>("Train");
            _mongoTicketCollection = mongoDatabase.GetCollection<TicketBooking>("Ticket");
        }
        public async Task CreateSchedule(Train newTrain)
        {
            await _mongoTrainCollection.InsertOneAsync(newTrain);
        }
        public async Task<Train> FindAlredayAddedScehedules(string startStation, string endStation)
        {
            return await _mongoTrainCollection.Find(Train => Train.StartStation == startStation && Train.EndStation == endStation).FirstOrDefaultAsync();
        }
        public async Task<List<Train>> GetAllSchedules()
        {
            return await _mongoTrainCollection.Find(_ => true).ToListAsync();
        }
        public async Task<bool> UpdateScheduleAsync(string id, Train updatedSchedule)
        {
            var filter = Builders<Train>.Filter.Eq(s => s.Id, id);
            var update = Builders<Train>.Update

                .Set(s => s.TrainName, updatedSchedule.TrainName)

                .Set(s => s.StartStation, updatedSchedule.StartStation)

                .Set(s => s.EndStation, updatedSchedule.EndStation)

                .Set(s => s.StartTime, updatedSchedule.StartTime)

                .Set(s => s.EndTime, updatedSchedule.EndTime)

                .Set(s => s.CreatedDate, updatedSchedule.CreatedDate);

            var result = await _mongoTrainCollection.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0;

        }

        public async Task<bool> HasBookingsForTrainAsync(string trainId)
        {
            var hasBookings = await _mongoTicketCollection
                .Find(ticket => ticket.TrainId == trainId)
                .AnyAsync();

            return hasBookings;
        }
        public async Task UpdateTrainStatusAsync(string id, bool isActive)
        {
            var filter = Builders<Train>.Filter.Eq(train => train.Id, id);
            var update = Builders<Train>.Update.Set(train => train.IsActive, isActive);
            await _mongoTrainCollection.UpdateOneAsync(filter, update);
        }







    }

}
