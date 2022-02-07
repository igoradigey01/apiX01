namespace ShopAPI.Models
{    /// <summary>
     /// Create in AppIdentityDbContext on init
     /// </summary>
    public static class Role
    {
        public const string Admin = "admin";

        public const string Shopper = "shopper"; // покупатель

        public const string Manager = "manager";
    }
}