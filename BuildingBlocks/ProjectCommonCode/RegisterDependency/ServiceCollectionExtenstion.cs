using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ProjectCommonCode.RegisterDependency
{
    public static class ServiceCollectionExtenstion
    {
        public static void RegisterServices(this IServiceCollection services,
            string assemblyName)
        {

            var types = Assembly.Load(assemblyName).GetTypes();

            var interfaces= types.Where(t=>t.GetCustomAttribute<RegisterDependencyAttribute>() != null && t.IsInterface);

            foreach (var @interface in interfaces)
            {
                var @class= GetImplementationClass(types, @interface);

                AddServiceScriptor(services, @interface, @class);
            }


        }

        private static void AddServiceScriptor(IServiceCollection services, Type @type, Type @class)
        {
            var scope= type.GetCustomAttribute<RegisterDependencyAttribute>().Scope;

            services.Add(new ServiceDescriptor(type, @class, scope));
        }
        public static Type GetImplementationClass(Type[] types, Type @interface)
        {
            var @class= types.FirstOrDefault(t=>t.GetTypeInfo().ImplementedInterfaces.
            Any(i=>i == @interface));


            return @class ?? throw new NotImplementedException($"No class implements this interface" +
                $"{@interface.Name}");
        }
    }

 
}
