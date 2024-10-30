using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace WEBCON.FormsGenerator.Presentation.Features
{
    public static class CustomHostExtension
    {
        public static IApplicationBuilder UseCustomHost(this IApplicationBuilder app, string basePath)
        {
            return app.Use((ctx, next) =>
            {
                ctx.Request.Host = new HostString(basePath);
                return next();
            });
        }
    }
}
