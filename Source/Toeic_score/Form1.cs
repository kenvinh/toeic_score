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
using Toeic_score;

namespace Toeic_score
{
    public partial class Form1 : Form
    {
        string input_path;

        private List<string> cmb_UsersList_Binding_FileListName = new List<string>();
        private List<string> cmb_UsersList_Binding_FileListDir = new List<string>();
        private string cmb_UsersList_Binding_Selected_FileDir;
        private string cmb_UsersList_Binding_Selected_FileName;

        private List<TestInfo> user_test_info;

        public Form1()
        {
            InitializeComponent();

            CheckUserSelection();

            txb_TestSelect.Text = input_path;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                toeic test = new toeic();
                TestInfo test_info = new TestInfo();
                TestStatus status;

                var engine_input = new FileHelperEngine<input_question>();

                var result = engine_input.ReadFile(input_path);

                test.question_list = result.ToList<input_question>();

                byte check = toeic.CheckQuestionList(test.question_list);

                /* Check complete, now  display the answer form dynamically*/
                /* Mark start time */
                test_info.TimeTaken = DateTime.Now.ToOADate();
                DynaForm_Ans ans_form_class = new DynaForm_Ans();
                ans_form_class.QuestionList = test.question_list;
                status = ans_form_class.CreateNewAnsForm();
                
                if (status == TestStatus.TEST_STATUS_FINISH)
                { 
                    /* User has finished answer question, now map the score  */
                    /* Mark stop time */
                    test_info.TimeFinish = DateTime.Now.ToOADate();
                    test_info.DurationInSecond = SystemProperties.DurationInSecondFromTimeSpan(test_info.TimeTaken, test_info.TimeFinish);
                    test_info.TestName = toeic.GetTestName(input_path);

                    for (int i=0; i< test.question_list.Count; i++)
                    {
                        /* Mapping between test input data and test result data */
                        AnsInfo ans = new AnsInfo();

                        ans.question_num = test.question_list[i].question_num;
                        ans.max_choice = test.question_list[i].max_choice;
                        ans.correct_choice = toeic.AnswerToByte(test.question_list[i].correct_choice);
                        ans.user_choice = ans_form_class.CheckUserAnswer((byte)i);

                        test_info.AnsDetail.Add(ans);
                    }

                    /* Mapping finished, now count correct answer */
                    test_info.CountCorrectAnswer();

                    /* Marking the score */
                    test_info.ScoreListening = toeic.MarkListeningScore(test_info.CorrectAnsListening);
                    test_info.ScoreReading = toeic.MarkListeningScore(test_info.CorrectAnsReading);
                    test_info.ScoreTotal = test_info.ScoreListening + test_info.ScoreReading;

                    /* Finish, display result to user and display review form */
                    var engine_output = new FileHelperEngine<TestInfo>();
                    user_test_info.Add(test_info);
                    engine_output.WriteFile(cmb_UsersList_Binding_Selected_FileDir, user_test_info);
                    UpdateUserInfo();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "File reading error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Multiselect = false;
            fd.InitialDirectory = SystemProperties.TestDataFolderName;
            fd.Filter = "CSV Files (*.csv)|*.csv";
            fd.ShowDialog();

            input_path = fd.FileName;

            txb_TestSelect.Text = input_path;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form_AddUser form_add = new Form_AddUser();

            form_add.PassFilePath = new Form_AddUser.dlg_PassFilePath(cmb_PassSelection);

            form_add.ShowDialog();

            form_add.Close();
        }

        private void cmb_UsersList_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void cmb_UsersList_DropDown(object sender, EventArgs e)
        {
            try
            {
                cmb_UserList_RefreshList();
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message, "Folder reading error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmb_UserList_RefreshList()
        {
            try
            {
                cmb_UsersList_Binding_FileListName = SystemProperties.GetFolderFilesName(SystemProperties.UserFolderName);
                cmb_UsersList_Binding_FileListDir = SystemProperties.GetFolderFilesDir(SystemProperties.UserFolderName);
                cmb_UsersList.DataSource = cmb_UsersList_Binding_FileListName;
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message, "Folder reading error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            this.Activate();//this.button1.Focus();
            btn_Start.Focus();
        }

        private void cmb_UsersList_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)(sender);

            UpdateUserSelection(cb.SelectedIndex);

            UpdateUserInfo();
        }

        private void cmb_PassSelection(object sender)
        {
            cmb_UserList_RefreshList();

            Label lb = (Label)(sender);

            string str_user_name = SystemProperties.GetFileNameFromPath(lb.Text);

            cmb_UsersList.SelectedIndex = cmb_UsersList_Binding_FileListName.IndexOf(str_user_name);

            UpdateUserSelection(cmb_UsersList.SelectedIndex);

            UpdateUserInfo();
        }

        private void CheckUserSelection()
        {
            if (cmb_UsersList.SelectedItem == null)
            {
                lbl_Status.Text = "Please select a user";
                txb_TestSelect.Enabled = false;
                btn_BrowseTestItem.Enabled = false;
                btn_Start.Enabled = false;
                btn_LookupUserInfo.Enabled = false;
            }
            else
            {
                lbl_Status.Text = "";
                txb_TestSelect.Enabled = true;
                btn_BrowseTestItem.Enabled = true;
                btn_Start.Enabled = true;
                btn_LookupUserInfo.Enabled = true;
            }
        }

        private void UpdateUserSelection(int selectedindex)
        {
            cmb_UsersList_Binding_Selected_FileDir = cmb_UsersList_Binding_FileListDir[selectedindex];
            cmb_UsersList_Binding_Selected_FileName = cmb_UsersList_Binding_FileListName[selectedindex];
        }

        private void UpdateUserInfo()
        {
            var engine = new FileHelperEngine<TestInfo>();

            var result = engine.ReadFile(cmb_UsersList_Binding_Selected_FileDir);

            user_test_info = result.ToList<TestInfo>();

            string user_hello = "Who are you? " + cmb_UsersList_Binding_Selected_FileName + "\n\r";
            user_hello = String.Format("{0:s} You've taken: {1:d} times.\n\r", user_hello, user_test_info.Count);
            if (user_test_info.Count != 0)
            {
                int last_test_index = user_test_info.Count - 1;
                user_hello = String.Format("{0:s} Your last test taken result: \n\r", user_hello);
                user_hello = String.Format("{0:s} ---- Test name: {1:s}\n\r", user_hello, user_test_info[last_test_index].TestName);
                user_hello = String.Format("{0:s} ---- Test duration (seconds): {1:d}\n\r", user_hello, user_test_info[last_test_index].DurationInSecond);
                user_hello = String.Format("{0:s} ---- Total score: {1:d}\n\r", user_hello, user_test_info[last_test_index].ScoreTotal);
            }
            lbl_UserHello.Text = user_hello;
            CheckUserSelection();
        }

        private void btn_LookupUserInfo_Click(object sender, EventArgs e)
        {
            /* Open UserControl to display detail */
            DynaForm_UserDetail form_userdetail = new DynaForm_UserDetail();

            form_userdetail.TestData = user_test_info;
            form_userdetail.CreateDynaForm_UserDetail();

        }
    }
}
