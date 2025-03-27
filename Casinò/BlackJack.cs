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

    public partial class BlackJack : Form
    {
        String nomeUtente;
        private Random random;
        public BlackJack(string nomeUtent)
        {
            InitializeComponent();
            nomeUtente = nomeUtent;
            random = new Random();
            CarteUtente();
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
        private void carte()
        {
            List<int> list = new List<int>();
            list.Add(random.Next(2, 53));


        }

        private void buttonEsci_Click(object sender, EventArgs e)
        {
            this.Close();
            Home formHome= new Home(nomeUtente);
            formHome.Show();
        }
        private void aggiungiPuntata(object sender, EventArgs e)
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

            //long importo;
            //.TryParse(textBox1.Text, out importo);
            //Utente.saldo += importo;
            //labelSaldo.Text = Utente.saldo.ToString("C");
            // DatiRegistrati = JsonSerializer.Serialize(listaDati);
            //File.WriteAllText("Credenziali.json", DatiRegistrati);
            //textBox1.Clear();

        }
        
        private void CarteUtente() {
            int[,] Carte = new int[4, 13];
            List<int> listSeme = new List<int>();
            Random random = new Random();
            while (listSeme.Count < 52)
            {
                int numero = random.Next(2, 53); 
                if (!listSeme.Contains(numero))  
                {
                    listSeme.Add(numero);
                }
            }
            int contAggiungi = 0;





            if (listSeme[0]/13 <= 1)
            {
                label2.Text = (listSeme[0] / 13).ToString();
            }else if (listSeme[0]/13 > 1 && listSeme[0]/13 <=2)
            {
                label2.Text = (listSeme[0] / 13).ToString();
            }
            else if (listSeme[0]/13 > 2 && listSeme[0]/13 <= 3)
            {
                label2.Text = (listSeme[0] / 13).ToString();
            }
            else if (listSeme[0]/13 > 3 && listSeme[0]/13 <= 4)
            {
                label2.Text = (listSeme[0] / 13).ToString();
            }

        }
    }
}
