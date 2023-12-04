
namespace WhatsAppBot
{
    partial class WhatsAppBot
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WhatsAppBot));
            this.contatosDataGridView = new System.Windows.Forms.DataGridView();
            this.cpfColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nomeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.telefoneColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MensagemEnviadaColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ArquivoEnviadoColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ContatoEncontradoColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contatosGridViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.executarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pararExecuçãoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gerarArquivoVcfToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reiniciarArquivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.arquivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.carregarContatosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.opçõesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.contatosDataGridView)).BeginInit();
            this.contatosGridViewContextMenuStrip.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contatosDataGridView
            // 
            this.contatosDataGridView.AllowUserToAddRows = false;
            this.contatosDataGridView.AllowUserToDeleteRows = false;
            this.contatosDataGridView.AllowUserToOrderColumns = true;
            this.contatosDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.contatosDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cpfColumn,
            this.nomeColumn,
            this.telefoneColumn,
            this.MensagemEnviadaColumn,
            this.ArquivoEnviadoColumn,
            this.ContatoEncontradoColumn});
            this.contatosDataGridView.ContextMenuStrip = this.contatosGridViewContextMenuStrip;
            this.contatosDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contatosDataGridView.Location = new System.Drawing.Point(0, 24);
            this.contatosDataGridView.Name = "contatosDataGridView";
            this.contatosDataGridView.ReadOnly = true;
            this.contatosDataGridView.Size = new System.Drawing.Size(585, 368);
            this.contatosDataGridView.TabIndex = 0;
            this.contatosDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.contatosDataGridView_CellClick);
            this.contatosDataGridView.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.contatosDataGridView_CellEnter);
            this.contatosDataGridView.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.contatosDataGridView_CellLeave);
            this.contatosDataGridView.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.contatosDataGridView_RowEnter);
            this.contatosDataGridView.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.contatosDataGridView_RowLeave);
            // 
            // cpfColumn
            // 
            this.cpfColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.cpfColumn.HeaderText = "Cpf";
            this.cpfColumn.Name = "cpfColumn";
            this.cpfColumn.ReadOnly = true;
            this.cpfColumn.Width = 56;
            // 
            // nomeColumn
            // 
            this.nomeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.nomeColumn.HeaderText = "Nome";
            this.nomeColumn.Name = "nomeColumn";
            this.nomeColumn.ReadOnly = true;
            this.nomeColumn.Width = 74;
            // 
            // telefoneColumn
            // 
            this.telefoneColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.telefoneColumn.HeaderText = "Telefone";
            this.telefoneColumn.Name = "telefoneColumn";
            this.telefoneColumn.ReadOnly = true;
            this.telefoneColumn.Width = 90;
            // 
            // MensagemEnviadaColumn
            // 
            this.MensagemEnviadaColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.MensagemEnviadaColumn.HeaderText = "MensagemEnviada";
            this.MensagemEnviadaColumn.Name = "MensagemEnviadaColumn";
            this.MensagemEnviadaColumn.ReadOnly = true;
            this.MensagemEnviadaColumn.Width = 140;
            // 
            // ArquivoEnviadoColumn
            // 
            this.ArquivoEnviadoColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ArquivoEnviadoColumn.HeaderText = "ArquivoEnviado";
            this.ArquivoEnviadoColumn.Name = "ArquivoEnviadoColumn";
            this.ArquivoEnviadoColumn.ReadOnly = true;
            this.ArquivoEnviadoColumn.Width = 116;
            // 
            // ContatoEncontradoColumn
            // 
            this.ContatoEncontradoColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ContatoEncontradoColumn.HeaderText = "ContatoEncontrado";
            this.ContatoEncontradoColumn.Name = "ContatoEncontradoColumn";
            this.ContatoEncontradoColumn.ReadOnly = true;
            this.ContatoEncontradoColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ContatoEncontradoColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ContatoEncontradoColumn.Width = 144;
            // 
            // contatosGridViewContextMenuStrip
            // 
            this.contatosGridViewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.executarToolStripMenuItem,
            this.pararExecuçãoToolStripMenuItem,
            this.gerarArquivoVcfToolStripMenuItem,
            this.reiniciarArquivoToolStripMenuItem});
            this.contatosGridViewContextMenuStrip.Name = "contextMenuStrip1";
            this.contatosGridViewContextMenuStrip.Size = new System.Drawing.Size(167, 92);
            // 
            // executarToolStripMenuItem
            // 
            this.executarToolStripMenuItem.Name = "executarToolStripMenuItem";
            this.executarToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.executarToolStripMenuItem.Text = "Iniciar execução";
            this.executarToolStripMenuItem.Click += new System.EventHandler(this.executarToolStripMenuItem_Click);
            // 
            // pararExecuçãoToolStripMenuItem
            // 
            this.pararExecuçãoToolStripMenuItem.Enabled = false;
            this.pararExecuçãoToolStripMenuItem.Name = "pararExecuçãoToolStripMenuItem";
            this.pararExecuçãoToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.pararExecuçãoToolStripMenuItem.Text = "Parar execução";
            this.pararExecuçãoToolStripMenuItem.Click += new System.EventHandler(this.pararExecucãoToolStripMenuItem_Click);
            // 
            // gerarArquivoVcfToolStripMenuItem
            // 
            this.gerarArquivoVcfToolStripMenuItem.Name = "gerarArquivoVcfToolStripMenuItem";
            this.gerarArquivoVcfToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.gerarArquivoVcfToolStripMenuItem.Text = "Gerar Arquivo Vcf";
            this.gerarArquivoVcfToolStripMenuItem.Click += new System.EventHandler(this.gerarArquivoVcfToolStripMenuItem_Click);
            // 
            // reiniciarArquivoToolStripMenuItem
            // 
            this.reiniciarArquivoToolStripMenuItem.Name = "reiniciarArquivoToolStripMenuItem";
            this.reiniciarArquivoToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.reiniciarArquivoToolStripMenuItem.Text = "Reiniciar arquivo";
            this.reiniciarArquivoToolStripMenuItem.Click += new System.EventHandler(this.reiniciarArquivoToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.arquivoToolStripMenuItem,
            this.opçõesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(585, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // arquivoToolStripMenuItem
            // 
            this.arquivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.carregarContatosToolStripMenuItem});
            this.arquivoToolStripMenuItem.Name = "arquivoToolStripMenuItem";
            this.arquivoToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.arquivoToolStripMenuItem.Text = "📁 Arquivos";
            // 
            // carregarContatosToolStripMenuItem
            // 
            this.carregarContatosToolStripMenuItem.Name = "carregarContatosToolStripMenuItem";
            this.carregarContatosToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.carregarContatosToolStripMenuItem.Text = "📝 Carregar contatos";
            this.carregarContatosToolStripMenuItem.Click += new System.EventHandler(this.carregarContatosToolStripMenuItem_Click);
            // 
            // opçõesToolStripMenuItem
            // 
            this.opçõesToolStripMenuItem.Name = "opçõesToolStripMenuItem";
            this.opçõesToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.opçõesToolStripMenuItem.Text = "⚙️ Opções";
            this.opçõesToolStripMenuItem.Click += new System.EventHandler(this.opcõesToolStripMenuItem_Click);
            // 
            // WhatsAppBot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 392);
            this.Controls.Add(this.contatosDataGridView);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "WhatsAppBot";
            this.Text = "WhatsApp Bot";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WhatsAppBot_FormClosing);
            this.Load += new System.EventHandler(this.WhatsAppBot_Load);
            ((System.ComponentModel.ISupportInitialize)(this.contatosDataGridView)).EndInit();
            this.contatosGridViewContextMenuStrip.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView contatosDataGridView;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem arquivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem carregarContatosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem opçõesToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contatosGridViewContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem executarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gerarArquivoVcfToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pararExecuçãoToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn cpfColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nomeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn telefoneColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn MensagemEnviadaColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ArquivoEnviadoColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContatoEncontradoColumn;
        private System.Windows.Forms.ToolStripMenuItem reiniciarArquivoToolStripMenuItem;
    }
}

