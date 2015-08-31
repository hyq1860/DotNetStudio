using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Contract.Product;
using DotNet.CloudFarm.Domain.Model.Product;
using DotNet.Common.Collections;
using DotNet.Data;

namespace DotNet.CloudFarm.Domain.DTO.Product
{
    /// <summary>
    /// 产品数据访问层
    /// </summary>
    public class ProductDataAccess:IProductDataAccess
    {
        /// <summary>
        /// 获取未删除的所有products
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PagedList<ProductModel> GetProducts(int pageIndex, int pageSize)
        {
            var data = new List<ProductModel>();
            var count = 0;
            using (var cmd = DataCommandManager.GetDataCommand("GetProducts"))
            {
                cmd.SetParameterValue("@PageIndex", pageIndex);
                cmd.SetParameterValue("@PageSize", pageSize);

                using (var ds = cmd.ExecuteDataSet())
                {
                   if(ds.Tables.Count>=2)
                   {
                       foreach (DataRow dr in ds.Tables[0].Rows)
                       {
                           data.Add(DataRowToProductModel(dr));
                       }
                       var drCount = ds.Tables[1].Rows[0][0];
                       count = !Convert.IsDBNull(drCount) ? Convert.ToInt32(drCount) : 0;
                   }
                  
                }
            }

            var result = new PagedList<ProductModel>(data, pageIndex, pageSize, count);
            //总记录数量
            return result;
        }


        public PagedList<ProductModel> GetProducts(int pageIndex, int pageSize, int status)
        {
            var data = new List<ProductModel>();
            var count = 0;
            using (var cmd = DataCommandManager.GetDataCommand("GetProductsWithStatus"))
            {
                cmd.SetParameterValue("@PageIndex", pageIndex);
                cmd.SetParameterValue("@PageSize", pageSize);
                cmd.SetParameterValue("@Status", status);
                using (var ds = cmd.ExecuteDataSet())
                {
                    if (ds.Tables.Count >= 2)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            data.Add(DataRowToProductModel(dr));
                        }
                        var drCount = ds.Tables[1].Rows[0][0];
                        count = !Convert.IsDBNull(drCount) ? Convert.ToInt32(drCount) : 0;
                    }
                }
            }

