﻿namespace Client.View.Main
{
    partial class MainView
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
            this.tableFourColumn = new System.Windows.Forms.TableLayoutPanel();
            this.tableLeftButtons = new System.Windows.Forms.TableLayoutPanel();
            this.OpenQuestionButton = new MetroFramework.Controls.MetroTile();
            this.ImportAccountButton = new MetroFramework.Controls.MetroTile();
            this.ImportAccountButton = new MetroFramework.Controls.MetroTile();
            this.EndSessionButton = new MetroFramework.Controls.MetroTile();
            this.tableFourColumn.SuspendLayout();
            this.tableLeftButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableFourColumn
            // 
            this.tableFourColumn.ColumnCount = 4;
            this.tableFourColumn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableFourColumn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableFourColumn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableFourColumn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0F));
            this.tableFourColumn.Controls.Add(this.tableLeftButtons, 0, 0);
            this.tableFourColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableFourColumn.Location = new System.Drawing.Point(20, 60);
            this.tableFourColumn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableFourColumn.Name = "tableFourColumn";
            this.tableFourColumn.RowCount = 1;
            this.tableFourColumn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableFourColumn.Size = new System.Drawing.Size(588, 238);
            this.tableFourColumn.TabIndex = 0;
            // 
            // tableLeftButtons
            // 
            this.tableLeftButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLeftButtons.ColumnCount = 1;
            this.tableLeftButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLeftButtons.Controls.Add(this.OpenQuestionButton, 0, 0);
            this.tableLeftButtons.Controls.Add(this.ImportAccountButton, 0, 1);
            this.tableLeftButtons.Location = new System.Drawing.Point(3, 3);
            this.tableLeftButtons.Name = "tableLeftButtons";
            this.tableLeftButtons.RowCount = 2;
            this.tableLeftButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLeftButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLeftButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLeftButtons.Size = new System.Drawing.Size(52, 228);
            this.tableLeftButtons.TabIndex = 1;
            // 
            // OpenQuestionButton
            // 
            this.OpenQuestionButton.ActiveControl = null;
            this.OpenQuestionButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OpenQuestionButton.Location = new System.Drawing.Point(3, 3);
            this.OpenQuestionButton.Name = "OpenQuestionButton";
            this.OpenQuestionButton.Size = new System.Drawing.Size(46, 54);
            this.OpenQuestionButton.Style = MetroFramework.MetroColorStyle.Teal;
            this.OpenQuestionButton.TabIndex = 2;
            this.OpenQuestionButton.Text = "Openvraag toevoegen";
            this.OpenQuestionButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.OpenQuestionButton.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.OpenQuestionButton.UseSelectable = true;
            this.OpenQuestionButton.UseTileImage = true;
            this.OpenQuestionButton.Click += new System.EventHandler(this.OpenQuestionButton_Click);
            // 
            // ImportAccountButton
            // 
            this.ImportAccountButton.ActiveControl = null;
            this.ImportAccountButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImportAccountButton.Location = new System.Drawing.Point(3, 63);
            this.ImportAccountButton.Name = "ImportAccountButton";
            this.ImportAccountButton.Size = new System.Drawing.Size(46, 162);
            this.ImportAccountButton.Style = MetroFramework.MetroColorStyle.Green;
            this.ImportAccountButton.TabIndex = 3;
            this.ImportAccountButton.Text = "Importeer accounts";
            this.ImportAccountButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ImportAccountButton.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.ImportAccountButton.UseSelectable = true;
            this.ImportAccountButton.Click += new System.EventHandler(this.ImportAccountButton_Click);
            // 
            // EndSessionButton
            // 
            this.EndSessionButton.BackColor = System.Drawing.Color.White;
            this.EndSessionButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold);
            this.EndSessionButton.Location = new System.Drawing.Point(3, 69);
            this.EndSessionButton.Name = "EndSessionButton";
            this.EndSessionButton.Size = new System.Drawing.Size(84, 49);
            this.EndSessionButton.TabIndex = 3;
            this.EndSessionButton.Text = "Sessie Eindigen";
            this.EndSessionButton.UseVisualStyleBackColor = false;
            this.EndSessionButton.Click += new System.EventHandler(this.EndSessionButton_Click);
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 318);
            this.Controls.Add(this.tableFourColumn);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainView";
            this.Text = "ViewMain2";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tableFourColumn.ResumeLayout(false);
            this.tableLeftButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableFourColumn;
        private System.Windows.Forms.TableLayoutPanel tableLeftButtons;
        private MetroFramework.Controls.MetroTile OpenQuestionButton;
        private MetroFramework.Controls.MetroTile ImportAccountButton;
        private MetroFramework.Controls.MetroTile EndSessionButton;
    }
}