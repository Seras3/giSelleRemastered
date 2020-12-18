namespace giSelleRemastered.Models
{
    public class UserRoleType
    {
        public readonly static string USER = "User";
        public readonly static string PARTNER = "Partner";
        public readonly static string ADMIN = "Admin";
        public readonly static string[] ALL = {USER, PARTNER, ADMIN};
    };
}