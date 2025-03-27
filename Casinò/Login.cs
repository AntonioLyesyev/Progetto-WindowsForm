using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace Casinò
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            textBoxPassword.UseSystemPasswordChar = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
        public class Credenziali
        {
            public string nome { get; set; }
            public string password { get; set; }
            public long saldo { get; set; }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1.Visible = true;
            linkLabel2.Visible = false;
            RegistraButton.Visible = false;
            AccediButton.Visible = true;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1.Visible = false;
            linkLabel2.Visible = true;
            AccediButton.Visible = false;
            RegistraButton.Visible = true;
        }

        private void esci_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = !textBoxPassword.UseSystemPasswordChar;
        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            string password = textBoxPassword.Text;
            string[] caratteriConsentiti = { "!", "?", "@", "-", "_", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0",
                                             "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
                                             "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

            foreach (var carattere in password)
            {
                if (!caratteriConsentiti.Contains(carattere.ToString()))
                {
                    labelErrorePassw.Visible = true;
                    RegistraButton.Enabled = false;
                    return;
                }
            }

            labelErrorePassw.Visible = false;
            RegistraButton.Enabled = true;
        }

        private void textBoxNome_TextChanged(object sender, EventArgs e)
        {
            string NomeUtente = textBoxNome.Text;
            string[] caratteriConsentiti = { "!", "?", "@", "-", "_", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0",
                                             "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
                                             "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

            foreach (var carattere in NomeUtente)
            {
                if (!caratteriConsentiti.Contains(carattere.ToString()))
                {
                    labelErroreNome.Visible = true;
                    RegistraButton.Enabled = false;
                    return;
                }
            }

            labelErroreNome.Visible = false;
            RegistraButton.Enabled = true;
        }

        private void RegistraButton_Click(object sender, EventArgs e)
        {
            string nome = textBoxNome.Text, password = textBoxPassword.Text;
            Registrazione(nome, password);
        }

        private void AccediButton_Click(object sender, EventArgs e)
        {
            if (labelErroreNome.Visible || labelErrorePassw.Visible)
            {
                MessageBox.Show("Una o più delle credenziali inserite non è valida");
            }
            else
            {
                string nome = textBoxNome.Text, password = textBoxPassword.Text;
                ReadDati(nome, password);
            }
        }

        private void Registrazione(string nomeUtente, string passwordUtente)
        {
            string DatiEsistenti = File.ReadAllText("Credenziali.json");
            List<Credenziali> listaDati = JsonSerializer.Deserialize<List<Credenziali>>(DatiEsistenti) ?? new List<Credenziali>();

            foreach (Credenziali utente in listaDati)
            {
                if (utente.nome == nomeUtente)
                {
                    MessageBox.Show("Account o Nome Utente già esistente");
                    return;
                }
            }

            Credenziali nuovoUtente = new Credenziali
            {
                nome = nomeUtente,
                password = passwordUtente,
                saldo = 0
            };
            listaDati.Add(nuovoUtente);

            string DatiRegistrati = JsonSerializer.Serialize(listaDati);
            File.WriteAllText("Credenziali.json", DatiRegistrati);

            MessageBox.Show("Registrazione completata!");
        }

        private void ReadDati(string nomeUtente, string passwordUtente)
        {
            string DatiEsistenti = File.ReadAllText("Credenziali.json");
            List<Credenziali> listaDati = JsonSerializer.Deserialize<List<Credenziali>>(DatiEsistenti);

            bool accessoTrovato = false;

            foreach (Credenziali utente in listaDati)
            {
                if (utente.nome == nomeUtente && utente.password == passwordUtente)
                {
                    MessageBox.Show("Accesso effettuato");
                    this.Hide();
                    Home formHome = new Home(nomeUtente);
                    formHome.Show();
                    accessoTrovato = true;
                    break;
                }
            }

            if (!accessoTrovato)
            {
                MessageBox.Show("Accesso negato");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string admin = "ADMIN";
            this.Hide();
            Home formHome = new Home(admin);
            formHome.Show();
        }
    }
}

    
