namespace Zavrsni.TeamOps.Features.Organizations.Validators
{
    public interface IOrganizationValidator
    {
        Task<bool> OrganizationHasUser(Guid userId, Guid organizationId);
    }
}
