using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Toeic_score
{
    public class DynaForm_Ans
    {
        private List<input_question> qulist;
        private List<byte> max_choice_list;
        private Form ans_form;
        private List<Label> lbl_list;
        private List<RdbArray> rdb_list;

        private TestStatus status;

        public TestStatus CreateNewAnsForm()
        {
            DynaForm_Common com_form = new DynaForm_Common();

            max_choice_list = QuestionListMaxChoiceExport(qulist);

            com_form.CreateCommonForm(max_choice_list);

            ans_form = com_form.FormInstance;
            lbl_list = com_form.LabelList;
            rdb_list = com_form.RdbArrayList;

            /* Submit button */
            Button btn1 = new Button();
            btn1.Name = "btn_Submit";
            btn1.Text = "Submit";
            btn1.Size = new Size(80, 40);
            btn1.Location = new Point(320, com_form.FormEndObjectPosY + 60);
            btn1.Click += new System.EventHandler(this.btn_SubmitClick);
            ans_form.Controls.Add(btn1);

            /* Cancel button */
            Button btn2 = new Button();
            btn2.Name = "btn_Discard";
            btn2.Text = "Discard";
            btn2.Size = new Size(80, 40);
            btn2.Location = new Point(440, com_form.FormEndObjectPosY + 60);
            btn2.Click += new System.EventHandler(this.btn_DiscardClick);
            ans_form.Controls.Add(btn2);

            ans_form.ShowDialog();

            return status;
        }

        private void btn_SubmitClick(object sender, EventArgs e)
        {
            DialogResult ans;
            ans = MessageBox.Show("You have done your test?", "Test done?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            status = TestStatus.TEST_STATUS_FINISH;

            if (ans == DialogResult.Yes)
            {
                this.ans_form.Close();
            }
        }

        private void btn_DiscardClick(object sender, EventArgs e)
        {
            DialogResult ans;
            ans = MessageBox.Show("Choose Yes will remove this test from record", "Discard your work?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            status = TestStatus.TEST_STATUS_DISCARD;

            if (ans == DialogResult.Yes)
            {
                this.ans_form.Close();
            }
        }

        private List<byte> QuestionListMaxChoiceExport(List<input_question> qulist)
        {
            List<byte> max_choice_list = new List<byte>();
            for (int i=0; i< qulist.Count; i++)
            {
                byte max = qulist[i].max_choice;
                max_choice_list.Add(max);
            }
            return max_choice_list;
        }

        public byte CheckUserAnswer(byte question_num)
        {
            for (int i=0; i<toeic.MaxChoicePerAnswer; i++)
            {
                if (rdb_list[question_num].array[i].Checked == true)
                {
                    return (byte)(i + 1);
                }
            }
            return 0;
        }

        public List<input_question> QuestionList
        {
            get { return qulist; }
            set { qulist = value; }
        }

    }

    public class DynaForm_Review
    {
        private List<AnsInfo> anslist;
        private List<byte> max_choice_list;
        private Form review_form;
        private List<Panel> pn_list;
        private List<Label> lbl_list;
        private List<RdbArray> rdb_list;
        private List<Label> lbl_correctans_list;
        private List<PictureBox> ptb_incorrect_list;

        public void CreateNewReviewForm()
        {
            DynaForm_Common com_form = new DynaForm_Common();

            max_choice_list = ReviewListMaxChoiceExport(anslist);

            com_form.CreateCommonForm(max_choice_list);

            review_form = com_form.FormInstance;
            lbl_list = com_form.LabelList;
            rdb_list = com_form.RdbArrayList;
            pn_list = com_form.PanelList;
            ptb_incorrect_list = com_form.PtbIncorrectAnsList;
            lbl_correctans_list = com_form.LabelCorrectAnsList;
            AppendUserChoiceInfo(ref rdb_list, ref ptb_incorrect_list, ref lbl_correctans_list);

            /* Finish button */
            Button btn1 = new Button();
            btn1.Name = "btn_Finish";
            btn1.Text = "Finish";
            btn1.Size = new Size(80, 40);
            btn1.Location = new Point((pn_list[0].Width - btn1.Width)/2, com_form.FormEndObjectPosY + 60);
            btn1.Click += new System.EventHandler(this.btn_FinishClick);
            review_form.Controls.Add(btn1);

            review_form.ShowDialog();

        }

        private void btn_FinishClick(object sender, EventArgs e)
        {
            this.review_form.Close();
        }

        private List<byte> ReviewListMaxChoiceExport(List<AnsInfo> anslist)
        {
            List<byte> max_choice_list = new List<byte>();
            for (int i = 0; i < anslist.Count; i++)
            {
                byte max = anslist[i].max_choice;
                max_choice_list.Add(max);
            }
            return max_choice_list;
        }

        private void AppendUserChoiceInfo(ref List<RdbArray> rdblist, 
                                            ref List<PictureBox> ptblist,
                                            ref List<Label> lbllist)
        {
            string str_user_choice;
            string str_correct_choice;

            for (int i=0; i< anslist.Count; i++)
            {
                str_user_choice = toeic.ByteToAnswer(anslist[i].user_choice);

                str_correct_choice = toeic.ByteToAnswer(anslist[i].correct_choice);

                /* Disable all radio button editing */
                for (int j = 0; j < rdblist[i].array.Length; j++)
                {
                    rdblist[i].array[j].Enabled = false;
                }

                /* Mark the user choice */
                if (rdblist[i].array.Any(x => x.Text.ToLower() == str_user_choice) == true)
                {
                    rdblist[i].array.Single(x => x.Text.ToLower() == str_user_choice).Checked = true;
                }

                /* Enable IncorrectSign and display CorrectAnswer */
                if ((rdblist[i].array.Any(x => x.Text.ToLower() == str_user_choice) != true) ||
                    (str_user_choice != str_correct_choice))
                {
                    ptblist[i].Visible = true;
                    lbllist[i].Text = "(" + str_correct_choice.ToUpper() + ")";
                }
            }
        }

        public List<AnsInfo> AnswerList
        {
            get { return anslist; }
            set { anslist = value; }
        }
    }

    public class DynaForm_UserDetail
    {
        private List<TestInfo> user_test_data;
        Form form_us_detail = new Form();
        DataGridView dgv = new DataGridView();

        public void CreateDynaForm_UserDetail()
        {
            DataGridViewButtonColumn btn_clm = new DataGridViewButtonColumn();
            btn_clm.UseColumnTextForButtonValue = true;
            btn_clm.Text = "Click me";
            btn_clm.Name = "Detail";
            dgv.Columns.Add("clm_Index", "Index");
            dgv.Columns.Add("clm_DateTime", "Date Taken");
            dgv.Columns.Add("clm_Duration", "Duration");
            dgv.Columns.Add("clm_ScoreTotal", "Score Total");
            dgv.Columns.Add("clm_ScoreListening", "Score Listening");
            dgv.Columns.Add("clm_ScoreReading", "Score Reading");
            dgv.Columns.Add(btn_clm);
            dgv.AutoSize = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgv.ReadOnly = true;
            form_us_detail.AutoSize = true;
            form_us_detail.Text = "User Detail";
            dgv.CellClick += new DataGridViewCellEventHandler(dgv_CellClickHandler);

            for (int i = 0; i < user_test_data.Count; i++)
            {
                dgv.Rows.Add(   i,
                                DateTime.FromOADate(user_test_data[i].TimeTaken),
                                user_test_data[i].DurationInSecond,
                                user_test_data[i].ScoreTotal,
                                user_test_data[i].ScoreListening,
                                user_test_data[i].ScoreReading
                              );
            }
            form_us_detail.Controls.Add(dgv);
            form_us_detail.ShowDialog();
        }

        public List<TestInfo> TestData
        {
            set { user_test_data = value; }
        }

        private void dgv_CellClickHandler(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != -1)
            {
                DataGridViewColumn clm = dgv.Columns[e.ColumnIndex];

                if (clm.GetType() == typeof(DataGridViewButtonColumn))
                {
                    btn_ClickHandler(e.RowIndex);
                }
            }
        }

        private void btn_ClickHandler(int row_index)
        {
            if (row_index != -1)
            {
                DynaForm_Review form_review = new DynaForm_Review();

                int user_index = (int)(dgv.Rows[row_index].Cells["clm_Index"].Value);

                form_review.AnswerList = user_test_data[user_index].AnsDetail;

                form_review.CreateNewReviewForm();
            }
        }
    }

    internal class DynaForm_Common
    {
        private Form common_form;
        private List<Panel> pn_list = new List<Panel>();
        private List<Label> lbl_list = new List<Label>();
        private List<RdbArray> rdb_list = new List<RdbArray>();
        private List<PictureBox> ptb_incorrectans_list = new List<PictureBox>();
        private List<Label> lbl_correctans_list = new List<Label>();
        private const int initial_x = 25;
        private const int initial_y = 25;
        private const int line_height = 25;
        private int ending_y;

        public void CreateCommonForm(List<byte> max_choice_list)
        {
            int i;
            Form output_form = new Form();
            output_form.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            output_form.AutoScaleDimensions = new SizeF(400, 400);
            output_form.Name = "form_ans";
            output_form.Text = "Answer Form";
            output_form.AutoScroll = true;
            output_form.Height = Screen.FromControl(output_form).Bounds.Height;
            output_form.Width = 900;
            output_form.Location = new Point((Screen.FromControl(output_form).Bounds.Width - output_form.Width) / 2, 0);

            output_form.AutoSize = false;


            for (i = 0; i < max_choice_list.Count; i++)
            {
                Panel pn = new Panel();
                pn.Location = new Point(initial_x, initial_y + (line_height * i));
                pn.Size = new Size(toeic.MaxChoicePerAnswer * 150 + 200, line_height);
                if ((i % 2) == 0)
                {
                    pn.BackColor = Color.AliceBlue;
                }
                else
                {
                    pn.BackColor = Color.ForestGreen;
                }

                pn.Name = "pnl_Q" + i.ToString();
                output_form.Controls.Add(pn);

                /* Add label */
                Label lbl = new Label();
                lbl.Location = new Point(5, 10);
                lbl.Name = "lbl_Question" + (i + 1).ToString("000");
                lbl.Text = "Question " + (i + 1).ToString("000");
                lbl_list.Add(lbl);
                pn.Controls.Add(lbl);

                /* Add radio button */
                RdbArray rdb_array = new RdbArray();
                rdb_list.Add(rdb_array);

                PlaceRadioButtonArray((byte)(i),
                                        max_choice_list[i],
                                        new Point(200, initial_y),
                                        ref rdb_list[i].array,
                                        ref pn);

                /* Add incorrect sign (hidden by default) */
                PictureBox ptb = new PictureBox();
                ptb.Name = "ptb_Question" + (i + 1).ToString("000");
                ptb.Image = new Bitmap(Toeic_score.Properties.Resources.IncorrectSign);
                ptb.Location = new Point(720, 0);
                ptb.Size = new Size(line_height - 5, line_height - 5);
                ptb.SizeMode = PictureBoxSizeMode.StretchImage;
                ptb.Visible = false;
                ptb_incorrectans_list.Add(ptb);
                pn.Controls.Add(ptb);

                /* Add correct ans label (hidden by default) */
                Label lbl_correct = new Label();
                lbl_correct.Location = new Point(750, 5);
                lbl_correct.Name = "lbl_CorrectAns" + (i + 1).ToString("000");
                lbl_correct.Text = "";
                lbl_correctans_list.Add(lbl_correct);
                pn.Controls.Add(lbl_correct);
                pn_list.Add(pn);
            }
            common_form = output_form;
            ending_y = max_choice_list.Count * line_height;
        }

        public Form FormInstance
        {
            get { return common_form; }
        }

        public List<Panel> PanelList
        {
            get { return pn_list; }
        }

        public List<Label> LabelList
        {
            get { return lbl_list; }
        }

        public List<RdbArray> RdbArrayList
        {
            get { return rdb_list; }
        }

        public List<Label> LabelCorrectAnsList
        {
            get { return lbl_correctans_list; }
        }

        public List<PictureBox> PtbIncorrectAnsList
        {
            get { return ptb_incorrectans_list; }
        }

        public int FormStartPosX
        {
            get { return initial_x; }
        }

        public int FormStartPosY
        {
            get { return initial_y; }
        }

        public int FormLineHeight
        {
            get { return line_height; }
        }

        public int FormEndObjectPosY
        {
            get { return ending_y; }
        }

        private static void PlaceRadioButtonArray(byte quindex,
                                                    byte qty,
                                                    Point form_pos,
                                                    ref RadioButton[] rdbarray,
                                                    ref Panel panel_obj)
        {
            int i;


            for (i = 0; i < qty; i++)
            {
                rdbarray[i].Location = new Point(200 + (150 * i), 5);
                rdbarray[i].Text = toeic.ByteToAnswer((byte)(i + 1)).ToUpper();
                rdbarray[i].Name = "rdb_Q" + quindex.ToString() + "_" + (i + 1).ToString();
                rdbarray[i].Width = 30;
                panel_obj.Controls.Add(rdbarray[i]);
            }

            for (i = qty; i < toeic.MaxChoicePerAnswer; i++)
            {
                rdbarray[i].Dispose();
            }
        }



    }

    internal class RdbArray
    {
        public RadioButton[] array = new RadioButton[toeic.MaxChoicePerAnswer];

        public RdbArray()
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = new RadioButton();
            }
        }
    }

    public enum TestStatus
    {
        TEST_STATUS_FINISH = 0,
        TEST_STATUS_DISCARD,
    }
}
