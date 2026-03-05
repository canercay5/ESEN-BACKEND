namespace ESEN.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public string Neighborhood { get; set; }
        public string DeviceToken { get; private set; }

        private readonly List<UserRegion> _followedRegions = new();
        public IReadOnlyCollection<UserRegion> FollowedRegions => _followedRegions.AsReadOnly();

        public User(string deviceToken)
        {
            DeviceToken = deviceToken;
        }

        public void UpdateDeviceToken(string newToken)
        {
            DeviceToken = newToken;
        }
        public void FollowRegion(Region region)
        {
            if (!_followedRegions.Any(ur => ur.RegionId == region.Id))
            {
                _followedRegions.Add(new UserRegion(this.Id, region.Id));
            }
        }
        public void UnfollowRegion(Guid regionId)
        {
            var userRegion = _followedRegions.FirstOrDefault(ur => ur.RegionId == regionId);
            if (userRegion != null)
            {
                _followedRegions.Remove(userRegion);
            }
        }
    }
}