using CLI.Konzola;
using CLI.DAO;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Channels;

namespace CLI;
//DODATI NOVE DAO IMPLEMENTIRATI DODAVATI GRANE NA GITU NOVI CSV FAJL
//STUDENTPREDMET(VEZE VISE NA VISE IDU U NOVI CSV)
//DODATI KONTROLU UNOSA ne mora za svaki ali za nesto tipa datum
//PROMENITI VEZE
//JA DA SREDIM DODAVANJE iz fajlova a vrana konzolu da testira sve funkcije
class Program
{
        static void Main()
        {
            StudentskaSluzbaDAO ssluzba = new StudentskaSluzbaDAO();
            StudentskaSluzbaKonzola konzola = new StudentskaSluzbaKonzola(ssluzba);
            konzola.PokreniMeni();
            /*for (int i = 0; i < ssluzba._studenti.Count; i++)
         {
             string s = ssluzba._studenti[i].ToString();
             Console.WriteLine(s+"\n");
         }
         for (int i = 0; i < ssluzba._profesori.Count; i++)
         {
             string s = ssluzba._profesori[i].ToString();
             Console.WriteLine(s + "\n");
         }
         for (int i = 0; i < ssluzba._ocene.Count; i++)
         {
             string s = ssluzba._ocene[i].ToString();
             Console.WriteLine(s + "\n");
         }*/
            /* for(int i = 0; i < ssluzba._studenti.Count; i++) {
                  string[] tnt = ssluzba._studenti[i].ToCSV();
                  for (int j = 0; j < tnt.Length; j++)
                  {
                      Console.WriteLine(tnt[j]);
                  }

             }
              for (int i = 0; i < ssluzba._profesori.Count; i++)
              {
                  string[] tnt = ssluzba._profesori[i].ToCSV();
                  for (int j = 0; j < tnt.Length; j++)
                  {
                      Console.WriteLine(tnt[j]);
                  }

              }
              for (int i = 0; i < ssluzba._ocene.Count; i++)
              {
                  string[] tnt = ssluzba._ocene[i].ToCSV();
                  for (int j = 0; j < tnt.Length; j++)
                  {
                      Console.WriteLine(tnt[j]);
                  }

              }
              for (int i = 0; i < ssluzba._predmeti.Count; i++)
              {
                  string[] tnt = ssluzba._predmeti[i].ToCSV();
                  for (int j = 0; j < tnt.Length; j++)
                  {
                      Console.WriteLine(tnt[j]);
                  }

              }*/
            // VehicleConsoleView view = new VehicleConsoleView(vehicles);
            // view.RunMenu();
            ssluzba.SaveWhenExit();
        }
    
}





