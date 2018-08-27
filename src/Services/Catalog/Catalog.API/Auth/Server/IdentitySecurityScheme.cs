namespace AnteyaSidOnContainers.Services.Catalog.API.Auth.Server
{
    using Swashbuckle.AspNetCore.Swagger;
    using System.Collections.Generic;

    public class IdentitySecurityScheme : SecurityScheme
    {
        public IdentitySecurityScheme()
        {
            Type = "IdentitySecurityScheme";
            Description = "Security definition that provides to the user of Swagger a mechanism to obtain a token from the identity service that secures the api";
            Extensions.Add("authorizationUrl", "http://localhost:5101/Auth/Client/popup.html");
            Extensions.Add("flow", "implicit");
            Extensions.Add("scopes", new List<string>
            {
                "catalog"
            });
        }
    }
}
