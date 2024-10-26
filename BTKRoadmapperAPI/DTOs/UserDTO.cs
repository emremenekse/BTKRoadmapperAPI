namespace BTKRoadmapperAPI.DTOs
{
    public class UserDTO
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
        public UserPreferencesDTO  UserPreferences { get; set; }
    }
}
