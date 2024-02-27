namespace Zavrsni.TeamOps.OrganizationDomain.Validators
{
    public interface IOrganizationValidator
    {
        Task<bool> OrganizationHasUser(Guid userId, Guid organizationId);
    }
}