            var result = new PagedList<ProductModel>(data, pageIndex, pageSize, count);
            //总记录数量
            return result;
        }



        public PagedList<ProductModel> GetProducts(int pageIndex, int pageSize, string condition)
        {
            var data = new List<ProductModel>();

            using (var cmd = DataCommandManager.GetDataCommand("GetProductsWithCondition"))
            {
                cmd.SetParameterValue("@condition", condition);

                using (var dr = cmd.ExecuteDataReader())
                {
                    while (dr.Read())
                    {
                        var productModel = DataReaderToProductModel(dr);

                        if (productModel.Id > 0)
                        {
                            data.Add(productModel);
                        }
                    }
                }
            }

            var result = new PagedList<ProductModel>(data, pageIndex, pageSize)
            {
                TotalCount = 100
            };
            //总记录数量
            return result;
        }

        /// <summary>
        /// 根据商品id获取商品详情
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ProductModel GetProductById(int productId)
        {
            var productModel = new ProductModel();
            using (var cmd = DataCommandManager.GetDataCommand("GetProductById"))
            {
                cmd.SetParameterValue("@Id", productId);
                using (var dr = cmd.ExecuteDataReader())
                {
                    while (dr.Read())
                    {
                        productModel = DataReaderToProductModel(dr);
                    }
                }
            }
            return productModel;
        }

        private ProductModel DataRowToProductModel(DataRow dr)
        {
            var productModel = new ProductModel();

            productModel.Id = !Convert.IsDBNull(dr["Id"]) ? Convert.ToInt32(dr["Id"]) : 0;
            productModel.Name = !Convert.IsDBNull(dr["ProductName"]) ? dr["ProductName"].ToString() : string.Empty;
            productModel.Stock = !Convert.IsDBNull(dr["Stock"]) ? Convert.ToInt32(dr["Stock"]) : 0;
            productModel.VirtualSaledCount = !Convert.IsDBNull(dr["VirtualSaledCount"]) ? Convert.ToInt32(dr["VirtualSaledCount"]) : 0;
            productModel.RealSaledCount = !Convert.IsDBNull(dr["SaledCount"]) ? Convert.ToInt32(dr["SaledCount"]) : 0;
            productModel.Price = !Convert.IsDBNull(dr["Price"]) ? Convert.ToDecimal(dr["Price"]) : 0;
            productModel.SheepPrice = !Convert.IsDBNull(dr["SheepPrice"]) ? Convert.ToDecimal(dr["SheepPrice"]) : 0;
            productModel.FeedPrice = !Convert.IsDBNull(dr["FeedPrice"]) ? Convert.ToDecimal(dr["FeedPrice"]) : 0;
            productModel.WorkPrice = !Convert.IsDBNull(dr["WorkPrice"]) ? Convert.ToDecimal(dr["WorkPrice"]) : 0;
            productModel.YearEarningRate = !Convert.IsDBNull(dr["YearEarningRate"]) ? Convert.ToDecimal(dr["YearEarningRate"]) : 0;
            productModel.EarningRate = !Convert.IsDBNull(dr["EarningRate"]) ? Convert.ToDecimal(dr["EarningRate"]) : 0;
            productModel.Earning = !Convert.IsDBNull(dr["Earning"]) ? Convert.ToDecimal(dr["Earning"]) : 0;
            productModel.StartTime = !Convert.IsDBNull(dr["StartTime"]) ? Convert.ToDateTime(dr["StartTime"]) : DateTime.MinValue;
            productModel.EndTime = !Convert.IsDBNull(dr["EndTime"]) ? Convert.ToDateTime(dr["EndTime"]) : DateTime.MinValue;
            productModel.CreateTime = !Convert.IsDBNull(dr["CreateTime"]) ? Convert.ToDateTime(dr["CreateTime"]) : DateTime.MinValue;
            productModel.Status = !Convert.IsDBNull(dr["Status"]) ? Convert.ToInt32(dr["Status"]) : 0;
            productModel.CreatorId = !Convert.IsDBNull(dr["CreatorId"]) ? Convert.ToInt32(dr["CreatorId"]) : 0;
            productModel.EarningDay = !Convert.IsDBNull(dr["EarningDay"]) ? Convert.ToInt32(dr["EarningDay"]) : 0;
            productModel.SheepType = !Convert.IsDBNull(dr["SheepType"]) ? dr["SheepType"].ToString() : string.Empty;
            productModel.SheepFactory = !Convert.IsDBNull(dr["SheepFactory"]) ? dr["SheepFactory"].ToString() : string.Empty;
            productModel.ImgUrl = !Convert.IsDBNull(dr["ImgUrl"]) ? dr["ImgUrl"].ToString() : string.Empty;
            return productModel;
        }


        private ProductModel DataReaderToProductModel(IDataReader dr)
        {
            var productModel = new ProductModel();

            productModel.Id = !Convert.IsDBNull(dr["Id"]) ? Convert.ToInt32(dr["Id"]) : 0;
            productModel.Name = !Convert.IsDBNull(dr["ProductName"]) ? dr["ProductName"].ToString() : string.Empty;
            productModel.Stock = !Convert.IsDBNull(dr["Stock"]) ? Convert.ToInt32(dr["Stock"]) : 0;
            productModel.VirtualSaledCount = !Convert.IsDBNull(dr["VirtualSaledCount"]) ? Convert.ToInt32(dr["VirtualSaledCount"]) : 0;
            productModel.RealSaledCount = !Convert.IsDBNull(dr["SaledCount"]) ? Convert.ToInt32(dr["SaledCount"]) : 0;
            productModel.Price = !Convert.IsDBNull(dr["Price"]) ? Convert.ToDecimal(dr["Price"]) : 0;
            productModel.SheepPrice = !Convert.IsDBNull(dr["SheepPrice"]) ? Convert.ToDecimal(dr["SheepPrice"]) : 0;
            productModel.FeedPrice = !Convert.IsDBNull(dr["FeedPrice"]) ? Convert.ToDecimal(dr["FeedPrice"]) : 0;
            productModel.WorkPrice = !Convert.IsDBNull(dr["WorkPrice"]) ? Convert.ToDecimal(dr["WorkPrice"]) : 0;
            productModel.YearEarningRate = !Convert.IsDBNull(dr["YearEarningRate"]) ? Convert.ToDecimal(dr["YearEarningRate"]) : 0;
            productModel.EarningRate = !Convert.IsDBNull(dr["EarningRate"]) ? Convert.ToDecimal(dr["EarningRate"]) : 0;
            productModel.Earning = !Convert.IsDBNull(dr["Earning"]) ? Convert.ToDecimal(dr["Earning"]) : 0;
            productModel.StartTime = !Convert.IsDBNull(dr["StartTime"]) ? Convert.ToDateTime(dr["StartTime"]) : DateTime.MinValue;
            productModel.EndTime = !Convert.IsDBNull(dr["EndTime"]) ? Convert.ToDateTime(dr["EndTime"]) : DateTime.MinValue;
            productModel.CreateTime = !Convert.IsDBNull(dr["CreateTime"]) ? Convert.ToDateTime(dr["CreateTime"]) : DateTime.MinValue;
            productModel.Status = !Convert.IsDBNull(dr["Status"]) ? Convert.ToInt32(dr["Status"]) : 0;
            productModel.CreatorId = !Convert.IsDBNull(dr["CreatorId"]) ? Convert.ToInt32(dr["CreatorId"]) : 0;
            productModel.EarningDay = !Convert.IsDBNull(dr["EarningDay"]) ? Convert.ToInt32(dr["EarningDay"]) : 0;
            productModel.SheepType = !Convert.IsDBNull(dr["SheepType"]) ? dr["SheepType"].ToString() : string.Empty;
            productModel.SheepFactory = !Convert.IsDBNull(dr["SheepFactory"]) ? dr["SheepFactory"].ToString() : string.Empty;
            productModel.ImgUrl = !Convert.IsDBNull(dr["ImgUrl"]) ? dr["ImgUrl"].ToString() : string.Empty;
            return productModel;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public int InserProduct(ProductModel product)
        {
            using (var cmd = DataCommandManager.GetDataCommand("InsertProduct"))
            {
                cmd.SetParameterValue("@CreateTime", product.CreateTime);
                cmd.SetParameterValue("@CreatorId", product.CreatorId);
                cmd.SetParameterValue("@Earning", product.Earning);
                cmd.SetParameterValue("@EarningDay", product.EarningDay);
                cmd.SetParameterValue("@EarningRate", product.EarningRate);
                cmd.SetParameterValue("@EndTime", product.EndTime);
                cmd.SetParameterValue("@FeedPrice", product.FeedPrice);
                cmd.SetParameterValue("@ImgUrl", product.ImgUrl);
                cmd.SetParameterValue("@ProductName", product.Name);
                cmd.SetParameterValue("@Price", product.Price);
                cmd.SetParameterValue("@SaledCount", product.SaledCount);
                cmd.SetParameterValue("@SheepFactory", product.SheepFactory);
                cmd.SetParameterValue("@SheepPrice", product.SheepPrice);
                cmd.SetParameterValue("@SheepType", product.SheepType);
                cmd.SetParameterValue("@StartTime", product.StartTime);
                cmd.SetParameterValue("@Status", product.Status);
                cmd.SetParameterValue("@Stock", product.Stock);
                cmd.SetParameterValue("@WorkPrice", product.WorkPrice);
                cmd.SetParameterValue("@YearEarningRate", product.YearEarningRate);
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
                return 0;
            }
        }

        /// <summary>
        /// 更新product
        /// </summary>
        /// <param name="product"></param>
        public void UpdateProduct(ProductModel product)
        {
            using (var cmd = DataCommandManager.GetDataCommand("UpdateProductById"))
            {
                cmd.SetParameterValue("@Id", product.Id);
                cmd.SetParameterValue("@CreateTime", product.CreateTime);
                cmd.SetParameterValue("@CreatorId", product.CreatorId);
                cmd.SetParameterValue("@Earning", product.Earning);
                cmd.SetParameterValue("@EarningDay", product.EarningDay);
                cmd.SetParameterValue("@EarningRate", product.EarningRate);
                cmd.SetParameterValue("@EndTime", product.EndTime);
                cmd.SetParameterValue("@FeedPrice", product.FeedPrice);
                cmd.SetParameterValue("@ImgUrl", product.ImgUrl);
                cmd.SetParameterValue("@ProductName", product.Name);
                cmd.SetParameterValue("@Price", product.Price);
                cmd.SetParameterValue("@SaledCount", product.SaledCount);
                cmd.SetParameterValue("@SheepFactory", product.SheepFactory);
                cmd.SetParameterValue("@SheepPrice", product.SheepPrice);
                cmd.SetParameterValue("@SheepType", product.SheepType);
                cmd.SetParameterValue("@StartTime", product.StartTime);
                cmd.SetParameterValue("@Status", product.Status);
                cmd.SetParameterValue("@Stock", product.Stock);
                cmd.SetParameterValue("@WorkPrice", product.WorkPrice);
                cmd.SetParameterValue("@YearEarningRate", product.YearEarningRate);
                cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// 更新product状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        public void UpdateStatus(int id, int status)
        {
            using (var cmd = DataCommandManager.GetDataCommand("UpdateProductStatusById"))
            {
                cmd.SetParameterValue("@Id", id);
                cmd.SetParameterValue("@Status", status);
                cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// 更新虚拟库存状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="VirtualSaledCount"></param>
        public void UpdateVirtualSaledCount(int id, int VirtualSaledCount)
        {
            using (var cmd = DataCommandManager.GetDataCommand("UpdateVirtualSaledCount"))
            {
                cmd.SetParameterValue("@Id", id);
                cmd.SetParameterValue("@VirtualSaledCount", VirtualSaledCount);
                cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// 根据条件获取单个产品
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ProductModel GetProductByCondition(string condition)
        {
            var productModel = new ProductModel();
            using (var cmd = DataCommandManager.GetDataCommand("GetSingleProductByCondition"))
            {
                cmd.CommandText = string.Format("{0} where 1=1 and {1}",cmd.CommandText,condition);
                using (var dr = cmd.ExecuteDataReader())
                {
                    while (dr.Read())
                    {
                        productModel = DataReaderToProductModel(dr);
                    }
                }
            }
            return productModel;
        }
    }
}
