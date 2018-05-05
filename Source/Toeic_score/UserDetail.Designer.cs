namespace Toeic_score
{
    partial class UserDetail
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.clm_Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clm_ScoreTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clm_ScoreListening = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clm_ScoreReading = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clm_ButtonDetail = new System.Windows.Forms.DataGridViewButtonColumn();
            this.userTestReviewBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userTestReviewBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clm_Index,
            this.clm_ScoreTotal,
            this.clm_ScoreListening,
            this.clm_ScoreReading,
            this.clm_ButtonDetail});
            this.dataGridView1.DataSource = this.userTestReviewBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(546, 288);
            this.dataGridView1.TabIndex = 0;
            // 
            // clm_Index
            // 
            this.clm_Index.HeaderText = "Items";
            this.clm_Index.Name = "clm_Index";
            // 
            // clm_ScoreTotal
            // 
            this.clm_ScoreTotal.HeaderText = "Score Total";
            this.clm_ScoreTotal.Name = "clm_ScoreTotal";
            // 
            // clm_ScoreListening
            // 
            this.clm_ScoreListening.HeaderText = "Score Listening";
            this.clm_ScoreListening.Name = "clm_ScoreListening";
            // 
            // clm_ScoreReading
            // 
            this.clm_ScoreReading.HeaderText = "Score Reading";
            this.clm_ScoreReading.Name = "clm_ScoreReading";
            // 
            // clm_ButtonDetail
            // 
            this.clm_ButtonDetail.HeaderText = "Detail";
            this.clm_ButtonDetail.Name = "clm_ButtonDetail";
            // 
            // userTestReviewBindingSource
            // 
            this.userTestReviewBindingSource.DataSource = typeof(Toeic_score.UserTestReview);
            // 
            // UserDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Name = "UserDetail";
            this.Size = new System.Drawing.Size(546, 288);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userTestReviewBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn clm_Index;
        private System.Windows.Forms.DataGridViewTextBoxColumn clm_ScoreTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn clm_ScoreListening;
        private System.Windows.Forms.DataGridViewTextBoxColumn clm_ScoreReading;
        private System.Windows.Forms.DataGridViewButtonColumn clm_ButtonDetail;
        private System.Windows.Forms.BindingSource userTestReviewBindingSource;
    }
}
