using Microsoft.EntityFrameworkCore;
using Zavrsni.TeamOps.Entity;
using Zavrsni.TeamOps.UserDomain.Repository;

namespace Zavrsni.TeamOps.OrganizationDomain.Validators
{
    public class OrganizationValidator : IOrganizationValidator
    {
        private readonly IUserRepository _userRepository;

        public OrganizationValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> OrganizationHasUser(Guid userId,Guid organizationId)
        {
            return await _userRepository.OrganizationHasUser(userId,organizationId);
        }
    }
}
