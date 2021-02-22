﻿using System;
using System.Collections.Generic;
using System.Text;
using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductDao _productDao;

        public ProductManager(IProductDao productDao)
        {
            _productDao = productDao;
        }

        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {

            //Loglama
            //cacheremove
            //performance
            //transaction
            //yetkilendirme => bunların hepsi burada yazılsa burası çorba olur
           
            _productDao.Add(product);
            return new SuccessResult(Messages.ProductAdded);
        }

        public IDataResult<List<Product>> GetAll()
        {
            //İş kodları
            //Yetkisi var mı?
            if (DateTime.Now.Hour == 23)
            {
                return new ErrorDataResult<List<Product>>(Messages.Maintenancetime);
            }

            return new SuccessDataResult<List<Product>>(_productDao.GetAll(), Messages.ProductsListed); ;
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDao.GetAll(p => p.CategoryId == id)); ;
        }

        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDao.Get(p => p.ProductId == productId));

        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDao.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max)); ;
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDao.GetProductDetails()); ;
        }

        public IResult Update(Product product)
        {

            _productDao.Update(product);
            return new SuccessResult(Messages.ProductUpdated);

        }
    }
}
