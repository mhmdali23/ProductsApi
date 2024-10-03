using WebAppApi.Enums;

namespace WebAppApi.Models
{
    public class UserPermission
    {
        public int UserId { get; set; }
        public Permission PermissionId { get; set; }
    }
}
