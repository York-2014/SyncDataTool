using System;
using System.Collections.Generic;
using SyncDataTool.Entity;
using SyncDataTool.Helper;
using System.Xml;
using System.IO;

namespace SyncDataTool.BLL
{
    public class Product
    {
        private static JsonRequest jsonRequest;
        private static List<string> lstPriceTags = new List<string>() { "$0 - $9.99", "$10 - $19.99", "$20 - $29.99", "$30 - $39.99", "$40 - $69.99", "$70 - $109" };

        #region 获取产品信息
        public static ProductEntity GetProduct(string strUrl)
        {
            jsonRequest = new JsonRequest(strUrl, "GET");
            jsonRequest.Credentials = new Credentials();
            jsonRequest.Credentials.UserName = Common.GetUserByUrl(strUrl);
            jsonRequest.Credentials.Password = Common.GetPwdByUrl(strUrl);
            ProductEntity productEntity = (ProductEntity)jsonRequest.Execute<ProductEntity>();
            return productEntity;
        }
        #endregion

        #region 产生产品信息
        public static bool CreateProduct(string strSourceUrl, string strUrl, ProductEntity sourceProduct, double source_price_offset, double target_price_offset, string country_code, string target_country_code)
        {
            bool result = false;
            double currentPrice = Convert.ToDouble(sourceProduct.product.variants[0].price);
            //价格
            double afterChangedPrice = 0.00;
            int iafterChangedPrice = 0;
            //修改后的产品价格
            double dbChangedPrice = 0.00;
            List<variantEntity> lstVariants = new List<variantEntity>();
            foreach (variantEntity variant in sourceProduct.product.variants)
            {
                if (variant.title.ToString().ToLower().Contains("bundle") == false)
                {
                    lstVariants.Add(variant);
                }
            }
            sourceProduct.product.variants.Clear();
            sourceProduct.product.variants = lstVariants;
            foreach (variantEntity variant in sourceProduct.product.variants)
            {
                #region 修改价格
                if (country_code.ToUpper() == "AU" && (target_country_code.ToUpper() == "SG" || target_country_code.ToUpper() == "MY"))
                {
                    if (Convert.ToDouble(variant.price) == 6.99 && target_country_code.ToUpper() == "SG")
                    {
                        //价格不变
                        afterChangedPrice = Convert.ToDouble(variant.price);
                    }
                    else
                    {
                        afterChangedPrice = getAfterChangedPrice(Convert.ToDouble(variant.price), Convert.ToDouble(variant.grams), target_price_offset, target_country_code.ToUpper());
                    }
                    variant.price = afterChangedPrice;
                    variant.compare_at_price = 2 * afterChangedPrice;
                }
                else if (country_code.ToUpper() == "FR" && target_country_code.ToUpper() == "BE")
                {
                    if ("Les Gratuits" != sourceProduct.product.product_type.ToString())
                    {
                        afterChangedPrice = Convert.ToDouble(variant.price) + 3;
                        variant.price = afterChangedPrice;
                        variant.compare_at_price = Convert.ToDouble(variant.compare_at_price) + 3;
                    }
                }
                else if (country_code.ToUpper() == "AU" && target_country_code.ToUpper() == "IE")
                {
                    if ("Free Drab Day" == sourceProduct.product.product_type.ToString())
                    {
                        afterChangedPrice = Convert.ToDouble(variant.price) - 2.0;
                        variant.price = afterChangedPrice;
                        variant.compare_at_price = Convert.ToDouble(variant.compare_at_price) - 2.0;
                    }
                    else
                    {
                        afterChangedPrice = Convert.ToDouble(variant.price) * target_price_offset;
                        if (afterChangedPrice % 1 < 0.5)
                        {
                            afterChangedPrice = (int)afterChangedPrice;
                        }
                        else
                        {
                            afterChangedPrice = (int)afterChangedPrice + 0.99;
                        }
                        double tempCAP = Convert.ToDouble(variant.compare_at_price) * target_price_offset;
                        if (tempCAP % 1 < 0.5)
                        {
                            tempCAP = (int)tempCAP;
                        }
                        else
                        {
                            tempCAP = (int)tempCAP + 0.99;
                        }
                        variant.price = afterChangedPrice;
                        variant.compare_at_price = tempCAP;
                    }
                }
                else
                {
                    int tempSourcePrice = Convert.ToInt32(Convert.ToDouble(variant.price).ToString("0.00").Substring(0, Convert.ToDouble(variant.price).ToString("0.00").IndexOf(".")));
                    if ("NZ" == target_country_code)
                    {
                        if (tempSourcePrice.ToString().Substring(tempSourcePrice.ToString().Length - 1) == "8")
                        {
                            afterChangedPrice = Convert.ToDouble(string.Format("{0}.99", tempSourcePrice + 1));
                        }
                        else if (Convert.ToDouble(variant.price).ToString("0.00") == "7.99")
                        {
                            afterChangedPrice = Convert.ToDouble(variant.price);
                        }
                        else
                        {
                            afterChangedPrice = Convert.ToDouble(variant.price) - source_price_offset + target_price_offset;
                        }
                        variant.compare_at_price = 2 * afterChangedPrice;
                    }
                    else if ("FR" == target_country_code)
                    {
                        if (Convert.ToDouble(variant.price).ToString("0.00") == "6.99")
                        {
                            afterChangedPrice = 5.99;
                            variant.compare_at_price = 2 * afterChangedPrice;
                        }
                    }
                    else if ("UK" == target_country_code)
                    {
                        if (Convert.ToDouble(variant.price).ToString("0.00") == "7.99")
                        {
                            afterChangedPrice = 5.99;
                            variant.compare_at_price = 2 * afterChangedPrice;
                        }
                        else
                        {
                            afterChangedPrice = (Convert.ToDouble(variant.price) - source_price_offset) * target_price_offset;
                            //获取整数部分
                            iafterChangedPrice = Convert.ToInt32(afterChangedPrice.ToString("0.00").Substring(0, afterChangedPrice.ToString("0.00").IndexOf(".")));
                            //判断小数是否有值
                            if (afterChangedPrice > iafterChangedPrice)
                            {
                                afterChangedPrice = Convert.ToDouble(string.Format("{0}.99", iafterChangedPrice));
                            }
                            variant.price = afterChangedPrice;

                            double tempCAPrice = Convert.ToDouble(variant.compare_at_price) * target_price_offset + 2;
                            tempCAPrice = Convert.ToInt32(tempCAPrice.ToString("0.00").Substring(0, tempCAPrice.ToString("0.00").IndexOf(".")));
                            variant.compare_at_price = Convert.ToDouble(string.Format("{0}.99", tempCAPrice));
                        }
                    }
                    else if ("US" == target_country_code && Convert.ToDouble(variant.price).ToString("0.00") == "7.99")
                    {
                        afterChangedPrice = 6.99;
                        variant.compare_at_price = 2 * afterChangedPrice;
                    }
                    else if ("CA" == target_country_code && Convert.ToDouble(variant.price).ToString("0.00") == "7.99")
                    {
                        afterChangedPrice = 6.99;
                        variant.compare_at_price = 2 * afterChangedPrice;
                    }
                    else
                    {
                        afterChangedPrice = Convert.ToDouble(variant.price) - source_price_offset + target_price_offset;
                        variant.compare_at_price = 2 * afterChangedPrice;
                    }
                    variant.price = afterChangedPrice;
                }
                #endregion

                #region 修改国家简码
                if (target_country_code == "MY" || target_country_code == "SG")
                {
                    if (null != variant.option1)
                    {
                        if (variant.option1.ToString().Contains(string.Format("IN {0}", country_code)))
                        {
                            variant.option1 = variant.option1.ToString().Substring(0, variant.option1.ToString().IndexOf("("));
                        }
                    }
                    if (null != variant.option2)
                    {
                        if (variant.option2.ToString().Contains(string.Format("IN {0}", country_code)))
                        {
                            variant.option2 = variant.option2.ToString().Substring(0, variant.option2.ToString().IndexOf("("));
                        }
                    }
                    if (null != variant.option3)
                    {
                        if (variant.option3.ToString().Contains(string.Format("IN {0}", country_code)))
                        {
                            variant.option3 = variant.option3.ToString().Substring(0, variant.option3.ToString().IndexOf("("));
                        }
                    }
                }
                else
                {
                    if (null != variant.option1)
                    {
                        //修改option1
                        if (variant.option1.ToString().Contains(string.Format("IN {0}", country_code)))
                        {
                            variant.option1 = variant.option1.ToString().Replace(string.Format("IN {0}", country_code), string.Format("IN {0}", target_country_code));
                        }
                    }
                    if (null != variant.option2)
                    {
                        //修改option2
                        if (variant.option2.ToString().Contains(string.Format("IN {0}", country_code)))
                        {
                            variant.option2 = variant.option2.ToString().Replace(string.Format("IN {0}", country_code), string.Format("IN {0}", target_country_code));
                        }
                    }
                    if (null != variant.option3)
                    {
                        //修改option3
                        if (variant.option3.ToString().Contains(string.Format("IN {0}", country_code)))
                        {
                            variant.option3 = variant.option3.ToString().Replace(string.Format("IN {0}", country_code), string.Format("IN {0}", target_country_code));
                        }
                    }
                }
                #endregion

                if (sourceProduct.product.variants.IndexOf(variant) == 0)
                {
                    //修改价格
                    dbChangedPrice = afterChangedPrice;
                }
            }

            //修改后的价格
            afterChangedPrice = dbChangedPrice;

            //获取整数部分
            int iCurrentPrice = Convert.ToInt32(currentPrice.ToString("0.00").Substring(0, currentPrice.ToString("0.00").IndexOf(".")));
            //获取修改后的价格的整数部分
            int iAfterChangedPrice = Convert.ToInt32(afterChangedPrice.ToString("0.00").Substring(0, afterChangedPrice.ToString("0.00").IndexOf(".")));

            //修改Title
            //sourceProduct.product.title = sourceProduct.product.title.ToString().Contains(iCurrentPrice.ToString()) ? sourceProduct.product.title.ToString().Replace(iCurrentPrice.ToString(), iAfterChangedPrice.ToString()) : sourceProduct.product.title;

            string strTransCurrentPrice = string.Empty;

            //判断小数是否有值
            if (currentPrice > iCurrentPrice)
            {
                //判断小数有一位值还是两位值
                if (Convert.ToInt32(currentPrice.ToString("0.00").Substring(currentPrice.ToString("0.00").Length - 1)) == 0)
                {
                    strTransCurrentPrice = currentPrice.ToString("0.0");
                    //修改handle
                    sourceProduct.product.handle = sourceProduct.product.handle.ToString().Contains(strTransCurrentPrice.Replace(".", "-")) ? sourceProduct.product.handle.ToString().Replace(strTransCurrentPrice.Replace(".", "-"), afterChangedPrice.ToString("0.0").Replace(".", "-")) : sourceProduct.product.handle;
                }
                else
                {
                    strTransCurrentPrice = currentPrice.ToString("0.00");
                    //修改handle
                    sourceProduct.product.handle = sourceProduct.product.handle.ToString().Contains(strTransCurrentPrice.Replace(".", "-")) ? sourceProduct.product.handle.ToString().Replace(strTransCurrentPrice.Replace(".", "-"), afterChangedPrice.ToString("0.00").Replace(".", "-")) : sourceProduct.product.handle;
                }
            }
            else
            {//小数没有值
                strTransCurrentPrice = iCurrentPrice.ToString();
                //修改handle
                sourceProduct.product.handle = sourceProduct.product.handle.ToString().Contains(strTransCurrentPrice) ? sourceProduct.product.handle.ToString().Replace(strTransCurrentPrice, afterChangedPrice.ToString()) : sourceProduct.product.handle;
            }

            #region 去除for之前的字符
            //if ("UK" == target_country_code)
            //{
            //    string tempTitle = sourceProduct.product.title.ToString();
            //    string tempHandle = sourceProduct.product.handle.ToString();
            //    if (tempTitle.Contains("for a "))
            //    {
            //        sourceProduct.product.title = tempTitle.Substring(tempTitle.IndexOf("for a ") + 6);
            //    }
            //    else if (tempTitle.Contains("for "))
            //    {
            //        sourceProduct.product.title = tempTitle.Substring(tempTitle.IndexOf("for ") + 4);
            //    }
            //    if (tempHandle.Contains("for-a-"))
            //    {
            //        sourceProduct.product.handle = tempHandle.Substring(tempHandle.IndexOf("for-a-") + 6);
            //    }
            //    else if (tempTitle.Contains("for-"))
            //    {
            //        sourceProduct.product.handle = tempHandle.Substring(tempHandle.IndexOf("for-") + 4);
            //    }
            //}
            #endregion

            string price_tag = getPriceTag(afterChangedPrice);
            foreach (string tag in lstPriceTags)
            {
                if (sourceProduct.product.tags.ToString().Contains(tag) == true)
                {
                    //修改tags中的价格tag
                    sourceProduct.product.tags = sourceProduct.product.tags.ToString().Replace(tag, price_tag);
                    if ("UK" == target_country_code)
                    {
                        if (sourceProduct.product.tags.ToString().Contains("$"))
                        {
                            sourceProduct.product.tags = sourceProduct.product.tags.ToString().Replace("$", "£");
                        }
                    }
                    if ("FR" == target_country_code || "BE" == target_country_code || "IE" == target_country_code)
                    {
                        if (sourceProduct.product.tags.ToString().Contains("$"))
                        {
                            sourceProduct.product.tags = sourceProduct.product.tags.ToString().Replace("$", "€");
                        }
                    }
                    if ("MY" == target_country_code)
                    {
                        if (sourceProduct.product.tags.ToString().Contains("$"))
                        {
                            sourceProduct.product.tags = sourceProduct.product.tags.ToString().Replace("$", "RM");
                        }
                    }
                    break;
                }
            }
            //在其它站点创建产品
            ProductEntity newProduct = CreateProduct(strUrl, sourceProduct);
            result = newProduct != null ? true : false;
            if (true == result)
            {
                //判断原站点有没有Metafield
                //处理strUrl
                string strMetafieldUrl = string.Format("{0}/products/{1}/metafields.json", strSourceUrl.Substring(0, strSourceUrl.IndexOf("admin") + 5), sourceProduct.product.id);
                MetafieldsEntity metafields = GetMetafields(strMetafieldUrl);
                MetafieldEntity sourceMetafield = new MetafieldEntity();
                if (metafields != null && metafields.metafields != null && metafields.metafields.Count > 0)
                {
                    foreach (MetafieldEntity m in metafields.metafields)
                    {
                        if (m.key.ToString().ToLower() == "title_tag")
                        {
                            sourceMetafield = m;
                            if (null != sourceMetafield.id)
                            {
                                string strPageTitle = Convert.ToString(sourceMetafield.value);
                                if (strPageTitle.Contains("$") || strPageTitle.Contains("£") || strPageTitle.Contains("€"))
                                {
                                    strPageTitle = strPageTitle.Substring(strPageTitle.Trim().IndexOf(" ") + 1);

                                    strPageTitle = string.Format("${0}{1}", Convert.ToDouble(afterChangedPrice).ToString("0.00"), strPageTitle);

                                    if (strPageTitle.Contains(".00"))
                                    {
                                        strPageTitle = strPageTitle.Replace(".00", "");
                                    }

                                    //有则在目标站点上也创建Product的metafield
                                    strPageTitle = "{\"metafield\":{\"namespace\":\""
                                        + Convert.ToString(m.@namespace)
                                        + "\",\"key\":\""
                                        + Convert.ToString(m.key) + "\",\"value\":\""
                                        + strPageTitle.Replace("\"", string.Format("{0}{1}", "\\", "\"")) + "\",\"value_type\":\""
                                        + Convert.ToString(m.value_type) + "\"}}";
                                    if ("UK" == target_country_code)
                                    {
                                        strPageTitle = strPageTitle.Replace("$", "£");
                                    }
                                    if ("FR" == target_country_code || "BE" == target_country_code || "IE" == target_country_code)
                                    {
                                        strPageTitle = strPageTitle.Replace("$", "€");
                                    }
                                    if ("MY" == target_country_code)
                                    {
                                        strPageTitle = strPageTitle.Replace("$", "RM");
                                    }
                                    strMetafieldUrl = string.Format("{0}/products/{1}/metafields.json", strUrl.Substring(0, strUrl.IndexOf("admin") + 5), newProduct.product.id);
                                    //创建Metafield
                                    result = CreateProductMetafields(strMetafieldUrl, strPageTitle);
                                }
                                else
                                {
                                    strPageTitle = Convert.ToString(sourceMetafield.value);

                                    //有则在目标站点上也创建Product的metafield
                                    strPageTitle = "{\"metafield\":{\"namespace\":\""
                                        + Convert.ToString(m.@namespace)
                                        + "\",\"key\":\""
                                        + Convert.ToString(m.key) + "\",\"value\":\""
                                        + strPageTitle.Replace("\"", string.Format("{0}{1}", "\\", "\"")) + "\",\"value_type\":\""
                                        + Convert.ToString(m.value_type) + "\"}}";

                                    strMetafieldUrl = string.Format("{0}/products/{1}/metafields.json", strUrl.Substring(0, strUrl.IndexOf("admin") + 5), newProduct.product.id);
                                    //创建Metafield
                                    result = CreateProductMetafields(strMetafieldUrl, strPageTitle);
                                }
                            }
                            else
                            {
                                result = false;
                            }
                            break;
                        }
                    }
                }
                else
                {
                    //没有则不创建Metafield
                    result = true;
                }
            }
            return result;
        }
        #endregion

