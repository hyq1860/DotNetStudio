﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Model.Product;
using DotNet.Common.Collections;

namespace DotNet.CloudFarm.Domain.Contract.Product
{
    /// <summary>
    /// yangke每期产品接口
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="status">产品状态</param>
        /// <returns></returns>
        PagedList<ProductModel> GetProducts(int pageIndex,int pageSize,int status);
    }
}
