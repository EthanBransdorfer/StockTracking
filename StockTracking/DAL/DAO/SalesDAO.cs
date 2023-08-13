using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockTracking.DAL.DTO;
using StockTracking.DAL;
using System.Security.Cryptography;

namespace StockTracking.DAL.DAO
{
    public class SalesDAO : StockContext, IDAO<SALE, SalesDetailDTO>
    {
        public bool Delete(SALE entity)
        {
            try
            {
                if (entity.ID != 0)
                {
                    SALE sales = db.SALEs.First(x => x.ID == entity.ID);
                    sales.isDeleted = true;
                    sales.DeletedDate = DateTime.Today;
                    db.SaveChanges();
                }
                else if (entity.ProductID != 0) 
                {
                    List<SALE> sales = db.SALEs.Where(x => x.ProductID == entity.ProductID).ToList();

                    foreach (var item in sales)
                    {
                        item.isDeleted = true;
                        item.DeletedDate = DateTime.Today;
                    }
                    db.SaveChanges();
                }
                else if (entity.CustomerID != 0)
                {
                    List<SALE> sales = db.SALEs.Where(x => x.CustomerID == entity.CustomerID).ToList();

                    foreach (var item in sales)
                    {
                        item.isDeleted = true;
                        item.DeletedDate = DateTime.Today;
                    }
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GetBack(int ID)
        {
            try
            {
                SALE sales = db.SALEs.First(x => x.ID == ID);
                sales.isDeleted = false;
                sales.DeletedDate = null;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(SALE entity)
        {
            try
            {
                db.SALEs.Add(entity);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SalesDetailDTO> Select()
        {
            List<SalesDetailDTO> sales = new List<SalesDetailDTO>();
            var list = (from s in db.SALEs.Where(x => x.isDeleted == false)
                        join p in db.PRODUCTs on s.ProductID equals p.ID
                        join c in db.CUSTOMERs on s.CustomerID equals c.ID
                        join category in db.CATEGORies on s.CategoryID equals category.ID
                        select new
                        {
                            productname = p.ProductName,
                            customername = c.CustomerName,
                            categoryname = category.CategoryName,
                            productID = s.ProductID,
                            customerID = s.CustomerID,
                            salesID = s.ID,
                            categoryID = s.CategoryID,
                            salesprice = s.ProductSalesPrice,
                            salesamount = s.ProductSalesAmount,
                            stockamount = p.StockAmount,
                            salesdate = s.SalesDate
                        }).OrderBy(x => x.salesdate).ToList();
            foreach (var item in list)
            {
                SalesDetailDTO dto =  new SalesDetailDTO();
                dto.ProductName = item.productname;
                dto.CustomerName = item.customername;
                dto.CategoryName = item.categoryname;
                dto.ProductID = item.productID;
                dto.CustomerID = item.customerID;
                dto.SalesID = item.salesID;
                dto.CategoryID = item.categoryID;
                dto.Price = item.salesprice;
                dto.SalesAmount = item.salesamount;
                dto.SalesDate = item.salesdate;
                dto.StockAmount = item.stockamount;
                sales.Add(dto);
            }
            return sales;
        }
        public List<SalesDetailDTO> Select(bool isDeleted)
        {
            List<SalesDetailDTO> sales = new List<SalesDetailDTO>();
            var list = (from s in db.SALEs.Where(x => x.isDeleted == isDeleted)
                        join p in db.PRODUCTs on s.ProductID equals p.ID
                        join c in db.CUSTOMERs on s.CustomerID equals c.ID
                        join category in db.CATEGORies on s.CategoryID equals category.ID
                        select new
                        {
                            productname = p.ProductName,
                            customername = c.CustomerName,
                            categoryname = category.CategoryName,
                            productID = s.ProductID,
                            customerID = s.CustomerID,
                            salesID = s.ID,
                            categoryID = s.CategoryID,
                            salesprice = s.ProductSalesPrice,
                            salesamount = s.ProductSalesAmount,
                            stockamount = p.StockAmount,
                            salesdate = s.SalesDate,
                            categoryisDeleted = category.isDeleted,
                            productisDeleted = p.isDeleted,
                            customerisDeleted = c.isDeleted
                        }).OrderBy(x => x.salesdate).ToList();
            foreach (var item in list)
            {
                SalesDetailDTO dto = new SalesDetailDTO();
                dto.ProductName = item.productname;
                dto.CustomerName = item.customername;
                dto.CategoryName = item.categoryname;
                dto.ProductID = item.productID;
                dto.CustomerID = item.customerID;
                dto.SalesID = item.salesID;
                dto.CategoryID = item.categoryID;
                dto.Price = item.salesprice;
                dto.SalesAmount = item.salesamount;
                dto.SalesDate = item.salesdate;
                dto.StockAmount = item.stockamount;
                dto.isCategoryDeleted = item.categoryisDeleted;
                dto.isCustomerDeleted = item.customerisDeleted;
                dto.isProductDeleted = item.productisDeleted;
                sales.Add(dto);
            }
            return sales;
        }

        public bool Update(SALE entity)
        {
            try
            {
                SALE sales = db.SALEs.First(x => x.ID == entity.ID);
                sales.ProductSalesAmount = entity.ProductSalesAmount;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
