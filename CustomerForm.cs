using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace Computer_Shop_System
{
    public partial class CustomerForm: Form
    {
        public CustomerForm()
        {
            InitializeComponent();
        }

        private void CustomerForm_Load(object sender, EventArgs e)
        {   
            DisplayProducts();
            products_DataGrid.ClearSelection();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        public static extern void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        public static extern void SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private void customerForm_Dashboard_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void minimizeBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            Session.UserId = 0;
            Session.Username = null;
            Session.Role = null;
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }

        public void AddToCart(int userId, int productId, int quantity)
        {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system"))
            {
                MySqlCommand insertCommand = new MySqlCommand("INSERT INTO shopping_cart(`User ID`, `Product ID`, `Quantity`) VALUES(@userId, @productId, @quantity)", connection);
                insertCommand.Parameters.AddWithValue("@userId", userId);
                insertCommand.Parameters.AddWithValue("@productId", productId);
                insertCommand.Parameters.AddWithValue("@quantity", quantity);

                try
                {
                    connection.Open();
                    insertCommand.ExecuteNonQuery();
                    MessageBox.Show("Item added to cart!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to add to cart: " + ex.Message);
                }
               
            }
        }

        // Display Grids

        public void DisplayProducts() {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=computer_shop_system")) 
            {
                MySqlCommand displayCommand = new MySqlCommand("SELECT `Image`, `Name`, `Price` FROM products", connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(displayCommand);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                try
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in dataTable.Rows)
                        {
                            // Convert image data to Image type
                            string name = row["Name"].ToString();
                            int price = Convert.ToInt32(row["Price"]);

                            products_DataGrid.Rows.Add(null, name, price);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load products: " + ex.Message);
                }
            }
        }

        int productId;
        int quantity;

        private void products_DataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) { 
                products_NameDisplay.Text = products_DataGrid.Rows[e.RowIndex].Cells[1].Value.ToString();
                products_PriceDisplay.Text = products_DataGrid.Rows[e.RowIndex].Cells[2].Value.ToString();
                products_QuantityDisplay.Text = "1";
            }
        }

        private void QuantityIncreaseBtn_Click(object sender, EventArgs e)
        {
            products_QuantityDisplay.Text = (Convert.ToInt32(products_QuantityDisplay.Text) + 1).ToString();
        }

        private void QuantityDecreaseBtn_Click(object sender, EventArgs e)
        {
            products_QuantityDisplay.Text = (Convert.ToInt32(products_QuantityDisplay.Text) - 1).ToString();
        }
    }
}
