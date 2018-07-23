using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Motohusaria.DTO;

namespace Motohusaria.Web.Utils
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
#if (DEBUG)
            var dynamicAssemblies = assemblies.Where(W => W.IsDynamic);
#endif
            var attributes = assemblies.Where(W => !W.IsDynamic).SelectMany(s => s.ExportedTypes)
                .Where(w => w.GetCustomAttribute<AutoMapAttribute>() != null)
                .SelectMany(s => new MapperPair[]
                {
                    new MapperPair { Target = s.GetCustomAttribute<AutoMapAttribute>().Type, Source = s },
                    new MapperPair { Target = s, Source = s.GetCustomAttribute<AutoMapAttribute>().Type }
                })
                .Distinct().ToList();
            foreach (var attribute in attributes)
            {
                CreateMap(attribute.Source, attribute.Target);
            }
        }

        class MapperPair
        {
            public Type Target { get; set; }

            public Type Source { get; set; }
        }
    }
}
