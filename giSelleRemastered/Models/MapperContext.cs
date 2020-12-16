using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace giSelleRemastered.Models
{
    public static class MapperContext
    {
        static public MapperConfiguration config { get; }
        static public IMapper mapper { get; }

        static MapperContext()
        {
            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductWithCategories>();
                cfg.CreateMap<ProductWithCategories, Product>();
                cfg.CreateMap<Product, ProductView>();
                cfg.CreateMap<ProductView, Product>();
            });

            mapper = config.CreateMapper();
        }
    }
}