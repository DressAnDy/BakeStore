namespace CakeStoreBE.Utils.JWTProcess
{
    public class JwtOptions
    {
        public string key {  get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int AccessTokenExpiryMinutes { get; set; }
        public int RefreshTokenExpiryDays { get; set; }
    }
}
