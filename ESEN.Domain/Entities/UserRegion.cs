namespace ESEN.Domain.Entities
{
    public class UserRegion : BaseEntity
    {
        public Guid UserId { get; private set; }
        public Guid RegionId { get; private set; }

        public virtual User User { get; private set; }
        public virtual Region Region { get; private set; }

        public UserRegion(Guid userId, Guid regionId)
        {
            UserId = userId;
            RegionId = regionId;
        }
    }
}