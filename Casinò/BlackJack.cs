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
        int punteggioDealer = 0;

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
            public int seme { get; set; }
            public int numero { get; set; }
            public int conta { get; set; }

            public List<int> mazzoSeme { get; set; }
            public List<int> mazzoNumero { get; set; }
        }

        private void buttonEsci_Click(object sender, EventArgs e)
        {
            this.Close();
            Home formHome = new Home(nomeUtente);
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
            CartaUtente.Visible = true;

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
            AssegnazioneCarte(nuovaGiocata.numero, nuovaGiocata.seme);
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
                if (nuovaGiocata.mazzoNumero.Contains(11))
                {
                    nuovaGiocata.conta -= 10;
                    label2.Text = nuovaGiocata.conta.ToString();
                }
                else
                {
                    CartaUtente.Image = null;
                    AssegnazioneCarte(nuovaGiocata.numero, nuovaGiocata.seme);
                    MessageBox.Show("Hai sforato! HAI PERSO!");
                    puntataAttuale = 0;
                    label6.Text = puntataAttuale.ToString("C");
                    button2.Text = "INIZIA";
                    button1.Visible = false;
                    label2.Text = "";
                    pictureBox1.Enabled = true;
                    CartaUtente.Visible = false;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "INIZIA")
            {
                if (puntataAttuale == 0)
                {
                    MessageBox.Show("Per favore fare una puntata!");
                }
                else
                {
                    IniziaPartita();
                    pictureBox1.Enabled = false;
                    pictureBox2.Enabled = false;
                    pictureBox3.Enabled = false;
                    pictureBox4.Enabled = false;
                    pictureBox5.Enabled = false;
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
            if (button3.Text == "AGGIUNGI")
            {
                button3.Text = "RIMUOVI";
            }
            else if (button3.Text == "RIMUOVI")
            {
                button3.Text = "AGGIUNGI";
            }
        }

        private void AssegnazioneCarte(int numero, int seme)
        {
            List<string> Semi = new List<string> { "cuori", "quadri", "fiori", "picche" };
            List<string> Numeri = new List<string> { "due", "tre", "quattro", "cinque", "sei", "sette", "otto", "nove", "dieci", "j", "q", "k", "asso" };

            string numeroScelto;

            if (numero >= 2 && numero <= 9)
            {
                numeroScelto = Numeri[numero - 2];
            }
            else if (numero == 10)
            {
                Random random = new Random();
                int sceltaCasuale = random.Next(9, 13);
                numeroScelto = Numeri[sceltaCasuale];
            }
            else if (numero == 11)
            {
                numeroScelto = "asso";
            }
            else
            {
                numeroScelto = "errore";
            }

            string semeScelto = Semi[seme - 1];

            string percorsoImmagine = $"Carte/{semeScelto}/{numeroScelto}_{semeScelto}.jpg";

            if (File.Exists(percorsoImmagine))
            {
                CartaUtente.SizeMode = PictureBoxSizeMode.StretchImage;
                CartaUtente.ImageLocation = percorsoImmagine;
            }
            else
            {
                MessageBox.Show("Immagine non trovata: " + percorsoImmagine);
            }
        }

        private void GeneraCarteDealer()
        {
            int sommaPuntiDealer = 0;
            List<int> mazzoSemeDealer = new List<int>();
            List<int> mazzoNumeroDealer = new List<int>();

            
            for (int i = 0; i < 2; i++)
            {
                bool posizioneLibera = false;
                while (posizioneLibera == false)
                {
                    int semeDealer = random.Next(1, 5);
                    int numeroDealer = random.Next(2, 11);

                    bool trovataPosizioneUguale = false;
                    for (int j = 0; j < mazzoSemeDealer.Count; j++)
                    {
                        if (semeDealer == mazzoSemeDealer[j] && numeroDealer == mazzoNumeroDealer[j])
                        {
                            trovataPosizioneUguale = true;
                            break;
                        }
                    }

                    if (trovataPosizioneUguale == false)
                    {
                        mazzoSemeDealer.Add(semeDealer);
                        mazzoNumeroDealer.Add(numeroDealer);
                        sommaPuntiDealer += numeroDealer;
                        posizioneLibera = true;
                        AssegnazioneCarteDealer(numeroDealer, semeDealer);
                    }
                }
            }

            
            if (sommaPuntiDealer > 21)
            {
                MessageBox.Show("Il dealer ha sforato! Hai vinto!");
                label3.Text = $"Punti Dealer: {sommaPuntiDealer}";  
                CartaDealer.Visible = false;  
                CartaDealer.Image = null;    
                return;
            }

            
            while (sommaPuntiDealer < 17)
            {
                bool posizioneLibera = false;
                while (posizioneLibera == false)
                {
                    int semeDealer = random.Next(1, 5);
                    int numeroDealer = random.Next(2, 11);

                    bool trovataPosizioneUguale = false;
                    for (int j = 0; j < mazzoSemeDealer.Count; j++)
                    {
                        if (semeDealer == mazzoSemeDealer[j] && numeroDealer == mazzoNumeroDealer[j])
                        {
                            trovataPosizioneUguale = true;
                            break;
                        }
                    }

                    if (trovataPosizioneUguale == false)
                    {
                        mazzoSemeDealer.Add(semeDealer);
                        mazzoNumeroDealer.Add(numeroDealer);
                        sommaPuntiDealer += numeroDealer;
                        posizioneLibera = true;
                        AssegnazioneCarteDealer(numeroDealer, semeDealer);
                    }
                }

                
                if (sommaPuntiDealer > 21)
                {
                    MessageBox.Show("Il dealer ha sforato! Hai vinto!");
                    label3.Text = $"Punti Dealer: {sommaPuntiDealer}";  
                    CartaDealer.Visible = false;  
                    CartaDealer.Image = null;    
                    return;
                }
            }

            
            label3.Text = $"Punti Dealer: {sommaPuntiDealer}";  
            MessageBox.Show($"Il dealer ha {sommaPuntiDealer} punti.");
            VerificaVittoria(sommaPuntiDealer);
        }

        private void AssegnazioneCarteDealer(int numero, int seme)
        {
            List<string> Semi = new List<string> { "cuori", "quadri", "fiori", "picche" };
            List<string> Numeri = new List<string> { "due", "tre", "quattro", "cinque", "sei", "sette", "otto", "nove", "dieci", "j", "q", "k", "asso" };

            string numeroScelto;

            if (numero >= 2 && numero <= 9)
            {
                numeroScelto = Numeri[numero - 2];
            }
            else if (numero == 10)
            {
                Random random = new Random();
                int sceltaCasuale = random.Next(9, 13);
                numeroScelto = Numeri[sceltaCasuale];
            }
            else if (numero == 11)
            {
                numeroScelto = "asso";
            }
            else
            {
                numeroScelto = "errore";
            }

            string semeScelto = Semi[seme - 1];

            string percorsoImmagine = $"Carte/{semeScelto}/{numeroScelto}_{semeScelto}.jpg";

            if (File.Exists(percorsoImmagine))
            {
                CartaDealer.SizeMode = PictureBoxSizeMode.StretchImage;
                CartaDealer.ImageLocation = percorsoImmagine;
            }
            else
            {
                MessageBox.Show("Immagine non trovata: " + percorsoImmagine);
            }
        }

        private void VerificaVittoria(int sommaPuntiDealer)
        {
            string Datinonserializzati = File.ReadAllText("Credenziali.json");
            List<Credenziali> listaDati = JsonSerializer.Deserialize<List<Credenziali>>(Datinonserializzati);

            Credenziali utente = listaDati.FirstOrDefault(u => u.nome == nomeUtente);
            if (utente == null)
            {
                MessageBox.Show("Errore: utente non trovato.");
                return;
            }

            if (nuovaGiocata.conta > sommaPuntiDealer || sommaPuntiDealer > 21)
            {
                MessageBox.Show("Hai vinto!");
                utente.saldo += puntataAttuale * 2;
                label5.Text = utente.saldo.ToString("C");
            }
            else if (nuovaGiocata.conta == sommaPuntiDealer)
            {
                MessageBox.Show("Pareggio!");
                utente.saldo += puntataAttuale;
                label5.Text = utente.saldo.ToString("C");
            }
            else
            {
                MessageBox.Show("Hai perso!");
            }

            string DatiRegistrati = JsonSerializer.Serialize(listaDati);
            File.WriteAllText("Credenziali.json", DatiRegistrati);

            label5.Text = utente.saldo.ToString("C");
            puntataAttuale = 0;
            label6.Text = puntataAttuale.ToString("C");
            button2.Text = "INIZIA";
            button1.Visible = false;
            label2.Text = "";
            pictureBox1.Enabled = true;
            CartaUtente.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            GeneraCarteDealer();
        }
    }
}