        #region 创建产品
        private static ProductEntity CreateProduct(string strUrl, ProductEntity sourceProduct)
        {
            jsonRequest = new JsonRequest();
            jsonRequest.Credentials = new Credentials();
            jsonRequest.Credentials.UserName = Common.GetUserByUrl(strUrl);
            jsonRequest.Credentials.Password = Common.GetPwdByUrl(strUrl);
            ProductEntity product = (ProductEntity)jsonRequest.Execute<ProductEntity>(strUrl, sourceProduct, "POST");
            return product;
        }
        #endregion

        #region 获取价格标签
        private static string getPriceTag(double price)
        {
            string price_tag = string.Empty;
            if (price < 10)
            {
                price_tag = lstPriceTags[0];
            }
            else if (price < 20)
            {
                price_tag = lstPriceTags[1];
            }
            else if (price < 30)
            {
                price_tag = lstPriceTags[2];
            }
            else if (price < 40)
            {
                price_tag = lstPriceTags[3];
            }
            else if (price < 70)
            {
                price_tag = lstPriceTags[4];
            }
            else if (price < 110)
            {
                price_tag = lstPriceTags[5];
            }
            return price_tag;
        }
        #endregion

        #region 获取产品Metafields信息
        public static MetafieldsEntity GetMetafields(string strUrl)
        {
            MetafieldsEntity metafields = new MetafieldsEntity();
            try
            {
                jsonRequest = new JsonRequest(strUrl, "GET");
                jsonRequest.Credentials = new Credentials();
                jsonRequest.Credentials.UserName = Common.GetUserByUrl(strUrl);
                jsonRequest.Credentials.Password = Common.GetPwdByUrl(strUrl);
                metafields = (MetafieldsEntity)jsonRequest.Execute<MetafieldsEntity>();
            }
            catch
            {
                metafields = new MetafieldsEntity();
            }
            return metafields;
        }
        #endregion

