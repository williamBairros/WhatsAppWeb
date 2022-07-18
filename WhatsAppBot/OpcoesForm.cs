using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WhatsAppBot.Enuns;

namespace WhatsAppBot
{
    public partial class OpcoesForm : Form
    {
        public OpcoesForm()
        {
            InitializeComponent();          
            campoComboBox.SelectedIndex = 0;
            CarregarConfiguracao();
        }

        private void diretorioArquivosButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ofd = new FolderBrowserDialog())
                {
                    ofd.Description = "Selecionar diretorio de arquivos";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        diretorioArquivosTextBox.Text = ofd.SelectedPath;
                    }
                }
            }
            catch (Exception ex) 
            {
                ExibirExcecao(ex);
            }
        }

        private void ExibirExcecao(Exception ex)
        {
            MessageBox.Show(this, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void addCompoButton_Click(object sender, EventArgs e)
        {
            try
            {
                camposListBox.Items.Add(campoComboBox.SelectedItem);
            }
            catch (Exception ex)
            {
                ExibirExcecao(ex);
            }
        }

        private void camposListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                delCampoButton.Enabled = camposListBox.SelectedItem != null;
            }
            catch (Exception ex)
            {
                ExibirExcecao(ex);
            }
        }

        private void delCampoButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (camposListBox.SelectedItem != null) 
                {
                    camposListBox.Items.RemoveAt(camposListBox.SelectedIndex);
                }
            }
            catch (Exception ex)
            {
                ExibirExcecao(ex);
            }
        }

        public void SalvarConfiguracao() 
        {
            var config = new Config();
            config.IntervaloMax = (int)intervaloMaximoiNumericUpDown.Value;
            config.IntervaloMin = (int)intervaloMinimoNumericUpDown.Value;
            config.SegundosDeProcura = (int)segundosDeProcuraNumericUpDown.Value;
            config.BuscarArquivos = new BuscarArquivos();
            config.BuscarArquivos.Delimitador = delimitadorTextBox.Text;
            config.BuscarArquivos.DiretorioArquivos = diretorioArquivosTextBox.Text;
            config.BuscarArquivos.Campos = new Dictionary<int, string>();
            var campos = camposListBox.Items.Cast<string>().ToArray();
            for (var i = 0; i < campos.Length; i++) 
            {
                config.BuscarArquivos.Campos.Add(i, campos[i]);
            }

            var jo = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText("config.json", jo);
        }

        public void CarregarConfiguracao() 
        {
            if (File.Exists("config.json"))
            {
                var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
                intervaloMaximoiNumericUpDown.Value = config.IntervaloMax;
                intervaloMinimoNumericUpDown.Value = config.IntervaloMin;
                segundosDeProcuraNumericUpDown.Value = config.SegundosDeProcura;
                delimitadorTextBox.Text = config?.BuscarArquivos?.Delimitador;
                diretorioArquivosTextBox.Text = config?.BuscarArquivos?.DiretorioArquivos;
                config?.BuscarArquivos?.Campos?.Values?.ToList().ForEach(c => camposListBox.Items.Add(c));
            }
        }

        private void OpcoesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                SalvarConfiguracao();
            }
            catch (Exception ex)
            {
                ExibirExcecao(ex);
            }
        }
    }
}
