using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Globalization;

namespace Casinò
{
    public partial class Home : Form
    {
        String nomeUtente;
        public Home(string nomeUtent)
        {
            InitializeComponent();
            nomeUtente = nomeUtent;
            CaricaDati();
           
        }
        public class Credenziali
        {
            public string nome { get; set; }
            public string password { get; set; }
            public long saldo { get; set; }
        }
        private void Home_Load(object sender, EventArgs e)
        {

        }
        private void CaricaDati()
        {
            string Datinonserializzati = File.ReadAllText("Credenziali.json");
            List<Credenziali> listaDati = JsonSerializer.Deserialize<List<Credenziali>>(Datinonserializzati);

            
            Credenziali utenteTrovato = new Credenziali();

            foreach (Credenziali utente in listaDati)
            {
                if (utente.nome == nomeUtente)
                {
                    utenteTrovato = utente;
                    break;
                }
            }

            labelNomeUtente.Text = utenteTrovato.nome;
            labelSaldo.Text = utenteTrovato.saldo.ToString("C");
            
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Close();
            Login formLogin = new Login ();
            formLogin.Show();
        }

        private void buttonRicarica_Click(object sender, EventArgs e)
        {

            string Datinonserializzati = File.ReadAllText("Credenziali.json");
            List<Credenziali> listaDati = JsonSerializer.Deserialize<List<Credenziali>>(Datinonserializzati);


            Credenziali Utente = new Credenziali();

            foreach (Credenziali utente in listaDati)
            {
                if (utente.nome == nomeUtente)
                {
                    Utente = utente;
                    break;
                }
            }
            long importo;
            long.TryParse(textBox1.Text, out importo);
                Utente.saldo += importo; 
                labelSaldo.Text = Utente.saldo.ToString("C");
                string DatiRegistrati = JsonSerializer.Serialize(listaDati);
            File.WriteAllText("Credenziali.json", DatiRegistrati);
            textBox1.Clear();
        }

        private void buttonPreleva_Click(object sender, EventArgs e)
        {
            string Datinonserializzati = File.ReadAllText("Credenziali.json");
            List<Credenziali> listaDati = JsonSerializer.Deserialize<List<Credenziali>>(Datinonserializzati);


            Credenziali Utente = new Credenziali();

            foreach (Credenziali utente in listaDati)
            {
                if (utente.nome == nomeUtente)
                {
                    Utente = utente;
                    break;
                }
            }
            long importo;
            long.TryParse(textBox2.Text, out importo);
            if(importo>Utente.saldo)
            {
                MessageBox.Show("Saldo insufficiente");
            }
            else { 
            Utente.saldo -= importo;
            labelSaldo.Text = Utente.saldo.ToString("C");
            string DatiRegistrati = JsonSerializer.Serialize(listaDati);
            File.WriteAllText("Credenziali.json", DatiRegistrati);
            textBox2.Clear();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            BlackJack FormBlackJack = new BlackJack(nomeUtente);
            FormBlackJack.Show();
        }
    }
}

