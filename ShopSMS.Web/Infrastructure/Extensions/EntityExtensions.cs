using ShopSMS.Model.Model;
using ShopSMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopSMS.Web.Infrastructure.Extensions
{
    public static class EntityExtensions
    {
        public static void UpdateProductCategory(this ProductCategory proCategory, ProductCategoryViewModel proCategoryVM)
        {
            proCategory.ProductCategoryID = proCategoryVM.ProductCategoryID;
            proCategory.ProductCategoryName = proCategoryVM.ProductCategoryName;
            proCategory.CategoryID = proCategoryVM.CategoryID;
            proCategory.DisplayOrder = proCategoryVM.DisplayOrder;
            proCategory.CreateDate = proCategoryVM.CreateDate;
            proCategory.CreateBy = proCategoryVM.CreateBy;
            proCategory.UpdateDate = proCategoryVM.UpdateDate;
            proCategory.UpdateBy = proCategoryVM.UpdateBy;
            proCategory.MetaKeyword = proCategoryVM.MetaKeyword;
            proCategory.MetaDescription = proCategoryVM.MetaDescription;
            proCategory.Status = proCategoryVM.Status;
        }

        public static void UpdateProduct(this Product product, ProductViewModel productVM) {
            product.ProductID = productVM.ProductID;
            product.ProductAlias = productVM.ProductAlias;          
            product.ProductCategoryID = productVM.ProductCategoryID;
            product.ProductCode = productVM.ProductCode;
            product.Description = productVM.Description;
            product.ProductHomeFlag = productVM.ProductHomeFlag;
            product.ProductHotFlag = productVM.ProductHotFlag;
            product.ProductSellingGood = productVM.ProductSellingGood;
            product.Avatar = productVM.Avatar;
            product.MoreImages = productVM.MoreImages;
            product.ProductName = productVM.ProductName;
            product.PriceInput = productVM.PriceInput;
            product.PriceSell = productVM.PriceSell;
            product.PromotionPrice = productVM.PromotionPrice;
            product.Quantity = productVM.Quantity;
            product.TaxVAT = productVM.TaxVAT;
            product.ProductViewCount = productVM.ProductViewCount;
            product.Warranty = productVM.Warranty;         
            product.CreateBy = productVM.CreateBy;
            product.CreateDate = productVM.CreateDate;
            product.MetaDescription = productVM.MetaDescription;
            product.MetaKeyword = productVM.MetaKeyword;
            product.UpdateBy = productVM.UpdateBy;
            product.UpdateDate = productVM.UpdateDate;

        }

        public static void UpdateCategory(this Category category, CategoryViewModel categoryVM)
        {
            category.CategoryID = categoryVM.CategoryID;
            category.CategoryName = categoryVM.CategoryName;
            category.DisplayOrder = categoryVM.DisplayOrder;
            category.CreateBy = categoryVM.CreateBy;
            category.CreateDate = categoryVM.CreateDate;
            category.MetaDescription = categoryVM.MetaDescription;
            category.MetaKeyword = categoryVM.MetaKeyword;
            category.UpdateBy = categoryVM.UpdateBy;
            category.Status = categoryVM.Status;
            category.UpdateDate = categoryVM.UpdateDate;
        }

        public static void UpdateSupplier(this Supplier obj, SupplierViewModel model)
        {
            obj.SupplierID = model.SupplierID;
            obj.SupplierName = model.SupplierName;
        }

        public static void UpdateProducer(this Producer obj, ProducerViewModel model)
        {
            obj.ProducerID = model.ProducerID;
            obj.ProducerName = model.ProducerName;
        }
    }
}