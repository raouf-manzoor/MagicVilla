﻿namespace MagicVilla_Utility
{
    public static class SD
    {
        public enum APIType
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        public static readonly string SessionToken = "JWTToken";
        public static readonly string CurrentAPIVersion = "v2";

        public const string Admin = "admin";
        public const string Customer = "customer";

    }
}