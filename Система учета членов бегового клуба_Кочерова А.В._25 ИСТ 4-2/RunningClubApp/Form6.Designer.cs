namespace RunningClubApp
{
    partial class Form6
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.listViewTrainings = new System.Windows.Forms.ListView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listViewTrainings
            // 
            this.listViewTrainings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewTrainings.HideSelection = false;
            this.listViewTrainings.Location = new System.Drawing.Point(12, 12);
            this.listViewTrainings.Name = "listViewTrainings";
            this.listViewTrainings.Size = new System.Drawing.Size(560, 300);
            this.listViewTrainings.TabIndex = 0;
            this.listViewTrainings.UseCompatibleStateImageBehavior = false;
            this.listViewTrainings.View = System.Windows.Forms.View.Details;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.BackColor = System.Drawing.Color.IndianRed;
            this.btnAdd.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnAdd.Location = new System.Drawing.Point(432, 330);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(140, 30);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Добавить тренировку";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // Form6
            // 
            this.ClientSize = new System.Drawing.Size(584, 380);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.listViewTrainings);
            this.Name = "Form6";
            this.Text = "Тренировки";
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.ListView listViewTrainings;
        private System.Windows.Forms.Button btnAdd;
    }
}