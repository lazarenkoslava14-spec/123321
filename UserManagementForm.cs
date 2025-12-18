using System;
using System.Drawing;
using System.Windows.Forms;
using PassControlSystem.Models;

namespace PassControlSystem
{
    public partial class UserManagementForm : Form
    {
        private Label lblLogin;
        private Label lblPassword;
        private Label lblFullName;
        private Label lblRole;
        private TextBox txtLogin;
        private TextBox txtPassword;
        private TextBox txtFullName;
        private ComboBox cmbRole;
        private CheckBox chkActive;
        private Button btnSave;
        private Button btnCancel;
        private Button btnGeneratePass;

        private User user;
        private DatabaseHelper dbHelper;
        private bool isEditMode;

        public UserManagementForm(User existingUser, DatabaseHelper dbHelper)
        {
            InitializeComponent();
            this.dbHelper = dbHelper;

            if (existingUser != null)
            {
                user = existingUser;
                isEditMode = true;
                LoadUserData();
                this.Text = "Редактирование пользователя";
            }
            else
            {
                user = new User();
                isEditMode = false;
                this.Text = "Добавление пользователя";
            }
        }

        private void InitializeComponent()
        {
            // Настройки формы
            this.Text = "Управление пользователем";
            this.Size = new Size(400, 350);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Логин
            lblLogin = new Label();
            lblLogin.Text = "Логин:";
            lblLogin.Location = new Point(20, 20);
            lblLogin.Size = new Size(100, 25);

            txtLogin = new TextBox();
            txtLogin.Location = new Point(120, 20);
            txtLogin.Size = new Size(250, 25);
            txtLogin.TabIndex = 0;

            // Пароль
            lblPassword = new Label();
            lblPassword.Text = "Пароль:";
            lblPassword.Location = new Point(20, 60);
            lblPassword.Size = new Size(100, 25);

            txtPassword = new TextBox();
            txtPassword.Location = new Point(120, 60);
            txtPassword.Size = new Size(180, 25);
            txtPassword.TabIndex = 1;

            btnGeneratePass = new Button();
            btnGeneratePass.Text = "Сгенерировать";
            btnGeneratePass.Location = new Point(310, 60);
            btnGeneratePass.Size = new Size(60, 25);
            btnGeneratePass.Click += btnGeneratePass_Click;

            // ФИО
            lblFullName = new Label();
            lblFullName.Text = "ФИО:";
            lblFullName.Location = new Point(20, 100);
            lblFullName.Size = new Size(100, 25);

            txtFullName = new TextBox();
            txtFullName.Location = new Point(120, 100);
            txtFullName.Size = new Size(250, 25);
            txtFullName.TabIndex = 2;

            // Роль
            lblRole = new Label();
            lblRole.Text = "Роль:";
            lblRole.Location = new Point(20, 140);
            lblRole.Size = new Size(100, 25);

            cmbRole = new ComboBox();
            cmbRole.Location = new Point(120, 140);
            cmbRole.Size = new Size(150, 25);
            cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRole.Items.AddRange(new string[] { "Admin", "Guard", "Employee" });
            cmbRole.SelectedIndex = 0;
            cmbRole.TabIndex = 3;

            // Активен
            chkActive = new CheckBox();
            chkActive.Text = "Активен";
            chkActive.Location = new Point(20, 180);
            chkActive.Size = new Size(100, 25);
            chkActive.Checked = true;
            chkActive.TabIndex = 4;

            // Кнопки
            btnSave = new Button();
            btnSave.Text = "Сохранить";
            btnSave.Location = new Point(100, 230);
            btnSave.Size = new Size(100, 35);
            btnSave.TabIndex = 5;
            btnSave.Click += btnSave_Click;

            btnCancel = new Button();
            btnCancel.Text = "Отмена";
            btnCancel.Location = new Point(220, 230);
            btnCancel.Size = new Size(100, 35);
            btnCancel.TabIndex = 6;
            btnCancel.Click += btnCancel_Click;

            // Добавляем на форму
            this.Controls.Add(lblLogin);
            this.Controls.Add(txtLogin);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnGeneratePass);
            this.Controls.Add(lblFullName);
            this.Controls.Add(txtFullName);
            this.Controls.Add(lblRole);
            this.Controls.Add(cmbRole);
            this.Controls.Add(chkActive);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private void LoadUserData()
        {
            txtLogin.Text = user.Login;
            txtPassword.Text = user.Password;
            txtFullName.Text = user.FullName;
            cmbRole.SelectedItem = user.Role;
            chkActive.Checked = user.IsActive;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            user.Login = txtLogin.Text.Trim();
            user.Password = txtPassword.Text;
            user.FullName = txtFullName.Text.Trim();
            user.Role = cmbRole.SelectedItem.ToString();
            user.IsActive = chkActive.Checked;

            bool success;

            if (isEditMode)
            {
                success = dbHelper.UpdateUser(user);
            }
            else
            {
                success = dbHelper.AddUser(user);
            }

            if (success)
            {
                MessageBox.Show("Данные сохранены", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка при сохранении", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtLogin.Text))
            {
                MessageBox.Show("Введите логин", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLogin.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Введите пароль", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Введите ФИО", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return false;
            }

            return true;
        }

        private void btnGeneratePass_Click(object sender, EventArgs e)
        {
            // Генерация случайного пароля
            Random rnd = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] password = new char[8];

            for (int i = 0; i < 8; i++)
            {
                password[i] = chars[rnd.Next(chars.Length)];
            }

            txtPassword.Text = new string(password);
        }
    }
}