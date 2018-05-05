using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using FileHelpers;

namespace Toeic_score
{
    public partial class Form_AddUser : Form
    {
        public delegate void dlg_PassFilePath(object sender);

        public dlg_PassFilePath PassFilePath;

        public Form_AddUser()
        {
            InitializeComponent();
        }

        private void btn_NewUser_Click(object sender, EventArgs e)
        {
            UserInfo newuser = new UserInfo();

            DirectoryInfo dir_info;

            newuser.Name = txb_NewUserName.Text;
            newuser.ID = 1;

            /* Loop through the 'users' folder to check */
            if (Directory.Exists(SystemProperties.UserFolderName) == false)
            {
                Directory.CreateDirectory(SystemProperties.UserFolderName);
            }

            /* Check the data of that user name */
            newuser.Name = newuser.Name.ToLower();

            dir_info = new DirectoryInfo(SystemProperties.UserFolderName);

            FileInfo[] flist = dir_info.GetFiles();

            if (flist.Length == 0)
            {
                if (flist.Any(x => x.Name == newuser.Name) == false)
                {
                    CreateNewUser(newuser.Name);
                }
                else
                {
                    MessageBox.Show("Choose in list or Make other name", "User already have", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                CreateNewUser(newuser.Name);
            }
        }

        private void CreateNewUser(string name)
        {
            /* Create new user data */
            var engine = new FileHelperEngine<TestInfo>();

            List<TestInfo> empty_info = new List<TestInfo>();

            string file_full_path = SystemProperties.UserFolderName + '\\' + name;

            /* Create dummy label for info transfer */
            Label lbl = new Label();
            lbl.Text = file_full_path;

            engine.WriteFile(file_full_path, empty_info);

            PassFilePath(lbl);

            MessageBox.Show("User added successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Form_AddUser_Load(object sender, EventArgs e)
        {

        }

        private void txb_NewUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_NewUser.PerformClick();
            }
        }
    }
}
