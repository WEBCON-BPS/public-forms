using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WEBCON.FormsGenerator.Presentation.Configuration
{
    public interface IWritableOptions<out T> : IOptionsSnapshot<T> where T : class, new()
    {
        void Update(Action<T> applyChanges);
    }
    [ExcludeFromCodeCoverage]
    public class WritableOptions<T> : IWritableOptions<T> where T : class, new()
    {
        private readonly IHostEnvironment environment;
        private readonly IOptionsMonitor<T> options;
        private readonly string section;

        public WritableOptions(
            IHostEnvironment environment,
            IOptionsMonitor<T> options,
            string section)
        {
            this.environment = environment;
            this.options = options;
            this.section = section;
        }

        public T Value => options.CurrentValue;
        public T Get(string name) => options.Get(name);

        public void Update(Action<T> applyChanges)
        {
            string fileName = $"appsettings.{environment.EnvironmentName}.json";
            var fileProvider = environment.ContentRootFileProvider;
            IFileInfo fileInfo = fileProvider.GetFileInfo(fileName);
            if (!File.Exists(fileInfo.PhysicalPath))
            {
                if (environment.IsProduction())
                    fileInfo = fileProvider.GetFileInfo("appsettings.json");
                else
                    throw new BusinessLogic.Domain.Exceptions.ApplicationException($"Could not find {fileName} in application directory");
            }

            var physicalPath = fileInfo.PhysicalPath;
            var jObject = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(physicalPath));
            var sectionObject = jObject.TryGetValue(this.section, out JToken section) ?
                JsonConvert.DeserializeObject<T>(section.ToString()) : (Value ?? new T());

            applyChanges(sectionObject);

            jObject[this.section] = JObject.Parse(JsonConvert.SerializeObject(sectionObject));
            File.WriteAllText(physicalPath, JsonConvert.SerializeObject(jObject, Formatting.Indented));
        }
        
    }
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureWritable<T>(
            this IServiceCollection services,
            IConfigurationSection section) where T : class, new()
        {
            services.Configure<T>(section);
            services.AddTransient<IWritableOptions<T>>(provider =>
            {
                var environment = provider.GetService<IHostEnvironment>();
                var options = provider.GetService<IOptionsMonitor<T>>();
                return new WritableOptions<T>(environment, options, section.Key);
            });
        }
    }
}
