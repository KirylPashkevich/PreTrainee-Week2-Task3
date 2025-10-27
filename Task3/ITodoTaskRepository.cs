using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task3.Models;

namespace Task3
{
    internal interface ITodoTaskRepository
    {
         public  Task<int> Add(TodoTask task);
         public Task<List<TodoTask>> GetAll();
         public Task Update(TodoTask task);
         public Task Delete(int id);
    }
}
