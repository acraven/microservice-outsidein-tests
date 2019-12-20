using Microsoft.AspNetCore.Http;

namespace Microservice.Components
{
   public class RequestHeaderTenantAccessor : ITenantAccessor
   {
      private readonly IHttpContextAccessor _httpContextAccessor;

      public RequestHeaderTenantAccessor(IHttpContextAccessor httpContextAccessor)
      {
         _httpContextAccessor = httpContextAccessor;
      }

      public string TenantID => _httpContextAccessor.HttpContext.Request.Headers["X-Tenant"].ToString();
   }
}