using System;
using Microsoft.Extensions.Logging;

namespace ShopWebMvc
{
    public class AppSettings
    {
        public string CatalogUrl { get; set; }
        public Logging Logging { get; set; }

    }

    public class Logging
    {
        public bool IncludeScopes { get; set; }
        public LogLevel LogLevel { get; set; }
    }
}
