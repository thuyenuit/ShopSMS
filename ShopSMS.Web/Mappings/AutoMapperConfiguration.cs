using AutoMapper;
using ShopSMS.Model.Model;
using ShopSMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopSMS.Web.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure() {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ProductCategory, ProductCategoryViewModel>();
                cfg.CreateMap<Product, ProductViewModel>();
                cfg.CreateMap<Category, CategoryViewModel>();

                /*cfg.CreateMap<Post, PostViewModel>();
                cfg.CreateMap<PostCategory, PostCategoryViewModel>();
                cfg.CreateMap<Tag, TagViewModel>();*/
                /* cfg.CreateMap<ProductTag, ProductTagViewModel>();
                 cfg.CreateMap<Footer, FooterViewModel>();
                 cfg.CreateMap<Slide, SlideViewModel>();
                 cfg.CreateMap<Page, PageViewModel>();
                 cfg.CreateMap<ContactDetail, ContactDetailViewModel>();
                 cfg.CreateMap<ApplicationGroup, ApplicationGroupViewModel>();
                 cfg.CreateMap<ApplicationRole, ApplicationRoleViewModel>();
                 cfg.CreateMap<ApplicationUser, ApplicationUserViewModel>();*/
            });
        }
    }
}