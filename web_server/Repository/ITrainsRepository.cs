using WEB_SERVER.Collections;

 

namespace WEB_SERVER.Repository
{
    public interface ITrainsRepository
    {
        Task CreateSchedule(Train newTrain);
        Task<Train> FindAlredayAddedScehedules(string startStation, string endStation);
        Task<List<Train>> GetAllSchedules();
        Task<bool> UpdateScheduleAsync(string id, Train updatedSchedule);
    }
}