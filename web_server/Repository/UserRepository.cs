using MongoDB.Driver;
using WEB_SERVER.Collections;

 

namespace WEB_SERVER.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _mongoUserCollection;

 

        public UserRepository(IMongoDatabase mongoDatabase)
        {
            _mongoUserCollection = mongoDatabase.GetCollection<User>("User");
        }

 

        public async Task<List<User>> GetAllUsers()
        {
            return await _mongoUserCollection.Find(_ => true).ToListAsync();
        }

 

        public async Task CreateUser(User newUser)
        {
            await _mongoUserCollection.InsertOneAsync(newUser);
        }

 

        public async Task<User> GetUserById(string id)
        {
            return await _mongoUserCollection.Find(_ => _.Id == id).FirstOrDefaultAsync();
        }

 

        public async Task<User> GetUserByNICAsync(string nic)
        {
            return await _mongoUserCollection.Find(user => user.NIC == nic).FirstOrDefaultAsync();
        }

 

 

        public async Task UpdateUser(User UserToUpdate)
        {
            await _mongoUserCollection.ReplaceOneAsync(_ => _.Id == UserToUpdate.Id, UserToUpdate);
        }

 

        public async Task DeleteAsync(string id)
        {
            await _mongoUserCollection.DeleteOneAsync(_ => _.Id == id);
        }

 

        public async Task DeactivateUser(string id)
        {
            var update = Builders<User>.Update.Set(u => u.IsActive, false);
            _mongoUserCollection.UpdateOne(u => u.Id == id, update);
        }

 

        public async Task ActivateUser(string id, bool isActive)
        {
            var update = Builders<User>.Update.Set(u => u.IsActive, isActive);
            _mongoUserCollection.UpdateOne(u => u.Id == id, update);
        }

 

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _mongoUserCollection.Find(user => user.Username == username).FirstOrDefaultAsync();
        }

 

        public async Task<User> GetUserByUsernameAndPasswordAsync(string username, string password)
        {
            return await _mongoUserCollection.Find(user => user.Username == username && user.Password == password).FirstOrDefaultAsync();
        }

 

        public async Task<List<User>> GetUserReservations(string userId)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(tb => tb.Id, userId);
                return await _mongoUserCollection.Find(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or return an appropriate response.
                Console.WriteLine($"Error in GetUserReservations: {ex.Message}");
                throw; // You may choose to throw the exception or return an error response.
            }
        }



    }
}