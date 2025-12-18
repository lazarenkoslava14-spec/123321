using System;
using System.Drawing;
using System.Windows.Forms;
using PassControlSystem.Models;

namespace PassControlSystem
{
    public partial class EmployeeForm : Form
    {
        private Label lblTitle;
        private Button btnExit;

        public EmployeeForm(User user)
        {
            InitializeComponent();
            this.Text = $"Портал сотрудника ({user.FullName})";
        }

        private void InitializeComponent()
        {
            this.Text = "Портал сотрудника";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            lblTitle = new Label();
            lblTitle.Text = "Портал сотрудника (в разработке)";
            lblTitle.Font = new Font("Arial", 14, FontStyle.Bold);
            lblTitle.Location = new Point(50, 50);
            lblTitle.Size = new Size(500, 30);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            btnExit = new Button();
            btnExit.Text = "Выход";
            btnExit.Location = new Point(250, 150);
            btnExit.Size = new Size(100, 40);
            btnExit.Click += btnExit_Click;

            this.Controls.Add(lblTitle);
            this.Controls.Add(btnExit);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }
    }
}