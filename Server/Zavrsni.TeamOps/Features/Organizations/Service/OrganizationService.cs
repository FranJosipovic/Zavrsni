using AutoMapper;
using Zavrsni.TeamOps.Common;
using Zavrsni.TeamOps.Entity.Models;
using Zavrsni.TeamOps.Features.Organizations.Models;
using Zavrsni.TeamOps.Features.Organizations.Repository;
using Zavrsni.TeamOps.Features.Organizations.Validators;
using Zavrsni.TeamOps.Features.Users.Models.DTOs;
using Zavrsni.TeamOps.Features.Users.Repository;

namespace Zavrsni.TeamOps.Features.Organizations.Service
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

        public async Task<ServiceActionResult> GetOrganizationIdByName(string name)
        {
            ServiceActionResult serviceActionResult = new ServiceActionResult();
            var id = await _organizationRepository.GetIdByNameAsync(name);
            if (id == null)
            {
                serviceActionResult.SetNotFound($"Organization {name} does not exists");
                return serviceActionResult;
            }
            serviceActionResult.SetOk(new { Id = id }, "Action Successfull");
            return serviceActionResult;
        }

        public async Task<ServiceActionResult> Get(Guid organizationId)
        {
            var serviceActionResult = new ServiceActionResult();
            var organization = await _organizationRepository.GetWithRelatedAsync(organizationId);
            serviceActionResult.SetOk(organization, "Action Successfull");
            return serviceActionResult;
        }

        public async Task<ServiceActionResult> RemoveOrganization(Guid organizationId)
        {
            var serviceActionResult = new ServiceActionResult();
            await _organizationRepository.RemoveAsync(organizationId);
            serviceActionResult.SetOk(null, "Organization removed successfully");
            return serviceActionResult;
        }

        public async Task<ServiceActionResult> UpdateOrganization(UpdateOrganizationDTO updateOrganizationDTO)
        {
            var serviceActionResult = new ServiceActionResult();
            var organization = await _organizationRepository.UpdateAsync(updateOrganizationDTO);
            serviceActionResult.SetOk(organization, "Action Successfull");
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
            if (!ownerExists)
            {
                serviceActionResult.SetBadRequest("User does not exists");
                return serviceActionResult;
            }

            var organizations = await _organizationRepository.GetByUserIdAsync(ownerId);
            serviceActionResult.SetOk(new CollectionResponseData<Organization>
            {
                Items = organizations,
                Count = organizations.Count
            }, "Action Successfull");
            return serviceActionResult;
        }

        public async Task<ServiceActionResult> GetOrganizationsWithUserId(Guid userId)
        {
            var serviceActionResult = new ServiceActionResult();

            var userExists = await _userRepository.UserExists(userId);
            if (!userExists)
            {
                serviceActionResult.SetBadRequest("User does not exists");
                return serviceActionResult;
            }

            var organizations = await _organizationRepository.GetWithUserIdAsync(userId);
            serviceActionResult.SetOk(new CollectionResponseData<Organization>
            {
                Items = organizations,
                Count = organizations.Count
            }, "Action Successfull");
            return serviceActionResult;
        }

        public async Task<ServiceActionResult> CreateOrganization(OrganizationPostModel organizationPostModel)
        {
            var serviceActionResult = new ServiceActionResult();
            var user = await _userRepository.GetAsync(organizationPostModel.OwnerId);

            Organization organization = _mapper.Map<Organization>(organizationPostModel);

            organization.CreatedOn = DateTime.Now;
            organization.UpdatedOn = DateTime.Now;

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
                    organization.Id,
                    organization.Name
                },
                User = _mapper.Map<UserNoSensitiveInfoDTO>(user)
            }, "User successfully added to organization");
            return result;
        }
    }
}
