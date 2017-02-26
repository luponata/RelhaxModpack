﻿namespace RelicModManager
{
    partial class VersionInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VersionInfo));
            this.downloadedVersionInfo = new System.Windows.Forms.RichTextBox();
            this.updateAcceptButton = new System.Windows.Forms.Button();
            this.updateDeclineButton = new System.Windows.Forms.Button();
            this.newVersionAvailableLabel = new System.Windows.Forms.Label();
            this.updateQuestionLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // downloadedVersionInfo
            // 
            this.downloadedVersionInfo.Location = new System.Drawing.Point(12, 31);
            this.downloadedVersionInfo.Name = "downloadedVersionInfo";
            this.downloadedVersionInfo.ReadOnly = true;
            this.downloadedVersionInfo.Size = new System.Drawing.Size(270, 144);
            this.downloadedVersionInfo.TabIndex = 0;
            this.downloadedVersionInfo.Text = "";
            // 
            // updateAcceptButton
            // 
            this.updateAcceptButton.Location = new System.Drawing.Point(207, 181);
            this.updateAcceptButton.Name = "updateAcceptButton";
            this.updateAcceptButton.Size = new System.Drawing.Size(75, 23);
            this.updateAcceptButton.TabIndex = 1;
            this.updateAcceptButton.Text = "yes";
            this.updateAcceptButton.UseVisualStyleBackColor = true;
            this.updateAcceptButton.Click += new System.EventHandler(this.updateAcceptButton_Click);
            // 
            // updateDeclineButton
            // 
            this.updateDeclineButton.Location = new System.Drawing.Point(12, 181);
            this.updateDeclineButton.Name = "updateDeclineButton";
            this.updateDeclineButton.Size = new System.Drawing.Size(75, 23);
            this.updateDeclineButton.TabIndex = 2;
            this.updateDeclineButton.Text = "no";
            this.updateDeclineButton.UseVisualStyleBackColor = true;
            this.updateDeclineButton.Click += new System.EventHandler(this.updateDeclineButton_Click);
            // 
            // newVersionAvailableLabel
            // 
            this.newVersionAvailableLabel.AutoSize = true;
            this.newVersionAvailableLabel.Location = new System.Drawing.Point(12, 9);
            this.newVersionAvailableLabel.Name = "newVersionAvailableLabel";
            this.newVersionAvailableLabel.Size = new System.Drawing.Size(113, 13);
            this.newVersionAvailableLabel.TabIndex = 3;
            this.newVersionAvailableLabel.Text = "New Version Available";
            // 
            // updateQuestionLabel
            // 
            this.updateQuestionLabel.AutoSize = true;
            this.updateQuestionLabel.Location = new System.Drawing.Point(119, 188);
            this.updateQuestionLabel.Name = "updateQuestionLabel";
            this.updateQuestionLabel.Size = new System.Drawing.Size(48, 13);
            this.updateQuestionLabel.TabIndex = 4;
            this.updateQuestionLabel.Text = "Update?";
            // 
            // VersionInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 210);
            this.Controls.Add(this.updateQuestionLabel);
            this.Controls.Add(this.newVersionAvailableLabel);
            this.Controls.Add(this.updateDeclineButton);
            this.Controls.Add(this.updateAcceptButton);
            this.Controls.Add(this.downloadedVersionInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "VersionInfo";
            this.Text = "VersionInfo";
            this.Load += new System.EventHandler(this.VersionInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.RichTextBox downloadedVersionInfo;
        private System.Windows.Forms.Button updateAcceptButton;
        private System.Windows.Forms.Button updateDeclineButton;
        private System.Windows.Forms.Label newVersionAvailableLabel;
        private System.Windows.Forms.Label updateQuestionLabel;

    }
}