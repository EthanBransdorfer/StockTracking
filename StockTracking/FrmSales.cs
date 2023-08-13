using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StockTracking.DAL;
using StockTracking.DAL.DTO;
using StockTracking.BLL;

namespace StockTracking
{
    public partial class FrmSales : Form
    {
        public FrmSales()
        {
            InitializeComponent();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public SalesDTO dto = new SalesDTO();
        private void FrmSales_Load(object sender, EventArgs e)
        {
            cmbCategory.DataSource = dto.Categories;
            cmbCategory.DisplayMember = "CategoryName";
            cmbCategory.ValueMember = "ID";
            cmbCategory.SelectedIndex = -1;
            GridCategory.DataSource = dto.Products;
            GridCategory.Columns[0].HeaderText = "Product Name";
            GridCategory.Columns[1].HeaderText = "Category Name";
            GridCategory.Columns[2].HeaderText = "Stock Amount";
            GridCategory.Columns[3].HeaderText = "Price";
            GridCategory.Columns[4].Visible = false;
            GridCategory.Columns[5].Visible = false;

            GridCustomer.DataSource = dto.Customers;
            GridCustomer.Columns[0].Visible = false;
            GridCustomer.Columns[1].HeaderText = "Customer Name";
            if (dto.Categories.Count > 0)
            {
                combofull = true;
            }
        }
        bool combofull = false;
        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combofull)
            {
                List<ProductDetailDTO> list = dto.Products;
                list = list.Where(x => x.CategoryID == Convert.ToInt32(cmbCategory.SelectedValue)).ToList();
                GridCategory.DataSource = list;
                if (list.Count == 0)
                {
                    txtPrice.Clear();
                    txtProductName.Clear();
                    txtStock.Clear();
                }
            }
        }

        private void txtCustomerSearch_TextChanged(object sender, EventArgs e)
        {
            List<CustomerDetailDTO> list = dto.Customers;
            list = list.Where(x => x.CustomerName.Contains(txtCustomerSearch.Text)).ToList();
            GridCustomer.DataSource = list;
            if (list.Count == 0)
            {
                txtCustomerName.Clear();
            }
        }
        SalesDetailDTO detail = new SalesDetailDTO();
        SalesBLL bll = new SalesBLL();
        private void GridCategory_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            detail.ProductName = GridCategory.Rows[e.RowIndex].Cells[0].Value.ToString();
            detail.Price = Convert.ToInt32(GridCategory.Rows[e.RowIndex].Cells[3].Value);
            detail.StockAmount = Convert.ToInt32(GridCategory.Rows[e.RowIndex].Cells[2].Value);
            detail.ProductID = Convert.ToInt32(GridCategory.Rows[e.RowIndex].Cells[4].Value);
            detail.CategoryID = Convert.ToInt32(GridCategory.Rows[e.RowIndex].Cells[5].Value);
            txtProductName.Text = detail.ProductName;
            txtPrice.Text = detail.Price.ToString();
            txtStock.Text = detail.StockAmount.ToString();
        }

        private void GridCustomer_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            detail.CustomerName = GridCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
            detail.CustomerID = Convert.ToInt32(GridCustomer.Rows[e.RowIndex].Cells[0].Value);
            txtCustomerName.Text = detail.CustomerName;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (detail.ProductID == 0)
            {
                MessageBox.Show("Please select a product from the top table.");
            }
            else if (detail.CustomerID == 0) 
            {
                MessageBox.Show("Please select a customer from the bottom table.");
            }
            else if (detail.StockAmount < Convert.ToInt32(txtSalesAmount.Text))
            {
                MessageBox.Show("Attempting to sell more product than we have.");
            }
            else
            {
                detail.SalesAmount = Convert.ToInt32(txtSalesAmount.Text);
                detail.SalesDate = DateTime.Today;
                if (bll.Insert(detail))
                {
                    MessageBox.Show("Sale was added succesfully");
                    bll = new SalesBLL();
                    dto = bll.Select();
                    GridCategory.DataSource = dto.Products;
                    GridCustomer.DataSource = dto.Customers;
                    combofull = false;
                    cmbCategory.DataSource = dto.Categories;
                    if (dto.Products.Count > 0)
                    {
                        combofull = true;
                    }
                    txtSalesAmount.Clear();
                }
            }
        }
    }
}
