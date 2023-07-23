using Dapper;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Common.Dapper
{
    public class UserRepositoryDapper
    {
        private readonly SqlConnection _db;

        public UserRepositoryDapper(string connectionString) => _db = new SqlConnection(connectionString);

        public async Task<List<User>> GetUsersAsync()
        {
            const string query = "Select * from User;";

            var users = await _db.QueryAsync<User>(query);

            return users.ToList();
        }


        public async Task<User> AddUserAynsc(User user)
        {
            const string query =
                "Insert into User(Name, Email, Password) Values(@Name, @Email, @Password);";

            int id = await _db.ExecuteScalarAsync<int>(query, user);

            user.Id = id;

            return user;
        }

        public async Task<User> UpdateUserAync(User user)
        {
            const string query = @"Update User Set Name = @Name, Email = @Email, Password = @Password where Id = @Id";

            await _db.ExecuteAsync(query, user);

            return user;
        }

        public async Task RemoveUserAsync(int id)
        {
            const string query = "Delete User Where Id = @Id";

            await _db.ExecuteAsync(query, new { id }, commandType:System.Data.CommandType.Text);
        }
    }
}
