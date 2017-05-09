namespace TableMLGUI
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCompileSelect = new System.Windows.Forms.Button();
            this.tbFileList = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbFileDir = new System.Windows.Forms.TextBox();
            this.btnCompileAll = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnUpdateCSSyntax = new System.Windows.Forms.Button();
            this.txtCodePath = new System.Windows.Forms.TextBox();
            this.btnSyncCode = new System.Windows.Forms.Button();
            this.btnSyncTml = new System.Windows.Forms.Button();
            this.txtTmlPath = new System.Windows.Forms.TextBox();
            this.btnCheckNameRepet = new System.Windows.Forms.Button();
            this.btnOpenCodeDir = new System.Windows.Forms.Button();
            this.btnOpenTmlDir = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCompileSelect
            // 
            this.btnCompileSelect.Location = new System.Drawing.Point(46, 443);
            this.btnCompileSelect.Name = "btnCompileSelect";
            this.btnCompileSelect.Size = new System.Drawing.Size(300, 50);
            this.btnCompileSelect.TabIndex = 0;
            this.btnCompileSelect.Text = "编译上面框中的Excel";
            this.btnCompileSelect.UseVisualStyleBackColor = true;
            this.btnCompileSelect.Click += new System.EventHandler(this.btnCompileSelect_Click);
            // 
            // tbFileList
            // 
            this.tbFileList.AllowDrop = true;
            this.tbFileList.Location = new System.Drawing.Point(3, 36);
            this.tbFileList.Multiline = true;
            this.tbFileList.Name = "tbFileList";
            this.tbFileList.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbFileList.Size = new System.Drawing.Size(447, 401);
            this.tbFileList.TabIndex = 1;
            this.tbFileList.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbFileList_DragDrop);
            this.tbFileList.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbFileList_DragEnter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(311, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "请拖入需要编译的单个或多个Excel文件(可手动输入路径)";
            // 
            // tbFileDir
            // 
            this.tbFileDir.AllowDrop = true;
            this.tbFileDir.Location = new System.Drawing.Point(12, 558);
            this.tbFileDir.Name = "tbFileDir";
            this.tbFileDir.Size = new System.Drawing.Size(438, 21);
            this.tbFileDir.TabIndex = 3;
            this.tbFileDir.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbFileDir_DragDrop);
            this.tbFileDir.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbFileDir_DragEnter);
            // 
            // btnCompileAll
            // 
            this.btnCompileAll.Location = new System.Drawing.Point(46, 601);
            this.btnCompileAll.Name = "btnCompileAll";
            this.btnCompileAll.Size = new System.Drawing.Size(300, 50);
            this.btnCompileAll.TabIndex = 0;
            this.btnCompileAll.Text = "编译此目录下的Excel";
            this.btnCompileAll.UseVisualStyleBackColor = true;
            this.btnCompileAll.Click += new System.EventHandler(this.btnCompileAll_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 525);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "编译这个目录下的Excel";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label3.Location = new System.Drawing.Point(320, 673);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(233, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "任务栏点击我选TableGUI，可查看输出日志";
            // 
            // btnUpdateCSSyntax
            // 
            this.btnUpdateCSSyntax.Location = new System.Drawing.Point(474, 36);
            this.btnUpdateCSSyntax.Name = "btnUpdateCSSyntax";
            this.btnUpdateCSSyntax.Size = new System.Drawing.Size(169, 40);
            this.btnUpdateCSSyntax.TabIndex = 0;
            this.btnUpdateCSSyntax.Text = "批量改表的前端字段类型";
            this.btnUpdateCSSyntax.UseVisualStyleBackColor = true;
            this.btnUpdateCSSyntax.Click += new System.EventHandler(this.btnUpdateCSSyntax_Click);
            // 
            // txtCodePath
            // 
            this.txtCodePath.Location = new System.Drawing.Point(474, 111);
            this.txtCodePath.Name = "txtCodePath";
            this.txtCodePath.Size = new System.Drawing.Size(398, 21);
            this.txtCodePath.TabIndex = 1;
            // 
            // btnSyncCode
            // 
            this.btnSyncCode.Location = new System.Drawing.Point(474, 148);
            this.btnSyncCode.Name = "btnSyncCode";
            this.btnSyncCode.Size = new System.Drawing.Size(169, 40);
            this.btnSyncCode.TabIndex = 0;
            this.btnSyncCode.Text = "生成代码同步到客户端";
            this.btnSyncCode.UseVisualStyleBackColor = true;
            this.btnSyncCode.Click += new System.EventHandler(this.btnSyncCode_Click);
            // 
            // btnSyncTml
            // 
            this.btnSyncTml.Location = new System.Drawing.Point(474, 257);
            this.btnSyncTml.Name = "btnSyncTml";
            this.btnSyncTml.Size = new System.Drawing.Size(169, 40);
            this.btnSyncTml.TabIndex = 0;
            this.btnSyncTml.Text = "编译后表同步到客户端";
            this.btnSyncTml.UseVisualStyleBackColor = true;
            this.btnSyncTml.Click += new System.EventHandler(this.btnSyncTml_Click);
            // 
            // txtTmlPath
            // 
            this.txtTmlPath.Location = new System.Drawing.Point(474, 220);
            this.txtTmlPath.Name = "txtTmlPath";
            this.txtTmlPath.Size = new System.Drawing.Size(398, 21);
            this.txtTmlPath.TabIndex = 1;
            // 
            // btnCheckNameRepet
            // 
            this.btnCheckNameRepet.Location = new System.Drawing.Point(660, 36);
            this.btnCheckNameRepet.Name = "btnCheckNameRepet";
            this.btnCheckNameRepet.Size = new System.Drawing.Size(169, 40);
            this.btnCheckNameRepet.TabIndex = 0;
            this.btnCheckNameRepet.Text = "检查前端字段名重复";
            this.btnCheckNameRepet.UseVisualStyleBackColor = true;
            this.btnCheckNameRepet.Click += new System.EventHandler(this.btnCheckNameRepet_Click);
            // 
            // btnOpenCodeDir
            // 
            this.btnOpenCodeDir.Location = new System.Drawing.Point(474, 443);
            this.btnOpenCodeDir.Name = "btnOpenCodeDir";
            this.btnOpenCodeDir.Size = new System.Drawing.Size(169, 40);
            this.btnOpenCodeDir.TabIndex = 0;
            this.btnOpenCodeDir.Text = "打开生成的代码目录";
            this.btnOpenCodeDir.UseVisualStyleBackColor = true;
            this.btnOpenCodeDir.Click += new System.EventHandler(this.btnOpenCodeDir_Click);
            // 
            // btnOpenTmlDir
            // 
            this.btnOpenTmlDir.Location = new System.Drawing.Point(660, 443);
            this.btnOpenTmlDir.Name = "btnOpenTmlDir";
            this.btnOpenTmlDir.Size = new System.Drawing.Size(169, 40);
            this.btnOpenTmlDir.TabIndex = 0;
            this.btnOpenTmlDir.Text = "打开编译后的表目录";
            this.btnOpenTmlDir.UseVisualStyleBackColor = true;
            this.btnOpenTmlDir.Click += new System.EventHandler(this.btnOpenTmlDir_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 697);
            this.Controls.Add(this.txtTmlPath);
            this.Controls.Add(this.btnOpenTmlDir);
            this.Controls.Add(this.btnOpenCodeDir);
            this.Controls.Add(this.btnSyncTml);
            this.Controls.Add(this.txtCodePath);
            this.Controls.Add(this.btnSyncCode);
            this.Controls.Add(this.btnCheckNameRepet);
            this.Controls.Add(this.btnUpdateCSSyntax);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbFileDir);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbFileList);
            this.Controls.Add(this.btnCompileAll);
            this.Controls.Add(this.btnCompileSelect);
            this.Name = "MainForm";
            this.Text = "Excel配置表编译 For C#";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCompileSelect;
        private System.Windows.Forms.TextBox tbFileList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbFileDir;
        private System.Windows.Forms.Button btnCompileAll;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnUpdateCSSyntax;
        private System.Windows.Forms.TextBox txtCodePath;
        private System.Windows.Forms.Button btnSyncCode;
        private System.Windows.Forms.Button btnSyncTml;
        private System.Windows.Forms.TextBox txtTmlPath;
        private System.Windows.Forms.Button btnCheckNameRepet;
        private System.Windows.Forms.Button btnOpenCodeDir;
        private System.Windows.Forms.Button btnOpenTmlDir;
    }
}

