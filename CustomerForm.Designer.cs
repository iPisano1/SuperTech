namespace Computer_Shop_System
{
    partial class CustomerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.customerForm_Dashboard = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.minimizeBtn = new System.Windows.Forms.Button();
            this.exitBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cartCounterLabel = new System.Windows.Forms.Label();
            this.products_DataGrid = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.logoutBtn = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.products_AddToCartBtn = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.productsPanel = new System.Windows.Forms.Panel();
            this.button8 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.QuantityDecreaseBtn = new System.Windows.Forms.Button();
            this.QuantityIncreaseBtn = new System.Windows.Forms.Button();
            this.products_QuantityDisplay = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.products_PriceDisplay = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.products_NameDisplay = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.products_Image = new System.Windows.Forms.Panel();
            this.imageColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priceColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerForm_Dashboard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.products_DataGrid)).BeginInit();
            this.panel2.SuspendLayout();
            this.productsPanel.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // customerForm_Dashboard
            // 
            this.customerForm_Dashboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(87)))), ((int)(((byte)(122)))));
            this.customerForm_Dashboard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.customerForm_Dashboard.Controls.Add(this.label2);
            this.customerForm_Dashboard.Controls.Add(this.panel1);
            this.customerForm_Dashboard.Controls.Add(this.minimizeBtn);
            this.customerForm_Dashboard.Controls.Add(this.exitBtn);
            this.customerForm_Dashboard.Controls.Add(this.label4);
            this.customerForm_Dashboard.Controls.Add(this.cartCounterLabel);
            this.customerForm_Dashboard.Location = new System.Drawing.Point(-1, -2);
            this.customerForm_Dashboard.Name = "customerForm_Dashboard";
            this.customerForm_Dashboard.Size = new System.Drawing.Size(1031, 59);
            this.customerForm_Dashboard.TabIndex = 1;
            this.customerForm_Dashboard.MouseDown += new System.Windows.Forms.MouseEventHandler(this.customerForm_Dashboard_MouseDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(436, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(158, 40);
            this.label2.TabIndex = 4;
            this.label2.Text = "SuperTech";
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::Computer_Shop_System.Properties.Resources.dashboards;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel1.Location = new System.Drawing.Point(13, 11);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(45, 40);
            this.panel1.TabIndex = 4;
            // 
            // minimizeBtn
            // 
            this.minimizeBtn.BackColor = System.Drawing.Color.Transparent;
            this.minimizeBtn.BackgroundImage = global::Computer_Shop_System.Properties.Resources.minimize;
            this.minimizeBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.minimizeBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.minimizeBtn.FlatAppearance.BorderSize = 0;
            this.minimizeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.minimizeBtn.ForeColor = System.Drawing.Color.Transparent;
            this.minimizeBtn.Location = new System.Drawing.Point(929, 11);
            this.minimizeBtn.Margin = new System.Windows.Forms.Padding(0);
            this.minimizeBtn.Name = "minimizeBtn";
            this.minimizeBtn.Size = new System.Drawing.Size(45, 40);
            this.minimizeBtn.TabIndex = 5;
            this.minimizeBtn.UseVisualStyleBackColor = false;
            this.minimizeBtn.Click += new System.EventHandler(this.minimizeBtn_Click);
            // 
            // exitBtn
            // 
            this.exitBtn.BackColor = System.Drawing.Color.Transparent;
            this.exitBtn.BackgroundImage = global::Computer_Shop_System.Properties.Resources.exit;
            this.exitBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.exitBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.exitBtn.FlatAppearance.BorderSize = 0;
            this.exitBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exitBtn.ForeColor = System.Drawing.Color.Transparent;
            this.exitBtn.Location = new System.Drawing.Point(974, 11);
            this.exitBtn.Margin = new System.Windows.Forms.Padding(0);
            this.exitBtn.Name = "exitBtn";
            this.exitBtn.Size = new System.Drawing.Size(45, 40);
            this.exitBtn.TabIndex = 4;
            this.exitBtn.UseVisualStyleBackColor = false;
            this.exitBtn.Click += new System.EventHandler(this.exitBtn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(56, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 25);
            this.label4.TabIndex = 8;
            this.label4.Text = "Cart:";
            // 
            // cartCounterLabel
            // 
            this.cartCounterLabel.AutoSize = true;
            this.cartCounterLabel.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cartCounterLabel.ForeColor = System.Drawing.Color.White;
            this.cartCounterLabel.Location = new System.Drawing.Point(106, 18);
            this.cartCounterLabel.Name = "cartCounterLabel";
            this.cartCounterLabel.Size = new System.Drawing.Size(22, 25);
            this.cartCounterLabel.TabIndex = 9;
            this.cartCounterLabel.Text = "0";
            // 
            // products_DataGrid
            // 
            this.products_DataGrid.AllowUserToAddRows = false;
            this.products_DataGrid.AllowUserToDeleteRows = false;
            this.products_DataGrid.AllowUserToResizeColumns = false;
            this.products_DataGrid.AllowUserToResizeRows = false;
            this.products_DataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.products_DataGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(87)))), ((int)(((byte)(122)))));
            this.products_DataGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.products_DataGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.products_DataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.products_DataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.products_DataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.imageColumn,
            this.nameColumn,
            this.priceColumn});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.products_DataGrid.DefaultCellStyle = dataGridViewCellStyle8;
            this.products_DataGrid.EnableHeadersVisualStyles = false;
            this.products_DataGrid.GridColor = System.Drawing.Color.Black;
            this.products_DataGrid.Location = new System.Drawing.Point(43, 125);
            this.products_DataGrid.Margin = new System.Windows.Forms.Padding(40);
            this.products_DataGrid.MultiSelect = false;
            this.products_DataGrid.Name = "products_DataGrid";
            this.products_DataGrid.ReadOnly = true;
            this.products_DataGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.products_DataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.products_DataGrid.RowHeadersVisible = false;
            this.products_DataGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.products_DataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.products_DataGrid.Size = new System.Drawing.Size(482, 382);
            this.products_DataGrid.TabIndex = 2;
            this.products_DataGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.products_DataGrid_CellClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(36, 24);
            this.label1.Margin = new System.Windows.Forms.Padding(40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 40);
            this.label1.TabIndex = 3;
            this.label1.Text = "Products";
            // 
            // logoutBtn
            // 
            this.logoutBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.logoutBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.logoutBtn.FlatAppearance.BorderSize = 0;
            this.logoutBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.logoutBtn.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logoutBtn.Location = new System.Drawing.Point(0, 502);
            this.logoutBtn.Name = "logoutBtn";
            this.logoutBtn.Size = new System.Drawing.Size(206, 42);
            this.logoutBtn.TabIndex = 0;
            this.logoutBtn.Text = "Log Out";
            this.logoutBtn.UseVisualStyleBackColor = false;
            this.logoutBtn.Click += new System.EventHandler(this.logoutBtn_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(87)))), ((int)(((byte)(122)))));
            this.panel2.Controls.Add(this.button4);
            this.panel2.Controls.Add(this.button3);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.logoutBtn);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Location = new System.Drawing.Point(-1, 57);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(206, 557);
            this.panel2.TabIndex = 5;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(137)))), ((int)(((byte)(214)))), ((int)(((byte)(251)))));
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(0, 174);
            this.button4.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(206, 44);
            this.button4.TabIndex = 3;
            this.button4.Text = "Cart";
            this.button4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.UseVisualStyleBackColor = false;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(137)))), ((int)(((byte)(214)))), ((int)(((byte)(251)))));
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(0, 238);
            this.button3.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(206, 44);
            this.button3.TabIndex = 2;
            this.button3.Text = "Check Out";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(137)))), ((int)(((byte)(214)))), ((int)(((byte)(251)))));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(0, 302);
            this.button2.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(206, 44);
            this.button2.TabIndex = 1;
            this.button2.Text = "Order History";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(137)))), ((int)(((byte)(214)))), ((int)(((byte)(251)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(0, 110);
            this.button1.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(206, 44);
            this.button1.TabIndex = 0;
            this.button1.Text = "Products";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.UseVisualStyleBackColor = false;
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(100, 93);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(115, 27);
            this.comboBox1.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(38, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 25);
            this.label3.TabIndex = 7;
            this.label3.Text = "Sort:";
            // 
            // products_AddToCartBtn
            // 
            this.products_AddToCartBtn.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.products_AddToCartBtn.Location = new System.Drawing.Point(551, 465);
            this.products_AddToCartBtn.Name = "products_AddToCartBtn";
            this.products_AddToCartBtn.Size = new System.Drawing.Size(241, 42);
            this.products_AddToCartBtn.TabIndex = 10;
            this.products_AddToCartBtn.Text = "Add To Cart";
            this.products_AddToCartBtn.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(233, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 25);
            this.label5.TabIndex = 12;
            this.label5.Text = "Search:";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(313, 93);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(135, 27);
            this.textBox1.TabIndex = 13;
            // 
            // productsPanel
            // 
            this.productsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(240)))), ((int)(((byte)(252)))));
            this.productsPanel.Controls.Add(this.button8);
            this.productsPanel.Controls.Add(this.panel3);
            this.productsPanel.Controls.Add(this.textBox1);
            this.productsPanel.Controls.Add(this.label1);
            this.productsPanel.Controls.Add(this.label5);
            this.productsPanel.Controls.Add(this.products_DataGrid);
            this.productsPanel.Controls.Add(this.products_AddToCartBtn);
            this.productsPanel.Controls.Add(this.comboBox1);
            this.productsPanel.Controls.Add(this.label3);
            this.productsPanel.Location = new System.Drawing.Point(205, 57);
            this.productsPanel.Name = "productsPanel";
            this.productsPanel.Size = new System.Drawing.Size(825, 557);
            this.productsPanel.TabIndex = 14;
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.White;
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button8.Location = new System.Drawing.Point(454, 93);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(71, 27);
            this.button8.TabIndex = 15;
            this.button8.Text = "Search";
            this.button8.UseVisualStyleBackColor = false;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.QuantityDecreaseBtn);
            this.panel3.Controls.Add(this.QuantityIncreaseBtn);
            this.panel3.Controls.Add(this.products_QuantityDisplay);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.products_PriceDisplay);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.products_NameDisplay);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.products_Image);
            this.panel3.Location = new System.Drawing.Point(551, 24);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(241, 422);
            this.panel3.TabIndex = 14;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(17, 282);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(23, 25);
            this.label9.TabIndex = 9;
            this.label9.Text = "₱";
            // 
            // QuantityDecreaseBtn
            // 
            this.QuantityDecreaseBtn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QuantityDecreaseBtn.Location = new System.Drawing.Point(21, 355);
            this.QuantityDecreaseBtn.Name = "QuantityDecreaseBtn";
            this.QuantityDecreaseBtn.Size = new System.Drawing.Size(39, 29);
            this.QuantityDecreaseBtn.TabIndex = 8;
            this.QuantityDecreaseBtn.Text = "-";
            this.QuantityDecreaseBtn.UseVisualStyleBackColor = true;
            this.QuantityDecreaseBtn.Click += new System.EventHandler(this.QuantityDecreaseBtn_Click);
            // 
            // QuantityIncreaseBtn
            // 
            this.QuantityIncreaseBtn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QuantityIncreaseBtn.Location = new System.Drawing.Point(121, 354);
            this.QuantityIncreaseBtn.Name = "QuantityIncreaseBtn";
            this.QuantityIncreaseBtn.Size = new System.Drawing.Size(39, 29);
            this.QuantityIncreaseBtn.TabIndex = 7;
            this.QuantityIncreaseBtn.Text = "+";
            this.QuantityIncreaseBtn.UseVisualStyleBackColor = true;
            this.QuantityIncreaseBtn.Click += new System.EventHandler(this.QuantityIncreaseBtn_Click);
            // 
            // products_QuantityDisplay
            // 
            this.products_QuantityDisplay.Enabled = false;
            this.products_QuantityDisplay.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.products_QuantityDisplay.Location = new System.Drawing.Point(66, 354);
            this.products_QuantityDisplay.Name = "products_QuantityDisplay";
            this.products_QuantityDisplay.ReadOnly = true;
            this.products_QuantityDisplay.Size = new System.Drawing.Size(49, 29);
            this.products_QuantityDisplay.TabIndex = 6;
            this.products_QuantityDisplay.Text = "0";
            this.products_QuantityDisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(18, 329);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 21);
            this.label8.TabIndex = 5;
            this.label8.Text = "Quantity";
            // 
            // products_PriceDisplay
            // 
            this.products_PriceDisplay.Enabled = false;
            this.products_PriceDisplay.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.products_PriceDisplay.Location = new System.Drawing.Point(38, 281);
            this.products_PriceDisplay.Name = "products_PriceDisplay";
            this.products_PriceDisplay.ReadOnly = true;
            this.products_PriceDisplay.Size = new System.Drawing.Size(86, 29);
            this.products_PriceDisplay.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(17, 254);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 21);
            this.label7.TabIndex = 3;
            this.label7.Text = "Price";
            // 
            // products_NameDisplay
            // 
            this.products_NameDisplay.Enabled = false;
            this.products_NameDisplay.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.products_NameDisplay.Location = new System.Drawing.Point(20, 204);
            this.products_NameDisplay.Name = "products_NameDisplay";
            this.products_NameDisplay.ReadOnly = true;
            this.products_NameDisplay.Size = new System.Drawing.Size(201, 29);
            this.products_NameDisplay.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(17, 180);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 21);
            this.label6.TabIndex = 1;
            this.label6.Text = "Name";
            // 
            // products_Image
            // 
            this.products_Image.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.products_Image.Location = new System.Drawing.Point(20, 20);
            this.products_Image.Margin = new System.Windows.Forms.Padding(20);
            this.products_Image.Name = "products_Image";
            this.products_Image.Size = new System.Drawing.Size(201, 133);
            this.products_Image.TabIndex = 0;
            // 
            // imageColumn
            // 
            this.imageColumn.HeaderText = "Image";
            this.imageColumn.Name = "imageColumn";
            this.imageColumn.ReadOnly = true;
            // 
            // nameColumn
            // 
            this.nameColumn.HeaderText = "Name";
            this.nameColumn.Name = "nameColumn";
            this.nameColumn.ReadOnly = true;
            // 
            // priceColumn
            // 
            this.priceColumn.HeaderText = "Price";
            this.priceColumn.Name = "priceColumn";
            this.priceColumn.ReadOnly = true;
            // 
            // CustomerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(240)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(1027, 613);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.customerForm_Dashboard);
            this.Controls.Add(this.productsPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CustomerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CustomerForm";
            this.Load += new System.EventHandler(this.CustomerForm_Load);
            this.customerForm_Dashboard.ResumeLayout(false);
            this.customerForm_Dashboard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.products_DataGrid)).EndInit();
            this.panel2.ResumeLayout(false);
            this.productsPanel.ResumeLayout(false);
            this.productsPanel.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel customerForm_Dashboard;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button minimizeBtn;
        private System.Windows.Forms.Button exitBtn;
        private System.Windows.Forms.DataGridView products_DataGrid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button logoutBtn;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label cartCounterLabel;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button products_AddToCartBtn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel productsPanel;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel products_Image;
        private System.Windows.Forms.TextBox products_NameDisplay;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button QuantityDecreaseBtn;
        private System.Windows.Forms.Button QuantityIncreaseBtn;
        private System.Windows.Forms.TextBox products_QuantityDisplay;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox products_PriceDisplay;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.DataGridViewImageColumn imageColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn priceColumn;
    }
}