using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StockTracking.DAL.DTO;
using StockTracking.BLL;

namespace StockTracking
{
    public partial class FrmProduct : Form
    {
        public FrmProduct()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);
        }
        public ProductDTO dto = new ProductDTO();
        public ProductDetailDTO detail = new ProductDetailDTO();
        public bool isUpdate = false;
        private void FrmProduct_Load(object sender, EventArgs e)
        {
            cmbCategory.DataSource = dto.Categories;
            cmbCategory.DisplayMember = "CategoryName";
            cmbCategory.ValueMember = "ID";
            cmbCategory.SelectedIndex = -1;
            if (isUpdate)
            {
                txtProductName.Text = detail.ProductName;
                txtPrice.Text = detail.Price.ToString();
                cmbCategory.SelectedValue = detail.CategoryID;


            }
        }

        ProductBLL bll = new ProductBLL();  
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtProductName.Text.Trim() == "")
            {
                MessageBox.Show("Product name is empty.");
            }
            else if (cmbCategory.SelectedIndex == -1) 
            {
                MessageBox.Show("No category is selected.");
            }
            else if (txtPrice.Text.Trim() == "")
            {
                MessageBox.Show("Price is empty.");
            }
            else
            {
                if (!isUpdate)
                {
                    ProductDetailDTO product = new ProductDetailDTO();
                    product.ProductName = txtProductName.Text;
                    product.CategoryID = Convert.ToInt32(cmbCategory.SelectedValue);
                    product.Price = Convert.ToInt32(txtPrice.Text);
                    if (bll.Insert(product))
                    {
                        MessageBox.Show("Product was added.");
                        cmbCategory.SelectedIndex = -1;
                        txtPrice.Clear();
                        txtProductName.Clear();
                    }
                }
                else
                {
                    if (txtProductName.Text == detail.ProductName.ToString() &&
                        Convert.ToInt32(txtPrice.Text) == detail.Price 
                        && detail.CategoryID == Convert.ToInt32(cmbCategory.SelectedValue))
                    {
                        MessageBox.Show("There is no change.");
                    }
                    else
                    {
                        detail.ProductName = txtProductName.Text;
                        detail.CategoryID = Convert.ToInt32(cmbCategory.SelectedValue);
                        detail.Price = Convert.ToInt32(txtPrice.Text);
                        if (bll.Update(detail)) 
                        {
                            MessageBox.Show("Product was updated");
                            this.Close();
                        }
                    }
                }
            }
        }
    }
}
