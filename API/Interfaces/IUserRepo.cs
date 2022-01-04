using API.DTOs;
using Data_Layer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUserRepo
    {
        Task<IEnumerable<Users>> GetUsersAsync();

        Task<Users> Authenticate(string username, string password);

        void Register(AccountsDto user);

        Task<bool> UserAlreadyExists(string username, string email);

        void DeleteUser(int id);

        Task<Users> GetUsersById(int id);
    }
}
