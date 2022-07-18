
namespace WhatsAppBot
{
    partial class OpcoesForm
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
            this.x = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.intervaloMinimoNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.intervaloMaximoiNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.segundosDeProcuraNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.delCampoButton = new System.Windows.Forms.Button();
            this.addCompoButton = new System.Windows.Forms.Button();
            this.camposListBox = new System.Windows.Forms.ListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.campoComboBox = new System.Windows.Forms.ComboBox();
            this.delimitadorTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.diretorioArquivosButton = new System.Windows.Forms.Button();
            this.diretorioArquivosTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.x.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.intervaloMinimoNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.intervaloMaximoiNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.segundosDeProcuraNumericUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // x
            // 
            this.x.Controls.Add(this.label2);
            this.x.Controls.Add(this.intervaloMaximoiNumericUpDown);
            this.x.Controls.Add(this.label1);
            this.x.Controls.Add(this.intervaloMinimoNumericUpDown);
            this.x.Controls.Add(this.label3);
            this.x.Controls.Add(this.segundosDeProcuraNumericUpDown);
            this.x.Controls.Add(this.groupBox1);
            this.x.Dock = System.Windows.Forms.DockStyle.Fill;
            this.x.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.x.Location = new System.Drawing.Point(3, 3);
            this.x.Margin = new System.Windows.Forms.Padding(4);
            this.x.Name = "x";
            this.x.Size = new System.Drawing.Size(654, 524);
            this.x.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Intervalo Mínimo";
            // 
            // intervaloMinimoNumericUpDown
            // 
            this.intervaloMinimoNumericUpDown.Location = new System.Drawing.Point(6, 69);
            this.intervaloMinimoNumericUpDown.Margin = new System.Windows.Forms.Padding(6, 3, 3, 6);
            this.intervaloMinimoNumericUpDown.Name = "intervaloMinimoNumericUpDown";
            this.intervaloMinimoNumericUpDown.Size = new System.Drawing.Size(131, 24);
            this.intervaloMinimoNumericUpDown.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Intervalo Máximo";
            // 
            // intervaloMaximoiNumericUpDown
            // 
            this.intervaloMaximoiNumericUpDown.Location = new System.Drawing.Point(6, 21);
            this.intervaloMaximoiNumericUpDown.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.intervaloMaximoiNumericUpDown.Name = "intervaloMaximoiNumericUpDown";
            this.intervaloMaximoiNumericUpDown.Size = new System.Drawing.Size(131, 24);
            this.intervaloMaximoiNumericUpDown.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "Segundos de procura";
            // 
            // segundosDeProcuraNumericUpDown
            // 
            this.segundosDeProcuraNumericUpDown.Location = new System.Drawing.Point(6, 120);
            this.segundosDeProcuraNumericUpDown.Margin = new System.Windows.Forms.Padding(6, 3, 3, 6);
            this.segundosDeProcuraNumericUpDown.Name = "segundosDeProcuraNumericUpDown";
            this.segundosDeProcuraNumericUpDown.Size = new System.Drawing.Size(131, 24);
            this.segundosDeProcuraNumericUpDown.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox1.Controls.Add(this.delCampoButton);
            this.groupBox1.Controls.Add(this.addCompoButton);
            this.groupBox1.Controls.Add(this.camposListBox);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.campoComboBox);
            this.groupBox1.Controls.Add(this.delimitadorTextBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.diretorioArquivosButton);
            this.groupBox1.Controls.Add(this.diretorioArquivosTextBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(3, 153);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(646, 309);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Buscar arquivos";
            // 
            // delCampoButton
            // 
            this.delCampoButton.Enabled = false;
            this.delCampoButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.delCampoButton.Location = new System.Drawing.Point(296, 153);
            this.delCampoButton.Name = "delCampoButton";
            this.delCampoButton.Size = new System.Drawing.Size(49, 26);
            this.delCampoButton.TabIndex = 12;
            this.delCampoButton.Text = "Del";
            this.delCampoButton.UseVisualStyleBackColor = true;
            this.delCampoButton.Click += new System.EventHandler(this.delCampoButton_Click);
            // 
            // addCompoButton
            // 
            this.addCompoButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addCompoButton.Location = new System.Drawing.Point(241, 153);
            this.addCompoButton.Name = "addCompoButton";
            this.addCompoButton.Size = new System.Drawing.Size(49, 26);
            this.addCompoButton.TabIndex = 11;
            this.addCompoButton.Text = "Add";
            this.addCompoButton.UseVisualStyleBackColor = true;
            this.addCompoButton.Click += new System.EventHandler(this.addCompoButton_Click);
            // 
            // camposListBox
            // 
            this.camposListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.camposListBox.FormattingEnabled = true;
            this.camposListBox.ItemHeight = 18;
            this.camposListBox.Location = new System.Drawing.Point(12, 185);
            this.camposListBox.Name = "camposListBox";
            this.camposListBox.Size = new System.Drawing.Size(333, 112);
            this.camposListBox.TabIndex = 10;
            this.camposListBox.SelectedIndexChanged += new System.EventHandler(this.camposListBox_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(9, 132);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 18);
            this.label7.TabIndex = 9;
            this.label7.Text = "Campo";
            // 
            // campoComboBox
            // 
            this.campoComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.campoComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.campoComboBox.FormattingEnabled = true;
            this.campoComboBox.Items.AddRange(new object[] {
            "Cpf",
            "Nome",
            "Telefone",
            "NADA"});
            this.campoComboBox.Location = new System.Drawing.Point(12, 153);
            this.campoComboBox.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.campoComboBox.Name = "campoComboBox";
            this.campoComboBox.Size = new System.Drawing.Size(223, 26);
            this.campoComboBox.TabIndex = 8;
            // 
            // delimitadorTextBox
            // 
            this.delimitadorTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.delimitadorTextBox.Location = new System.Drawing.Point(12, 98);
            this.delimitadorTextBox.Name = "delimitadorTextBox";
            this.delimitadorTextBox.Size = new System.Drawing.Size(87, 24);
            this.delimitadorTextBox.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(9, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(182, 18);
            this.label6.TabIndex = 3;
            this.label6.Text = "Delimitardor nome arquivo";
            // 
            // diretorioArquivosButton
            // 
            this.diretorioArquivosButton.Location = new System.Drawing.Point(384, 50);
            this.diretorioArquivosButton.Name = "diretorioArquivosButton";
            this.diretorioArquivosButton.Size = new System.Drawing.Size(49, 24);
            this.diretorioArquivosButton.TabIndex = 2;
            this.diretorioArquivosButton.Text = "📁";
            this.diretorioArquivosButton.UseVisualStyleBackColor = true;
            this.diretorioArquivosButton.Click += new System.EventHandler(this.diretorioArquivosButton_Click);
            // 
            // diretorioArquivosTextBox
            // 
            this.diretorioArquivosTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.diretorioArquivosTextBox.Location = new System.Drawing.Point(12, 50);
            this.diretorioArquivosTextBox.Name = "diretorioArquivosTextBox";
            this.diretorioArquivosTextBox.Size = new System.Drawing.Size(366, 24);
            this.diretorioArquivosTextBox.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(9, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 18);
            this.label5.TabIndex = 0;
            this.label5.Text = "Diretório";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(668, 561);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.x);
            this.tabPage1.Location = new System.Drawing.Point(4, 27);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(660, 530);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Configurações";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // OpcoesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 561);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "OpcoesForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Opções";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OpcoesForm_FormClosing);
            this.x.ResumeLayout(false);
            this.x.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.intervaloMinimoNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.intervaloMaximoiNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.segundosDeProcuraNumericUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel x;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown intervaloMinimoNumericUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown intervaloMaximoiNumericUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown segundosDeProcuraNumericUpDown;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button diretorioArquivosButton;
        private System.Windows.Forms.TextBox diretorioArquivosTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox delimitadorTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox campoComboBox;
        private System.Windows.Forms.Button delCampoButton;
        private System.Windows.Forms.Button addCompoButton;
        private System.Windows.Forms.ListBox camposListBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
    }
}