        #region 创建产品Metafields信息
        private static bool CreateProductMetafields(string strUrl, object metafield)
        {
            bool result = false;
            jsonRequest = new JsonRequest();
            jsonRequest.Credentials = new Credentials();
            jsonRequest.Credentials.UserName = Common.GetUserByUrl(strUrl);
            jsonRequest.Credentials.Password = Common.GetPwdByUrl(strUrl);
            MetafieldUpdateResultEntity response = (MetafieldUpdateResultEntity)jsonRequest.Execute<MetafieldUpdateResultEntity>(strUrl, metafield, "POST");
            if (response != null)
            {
                result = true;
            }
            return result;
        }
        #endregion

        #region 更新产品Metafield
        public static bool UpdateMetafield(string strUrl, object metafield)
        {
            bool result = false;
            try
            {
                jsonRequest = new JsonRequest();
                jsonRequest.Credentials = new Credentials();
                jsonRequest.Credentials.UserName = Common.GetUserByUrl(strUrl);
                jsonRequest.Credentials.Password = Common.GetPwdByUrl(strUrl);
                MetafieldUpdateResultEntity metafieldContent = (MetafieldUpdateResultEntity)jsonRequest.Execute<MetafieldUpdateResultEntity>(strUrl, metafield, "PUT");
                if (string.IsNullOrEmpty(metafieldContent.metafield.id.ToString()) == false)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        #endregion



        private static double getAfterChangedPrice(double auPrice, double weight, double target_price_offset, string strCode)
        {
            #region 价格公式
            //如果小包价格>东南亚专线
            //      新加坡  =（AU产品售价-(小包-东南亚专线)/5)*1.1 
            //      马来     =（AU产品售价-(小包-东南亚专线)/5)*2.9
            //否则
            //      新加坡  =（AU产品售价 +（东南亚专线-小包）/5)*1.1 
            //      马来     =（AU产品售价+(东南亚专线-小包)/5)*2.9
            //如果结果小数小于0.5 把小数修改成.00 否则修改成.99
            #endregion

            double newPrice = 0;
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(File.ReadAllText(Path.Combine(System.Environment.CurrentDirectory, strCode.ToLower() + ".xml")));
            XmlNodeList xmlNodes = xdoc["postage"].ChildNodes;
            foreach (XmlNode node in xmlNodes)
            {
                double xmlWeight = Convert.ToDouble(node.Attributes["weight"].Value.Trim());
                if (xmlWeight == weight || Math.Abs(xmlWeight - weight) < 50)
                {
                    double packet = Convert.ToDouble(node.Attributes["packet"].Value.Trim());
                    double specialline = Convert.ToDouble(node.Attributes["specialline"].Value.Trim());
                    if (packet > specialline)
                    {
                        if ("MY" == strCode)
                        {
                            newPrice = (auPrice - (packet - specialline) / 0.6 / 5) * target_price_offset;
                        }
                        else
                        {
                            newPrice = (auPrice - (packet - specialline) / 5) * target_price_offset;
                        }
                    }
                    else
                    {
                        if ("MY" == strCode)
                        {
                            newPrice = (auPrice + (specialline - packet) / 0.6 / 5) * target_price_offset;
                        }
                        else
                        {
                            newPrice = (auPrice + (specialline - packet) / 5) * target_price_offset;
                        }
                    }
                    int tempInt = (int)newPrice;
                    double tempDecimal = newPrice % 1;
                    if (tempDecimal > 0.5)
                    {
                        newPrice = Convert.ToDouble(tempInt) + 0.90;
                    }
                    else
                    {
                        newPrice = Convert.ToDouble(tempInt);
                    }
                    break;
                }
            }
            return newPrice;
        }
    }
}