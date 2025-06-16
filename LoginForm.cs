using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace furniture
{
    public partial class LoginForm : Form
    {
        private readonly Database db;

        public LoginForm()
        {
            InitializeComponent();
            db = new Database("Host=localhost;Port=5432;Database=furniture_shop;Username=postgres;Password=postgres");
            InitializeControls();
        }

        private void InitializeControls()
        {
            this.Size = new Size(725, 500);
            this.Text = "Авторизация";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            var lblLogin = new Label
            {
                Text = "Логин:",
                Location = new Point(250, 100),
                Size = new Size(50, 20)
            };

            var txtLogin = new TextBox
            {
                Location = new Point(250, 120),
                Size = new Size(180, 20)
            };

            var lblPassword = new Label
            {
                Text = "Пароль:",
                Location = new Point(250, 150),
                Size = new Size(50, 20)
            };

            var txtPassword = new TextBox
            {
                Location = new Point(250, 170),
                Size = new Size(180, 20),
                UseSystemPasswordChar = true
            };

            var btnLogin = new Button
            {
                Text = "Войти",
                Location = new Point(290, 210),
                Size = new Size(100, 30)
            };
            btnLogin.Click += async (s, e) => await HandleLoginAsync(txtLogin.Text, txtPassword.Text);

            this.Controls.AddRange(new Control[] { lblLogin, txtLogin, lblPassword, txtPassword, btnLogin });
        }

        private async Task HandleLoginAsync(string login, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Введите логин и пароль!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool isAuthenticated = await db.AuthenticateUserAsync(login, password);
                if (isAuthenticated)
                {
                    this.Hide();
                    new MainForm().ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка авторизации: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
