﻿namespace LayoutCeiling
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.mainMenu = new System.Windows.Forms.MenuStrip();
			this.mmEdit = new System.Windows.Forms.ToolStripMenuItem();
			this.editTools = new System.Windows.Forms.ToolStrip();
			this.panel = new System.Windows.Forms.Panel();
			this.lbUndoStack = new System.Windows.Forms.ListBox();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addTools = new System.Windows.Forms.ToolStrip();
			this.mainMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmEdit});
			this.mainMenu.Location = new System.Drawing.Point(0, 0);
			this.mainMenu.Name = "mainMenu";
			this.mainMenu.Size = new System.Drawing.Size(872, 24);
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
			this.editTools.Size = new System.Drawing.Size(872, 39);
			this.editTools.TabIndex = 1;
			this.editTools.Text = "Инструменты";
			// 
			// panel
			// 
			this.panel.BackColor = System.Drawing.Color.White;
			this.panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel.Location = new System.Drawing.Point(14, 107);
			this.panel.Margin = new System.Windows.Forms.Padding(5);
			this.panel.Name = "panel";
			this.panel.Size = new System.Drawing.Size(612, 433);
			this.panel.TabIndex = 2;
			// 
			// lbUndoStack
			// 
			this.lbUndoStack.FormattingEnabled = true;
			this.lbUndoStack.Location = new System.Drawing.Point(650, 107);
			this.lbUndoStack.Name = "lbUndoStack";
			this.lbUndoStack.Size = new System.Drawing.Size(161, 433);
			this.lbUndoStack.TabIndex = 4;
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.newToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
			this.newToolStripMenuItem.Text = "New";
			// 
			// addTools
			// 
			this.addTools.ImageScalingSize = new System.Drawing.Size(32, 32);
			this.addTools.Location = new System.Drawing.Point(0, 63);
			this.addTools.MinimumSize = new System.Drawing.Size(0, 39);
			this.addTools.Name = "addTools";
			this.addTools.Size = new System.Drawing.Size(872, 39);
			this.addTools.TabIndex = 5;
			this.addTools.Text = "Фигуры";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(872, 566);
			this.Controls.Add(this.addTools);
			this.Controls.Add(this.lbUndoStack);
			this.Controls.Add(this.panel);
			this.Controls.Add(this.editTools);
			this.Controls.Add(this.mainMenu);
			this.DoubleBuffered = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
		private System.Windows.Forms.ToolStrip addTools;
	}
}

