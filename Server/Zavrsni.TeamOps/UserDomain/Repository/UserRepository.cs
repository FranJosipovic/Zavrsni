using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Zavrsni.TeamOps.Entity;
using Zavrsni.TeamOps.UserDomain.Models;

namespace Zavrsni.TeamOps.UserDomain.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly TeamOpsDbContext _db;
        private readonly IMapper _mapper;

        public UserRepository(TeamOpsDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<DbActionResult> PostAsync(UserSignUpModel userSignUpModel)
        {
            var result = new DbActionResult();
            try
            {
                var user = _mapper.Map<Entity.Models.User>(userSignUpModel);
                _db.Users.Add(user);
                await _db.SaveChangesAsync();
                result.SetResultCreated(user);
                return result;
            }
            catch (Exception)
            {
                result.SetInternalError();
                return result;
            }
        }

        public async Task<DbActionResult> GetAsync(string usernameOrEmail)
        {
            var result = new DbActionResult();
            try
            {
                var user = await _db.Users.Where(u=>u.Username == usernameOrEmail || u.Email == usernameOrEmail).AsNoTracking().FirstOrDefaultAsync();
                if (user is not null)
                {
                    result.SetOk(user);
                }
                else
                {
                    result.SetNotFound();
                }
                return result;
            }
            catch (Exception)
            {
                result.SetInternalError();
                return result;
            }
        }
    }
}
