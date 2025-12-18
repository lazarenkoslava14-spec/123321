using System;
using System.Drawing;
using System.Windows.Forms;
using PassControlSystem.Models;

namespace PassControlSystem
{
    public partial class AdminForm : Form
    {
        private DataGridView dataGridViewUsers;
        private Button btnAddUser;
        private Button btnEditUser;
        private Button btnDeleteUser;
        private Button btnExit;
        private Label lblTitle;
        private Label lblStatus;

        private User currentUser;
        private DatabaseHelper dbHelper;

        public AdminForm(User user)
        {
            InitializeComponent();
            currentUser = user;
            dbHelper = new DatabaseHelper();

            this.Text = $"PassControl System - Администратор ({user.FullName})";
            LoadUsers();
        }

        private void InitializeComponent()
        {
            // Настройки формы
            this.Text = "Администратор";
            this.Size = new Size(900, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosing += AdminForm_FormClosing;

            // Заголовок
            lblTitle = new Label();
            lblTitle.Text = "Управление пользователями";
            lblTitle.Font = new Font("Arial", 14, FontStyle.Bold);
            lblTitle.Location = new Point(20, 20);
            lblTitle.Size = new Size(400, 30);

            // Таблица пользователей
            dataGridViewUsers = new DataGridView();
            dataGridViewUsers.Location = new Point(20, 60);
            dataGridViewUsers.Size = new Size(850, 300);
            dataGridViewUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewUsers.ReadOnly = true;
            dataGridViewUsers.AllowUserToAddRows = false;
            dataGridViewUsers.AllowUserToDeleteRows = false;

            // Кнопки
            btnAddUser = new Button();
            btnAddUser.Text = "Добавить пользователя";
            btnAddUser.Location = new Point(20, 380);
            btnAddUser.Size = new Size(180, 40);
            btnAddUser.Click += btnAddUser_Click;

            btnEditUser = new Button();
            btnEditUser.Text = "Редактировать";
            btnEditUser.Location = new Point(220, 380);
            btnEditUser.Size = new Size(150, 40);
            btnEditUser.Click += btnEditUser_Click;

            btnDeleteUser = new Button();
            btnDeleteUser.Text = "Удалить";
            btnDeleteUser.Location = new Point(390, 380);
            btnDeleteUser.Size = new Size(150, 40);
            btnDeleteUser.Click += btnDeleteUser_Click;

            btnExit = new Button();
            btnExit.Text = "Выход";
            btnExit.Location = new Point(720, 380);
            btnExit.Size = new Size(150, 40);
            btnExit.Click += btnExit_Click;

            // Статус
            lblStatus = new Label();
            lblStatus.Location = new Point(20, 430);
            lblStatus.Size = new Size(500, 25);
            lblStatus.Text = "Готово";

            // Добавляем на форму
            this.Controls.Add(lblTitle);
            this.Controls.Add(dataGridViewUsers);
            this.Controls.Add(btnAddUser);
            this.Controls.Add(btnEditUser);
            this.Controls.Add(btnDeleteUser);
            this.Controls.Add(btnExit);
            this.Controls.Add(lblStatus);
        }

        private void LoadUsers()
        {
            var users = dbHelper.GetAllUsers();
            dataGridViewUsers.DataSource = users;

            if (dataGridViewUsers.Columns.Count > 0)
            {
                dataGridViewUsers.Columns["Id"].Visible = false;
                dataGridViewUsers.Columns["Password"].Visible = false;
                dataGridViewUsers.Columns["CreatedDate"].HeaderText = "Дата создания";
                dataGridViewUsers.Columns["IsActive"].HeaderText = "Активен";
            }

            lblStatus.Text = $"Загружено пользователей: {users.Count}";
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            UserManagementForm userForm = new UserManagementForm(null, dbHelper);
            if (userForm.ShowDialog() == DialogResult.OK)
            {
                LoadUsers();
            }
        }

        private void btnEditUser_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsers.SelectedRows.Count > 0)
            {
                User selectedUser = dataGridViewUsers.SelectedRows[0].DataBoundItem as User;
                if (selectedUser != null)
                {
                    UserManagementForm userForm = new UserManagementForm(selectedUser, dbHelper);
                    if (userForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadUsers();
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя для редактирования",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsers.SelectedRows.Count > 0)
            {
                User selectedUser = dataGridViewUsers.SelectedRows[0].DataBoundItem as User;

                if (selectedUser != null)
                {
                    DialogResult result = MessageBox.Show(
                        $"Удалить пользователя {selectedUser.FullName}?",
                        "Подтверждение",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        if (dbHelper.DeleteUser(selectedUser.Id))
                        {
                            MessageBox.Show("Пользователь удален", "Успех",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadUsers();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя для удаления",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }

        private void AdminForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
            }
        }
    }
}