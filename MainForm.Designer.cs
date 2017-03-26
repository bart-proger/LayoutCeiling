namespace LayoutCeiling
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
			this.mainMenu = new System.Windows.Forms.MenuStrip();
			this.mmEdit = new System.Windows.Forms.ToolStripMenuItem();
			this.editTools = new System.Windows.Forms.ToolStrip();
			this.panel = new System.Windows.Forms.Panel();
			this.lbUndoStack = new System.Windows.Forms.ListBox();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mainMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmEdit});
			this.mainMenu.Location = new System.Drawing.Point(0, 0);
			this.mainMenu.Name = "mainMenu";
			this.mainMenu.Size = new System.Drawing.Size(850, 24);
			this.mainMenu.TabIndex = 0;
			this.mainMenu.Text = "menuStrip1";
			// 
			// mmEdit
			// 
			this.mmEdit.Name = "mmEdit";
			this.mmEdit.Size = new System.Drawing.Size(59, 20);
			this.mmEdit.Text = "Правка";
			// 
			// editTools
			// 
			this.editTools.ImageScalingSize = new System.Drawing.Size(32, 32);
			this.editTools.Location = new System.Drawing.Point(0, 24);
			this.editTools.MinimumSize = new System.Drawing.Size(0, 39);
			this.editTools.Name = "editTools";
			this.editTools.Size = new System.Drawing.Size(850, 39);
			this.editTools.TabIndex = 1;
			this.editTools.Text = "Инструменты";
			// 
			// panel
			// 
			this.panel.BackColor = System.Drawing.Color.White;
			this.panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel.Location = new System.Drawing.Point(12, 91);
			this.panel.Name = "panel";
			this.panel.Size = new System.Drawing.Size(612, 433);
			this.panel.TabIndex = 2;
			// 
			// lbUndoStack
			// 
			this.lbUndoStack.FormattingEnabled = true;
			this.lbUndoStack.Location = new System.Drawing.Point(641, 91);
			this.lbUndoStack.Name = "lbUndoStack";
			this.lbUndoStack.Size = new System.Drawing.Size(161, 407);
			this.lbUndoStack.TabIndex = 4;
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.newToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
			this.newToolStripMenuItem.Text = "New";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(850, 536);
			this.Controls.Add(this.lbUndoStack);
			this.Controls.Add(this.panel);
			this.Controls.Add(this.editTools);
			this.Controls.Add(this.mainMenu);
			this.DoubleBuffered = true;
			this.KeyPreview = true;
			this.MainMenuStrip = this.mainMenu;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Натяжные потолки \"Лидер\" - ok.ru/liderrr";
			this.Shown += new System.EventHandler(this.MainForm_Shown);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
			this.mainMenu.ResumeLayout(false);
			this.mainMenu.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.MenuStrip mainMenu;
		private System.Windows.Forms.ToolStrip editTools;
		private System.Windows.Forms.Panel panel;
		public  System.Windows.Forms.ListBox lbUndoStack;
		private System.Windows.Forms.ToolStripMenuItem mmEdit;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
    }
}

