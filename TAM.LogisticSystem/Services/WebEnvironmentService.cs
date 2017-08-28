using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace TAM.LogisticSystem.Services
{
    public class WebEnvironmentService
    {
        private readonly IHttpContextAccessor Context;
        private readonly ConfigurationWatch Watch;

        private string _ChangePageUrl { set; get; }

        public WebEnvironmentService(ConfigurationWatch watch, IHttpContextAccessor context)
        {
            this.Watch = watch;
            this.Context = context;
        }

        public string TamPassportUrl => Watch.PassportUrl;

        public Guid TamPassportAppId => Guid.Parse(Watch.PassportAppId);

        public int ExcelMaxRow => Watch.ExcelMaxRow;

        public string TLSUser => Watch.TLSUser;

        public string TLSPassword => Watch.TLSPassword;

        public string DMSUser => Watch.DMSUser;

        public string DMSPassword => Watch.DMSPassword;

        public string ChangePageUrl(int page)
        {
            if (string.IsNullOrEmpty(_ChangePageUrl) == false)
            {
                return _ChangePageUrl + page;
            }

            var queries = Context.HttpContext.Request.Query.ToDictionary(Q => Q.Key, Q => Q.Value.ToList());
            if (queries.ContainsKey("page"))
            {
                queries.Remove("page");
            }

            var hasQuery = queries.Any();
            if (hasQuery == false)
            {
                _ChangePageUrl = "?page=";
                return _ChangePageUrl + page;
            }

            var parameters = new List<string>();
            foreach (var query in queries)
            {
                foreach (var parameter in query.Value)
                {
                    parameters.Add($"{query.Key}={Uri.EscapeDataString(parameter)}");
                }
            }
            _ChangePageUrl = $"?{string.Join("&", parameters)}&page=";
            return _ChangePageUrl + page;
        }

        public ClaimsPrincipal GetCurrentUserPrincipal()
        {
            return Context.HttpContext.User;
        }

        public string UserHumanName => Context.HttpContext.User.Identity.Name;
        public string UserLocation => Context.HttpContext.User.Claims.FirstOrDefault(Q=>Q.Type == "Location")?.Value;

        public string Username => Context.HttpContext.User.Claims.FirstOrDefault(Q => Q.Type == ClaimTypes.NameIdentifier)?.Value;

        public string DMSDomainUrl => this.Watch.DMSDomainUrl;
    }
}
