namespace BTKRoadmapperAPI.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; } 
        public ICollection<UserPreference> Preferences { get; set; } 
    }

}
