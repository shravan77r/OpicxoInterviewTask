using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SSXImport
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        string username, password;
        public Login()
        {
            InitializeComponent();
            txtUsername.Focus();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            txtUsername.Text = string.Empty;
            txtPassword.Text = string.Empty;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            username = txtUsername.Text;
            password = txtPassword.Text;

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show(this, "Please Enter UserName");
                txtUsername.Focus();
            }
            else if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show(this, "Please Enter Password");
                txtPassword.Focus();
            }
            else
            {
                if (username.Equals("admin") && password.Equals("admin"))
                {
                    new SourceWindow().Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show(this, "Invalid Username password");
                }
            }
        }
    }
}
