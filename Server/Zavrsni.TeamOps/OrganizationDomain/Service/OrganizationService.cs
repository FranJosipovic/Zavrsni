using AutoMapper;
using Zavrsni.TeamOps.Common;
using Zavrsni.TeamOps.Entity.Models;
using Zavrsni.TeamOps.OrganizationDomain.Models;
using Zavrsni.TeamOps.OrganizationDomain.Repository;
using Zavrsni.TeamOps.OrganizationDomain.Validators;
using Zavrsni.TeamOps.UserDomain.Models;
using Zavrsni.TeamOps.UserDomain.Repository;

namespace Zavrsni.TeamOps.OrganizationDomain.Service
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IOrganizationValidator _organizationValidator;
        public OrganizationService(IOrganizationRepository organizationRepository, IMapper mapper, IUserRepository userRepository, IOrganizationValidator organizationValidator)
        {
            _organizationRepository = organizationRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _organizationValidator = organizationValidator;
        }

        public async Task<ServiceActionResult> RemoveOrganization(Guid organizationId)
        {
            var serviceActionResult = new ServiceActionResult();
            await _organizationRepository.RemoveAsync(organizationId);
            serviceActionResult.SetOk(null,"Organization removed successfully");
            return serviceActionResult;
        }

        public async Task<ServiceActionResult> ChangeOrganizationName(Guid organizationId, string newName)
        {
            var serviceActionResult = new ServiceActionResult();

            var organizationExists = await _organizationRepository.OrganizationExists(organizationId);
            if (!organizationExists)
            {
                serviceActionResult.SetBadRequest("Organization does not exists");
                return serviceActionResult;
            }

            var newOrg = await _organizationRepository.ChangeNameAsync(organizationId, newName);
            serviceActionResult.SetOk(newOrg, "Organization name changed successfully");
            return serviceActionResult;
        }

        public async Task<ServiceActionResult> GetOrganizationsByOwnerId(Guid ownerId)
        {
            var serviceActionResult = new ServiceActionResult();

            var ownerExists = await _userRepository.UserExists(ownerId);
            if(!ownerExists) 
            {
                serviceActionResult.SetBadRequest("User does not exists");
                return serviceActionResult;
            }

            var organizations = await _organizationRepository.GetByUserIdAsync(ownerId);
            serviceActionResult.SetOk(new
            {
                Organizations = organizations,
                Count = organizations.Count
            }, "Action Successfull");
            return serviceActionResult;
        }

        public async Task<ServiceActionResult> CreateOrganization(OrganizationPostModel organizationPostModel)
        {
            var serviceActionResult = new ServiceActionResult();
            var user = await _userRepository.GetAsync(organizationPostModel.OwnerId);

            Organization organization = _mapper.Map<Organization>(organizationPostModel);

            await _organizationRepository.AddAsync(organization);

            Organization newOrganization = await _organizationRepository.GetAsync(organization.Id);
            await _organizationRepository.AddUserToOrganizationAsync(user.Id, organization.Id);
            serviceActionResult.SetOk(organization, "Organization Created succesfully");

            return serviceActionResult;
        }

        public async Task<ServiceActionResult> AddUser(Guid userId, Guid organizationId)
        {
            var result = new ServiceActionResult();
            var user = await _userRepository.GetAsync(userId);

            var organization = await _organizationRepository.GetAsync(organizationId);

            if (await _organizationValidator.OrganizationHasUser(user.Id, organization.Id))
            {
                result.SetBadRequest("Organization already has user");
                return result;
            }

            await _organizationRepository.AddUserToOrganizationAsync(user, organization.Id);
            result.SetOk(new
            {
                Organizaion = new
                {
                    Id = organization.Id,
                    Name = organization.Name
                },
                User = _mapper.Map<UserNoSensitiveInfoDTO>(user)
            }, "User successfully added to organization");
            return result;
        }
    }
}
