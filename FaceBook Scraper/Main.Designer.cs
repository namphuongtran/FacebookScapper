namespace FaceBook_Scraper
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.Panel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPageUrl = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtLikesLimit = new System.Windows.Forms.TextBox();
            this.txtCommentsLimit = new System.Windows.Forms.TextBox();
            this.txtPostsLimit = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtKeywords = new System.Windows.Forms.TextBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.ScrollSpeedTB = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel
            // 
            this.Panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel.Location = new System.Drawing.Point(16, 56);
            this.Panel.Margin = new System.Windows.Forms.Padding(4);
            this.Panel.Name = "Panel";
            this.Panel.Size = new System.Drawing.Size(1243, 505);
            this.Panel.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Page :";
            // 
            // txtPageUrl
            // 
            this.txtPageUrl.Location = new System.Drawing.Point(56, 16);
            this.txtPageUrl.Name = "txtPageUrl";
            this.txtPageUrl.Size = new System.Drawing.Size(234, 25);
            this.txtPageUrl.TabIndex = 3;
            this.txtPageUrl.Text = "freewebknowledge";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtLikesLimit);
            this.groupBox1.Controls.Add(this.txtCommentsLimit);
            this.groupBox1.Controls.Add(this.txtPostsLimit);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(593, -2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(358, 51);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Limits";
            // 
            // txtLikesLimit
            // 
            this.txtLikesLimit.Location = new System.Drawing.Point(302, 19);
            this.txtLikesLimit.Name = "txtLikesLimit";
            this.txtLikesLimit.Size = new System.Drawing.Size(43, 25);
            this.txtLikesLimit.TabIndex = 4;
            this.txtLikesLimit.Text = "200";
            // 
            // txtCommentsLimit
            // 
            this.txtCommentsLimit.Location = new System.Drawing.Point(200, 19);
            this.txtCommentsLimit.Name = "txtCommentsLimit";
            this.txtCommentsLimit.Size = new System.Drawing.Size(46, 25);
            this.txtCommentsLimit.TabIndex = 3;
            this.txtCommentsLimit.Text = "200";
            // 
            // txtPostsLimit
            // 
            this.txtPostsLimit.Location = new System.Drawing.Point(60, 18);
            this.txtPostsLimit.Name = "txtPostsLimit";
            this.txtPostsLimit.Size = new System.Drawing.Size(47, 25);
            this.txtPostsLimit.TabIndex = 0;
            this.txtPostsLimit.Text = "1000";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(252, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "Likes :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(113, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Comments :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Posts :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(294, 19);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 17);
            this.label5.TabIndex = 5;
            this.label5.Text = "Keywords :";
            // 
            // txtKeywords
            // 
            this.txtKeywords.Location = new System.Drawing.Point(375, 16);
            this.txtKeywords.Name = "txtKeywords";
            this.txtKeywords.Size = new System.Drawing.Size(209, 25);
            this.txtKeywords.TabIndex = 6;
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Image = global::FaceBook_Scraper.Properties.Resources.pause;
            this.btnStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStop.Location = new System.Drawing.Point(1184, 11);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 35);
            this.btnStop.TabIndex = 7;
            this.btnStop.Text = "Stop";
            this.btnStop.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Image = global::FaceBook_Scraper.Properties.Resources.start;
            this.btnStart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStart.Location = new System.Drawing.Point(1086, 11);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(90, 35);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(18, 576);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(30, 17);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.Text = "Idle";
            // 
            // ScrollSpeedTB
            // 
            this.ScrollSpeedTB.Location = new System.Drawing.Point(1050, 17);
            this.ScrollSpeedTB.Name = "ScrollSpeedTB";
            this.ScrollSpeedTB.Size = new System.Drawing.Size(29, 25);
            this.ScrollSpeedTB.TabIndex = 9;
            this.ScrollSpeedTB.Text = "10";
            this.ScrollSpeedTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ScrollSpeedTB_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(957, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 17);
            this.label6.TabIndex = 10;
            this.label6.Text = "Scroll speed :";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1268, 602);
            this.Controls.Add(this.ScrollSpeedTB);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.txtKeywords);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtPageUrl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.Panel);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Facebook Scraper";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel Panel;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPageUrl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtLikesLimit;
        private System.Windows.Forms.TextBox txtCommentsLimit;
        private System.Windows.Forms.TextBox txtPostsLimit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtKeywords;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox ScrollSpeedTB;
        private System.Windows.Forms.Label label6;
    }
}

