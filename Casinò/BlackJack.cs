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
using System.Dynamic;

namespace Casinò
{

    public partial class BlackJack : Form
    {
        String nomeUtente;
        private Random random;
        private Giocata nuovaGiocata;
        int puntataAttuale = 0;
        public BlackJack(string nomeUtent)
        {
            InitializeComponent();
            nomeUtente = nomeUtent;
            random = new Random();
            button2.Text = "INIZIA";
            button1.Visible = false;
            CaricaDati();
            
            

        }

        private void BlackJack_Load(object sender, EventArgs e)
        {
            
        }
        
        public class Credenziali
        {
            public string nome { get; set; }
            public string password { get; set; }
            public long saldo { get; set; }
        }
        public class Giocata
        {
            public int seme {  get; set; }
            public int numero { get; set; }
            public int conta { get; set; }

            public List<int> mazzoSeme { get; set; }
            public List<int> mazzoNumero { get; set; }
        }
        

        private void buttonEsci_Click(object sender, EventArgs e)
        {
            this.Close();
            Home formHome= new Home(nomeUtente);
            formHome.Show();
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

            
            label5.Text = utenteTrovato.saldo.ToString("C");
            
        }
        private void aggiungiPuntata(int Puntata)
        {
            string Datinonserializzati = File.ReadAllText("Credenziali.json");
            List<Credenziali> listaDati = JsonSerializer.Deserialize<List<Credenziali>>(Datinonserializzati);

            for (int i = 0; i < listaDati.Count; i++)
            {
                if (listaDati[i].nome == nomeUtente)
                {
                    if (button3.Text == "RIMUOVI")
                    {
                        if (puntataAttuale > 0)
                        {
                            puntataAttuale -= Puntata;
                            listaDati[i].saldo += Puntata;
                        }
                        else MessageBox.Show("Puntata vuota!");
                    }
                    else
                    {
                        if (listaDati[i].saldo >= Puntata)
                        {
                            puntataAttuale += Puntata;
                            listaDati[i].saldo -= Puntata;
                        }
                        else
                        {
                            MessageBox.Show("Saldo insufficiente per questa puntata!");
                            return;
                        }
                    }
                    break;
                }
            }

            label6.Text = puntataAttuale.ToString("C");
            label5.Text = listaDati.First(u => u.nome == nomeUtente).saldo.ToString("C");
            string DatiRegistrati = JsonSerializer.Serialize(listaDati);
            File.WriteAllText("Credenziali.json", DatiRegistrati);
        }



        private void IniziaPartita()
        {
            button2.Text = "CARTA";
            button1.Visible = true;
            nuovaGiocata = new Giocata();
            nuovaGiocata.conta = 0;
            nuovaGiocata.mazzoSeme = new List<int>();
            nuovaGiocata.mazzoNumero = new List<int>();

            for (int i = 0; i < 2; i++)
            {
                bool posizioneLibera = false;
                while (posizioneLibera == false)
                {
                    nuovaGiocata.seme = random.Next(1, 5);
                    nuovaGiocata.numero = random.Next(2, 11);

                    bool trovataPosizioneUguale = false;
                    for (int j = 0; j < nuovaGiocata.mazzoSeme.Count; j++)
                    {
                        if (nuovaGiocata.seme == nuovaGiocata.mazzoSeme[j] && nuovaGiocata.numero == nuovaGiocata.mazzoNumero[j])
                        {
                            trovataPosizioneUguale = true;
                            break;
                        }
                    }

                    if (trovataPosizioneUguale == false)
                    {
                        nuovaGiocata.mazzoSeme.Add(nuovaGiocata.seme);
                        nuovaGiocata.mazzoNumero.Add(nuovaGiocata.numero);
                        posizioneLibera = true;
                        nuovaGiocata.conta += nuovaGiocata.numero;
                        label2.Text = nuovaGiocata.conta.ToString();
                    }
                }
            }
        }



        private void AggiungiCarta()
        {
            bool posizioneLibera = false;
            while (posizioneLibera == false)
            {
                nuovaGiocata.seme = random.Next(1, 5);
                nuovaGiocata.numero = random.Next(2, 11);

                bool trovataPosizioneUguale = false;
                for (int j = 0; j < nuovaGiocata.mazzoSeme.Count; j++)
                {
                    if (nuovaGiocata.seme == nuovaGiocata.mazzoSeme[j] && nuovaGiocata.numero == nuovaGiocata.mazzoNumero[j])
                    {
                        trovataPosizioneUguale = true;
                        break;
                    }
                }

                if (trovataPosizioneUguale == false)
                {
                    nuovaGiocata.mazzoSeme.Add(nuovaGiocata.seme);
                    nuovaGiocata.mazzoNumero.Add(nuovaGiocata.numero);
                    posizioneLibera = true;
                    nuovaGiocata.conta += nuovaGiocata.numero;
                    label2.Text = nuovaGiocata.conta.ToString();
                }
            }

            if (nuovaGiocata.conta > 21)
            {
                if (nuovaGiocata.mazzoNumero.Contains(11) == true)
                {
                    nuovaGiocata.conta -= 10;
                    label2.Text = nuovaGiocata.conta.ToString();
                }
                else
                {
                    MessageBox.Show("Hai sforato! HAI PERSO!");
                    puntataAttuale = 0;
                    label6.Text = puntataAttuale.ToString("C");
                    button2.Text = "INIZIA";
                    button1.Visible = false;
                    label2.Text = "";
                    pictureBox1.Enabled = true;
                }
               
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "INIZIA")
            {
                if (puntataAttuale == 0) {
                    MessageBox.Show("Per favore fare una puntata!");
                }
                else { 
                    IniziaPartita();
                    pictureBox1.Enabled = false;
                }
            }
            else
            {
                if (puntataAttuale == 0)
                {
                    MessageBox.Show("Per favore fare una puntata");
                }
                else
                {
                    AggiungiCarta();
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
            int Puntata = 1;
            aggiungiPuntata(Puntata);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            int Puntata = 5;
            aggiungiPuntata(Puntata);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            int Puntata = 20;
            aggiungiPuntata(Puntata);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            int Puntata = 50;
            aggiungiPuntata(Puntata);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            int Puntata = 100;
            aggiungiPuntata(Puntata);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "AGGIUNGI") {
                button3.Text = "RIMUOVI";
            } 
            else if (button3.Text == "RIMUOVI"){
                button3.Text = "AGGIUNGI";
            }
        }
    }
}
