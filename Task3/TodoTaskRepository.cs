using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task3.Models;


namespace Task3
{
    internal class TodoTaskRepository : ITodoTaskRepository
    {
        private readonly string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=MyTasksDb;Trusted_Connection=True;";

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);

        }

        public async Task<int> Add(TodoTask task)
        {
            string sql = @"INSERT INTO Tasks (Title, Description, IsCompleted, CreatedAt) 
                           VALUES (@Title, @Description, @IsCompleted, @CreatedAt);
                           SELECT CAST(SCOPE_IDENTITY() as int);";

            using (IDbConnection connection = CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<int>(sql, task);
            }
        }
        public async Task<List<TodoTask>> GetAll()
        {
            string sql = @"Select * FROM Tasks";
            using (IDbConnection connection = CreateConnection())
            {
                var tasks = await connection.QueryAsync<TodoTask>(sql);
                return tasks.ToList();
            }
        }
        public async Task Update(TodoTask task)
        {
            int id = task.Id;
            string sql = @"UPDATE Tasks
                           SET Title = @Title, Description = @Description, IsCompleted = @Iscompleted, CreatedAt = @CreatedAt
                           WHERE Id = @id";



            using (IDbConnection connection = CreateConnection())
            {
                await connection.ExecuteAsync(sql, task);

            }
        }
        public async Task Delete(int id)
        {
            string sql = @"DELETE FROM Tasks
                           WHERE id = @id";
            using (IDbConnection connection = CreateConnection())
            {
                await connection.ExecuteAsync(sql, new { id = id});


            }
        }
    }
}
