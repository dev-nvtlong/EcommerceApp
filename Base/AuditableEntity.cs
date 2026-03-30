using Microsoft.AspNetCore.Identity;

namespace EcommerceApp.Base
{
    public abstract class AuditableEntity : BaseEntity
    {
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public int? CreateByUserID { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedByUserID { get; set; }
        public bool ? IsDeleted { get; set; }
    }
}
