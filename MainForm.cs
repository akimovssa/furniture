using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace furniture
{
    public partial class MainForm : Form
    {
        private readonly Database db;
        private DataGridView dgvPurchases;
        private DataGridView dgvSales;
        private DataGridView dgvSuppliers;
        private DataGridView dgvEmployees;
        private DataGridView dgvProductCategories;
        private DataGridView dgvProducts;
        private DataGridView dgvClients;
        private TabControl tabControl;

        public MainForm()
        {
            InitializeComponent();
            db = new Database("Host=localhost;Port=5432;Database=furniture_shop;Username=postgres;Password=postgres");
            InitializeComponents();
            LoadDataAsync();
        }

        private void InitializeComponents()
        {
            this.Size = new Size(1000, 600);
            this.Text = "ИП Магомедова Анжела Демировна - Отдел закупок и продаж";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            tabControl = new TabControl
            {
                Dock = DockStyle.Fill
            };

            // Закупки
            var tabPurchases = new TabPage("Закупки");
            dgvPurchases = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.Gray,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            var btnAddPurchase = new Button
            {
                Text = "Добавить закупку",
                Dock = DockStyle.Top,
                Height = 30
            };
            btnAddPurchase.Click += async (s, e) => await ShowPurchaseDialogAsync(null);
            var btnEditPurchase = new Button
            {
                Text = "Редактировать закупку",
                Dock = DockStyle.Top,
                Height = 30
            };
            btnEditPurchase.Click += async (s, e) => await EditPurchaseAsync();
            var btnDeletePurchase = new Button
            {
                Text = "Удалить закупку",
                Dock = DockStyle.Top,
                Height = 30
            };
            btnDeletePurchase.Click += async (s, e) => await DeletePurchaseAsync();
            var panelPurchases = new Panel { Dock = DockStyle.Top, Height = 90 };
            panelPurchases.Controls.AddRange(new Control[] { btnAddPurchase, btnEditPurchase, btnDeletePurchase });
            tabPurchases.Controls.AddRange(new Control[] { dgvPurchases, panelPurchases });

            // Продажи
            var tabSales = new TabPage("Продажи");
            dgvSales = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.Gray,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            var btnAddSale = new Button
            {
                Text = "Добавить продажу",
                Dock = DockStyle.Top,
                Height = 30
            };
            btnAddSale.Click += async (s, e) => await ShowSaleDialogAsync(null);
            var btnEditSale = new Button
            {
                Text = "Редактировать продажу",
                Dock = DockStyle.Top,
                Height = 30
            };
            btnEditSale.Click += async (s, e) => await EditSaleAsync();
            var btnDeleteSale = new Button
            {
                Text = "Удалить продажу",
                Dock = DockStyle.Top,
                Height = 30
            };
            btnDeleteSale.Click += async (s, e) => await DeleteSaleAsync();
            var panelSales = new Panel { Dock = DockStyle.Top, Height = 90 };
            panelSales.Controls.AddRange(new Control[] { btnAddSale, btnEditSale, btnDeleteSale });
            tabSales.Controls.AddRange(new Control[] { dgvSales, panelSales });

            // Поставщики
            var tabSuppliers = new TabPage("Поставщики");
            dgvSuppliers = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.Gray,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            var btnAddSupplier = new Button
            {
                Text = "Добавить поставщика",
                Dock = DockStyle.Top,
                Height = 30
            };
            btnAddSupplier.Click += async (s, e) => await ShowSupplierDialogAsync(null);
            var btnEditSupplier = new Button
            {
                Text = "Редактировать поставщика",
                Dock = DockStyle.Top,
                Height = 30
            };
            btnEditSupplier.Click += async (s, e) => await EditSupplierAsync();
            var btnDeleteSupplier = new Button
            {
                Text = "Удалить поставщика",
                Dock = DockStyle.Top,
                Height = 30
            };
            btnDeleteSupplier.Click += async (s, e) => await DeleteSupplierAsync();
            var panelSuppliers = new Panel { Dock = DockStyle.Top, Height = 90 };
            panelSuppliers.Controls.AddRange(new Control[] { btnAddSupplier, btnEditSupplier, btnDeleteSupplier });
            tabSuppliers.Controls.AddRange(new Control[] { dgvSuppliers, panelSuppliers });

            // Сотрудники
            var tabEmployees = new TabPage("Сотрудники");
            dgvEmployees = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.Gray,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            var btnAddEmployee = new Button
            {
                Text = "Добавить сотрудника",
                Dock = DockStyle.Top,
                Height = 30
            };
            btnAddEmployee.Click += async (s, e) => await ShowEmployeeDialogAsync(null);
            var btnEditEmployee = new Button
            {
                Text = "Редактировать сотрудника",
                Dock = DockStyle.Top,
                Height = 30
            };
            btnEditEmployee.Click += async (s, e) => await EditEmployeeAsync();
            var btnDeleteEmployee = new Button
            {
                Text = "Удалить сотрудника",
                Dock = DockStyle.Top,
                Height = 30
            };
            btnDeleteEmployee.Click += async (s, e) => await DeleteEmployeeAsync();
            var panelEmployees = new Panel { Dock = DockStyle.Top, Height = 90 };
            panelEmployees.Controls.AddRange(new Control[] { btnAddEmployee, btnEditEmployee, btnDeleteEmployee });
            tabEmployees.Controls.AddRange(new Control[] { dgvEmployees, panelEmployees });

            // Категории товаров
            var tabProductCategories = new TabPage("Категории товаров");
            dgvProductCategories = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.Gray,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            var btnAddCategory = new Button
            {
                Text = "Добавить категорию",
                Dock = DockStyle.Top,
                Height = 30
            };
            btnAddCategory.Click += async (s, e) => await ShowCategoryDialogAsync(null);
            var btnEditCategory = new Button
            {
                Text = "Редактировать категорию",
                Dock = DockStyle.Top,
                Height = 30
            };
            btnEditCategory.Click += async (s, e) => await EditCategoryAsync();
            var btnDeleteCategory = new Button
            {
                Text = "Удалить категорию",
                Dock = DockStyle.Top,
                Height = 30
            };
            btnDeleteCategory.Click += async (s, e) => await DeleteCategoryAsync();
            var panelCategories = new Panel { Dock = DockStyle.Top, Height = 90 };
            panelCategories.Controls.AddRange(new Control[] { btnAddCategory, btnEditCategory, btnDeleteCategory });
            tabProductCategories.Controls.AddRange(new Control[] { dgvProductCategories, panelCategories });

            // Товары
            var tabProducts = new TabPage("Товары");
            dgvProducts = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.Gray,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            var btnAddProduct = new Button
            {
                Text = "Добавить товар",
                Dock = DockStyle.Top,
                Height = 30
            };
            btnAddProduct.Click += async (s, e) => await ShowProductDialogAsync(null);
            var btnEditProduct = new Button
            {
                Text = "Редактировать товар",
                Dock = DockStyle.Top,
                Height = 30
            };
            btnEditProduct.Click += async (s, e) => await EditProductAsync();
            var btnDeleteProduct = new Button
            {
                Text = "Удалить товар",
                Dock = DockStyle.Top,
                Height = 30
            };
            btnDeleteProduct.Click += async (s, e) => await DeleteProductAsync();
            var panelProducts = new Panel { Dock = DockStyle.Top, Height = 90 };
            panelProducts.Controls.AddRange(new[] { btnAddProduct, btnEditProduct, btnDeleteProduct });
            tabProducts.Controls.AddRange(new Control[] { dgvProducts, panelProducts });

            // Клиенты
            var tabClients = new TabPage("Клиенты");
            dgvClients = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.Gray,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            var btnAddClient = new Button
            {
                Text = "Добавить клиента",
                Dock = DockStyle.Top,
                Height = 30
            };
            btnAddClient.Click += async (s, e) => await ShowClientDialogAsync(null);
            var btnEditClient = new Button
            {
                Text = "Редактировать клиента",
                Dock = DockStyle.Top,
                Height = 30
            };
            btnEditClient.Click += async (s, e) => await EditClientAsync();
            var btnDeleteClient = new Button
            {
                Text = "Удалить клиента",
                Dock = DockStyle.Top,
                Height = 30
            };
            btnDeleteClient.Click += async (s, e) => await DeleteClientAsync();
            var panelClients = new Panel { Dock = DockStyle.Top, Height = 90 };
            panelClients.Controls.AddRange(new[] { btnAddClient, btnEditClient, btnDeleteClient });
            tabClients.Controls.AddRange(new Control[] { dgvClients, panelClients });

            // Руководство пользователя
            var tabUserGuide = new TabPage("Руководство пользователя");
            var rtbUserGuide = new RichTextBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Font = new Font("Arial", 12),
                Text = @"
Руководство пользователя

1. Вход в систему
   - Запустите приложение и введите логин и пароль на форме авторизации.
   - Пример учетной записи: логин ""admin"", пароль ""12345"".
   - Нажмите ""Войти"" для доступа к главному окну.

2. Главное окно
   - Главное окно содержит вкладки: Закупки, Продажи, Поставщики, Сотрудники, Категории товаров, Товары, Клиенты и Руководство пользователя.
   - Каждая вкладка отображает данные в таблице и предоставляет кнопки для добавления, редактирования и удаления записей.

3. Работа с данными
   - Добавление записи: Нажмите кнопку ""Добавить"" на соответствующей вкладке, заполните поля в открывшейся форме и нажмите ""Сохранить"".
   - Редактирование записи: Выберите строку в таблице и нажмите ""Редактировать"", измените данные и сохраните.
   - Удаление записи: Выберите строку, нажмите ""Удалить"" и подтвердите действие.
   - Обязательные поля помечены в формах. Некорректные данные вызовут сообщение об ошибке.

4. Вкладки
   - Закупки: Управление закупками материалов у поставщиков (название материала, количество, цена, дата).
   - Продажи: Регистрация продаж товаров клиентам (сотрудник, товар, клиент, количество, дата).
   - Поставщики: Управление данными поставщиков (название, контактное лицо, телефон, email, адрес, ИНН).
   - Сотрудники: Управление сотрудниками (имя, фамилия, должность, телефон, email, стаж).
   - Категории товаров: Создание и редактирование категорий товаров.
   - Товары: Управление товарами (категория, название, описание, цена, количество на складе).
   - Клиенты: Управление данными клиентов (имя, фамилия, телефон, email, адрес).

5. Советы
   - Перед удалением категории товаров убедитесь, что она не связана с товарами, чтобы избежать каскадного удаления.
   - Проверяйте корректность ИНН поставщика (10-12 символов).
   - Для больших объемов данных используйте прокрутку таблицы.

6. Техническая поддержка
   - При возникновении ошибок обратитесь к администратору базы данных.
   - Проверьте правильность настроек подключения к базе данных в случае проблем с загрузкой данных.

Это руководство поможет вам эффективно использовать программу.
"
            };
            tabUserGuide.Controls.Add(rtbUserGuide);

            tabControl.TabPages.AddRange(new[] { tabPurchases, tabSales, tabSuppliers, tabEmployees, tabProductCategories, tabProducts, tabClients, tabUserGuide });
            this.Controls.Add(tabControl);
        }

        private async Task LoadDataAsync()
        {
            try
            {
                dgvPurchases.DataSource = await db.GetPurchasesAsync();
                dgvSales.DataSource = await db.GetSalesAsync();
                dgvSuppliers.DataSource = await db.GetSuppliersAsync();
                dgvEmployees.DataSource = await db.GetEmployeesAsync();
                dgvProductCategories.DataSource = await db.GetProductCategoriesAsync();
                dgvProducts.DataSource = await db.GetProductsAsync();
                dgvClients.DataSource = await db.GetClientsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ShowPurchaseDialogAsync(DataGridViewRow row)
        {
            var form = new Form
            {
                Size = new Size(400, 350),
                Text = row == null ? "Добавить закупку" : "Редактировать закупку",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent
            };

            var lblSupplier = new Label { Text = "Поставщик:", Location = new Point(20, 20) };
            var cmbSupplier = new ComboBox { Location = new Point(20, 40), Width = 350, DropDownStyle = ComboBoxStyle.DropDownList };
            var lblMaterialName = new Label { Text = "Материал:", Location = new Point(20, 70) };
            var txtMaterialName = new TextBox { Location = new Point(20, 90), Width = 350 };
            var lblQuantity = new Label { Text = "Количество:", Location = new Point(20, 120) };
            var txtQuantity = new TextBox { Location = new Point(20, 140), Width = 350 };
            var lblPrice = new Label { Text = "Цена:", Location = new Point(20, 170) };
            var txtPrice = new TextBox { Location = new Point(20, 190), Width = 350 };
            var lblDate = new Label { Text = "Дата:", Location = new Point(20, 220) };
            var dtpDate = new DateTimePicker { Location = new Point(20, 240), Width = 350 };
            var btnSave = new Button { Text = "Сохранить", Location = new Point(150, 280), Width = 100 };

            try
            {
                var suppliers = await db.GetSuppliersAsync();
                cmbSupplier.DataSource = suppliers;
                cmbSupplier.DisplayMember = "name";
                cmbSupplier.ValueMember = "id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки поставщиков: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            btnSave.Click += async (s, e) =>
            {
                try
                {
                    if (cmbSupplier.SelectedValue == null || string.IsNullOrEmpty(txtMaterialName.Text) || !int.TryParse(txtQuantity.Text, out int quantity) || !decimal.TryParse(txtPrice.Text, out decimal price))
                    {
                        MessageBox.Show("Заполните все обязательные поля корректно!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (row == null)
                    {
                        await db.AddPurchaseAsync((int)cmbSupplier.SelectedValue, txtMaterialName.Text, quantity, price, dtpDate.Value);
                    }
                    else
                    {
                        int id = Convert.ToInt32(row.Cells["id"].Value);
                        await db.UpdatePurchaseAsync(id, (int)cmbSupplier.SelectedValue, txtMaterialName.Text, quantity, price, dtpDate.Value);
                    }
                    await LoadDataAsync();
                    form.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            if (row != null)
            {
                cmbSupplier.SelectedValue = (await db.GetSuppliersAsync()).AsEnumerable().FirstOrDefault(r => r["name"].ToString() == row.Cells["supplier_name"].Value.ToString())?["id"];
                txtMaterialName.Text = row.Cells["material_name"].Value.ToString();
                txtQuantity.Text = row.Cells["quantity"].Value.ToString();
                txtPrice.Text = row.Cells["price"].Value.ToString();
                dtpDate.Value = Convert.ToDateTime(row.Cells["purchase_date"].Value);
            }

            form.Controls.AddRange(new Control[] { lblSupplier, cmbSupplier, lblMaterialName, txtMaterialName, lblQuantity, txtQuantity, lblPrice, txtPrice, lblDate, dtpDate, btnSave });
            form.ShowDialog();
        }

        private async Task ShowSaleDialogAsync(DataGridViewRow row)
        {
            var form = new Form
            {
                Size = new Size(400, 400),
                Text = row == null ? "Добавить продажу" : "Редактировать продажу",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent
            };

            var lblEmployee = new Label { Text = "Сотрудник:", Location = new Point(20, 20) };
            var cmbEmployee = new ComboBox { Location = new Point(20, 40), Width = 350, DropDownStyle = ComboBoxStyle.DropDownList };
            var lblProduct = new Label { Text = "Товар:", Location = new Point(20, 70) };
            var cmbProduct = new ComboBox { Location = new Point(20, 90), Width = 350, DropDownStyle = ComboBoxStyle.DropDownList };
            var lblClient = new Label { Text = "Клиент:", Location = new Point(20, 120) };
            var cmbClient = new ComboBox { Location = new Point(20, 140), Width = 350, DropDownStyle = ComboBoxStyle.DropDownList };
            var lblQuantity = new Label { Text = "Количество:", Location = new Point(20, 170) };
            var txtQuantity = new TextBox { Location = new Point(20, 190), Width = 350 };
            var lblDate = new Label { Text = "Дата:", Location = new Point(20, 220) };
            var dtpDate = new DateTimePicker { Location = new Point(20, 240), Width = 350 };
            var btnSave = new Button { Text = "Сохранить", Location = new Point(150, 280), Width = 100 };

            try
            {
                var employees = await db.GetEmployeesAsync();
                var products = await db.GetProductsAsync();
                var clients = await db.GetClientsAsync();

                cmbEmployee.DataSource = employees;
                cmbEmployee.DisplayMember = "FullNameWithPosition";
                cmbEmployee.ValueMember = "id";
                cmbEmployee.SelectedIndex = -1;

                cmbProduct.DataSource = products;
                cmbProduct.DisplayMember = "ProductWithCategory";
                cmbProduct.ValueMember = "id";
                cmbProduct.SelectedIndex = -1;

                var clientsWithNull = new DataTable();
                clientsWithNull.Columns.Add("id", typeof(int));
                clientsWithNull.Columns.Add("FullNameWithPhone", typeof(string));
                clientsWithNull.Rows.Add(DBNull.Value, "Не выбран");
                foreach (DataRow clientRow in clients.Rows)
                {
                    clientsWithNull.Rows.Add(clientRow["id"], clientRow["FullNameWithPhone"]);
                }
                cmbClient.DataSource = clientsWithNull;
                cmbClient.DisplayMember = "FullNameWithPhone";
                cmbClient.ValueMember = "id";
                cmbClient.SelectedIndex = -1; // Клиент необязателен
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            btnSave.Click += async (s, e) =>
            {
                try
                {
                    if (cmbEmployee.SelectedValue == null || cmbProduct.SelectedValue == null || !int.TryParse(txtQuantity.Text, out int quantity))
                    {
                        MessageBox.Show("Заполните все обязательные поля корректно!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int? clientId = cmbClient.SelectedValue != null ? (int?)cmbClient.SelectedValue : null;
                    if (row == null)
                    {
                        await db.AddSaleAsync((int)cmbEmployee.SelectedValue, (int)cmbProduct.SelectedValue, clientId, quantity, dtpDate.Value);
                    }
                    else
                    {
                        int id = Convert.ToInt32(row.Cells["id"].Value);
                        await db.UpdateSaleAsync(id, (int)cmbEmployee.SelectedValue, (int)cmbProduct.SelectedValue, clientId, quantity, dtpDate.Value);
                    }
                    await LoadDataAsync();
                    form.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            if (row != null)
            {
                var employees = await db.GetEmployeesAsync();
                var products = await db.GetProductsAsync();
                var clients = await db.GetClientsAsync();
                cmbEmployee.SelectedValue = employees.AsEnumerable().FirstOrDefault(r => (r["first_name"].ToString() + " " + r["last_name"].ToString()) == row.Cells["employee_name"].Value.ToString())?["id"];
                cmbProduct.SelectedValue = products.AsEnumerable().FirstOrDefault(r => r["name"].ToString() == row.Cells["product_name"].Value.ToString())?["id"];
                if (row.Cells["client_name"].Value != null && row.Cells["client_name"].Value.ToString() != "")
                {
                    cmbClient.SelectedValue = clients.AsEnumerable().FirstOrDefault(r => (r["first_name"].ToString() + " " + r["last_name"].ToString()) == row.Cells["client_name"].Value.ToString())?["id"];
                }
                txtQuantity.Text = row.Cells["quantity"].Value.ToString();
                dtpDate.Value = Convert.ToDateTime(row.Cells["sale_date"].Value);
            }

            form.Controls.AddRange(new Control[] { lblEmployee, cmbEmployee, lblProduct, cmbProduct, lblClient, cmbClient, lblQuantity, txtQuantity, lblDate, dtpDate, btnSave });
            form.ShowDialog();
        }

        private async Task ShowSupplierDialogAsync(DataGridViewRow row)
        {
            var form = new Form
            {
                Size = new Size(400, 400),
                Text = row == null ? "Добавить поставщика" : "Редактировать поставщика",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent
            };

            var lblName = new Label { Text = "Название:", Location = new Point(20, 20) };
            var txtName = new TextBox { Location = new Point(20, 40), Width = 350 };
            var lblContactPerson = new Label { Text = "Контакт:", Location = new Point(20, 70) };
            var txtContactPerson = new TextBox { Location = new Point(20, 90), Width = 350 };
            var lblPhone = new Label { Text = "Телефон:", Location = new Point(20, 120) };
            var txtPhone = new MaskedTextBox { Location = new Point(20, 140), Width = 350, Mask = "+7 (000) 000 00 00" };
            var lblEmail = new Label { Text = "Email:", Location = new Point(20, 170) };
            var txtEmail = new TextBox { Location = new Point(20, 190), Width = 350 };
            var lblAddress = new Label { Text = "Адрес:", Location = new Point(20, 220) };
            var txtAddress = new TextBox { Location = new Point(20, 240), Width = 350 };
            var lblInn = new Label { Text = "ИНН:", Location = new Point(20, 270) };
            var txtInn = new TextBox { Location = new Point(20, 290), Width = 350 };
            var btnSave = new Button { Text = "Сохранить", Location = new Point(150, 330), Width = 100 };

            btnSave.Click += async (s, e) =>
            {
                try
                {
                    if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtContactPerson.Text) || !txtPhone.MaskCompleted ||
                        string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtAddress.Text) || string.IsNullOrEmpty(txtInn.Text) ||
                        txtInn.Text.Length < 10 || txtInn.Text.Length > 12)
                    {
                        MessageBox.Show("Заполните все обязательные поля корректно! Телефон должен быть в формате +7(XXX)XXX-XX-XX, ИНН — от 10 до 12 символов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                    if (!Regex.IsMatch(txtEmail.Text, emailPattern))
                    {
                        MessageBox.Show("Некорректный формат email!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (row == null)
                    {
                        await db.AddSupplierAsync(txtName.Text, txtContactPerson.Text, txtPhone.Text, txtEmail.Text, txtAddress.Text, txtInn.Text);
                    }
                    else
                    {
                        int id = Convert.ToInt32(row.Cells["id"].Value);
                        await db.UpdateSupplierAsync(id, txtName.Text, txtContactPerson.Text, txtPhone.Text, txtEmail.Text, txtAddress.Text, txtInn.Text);
                    }
                    await LoadDataAsync();
                    form.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            if (row != null)
            {
                txtName.Text = row.Cells["name"].Value.ToString();
                txtContactPerson.Text = row.Cells["contact_person"].Value.ToString();
                txtPhone.Text = row.Cells["phone"].Value.ToString();
                txtEmail.Text = row.Cells["email"].Value.ToString();
                txtAddress.Text = row.Cells["address"].Value.ToString();
                txtInn.Text = row.Cells["inn"].Value.ToString();
            }

            form.Controls.AddRange(new Control[] { lblName, txtName, lblContactPerson, txtContactPerson, lblPhone, txtPhone, lblEmail, txtEmail, lblAddress, txtAddress, lblInn, txtInn, btnSave });
            form.ShowDialog();
        }

        private async Task ShowEmployeeDialogAsync(DataGridViewRow row)
        {
            var form = new Form
            {
                Size = new Size(400, 500),
                Text = row == null ? "Добавить сотрудника" : "Редактировать сотрудника",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent
            };

            var lblUser = new Label { Text = "Пользователь:", Location = new Point(20, 20) };
            var cmbUser = new ComboBox { Location = new Point(20, 40), Width = 350, DropDownStyle = ComboBoxStyle.DropDownList };
            var lblFirstName = new Label { Text = "Имя:", Location = new Point(20, 70) };
            var txtFirstName = new TextBox { Location = new Point(20, 90), Width = 350 };
            var lblLastName = new Label { Text = "Фамилия:", Location = new Point(20, 120) };
            var txtLastName = new TextBox { Location = new Point(20, 140), Width = 350 };
            var lblPatronymic = new Label { Text = "Отчество:", Location = new Point(20, 170) };
            var txtPatronymic = new TextBox { Location = new Point(20, 190), Width = 350 };
            var lblPosition = new Label { Text = "Должность:", Location = new Point(20, 220) };
            var txtPosition = new TextBox { Location = new Point(20, 240), Width = 350 };
            var lblPhone = new Label { Text = "Телефон:", Location = new Point(20, 270) };
            var txtPhone = new MaskedTextBox { Location = new Point(20, 290), Width = 350, Mask = "+7 (000) 000 00 00" };
            var lblEmail = new Label { Text = "Email:", Location = new Point(20, 320) };
            var txtEmail = new TextBox { Location = new Point(20, 340), Width = 350 };
            var lblExperience = new Label { Text = "Стаж:", Location = new Point(20, 370) };
            var txtExperience = new TextBox { Location = new Point(20, 390), Width = 350 };
            var btnSave = new Button { Text = "Сохранить", Location = new Point(150, 430), Width = 100 };

            try
            {
                var users = await db.GetUsersAsync();
                cmbUser.DataSource = users;
                cmbUser.DisplayMember = "login";
                cmbUser.ValueMember = "id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки пользователей: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            btnSave.Click += async (s, e) =>
            {
                try
                {
                    if (cmbUser.SelectedValue == null || string.IsNullOrEmpty(txtFirstName.Text) || string.IsNullOrEmpty(txtLastName.Text) ||
                        string.IsNullOrEmpty(txtPosition.Text) || !txtPhone.MaskCompleted || string.IsNullOrEmpty(txtEmail.Text))
                    {
                        MessageBox.Show("Заполните все обязательные поля корректно! Телефон должен быть в формате +7(XXX)XXX-XX-XX!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                    if (!Regex.IsMatch(txtEmail.Text, emailPattern))
                    {
                        MessageBox.Show("Некорректный формат email!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int? experience = null;
                    if (int.TryParse(txtExperience.Text, out int exp))
                    {
                        experience = exp;
                    }

                    if (row == null)
                    {
                        await db.AddEmployeeAsync((int)cmbUser.SelectedValue, txtFirstName.Text, txtLastName.Text, txtPatronymic.Text, txtPosition.Text, txtPhone.Text, txtEmail.Text, experience);
                    }
                    else
                    {
                        int id = Convert.ToInt32(row.Cells["id"].Value);
                        await db.UpdateEmployeeAsync(id, (int)cmbUser.SelectedValue, txtFirstName.Text, txtLastName.Text, txtPatronymic.Text, txtPosition.Text, txtPhone.Text, txtEmail.Text, experience);
                    }
                    await LoadDataAsync();
                    form.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            if (row != null)
            {
                var users = await db.GetUsersAsync();
                cmbUser.SelectedValue = users.AsEnumerable().FirstOrDefault(r => r["login"].ToString() == row.Cells["user_login"].Value.ToString())?["id"];
                txtFirstName.Text = row.Cells["first_name"].Value.ToString();
                txtLastName.Text = row.Cells["last_name"].Value.ToString();
                txtPatronymic.Text = row.Cells["patronymic"].Value?.ToString() ?? "";
                txtPosition.Text = row.Cells["position"].Value.ToString();
                txtPhone.Text = row.Cells["phone"].Value.ToString();
                txtEmail.Text = row.Cells["email"].Value.ToString();
                txtExperience.Text = row.Cells["experience"].Value?.ToString() ?? "";
            }

            form.Controls.AddRange(new Control[] { lblUser, cmbUser, lblFirstName, txtFirstName, lblLastName, txtLastName, lblPatronymic, txtPatronymic, lblPosition, txtPosition, lblPhone, txtPhone, lblEmail, txtEmail, lblExperience, txtExperience, btnSave });
            form.ShowDialog();
        }

        private async Task ShowCategoryDialogAsync(DataGridViewRow row)
        {
            var form = new Form
            {
                Size = new Size(400, 200),
                Text = row == null ? "Добавить категорию" : "Редактировать категорию",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent
            };

            var lblCategoryName = new Label { Text = "Название категории:", Location = new Point(20, 20) };
            var txtCategoryName = new TextBox { Location = new Point(20, 40), Width = 350 };
            var btnSave = new Button { Text = "Сохранить", Location = new Point(150, 80), Width = 100 };

            btnSave.Click += async (s, e) =>
            {
                try
                {
                    if (string.IsNullOrEmpty(txtCategoryName.Text))
                    {
                        MessageBox.Show("Название категории обязательно!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (row == null)
                    {
                        await db.AddProductCategoryAsync(txtCategoryName.Text);
                    }
                    else
                    {
                        int id = Convert.ToInt32(row.Cells["id"].Value);
                        await db.UpdateProductCategoryAsync(id, txtCategoryName.Text);
                    }
                    await LoadDataAsync();
                    form.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            if (row != null)
            {
                txtCategoryName.Text = row.Cells["category_name"].Value.ToString();
            }

            form.Controls.AddRange(new Control[] { lblCategoryName, txtCategoryName, btnSave });
            form.ShowDialog();
        }

        private async Task ShowProductDialogAsync(DataGridViewRow row)
        {
            var form = new Form
            {
                Size = new Size(400, 350),
                Text = row == null ? "Добавить товар" : "Редактировать товар",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent
            };

            var lblCategory = new Label { Text = "Категория:", Location = new Point(20, 20) };
            var cmbCategory = new ComboBox { Location = new Point(20, 40), Width = 350, DropDownStyle = ComboBoxStyle.DropDownList };
            var lblName = new Label { Text = "Название:", Location = new Point(20, 70) };
            var txtName = new TextBox { Location = new Point(20, 90), Width = 350 };
            var lblDescription = new Label { Text = "Описание:", Location = new Point(20, 120) };
            var txtDescription = new TextBox { Location = new Point(20, 140), Width = 350 };
            var lblPrice = new Label { Text = "Цена:", Location = new Point(20, 170) };
            var txtPrice = new TextBox { Location = new Point(20, 190), Width = 350 };
            var lblStockQuantity = new Label { Text = "Количество на складе:", Location = new Point(20, 220) };
            var txtStockQuantity = new TextBox { Location = new Point(20, 240), Width = 350 };
            var btnSave = new Button { Text = "Сохранить", Location = new Point(150, 280), Width = 100 };

            try
            {
                var categories = await db.GetProductCategoriesAsync();
                cmbCategory.DataSource = categories;
                cmbCategory.DisplayMember = "category_name";
                cmbCategory.ValueMember = "id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            btnSave.Click += async (s, e) =>
            {
                try
                {
                    if (cmbCategory.SelectedValue == null || string.IsNullOrEmpty(txtName.Text) || !decimal.TryParse(txtPrice.Text, out decimal price) ||
                        !int.TryParse(txtStockQuantity.Text, out int stockQuantity))
                    {
                        MessageBox.Show("Заполните все обязательные поля корректно!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (row == null)
                    {
                        await db.AddProductAsync((int)cmbCategory.SelectedValue, txtName.Text, txtDescription.Text, price, stockQuantity);
                    }
                    else
                    {
                        int id = Convert.ToInt32(row.Cells["id"].Value);
                        await db.UpdateProductAsync(id, (int)cmbCategory.SelectedValue, txtName.Text, txtDescription.Text, price, stockQuantity);
                    }
                    await LoadDataAsync();
                    form.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            if (row != null)
            {
                var categories = await db.GetProductCategoriesAsync();
                cmbCategory.SelectedValue = categories.AsEnumerable().FirstOrDefault(r => r["category_name"].ToString() == row.Cells["category_name"].Value.ToString())?["id"];
                txtName.Text = row.Cells["name"].Value.ToString();
                txtDescription.Text = row.Cells["description"].Value?.ToString() ?? "";
                txtPrice.Text = row.Cells["price"].Value.ToString();
                txtStockQuantity.Text = row.Cells["stock_quantity"].Value.ToString();
            }

            form.Controls.AddRange(new Control[] { lblCategory, cmbCategory, lblName, txtName, lblDescription, txtDescription, lblPrice, txtPrice, lblStockQuantity, txtStockQuantity, btnSave });
            form.ShowDialog();
        }

        private async Task ShowClientDialogAsync(DataGridViewRow row)
        {
            var form = new Form
            {
                Size = new Size(400, 400),
                Text = row == null ? "Добавить клиента" : "Редактировать клиента",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent
            };

            var lblFirstName = new Label { Text = "Имя:", Location = new Point(20, 20) };
            var txtFirstName = new TextBox { Location = new Point(20, 40), Width = 350 };
            var lblLastName = new Label { Text = "Фамилия:", Location = new Point(20, 70) };
            var txtLastName = new TextBox { Location = new Point(20, 90), Width = 350 };
            var lblPatronymic = new Label { Text = "Отчество:", Location = new Point(20, 120) };
            var txtPatronymic = new TextBox { Location = new Point(20, 140), Width = 350 };
            var lblPhone = new Label { Text = "Телефон:", Location = new Point(20, 170) };
            var txtPhone = new MaskedTextBox { Location = new Point(20, 190), Width = 350, Mask = "+7 (000) 000 00 00" };
            var lblEmail = new Label { Text = "Email:", Location = new Point(20, 220) };
            var txtEmail = new TextBox { Location = new Point(20, 240), Width = 350 };
            var lblAddress = new Label { Text = "Адрес:", Location = new Point(20, 270) };
            var txtAddress = new TextBox { Location = new Point(20, 290), Width = 350 };
            var btnSave = new Button { Text = "Сохранить", Location = new Point(150, 330), Width = 100 };

            btnSave.Click += async (s, e) =>
            {
                try
                {
                    if (string.IsNullOrEmpty(txtFirstName.Text) || string.IsNullOrEmpty(txtLastName.Text) || !txtPhone.MaskCompleted)
                    {
                        MessageBox.Show("Заполните все обязательные поля корректно! Телефон должен быть в формате +7(XXX)XXX-XX-XX!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!string.IsNullOrEmpty(txtEmail.Text))
                    {
                        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                        if (!Regex.IsMatch(txtEmail.Text, emailPattern))
                        {
                            MessageBox.Show("Некорректный формат email!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    if (row == null)
                    {
                        await db.AddClientAsync(txtFirstName.Text, txtLastName.Text, txtPatronymic.Text, txtPhone.Text, txtEmail.Text, txtAddress.Text);
                    }
                    else
                    {
                        int id = Convert.ToInt32(row.Cells["id"].Value);
                        await db.UpdateClientAsync(id, txtFirstName.Text, txtLastName.Text, txtPatronymic.Text, txtPhone.Text, txtEmail.Text, txtAddress.Text);
                    }
                    await LoadDataAsync();
                    form.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            if (row != null)
            {
                txtFirstName.Text = row.Cells["first_name"].Value.ToString();
                txtLastName.Text = row.Cells["last_name"].Value.ToString();
                txtPatronymic.Text = row.Cells["patronymic"].Value?.ToString() ?? "";
                txtPhone.Text = row.Cells["phone"].Value.ToString();
                txtEmail.Text = row.Cells["email"].Value?.ToString() ?? "";
                txtAddress.Text = row.Cells["address"].Value?.ToString() ?? "";
            }

            form.Controls.AddRange(new Control[] { lblFirstName, txtFirstName, lblLastName, txtLastName, lblPatronymic, txtPatronymic, lblPhone, txtPhone, lblEmail, txtEmail, lblAddress, txtAddress, btnSave });
            form.ShowDialog();
        }

        private async Task EditPurchaseAsync()
        {
            if (dgvPurchases.SelectedRows.Count > 0)
            {
                await ShowPurchaseDialogAsync(dgvPurchases.SelectedRows[0]);
            }
            else
            {
                MessageBox.Show("Выберите запись в таблице для редактирования!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async Task EditSaleAsync()
        {
            if (dgvSales.SelectedRows.Count > 0)
            {
                await ShowSaleDialogAsync(dgvSales.SelectedRows[0]);
            }
            else
            {
                MessageBox.Show("Выберите запись в таблице для редактирования!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async Task EditSupplierAsync()
        {
            if (dgvSuppliers.SelectedRows.Count > 0)
            {
                await ShowSupplierDialogAsync(dgvSuppliers.SelectedRows[0]);
            }
            else
            {
                MessageBox.Show("Выберите запись в таблице для редактирования!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async Task EditEmployeeAsync()
        {
            if (dgvEmployees.SelectedRows.Count > 0)
            {
                await ShowEmployeeDialogAsync(dgvEmployees.SelectedRows[0]);
            }
            else
            {
                MessageBox.Show("Выберите запись в таблице для редактирования!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async Task EditCategoryAsync()
        {
            if (dgvProductCategories.SelectedRows.Count > 0)
            {
                await ShowCategoryDialogAsync(dgvProductCategories.SelectedRows[0]);
            }
            else
            {
                MessageBox.Show("Выберите запись в таблице для редактирования!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async Task EditProductAsync()
        {
            if (dgvProducts.SelectedRows.Count > 0)
            {
                await ShowProductDialogAsync(dgvProducts.SelectedRows[0]);
            }
            else
            {
                MessageBox.Show("Выберите запись в таблице для редактирования!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async Task EditClientAsync()
        {
            if (dgvClients.SelectedRows.Count > 0)
            {
                await ShowClientDialogAsync(dgvClients.SelectedRows[0]);
            }
            else
            {
                MessageBox.Show("Выберите запись в таблице для редактирования!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async Task DeletePurchaseAsync()
        {
            if (dgvPurchases.SelectedRows.Count > 0)
            {
                try
                {
                    int id = Convert.ToInt32(dgvPurchases.SelectedRows[0].Cells["id"].Value);
                    if (MessageBox.Show("Вы уверены, что хотите удалить эту закупку?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        await db.DeletePurchaseAsync(id);
                        await LoadDataAsync();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите запись в таблице для удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async Task DeleteSaleAsync()
        {
            if (dgvSales.SelectedRows.Count > 0)
            {
                try
                {
                    int id = Convert.ToInt32(dgvSales.SelectedRows[0].Cells["id"].Value);
                    if (MessageBox.Show("Вы уверены, что хотите удалить эту продажу?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        await db.DeleteSaleAsync(id);
                        await LoadDataAsync();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите запись в таблице для удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async Task DeleteSupplierAsync()
        {
            if (dgvSuppliers.SelectedRows.Count > 0)
            {
                try
                {
                    int id = Convert.ToInt32(dgvSuppliers.SelectedRows[0].Cells["id"].Value);
                    if (MessageBox.Show("Вы уверены, что хотите удалить этого поставщика?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        await db.DeleteSupplierAsync(id);
                        await LoadDataAsync();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите запись в таблице для удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async Task DeleteEmployeeAsync()
        {
            if (dgvEmployees.SelectedRows.Count > 0)
            {
                try
                {
                    int id = Convert.ToInt32(dgvEmployees.SelectedRows[0].Cells["id"].Value);
                    if (MessageBox.Show("Вы уверены, что хотите удалить сотрудника?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        await db.DeleteEmployeeAsync(id);
                        await LoadDataAsync();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите запись в таблице для удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async Task DeleteCategoryAsync()
        {
            if (dgvProductCategories.SelectedRows.Count > 0)
            {
                try
                {
                    int id = Convert.ToInt32(dgvProductCategories.SelectedRows[0].Cells["id"].Value);
                    if (MessageBox.Show("Вы уверены, что хотите удалить категорию? Это может повлиять на связанные товары!", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        await db.DeleteProductCategoryAsync(id);
                        await LoadDataAsync();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите запись в таблице для удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async Task DeleteProductAsync()
        {
            if (dgvProducts.SelectedRows.Count > 0)
            {
                try
                {
                    int id = Convert.ToInt32(dgvProducts.SelectedRows[0].Cells["id"].Value);
                    if (MessageBox.Show("Вы уверены, что хотите удалить товар?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        await db.DeleteProductAsync(id);
                        await LoadDataAsync();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите запись в таблице для удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async Task DeleteClientAsync()
        {
            if (dgvClients.SelectedRows.Count > 0)
            {
                try
                {
                    int id = Convert.ToInt32(dgvClients.SelectedRows[0].Cells["id"].Value);
                    if (MessageBox.Show("Вы уверены, что хотите удалить клиента?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        await db.DeleteClientAsync(id);
                        await LoadDataAsync();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите запись в таблице для удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
