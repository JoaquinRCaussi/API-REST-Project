using DataAccess.Repositories;
using Domain;
using IBusinessLogic;
using IDataAccess;
using Models;

namespace BusinessLogic; 
    public class SessionLogic : ISessionLogic
    {
        private readonly IUserRepository _repository;

        public SessionLogic(IUserRepository repository)
        {
            _repository = repository;
        }

        public AuthenticationResult Authenticate(string mail, string password)
        {
            var user = _repository.FindByMail(mail);

            if (user == null || user.Password != password)
            {
                throw new Exception("Invalid email or password.");
            }
            
            var userId = user.Id;
            var roleId = user.Role;

            var result = new AuthenticationResult
            {
                UserId = userId,
                RoleId = roleId,
            };

            return result;
        }
        
    }
