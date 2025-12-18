using System;
using System.Drawing;
using System.Windows.Forms;
using PassControlSystem.Models;

namespace PassControlSystem
{
    public partial class LoginForm : Form
    {
        private Label lblTitle;
        private Label lblLogin;
        private Label lblPassword;
        private TextBox txtLogin;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnExit;

        private DatabaseHelper dbHelper;

        public LoginForm()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            dbHelper.InitializeDatabase();
        }

        private void InitializeComponent()
        {
            // Настройки формы
            this.Text = "PassControl System - Вход";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Load += LoginForm_Load;

            // Заголовок
            lblTitle = new Label();
            lblTitle.Text = "Пропускная система предприятия";
            lblTitle.Font = new Font("Arial", 14, FontStyle.Bold);
            lblTitle.Location = new Point(40, 20);
            lblTitle.Size = new Size(320, 30);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // Поле логина
            lblLogin = new Label();
            lblLogin.Text = "Логин:";
            lblLogin.Location = new Point(50, 80);
            lblLogin.Size = new Size(100, 25);

            txtLogin = new TextBox();
            txtLogin.Location = new Point(150, 80);
            txtLogin.Size = new Size(200, 25);
            txtLogin.TabIndex = 0;

            // Поле пароля
            lblPassword = new Label();
            lblPassword.Text = "Пароль:";
            lblPassword.Location = new Point(50, 120);
            lblPassword.Size = new Size(100, 25);

            txtPassword = new TextBox();
            txtPassword.Location = new Point(150, 120);
            txtPassword.Size = new Size(200, 25);
            txtPassword.PasswordChar = '*';
            txtPassword.TabIndex = 1;

            // Кнопки
            btnLogin = new Button();
            btnLogin.Text = "Войти";
            btnLogin.Location = new Point(100, 180);
            btnLogin.Size = new Size(100, 35);
            btnLogin.TabIndex = 2;
            btnLogin.Click += btnLogin_Click;

            btnExit = new Button();
            btnExit.Text = "Выход";
            btnExit.Location = new Point(220, 180);
            btnExit.Size = new Size(100, 35);
            btnExit.TabIndex = 3;
            btnExit.Click += btnExit_Click;

            // Добавляем элементы на форму
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblLogin);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtLogin);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnExit);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            User user = dbHelper.AuthenticateUser(login, password);

            if (user != null)
            {
                MessageBox.Show($"Добро пожаловать, {user.FullName}!", "Успешный вход",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Hide();

                if (user.Role == "Admin")
                {
                    AdminForm adminForm = new AdminForm(user);
                    adminForm.Show();
                }
                else if (user.Role == "Guard")
                {
                    GuardForm guardForm = new GuardForm(user);
                    guardForm.Show();
                }
                else if (user.Role == "Employee")
                {
                    EmployeeForm employeeForm = new EmployeeForm(user);
                    employeeForm.Show();
                }
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка входа",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            txtLogin.Focus();
        }
    }
}