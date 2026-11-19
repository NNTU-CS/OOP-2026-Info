namespace RunningClubApp
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listViewRunners = new System.Windows.Forms.ListView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.данныеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFields = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveDB = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLoadDB = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewRunners
            // 
            this.listViewRunners.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewRunners.HideSelection = false;
            this.listViewRunners.Location = new System.Drawing.Point(80, 27);
            this.listViewRunners.Name = "listViewRunners";
            this.listViewRunners.Size = new System.Drawing.Size(634, 382);
            this.listViewRunners.TabIndex = 0;
            this.listViewRunners.UseCompatibleStateImageBehavior = false;
            this.listViewRunners.View = System.Windows.Forms.View.Details;
            this.listViewRunners.DoubleClick += new System.EventHandler(this.listViewRunners_DoubleClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.данныеToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // данныеToolStripMenuItem
            // 
            this.данныеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFields,
            this.menuFilter,
            this.menuAdd,
            this.menuSaveDB,
            this.menuLoadDB,
            this.menuDelete});
            this.данныеToolStripMenuItem.Name = "данныеToolStripMenuItem";
            this.данныеToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.данныеToolStripMenuItem.Text = "Данные";
            // 
            // menuFields
            // 
            this.menuFields.Name = "menuFields";
            this.menuFields.Size = new System.Drawing.Size(197, 22);
            this.menuFields.Text = "Настроить";
            // 
            // menuFilter
            // 
            this.menuFilter.Name = "menuFilter";
            this.menuFilter.Size = new System.Drawing.Size(197, 22);
            this.menuFilter.Text = "Фильтровать";
            // 
            // menuAdd
            // 
            this.menuAdd.Name = "menuAdd";
            this.menuAdd.Size = new System.Drawing.Size(197, 22);
            this.menuAdd.Text = "Добавить члена клуба";
            this.menuAdd.Click += new System.EventHandler(this.menuAdd_Click);
            // 
            // menuSaveDB
            // 
            this.menuSaveDB.Name = "menuSaveDB";
            this.menuSaveDB.Size = new System.Drawing.Size(197, 22);
            this.menuSaveDB.Text = "Сохранить в БД";
            this.menuSaveDB.Click += new System.EventHandler(this.menuSaveDB_Click);
            // 
            // menuLoadDB
            // 
            this.menuLoadDB.Name = "menuLoadDB";
            this.menuLoadDB.Size = new System.Drawing.Size(197, 22);
            this.menuLoadDB.Text = "Загрузить из БД";
            this.menuLoadDB.Click += new System.EventHandler(this.menuLoadDB_Click);
            // 
            // menuDelete
            // 
            this.menuDelete.Name = "menuDelete";
            this.menuDelete.Size = new System.Drawing.Size(197, 22);
            this.menuDelete.Text = "Удалить члена клуба";
            this.menuDelete.Click += new System.EventHandler(this.menuDelete_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.listViewRunners);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Система учёта членов бегового клуба";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.DoubleClick += new System.EventHandler(this.Form1_DoubleClick);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewRunners;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem данныеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuFields;
        private System.Windows.Forms.ToolStripMenuItem menuFilter;
        private System.Windows.Forms.ToolStripMenuItem menuAdd;
        private System.Windows.Forms.ToolStripMenuItem menuSaveDB;
        private System.Windows.Forms.ToolStripMenuItem menuLoadDB;
        private System.Windows.Forms.ToolStripMenuItem menuDelete;
    }
}