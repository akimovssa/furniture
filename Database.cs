using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace furniture
{
    public class Database
    {
        private readonly string connectionString;

        public Database(string connString)
        {
            connectionString = connString;
        }

        public async Task<bool> AuthenticateUserAsync(string login, string password)
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM users WHERE login = @login AND password = @password", conn);
                cmd.Parameters.AddWithValue("login", login);
                cmd.Parameters.AddWithValue("password", password);
                var result = await cmd.ExecuteScalarAsync();
                return Convert.ToInt32(result) > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка авторизации: {ex.Message}");
                return false;
            }
        }

        public async Task<DataTable> GetUsersAsync()
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("SELECT * FROM users", conn);
                var adapter = new NpgsqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка получения данных пользователей: {ex.Message}");
                return new DataTable();
            }
        }

        public async Task<DataTable> GetPurchasesAsync()
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("SELECT p.id, s.name AS supplier_name, p.material_name, p.quantity, p.price, p.amount, p.purchase_date FROM purchases p JOIN suppliers s ON p.supplier_id = s.id", conn);
                var adapter = new NpgsqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка получения данных закупок: {ex.Message}");
                return new DataTable();
            }
        }

        public async Task<DataTable> GetSalesAsync()
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand(
                    "SELECT s.id, e.first_name || ' ' || e.last_name AS employee_name, p.name AS product_name, c.first_name || ' ' || c.last_name AS client_name, s.quantity, s.amount, s.sale_date " +
                    "FROM sales s JOIN employees e ON s.employee_id = e.id JOIN products p ON s.product_id = p.id LEFT JOIN clients c ON s.client_id = c.id",
                    conn);
                var adapter = new NpgsqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка получения данных продаж: {ex.Message}");
                return new DataTable();
            }
        }

        public async Task<DataTable> GetSuppliersAsync()
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("SELECT * FROM suppliers", conn);
                var adapter = new NpgsqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка получения данных поставщиков: {ex.Message}");
                return new DataTable();
            }
        }

        public async Task<DataTable> GetEmployeesAsync()
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand(
                    "SELECT e.id, e.first_name, e.last_name, e.patronymic, e.position, e.phone, e.email, e.experience, u.login AS user_login " +
                    "FROM employees e LEFT JOIN users u ON e.user_id = u.id",
                    conn);
                var adapter = new NpgsqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);

                // Добавляем вычисляемый столбец FullNameWithPosition
                dt.Columns.Add("FullNameWithPosition", typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    string patronymic = row.IsNull("patronymic") ? "" : row["patronymic"].ToString();
                    string fullName = $"{row["first_name"]} {row["last_name"]} {patronymic} ({row["position"]})".Trim();
                    row["FullNameWithPosition"] = fullName;
                }

                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка получения данных сотрудников: {ex.Message}");
                return new DataTable();
            }
        }

        public async Task<DataTable> GetProductCategoriesAsync()
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("SELECT * FROM product_category", conn);
                var adapter = new NpgsqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка получения данных категорий товаров: {ex.Message}");
                return new DataTable();
            }
        }

        public async Task<DataTable> GetProductsAsync()
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand(
                    "SELECT p.id, p.name, pc.category_name, p.description, p.price, p.stock_quantity " +
                    "FROM products p JOIN product_category pc ON p.category_id = pc.id",
                    conn);
                var adapter = new NpgsqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);

                // Добавляем вычисляемый столбец ProductWithCategory
                dt.Columns.Add("ProductWithCategory", typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    string productInfo = $"{row["name"]} ({row["category_name"]})";
                    row["ProductWithCategory"] = productInfo;
                }

                return dt;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка получения данных товаров: {ex.Message}");
                return new DataTable();
            }
        }

        public async Task<DataTable> GetClientsAsync()
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("SELECT * FROM clients", conn);
                var adapter = new NpgsqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);

                // Добавляем вычисляемый столбец FullName
                dt.Columns.Add("FullNameWithPhone", typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    string patronymic = row.IsNull("patronymic") ? "" : row["patronymic"].ToString();
                    string fullNameWithPhone = $"{row["first_name"]} {row["last_name"]} {patronymic} ({row["phone"]})".Trim();
                    row["FullNameWithPhone"] = fullNameWithPhone;
                }

                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка получения данных клиентов: {ex.Message}");
                return new DataTable();
            }
        }

        public async Task AddPurchaseAsync(int supplierId, string materialName, int quantity, decimal price, DateTime purchaseDate)
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand(
                    "INSERT INTO purchases (supplier_id, material_name, quantity, price, amount, purchase_date) " +
                    "VALUES (@supplierId, @materialName, @quantity, @price, @amount, @purchaseDate)",
                    conn);
                cmd.Parameters.AddWithValue("supplierId", supplierId);
                cmd.Parameters.AddWithValue("materialName", materialName);
                cmd.Parameters.AddWithValue("quantity", quantity);
                cmd.Parameters.AddWithValue("price", price);
                cmd.Parameters.AddWithValue("amount", quantity * price);
                cmd.Parameters.AddWithValue("purchaseDate", purchaseDate);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка добавления закупки: {ex.Message}");
            }
        }

        public async Task UpdatePurchaseAsync(int id, int supplierId, string materialName, int quantity, decimal price, DateTime purchaseDate)
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand(
                    "UPDATE purchases SET supplier_id = @supplierId, material_name = @materialName, quantity = @quantity, " +
                    "price = @price, amount = @amount, purchase_date = @purchaseDate WHERE id = @id",
                    conn);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("supplierId", supplierId);
                cmd.Parameters.AddWithValue("materialName", materialName);
                cmd.Parameters.AddWithValue("quantity", quantity);
                cmd.Parameters.AddWithValue("price", price);
                cmd.Parameters.AddWithValue("amount", quantity * price);
                cmd.Parameters.AddWithValue("purchaseDate", purchaseDate);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка обновления закупки: {ex.Message}");
            }
        }

        public async Task DeletePurchaseAsync(int id)
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("DELETE FROM purchases WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("id", id);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка удаления закупки: {ex.Message}");
            }
        }

        public async Task<int> GetStockQuantityAsync(int productId)
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("SELECT stock_quantity FROM products WHERE id = @productId", conn);
                cmd.Parameters.AddWithValue("productId", productId);
                var result = await cmd.ExecuteScalarAsync();
                return result != null ? Convert.ToInt32(result) : 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения количества товара: {ex.Message}");
            }
        }

        public async Task UpdateStockQuantityAsync(int productId, int newQuantity)
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand(
                    "UPDATE products SET stock_quantity = @newQuantity WHERE id = @productId",
                    conn);
                cmd.Parameters.AddWithValue("productId", productId);
                cmd.Parameters.AddWithValue("newQuantity", newQuantity);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка обновления количества товара: {ex.Message}");
            }
        }

        public async Task AddSaleAsync(int employeeId, int productId, int? clientId, int quantity, DateTime saleDate)
        {
            var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();
            var transaction = conn.BeginTransaction();
            try
            {
                // Проверяем наличие товара на складе
                int stockQuantity = await GetStockQuantityAsync(productId);
                if (stockQuantity < quantity)
                {
                    throw new Exception("Недостаточно товара на складе!");
                }

                // Вычисляем сумму продажи
                var priceCmd = new NpgsqlCommand("SELECT price FROM products WHERE id = @productId", conn, transaction);
                priceCmd.Parameters.AddWithValue("productId", productId);
                var price = Convert.ToDecimal(await priceCmd.ExecuteScalarAsync());
                var amount = price * quantity;

                // Добавляем продажу
                var cmd = new NpgsqlCommand(
                    "INSERT INTO sales (employee_id, product_id, client_id, quantity, amount, sale_date) " +
                    "VALUES (@employeeId, @productId, @clientId, @quantity, @amount, @saleDate)",
                    conn, transaction);
                cmd.Parameters.AddWithValue("employeeId", employeeId);
                cmd.Parameters.AddWithValue("productId", productId);
                cmd.Parameters.AddWithValue("clientId", clientId.HasValue ? (object)clientId.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("quantity", quantity);
                cmd.Parameters.AddWithValue("amount", amount);
                cmd.Parameters.AddWithValue("saleDate", saleDate);
                await cmd.ExecuteNonQueryAsync();

                // Обновляем количество на складе
                await UpdateStockQuantityAsync(productId, stockQuantity - quantity);

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Ошибка добавления продажи: {ex.Message}");
            }
        }

        public async Task UpdateSaleAsync(int id, int employeeId, int productId, int? clientId, int quantity, DateTime saleDate)
        {
            var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();
            var transaction = conn.BeginTransaction();
            try
            {
                // Получаем текущее количество в продаже
                var oldSaleCmd = new NpgsqlCommand("SELECT product_id, quantity FROM sales WHERE id = @id", conn, transaction);
                oldSaleCmd.Parameters.AddWithValue("id", id);
                var reader = await oldSaleCmd.ExecuteReaderAsync();
                int oldProductId = 0;
                int oldQuantity = 0;
                if (await reader.ReadAsync())
                {
                    oldProductId = reader.GetInt32(0);
                    oldQuantity = reader.GetInt32(1);
                }
                reader.Close();

                // Проверяем наличие товара на складе
                int stockQuantity = await GetStockQuantityAsync(productId);
                int quantityDifference = quantity - oldQuantity;
                if (productId == oldProductId)
                {
                    if (stockQuantity < quantityDifference)
                    {
                        throw new Exception("Недостаточно товара на складе!");
                    }
                }
                else
                {
                    if (stockQuantity < quantity)
                    {
                        throw new Exception("Недостаточно товара на складе!");
                    }
                    // Возвращаем количество старого товара
                    await UpdateStockQuantityAsync(oldProductId, await GetStockQuantityAsync(oldProductId) + oldQuantity);
                }

                // Вычисляем сумму продажи
                var priceCmd = new NpgsqlCommand("SELECT price FROM products WHERE id = @productId", conn, transaction);
                priceCmd.Parameters.AddWithValue("productId", productId);
                var price = Convert.ToDecimal(await priceCmd.ExecuteScalarAsync());
                var amount = price * quantity;

                // Обновляем продажу
                var cmd = new NpgsqlCommand(
                    "UPDATE sales SET employee_id = @employeeId, product_id = @productId, client_id = @clientId, " +
                    "quantity = @quantity, amount = @amount, sale_date = @saleDate WHERE id = @id",
                    conn, transaction);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("employeeId", employeeId);
                cmd.Parameters.AddWithValue("productId", productId);
                cmd.Parameters.AddWithValue("clientId", clientId.HasValue ? (object)clientId.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("quantity", quantity);
                cmd.Parameters.AddWithValue("amount", amount);
                cmd.Parameters.AddWithValue("saleDate", saleDate);
                await cmd.ExecuteNonQueryAsync();

                // Обновляем количество на складе
                if (productId == oldProductId)
                {
                    await UpdateStockQuantityAsync(productId, stockQuantity - quantityDifference);
                }
                else
                {
                    await UpdateStockQuantityAsync(productId, stockQuantity - quantity);
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Ошибка обновления продажи: {ex.Message}");
            }
        }

        public async Task DeleteSaleAsync(int id)
        {
            var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();
            var transaction = conn.BeginTransaction();
            try
            {
                // Получаем данные о продаже
                var saleCmd = new NpgsqlCommand("SELECT product_id, quantity FROM sales WHERE id = @id", conn, transaction);
                saleCmd.Parameters.AddWithValue("id", id);
                var reader = await saleCmd.ExecuteReaderAsync();
                int productId = 0;
                int quantity = 0;
                if (await reader.ReadAsync())
                {
                    productId = reader.GetInt32(0);
                    quantity = reader.GetInt32(1);
                }
                reader.Close();

                // Удаляем продажу
                var cmd = new NpgsqlCommand("DELETE FROM sales WHERE id = @id", conn, transaction);
                cmd.Parameters.AddWithValue("id", id);
                await cmd.ExecuteNonQueryAsync();

                // Восстанавливаем количество на складе
                int stockQuantity = await GetStockQuantityAsync(productId);
                await UpdateStockQuantityAsync(productId, stockQuantity + quantity);

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Ошибка удаления продажи: {ex.Message}");
            }
        }

        public async Task AddSupplierAsync(string name, string contactPerson, string phone, string email, string address, string inn)
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand(
                    "INSERT INTO suppliers (name, contact_person, phone, email, address, inn) " +
                    "VALUES (@name, @contactPerson, @phone, @email, @address, @inn)",
                    conn);
                cmd.Parameters.AddWithValue("name", name);
                cmd.Parameters.AddWithValue("contactPerson", contactPerson);
                cmd.Parameters.AddWithValue("phone", phone);
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("address", address);
                cmd.Parameters.AddWithValue("inn", inn);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка добавления поставщика: {ex.Message}");
            }
        }

        public async Task UpdateSupplierAsync(int id, string name, string contactPerson, string phone, string email, string address, string inn)
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand(
                    "UPDATE suppliers SET name = @name, contact_person = @contactPerson, phone = @phone, " +
                    "email = @email, address = @address, inn = @inn WHERE id = @id",
                    conn);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("name", name);
                cmd.Parameters.AddWithValue("contactPerson", contactPerson);
                cmd.Parameters.AddWithValue("phone", phone);
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("address", address);
                cmd.Parameters.AddWithValue("inn", inn);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка обновления поставщика: {ex.Message}");
            }
        }

        public async Task DeleteSupplierAsync(int id)
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("DELETE FROM suppliers WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("id", id);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка удаления поставщика: {ex.Message}");
            }
        }

        public async Task AddEmployeeAsync(int userId, string firstName, string lastName, string patronymic, string position, string phone, string email, int? experience)
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand(
                    "INSERT INTO employees (user_id, first_name, last_name, patronymic, position, phone, email, experience) " +
                    "VALUES (@userId, @firstName, @lastName, @patronymic, @position, @phone, @email, @experience)",
                    conn);
                cmd.Parameters.AddWithValue("userId", userId);
                cmd.Parameters.AddWithValue("firstName", firstName);
                cmd.Parameters.AddWithValue("lastName", lastName);
                cmd.Parameters.AddWithValue("patronymic", patronymic ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("position", position);
                cmd.Parameters.AddWithValue("phone", phone);
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("experience", experience.HasValue ? (object)experience.Value : DBNull.Value);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка добавления сотрудника: {ex.Message}");
            }
        }

        public async Task UpdateEmployeeAsync(int id, int userId, string firstName, string lastName, string patronymic, string position, string phone, string email, int? experience)
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand(
                    "UPDATE employees SET user_id = @userId, first_name = @firstName, last_name = @lastName, " +
                    "patronymic = @patronymic, position = @position, phone = @phone, email = @email, experience = @experience WHERE id = @id",
                    conn);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("userId", userId);
                cmd.Parameters.AddWithValue("firstName", firstName);
                cmd.Parameters.AddWithValue("lastName", lastName);
                cmd.Parameters.AddWithValue("patronymic", patronymic ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("position", position);
                cmd.Parameters.AddWithValue("phone", phone);
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("experience", experience.HasValue ? (object)experience.Value : DBNull.Value);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка обновления сотрудника: {ex.Message}");
            }
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("DELETE FROM employees WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("id", id);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка удаления сотрудника: {ex.Message}");
            }
        }

        public async Task AddProductCategoryAsync(string categoryName)
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand(
                    "INSERT INTO product_category (category_name) VALUES (@categoryName)",
                    conn);
                cmd.Parameters.AddWithValue("categoryName", categoryName);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка добавления категории: {ex.Message}");
            }
        }

        public async Task UpdateProductCategoryAsync(int id, string categoryName)
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand(
                    "UPDATE product_category SET category_name = @categoryName WHERE id = @id",
                    conn);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("categoryName", categoryName);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка обновления категории: {ex.Message}");
            }
        }

        public async Task DeleteProductCategoryAsync(int id)
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("DELETE FROM product_category WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("id", id);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка удаления категории: {ex.Message}");
            }
        }

        public async Task AddProductAsync(int categoryId, string name, string description, decimal price, int stockQuantity)
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand(
                    "INSERT INTO products (category_id, name, description, price, stock_quantity) " +
                    "VALUES (@categoryId, @name, @description, @price, @stockQuantity)",
                    conn);
                cmd.Parameters.AddWithValue("categoryId", categoryId);
                cmd.Parameters.AddWithValue("name", name);
                cmd.Parameters.AddWithValue("description", description ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("price", price);
                cmd.Parameters.AddWithValue("stockQuantity", stockQuantity);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка добавления товара: {ex.Message}");
            }
        }

        public async Task UpdateProductAsync(int id, int categoryId, string name, string description, decimal price, int stockQuantity)
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand(
                    "UPDATE products SET category_id = @categoryId, name = @name, description = @description, " +
                    "price = @price, stock_quantity = @stockQuantity WHERE id = @id",
                    conn);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("categoryId", categoryId);
                cmd.Parameters.AddWithValue("name", name);
                cmd.Parameters.AddWithValue("description", description ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("price", price);
                cmd.Parameters.AddWithValue("stockQuantity", stockQuantity);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка обновления товара: {ex.Message}");
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("DELETE FROM products WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("id", id);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка удаления товара: {ex.Message}");
            }
        }

        public async Task AddClientAsync(string firstName, string lastName, string patronymic, string phone, string email, string address)
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand(
                    "INSERT INTO clients (first_name, last_name, patronymic, phone, email, address) " +
                    "VALUES (@firstName, @lastName, @patronymic, @phone, @email, @address)",
                    conn);
                cmd.Parameters.AddWithValue("firstName", firstName);
                cmd.Parameters.AddWithValue("lastName", lastName);
                cmd.Parameters.AddWithValue("patronymic", patronymic ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("phone", phone);
                cmd.Parameters.AddWithValue("email", email ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("address", address ?? (object)DBNull.Value);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка добавления клиента: {ex.Message}");
            }
        }

        public async Task UpdateClientAsync(int id, string firstName, string lastName, string patronymic, string phone, string email, string address)
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand(
                    "UPDATE clients SET first_name = @firstName, last_name = @lastName, patronymic = @patronymic, " +
                    "phone = @phone, email = @email, address = @address WHERE id = @id",
                    conn);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("firstName", firstName);
                cmd.Parameters.AddWithValue("lastName", lastName);
                cmd.Parameters.AddWithValue("patronymic", patronymic ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("phone", phone);
                cmd.Parameters.AddWithValue("email", email ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("address", address ?? (object)DBNull.Value);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка обновления клиента: {ex.Message}");
            }
        }

        public async Task DeleteClientAsync(int id)
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("DELETE FROM clients WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("id", id);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка удаления клиента: {ex.Message}");
            }
        }
    }
}
