using web_server.Collections;

 

namespace web_server.Repository
{
    public interface ITrainsRepository
    {
        Task CreateSchedule(Train newTrain);
        Task<Train> FindAlredayAddedScehedules(string startStation, string endStation);
        Task<List<Train>> GetAllSchedules();
        Task<bool> UpdateScheduleAsync(string id, Train updatedSchedule);
    }
}