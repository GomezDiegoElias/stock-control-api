namespace org.pos.software.Infrastructure.Rest.Dto.Request
{
    public class UpdateRolePermissionsRequest
    {
        public IEnumerable<string> AddPermissions { get; set; } = new List<string>();
        public IEnumerable<string> RemovePermissions { get; set; } = new List<string>();
    }
}
