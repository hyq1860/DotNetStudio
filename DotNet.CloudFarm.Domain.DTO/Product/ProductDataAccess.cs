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
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public PagedList<ProductModel> GetProducts(int pageIndex, int pageSize, int status)
        {
            var data = new List<ProductModel>();

            using (var cmd = DataCommandManager.GetDataCommand("GetProducts"))
            {
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

        private ProductModel DataReaderToProductModel(IDataReader dr)
        {
            var productModel = new ProductModel();

            productModel.Id = !Convert.IsDBNull(dr["Id"]) ? Convert.ToInt32(dr["Id"]) : 0;
            productModel.Name = !Convert.IsDBNull(dr["ProductName"]) ? dr["ProductName"].ToString() : string.Empty;
            productModel.Stock = !Convert.IsDBNull(dr["Stock"]) ? Convert.ToInt32(dr["Stock"]) : 0;
            productModel.SaledCount = !Convert.IsDBNull(dr["SaledCount"]) ? Convert.ToInt32(dr["SaledCount"]) : 0;
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
            return productModel;
        }
    }
}
