using CLI.DAO;
using CLI.Model;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace CLI.Konzola
{
    // klasa za interakciju korisnika i aplikacije
    internal class StudentskaSluzbaKonzola
    {
        private StudentskaSluzbaDAO ssluzba;

        public StudentskaSluzbaKonzola(StudentskaSluzbaDAO ssluzba)
        {
            this.ssluzba = ssluzba;
        }
        // :qa za izlazak iz Vim-a
        // branislav.andjelic@uns.ac.rs

        public void PokreniMeni()
        {
            while (true)
            {
                PrikaziMeni();
                string userInput = System.Console.ReadLine() ?? "0";
                if (userInput == "0")
                {
                    ssluzba.SaveWhenExit();
                    break;
                }
                DaljeMeni(userInput);
                string userInput2 = System.Console.ReadLine() ?? "0";
                if (userInput2 == "0")
                {
                    ssluzba.SaveWhenExit();
                    break;
                }
                OdradiFunkcionalnost(userInput, userInput2);
            }
        }


        //---------------------------------------------------CREATE KONZOLA DEO-------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------
        private void DodajStudenta(int id = -1)
        {
            string prezime, ime, trenutnaGodinaStudija, kontaktTelefon, emailAdresa;
            bool regexTest = false;
            string datumRodjenjaString;
            string adresaString;
            string indeksString;
            Datum datumRodjenja = new Datum();
            Adresa adresaStanovanja = new Adresa();
            Indeks brojIndeksa = new Indeks();
            Status status;

            Console.WriteLine("Ime studenta: ");
            ime = Console.ReadLine();
            Console.WriteLine("Prezime studenta: ");
            prezime = Console.ReadLine();
            Console.WriteLine("Godina studija: ");
            trenutnaGodinaStudija = Console.ReadLine();
            Console.WriteLine("Kontakt telefon: ");
            kontaktTelefon = Console.ReadLine();
            Console.WriteLine("E-mail: ");
            emailAdresa = Console.ReadLine();
            do
            {
                regexTest = false;
                Console.WriteLine("Datum rodjenja dd.mm.yyyy.: ");
                datumRodjenjaString = Console.ReadLine();
                if (ssluzba.DatumRegex(datumRodjenjaString))
                {
                    regexTest = true;
                    datumRodjenja.setDatum(datumRodjenjaString);
                }
                else
                    Console.WriteLine("Datum nije validan");

            } while (regexTest == false);

            do
            {
                regexTest = false;
                Console.WriteLine("Adresa stanovanja u obliku ulica broj grad, drzava: ");
                adresaString = Console.ReadLine();
                if (ssluzba.AdresaRegex(adresaString))
                {
                    adresaStanovanja.setAdresa(adresaString);
                    regexTest = true;
                }
                else
                    Console.WriteLine("Adresa nije validan");
            } while (regexTest == false);

            do
            {
                regexTest = false;
                Console.WriteLine("Broj indeksa u obliku RA brojIndeksa/godinaUpisa: ");
                indeksString = Console.ReadLine();
                if (ssluzba.IndexRegex(indeksString))
                {
                    brojIndeksa.SetIndex(indeksString);
                    regexTest = true;
                }
                else
                    Console.WriteLine("Indeks nije validan");
            } while (regexTest == false);

            Console.WriteLine("Status studenta: ");
            if (Console.ReadLine() == "B") status = Status.B;
            else status = Status.S;

            Student student = new Student(prezime, ime, datumRodjenja, adresaStanovanja, kontaktTelefon,
                emailAdresa, brojIndeksa, trenutnaGodinaStudija, status);

            if(id == -1)
                ssluzba.AddStudent(student);
            else
            {
                student.id = id;
                ssluzba.UpdateStudent(student);
            }

        }

        private void DodajProfesora(int idProf = -1)
        {
            string imeP, prezimeP;
            Adresa adresaStanovanjaP = new Adresa();
            bool regexTest = false;
            string datumRodjenjaString;
            string adresaString;
            string kontaktTelefonP, emailAdresaP, brojLicneKarte, zvanje, godineStaza;
            int idKatedre;
            Datum datumRodjenjaP = new Datum();
            Console.WriteLine("Ime profesora: ");
            imeP = Console.ReadLine();
            Console.WriteLine("Prezime profesora: ");
            prezimeP = Console.ReadLine();

            do
            {
                regexTest = false;
                Console.WriteLine("Adresa stanovanja u obliku ulica broj grad, drzava: ");
                adresaString = Console.ReadLine();
                if (ssluzba.AdresaRegex(adresaString))
                {
                    adresaStanovanjaP.setAdresa(adresaString);
                    regexTest = true;
                }
                else
                    Console.WriteLine("Adresa nije validan");
            } while (regexTest == false);

            Console.WriteLine("Kontakt telefon: ");
            kontaktTelefonP = Console.ReadLine();
            Console.WriteLine("E-mail adresa: ");
            emailAdresaP = Console.ReadLine();
            Console.WriteLine("Broj licne karte: ");
            brojLicneKarte = Console.ReadLine();

            do
            {
                regexTest = false;
                Console.WriteLine("Datum rodjenja dd.mm.yyyy.: ");
                datumRodjenjaString = Console.ReadLine();
                if (ssluzba.DatumRegex(datumRodjenjaString))
                {
                    regexTest = true;
                    datumRodjenjaP.setDatum(datumRodjenjaString);
                }
                else
                    Console.WriteLine("Datum nije validan");

            } while (regexTest == false);
            Console.WriteLine("Zvanje: ");
            zvanje = Console.ReadLine();
            Console.WriteLine("Godine staza: ");
            godineStaza = Console.ReadLine();
            idKatedre = -1;
            
            bool uslov;
            do
            {
            uslov = false;
            Console.WriteLine("Id katedre: ");
            idKatedre = int.Parse(Console.ReadLine());
            if (ssluzba.GetAllKatedre().FindIndex(k => k.id == idKatedre) == -1)
                {
                    Console.WriteLine("Katedra sa unetim ID ne postoji, pokusajte ponovo.");
                    uslov = true;
                }
            } while (uslov);
            
          
                Profesor profesor = new Profesor(imeP, prezimeP, datumRodjenjaP,
                    adresaStanovanjaP, kontaktTelefonP, emailAdresaP, brojLicneKarte, zvanje, godineStaza, idKatedre);

            if(idProf == -1)
                ssluzba.AddProfesor(profesor);
            else
            {
                profesor.id = idProf;
                ssluzba.UpdateProfessor(profesor);
            }

        }

        private void DodajPredmet(int id = -1)
        {
            Predmet predmet;
            string naziv;
            int idProf, espb, sifra, godinaStudija;
            Semestar semestar;
            Console.WriteLine("Unesite sifru predmeta: ");
            sifra = int.Parse(Console.ReadLine());
            Console.WriteLine("Unesite naziv predmeta: ");
            naziv = Console.ReadLine();
            Console.WriteLine("Unesite semestar u kom se predmet slusa: ");
            if (Console.ReadLine() == "Zimski")
                semestar = Semestar.Zimski;
            else
                semestar = Semestar.Letnji;

            Console.WriteLine("Unesite broj godine na kojoj se predmet slusa: ");
            godinaStudija = int.Parse(Console.ReadLine());


            do
            {
                Console.WriteLine("Unesite ID profesora koji predaje ovaj predmet: ");
                idProf = int.Parse(Console.ReadLine());
                if (ssluzba.GetAllProfessors().FindIndex(p => p.id == idProf) == -1)
                {
                    Console.WriteLine("Profesor sa unetim ID ne postoji, pokusajte ponovo.");
                }
            } while (ssluzba.GetAllProfessors().FindIndex(p => p.id == idProf) == -1);

            Console.WriteLine("Unesite broj ESPB bodova koje predmet nosi:");
            espb = int.Parse(Console.ReadLine());
            predmet = new Predmet(sifra, naziv, semestar, godinaStudija, idProf, espb);
            if(id == -1)
                ssluzba.AddPredmet(predmet);
            else
            {
                predmet.id = id;
                ssluzba.UpdatePredmet(predmet);
            }
        }

        private void DodajKatedru(int id = -1)
        {
            Katedra katedra;
            string sifra, naziv;
            int sefId;
            bool uslov;
            string imasefa = "n";

            Console.WriteLine("Unesite sifru katedre: ");
            sifra = Console.ReadLine();
            Console.WriteLine("Unesite naziv katedre: ");
            naziv = Console.ReadLine();
                if(ssluzba.GetAllProfessors().Count() == 0)
                {
                    sefId = -1;
                }
                else
                {
                    int brsefova = 0;
                    List<int> sefoviID = new List<int>();
                    ssluzba.GetAllKatedre().ForEach(k => { if (k.sefId != -1) { brsefova++; sefoviID.Add(k.sefId); } });
               
                    if (brsefova == ssluzba.GetAllProfessors().Count() && id==-1)
                    {
                        Console.WriteLine("Svi profesori su vec sefovi dodeljen id sefa na -1");
                        sefId = -1;
                    }
                    else
                    {
                    if (id != -1)
                    { 
                        int trenutniSef=-5;
                        ssluzba.GetAllKatedre().ForEach(k => { if (k.id == id) trenutniSef = k.sefId; });
                        sefoviID.Remove(trenutniSef);
                    }
                        Console.WriteLine("Unesite Y ako zelite da katedra ima sefa");
                        imasefa = Console.ReadLine();
                        if (imasefa == "Y")
                        {
                            do
                            {
                                uslov = false;
                                Console.WriteLine("Unesite ID profesora koji je sef ove katedre: ");
                                sefId = int.Parse(Console.ReadLine());
                                if(sefoviID.FindIndex(sid => sid == sefId) != -1)
                                {
                                    Console.WriteLine("Profesor sa unetim ID je vec sef, pokusajte ponovo");
                                    uslov = true;
                                }
                                if (ssluzba.GetAllProfessors().FindIndex(p => p.id == sefId) == -1)
                                {
                                    Console.WriteLine("Profesor sa unetim ID ne postoji katedra trenutno nema sefa, pokusajte ponovo");
                                    uslov = true;
                                }
                            } while (uslov);
                        }
                        else
                        {
                            Console.WriteLine("Dodata katedra nema sefa");
                            sefId = -1;
                        }
                    }

                }
              

            katedra = new Katedra(sifra, naziv, sefId);
            if(id == -1)
                ssluzba.AddKatedra(katedra);
            else
            {
                katedra.id = id;
                ssluzba.UpdateKatedra(katedra);
            }

        }

        private void DodajOcenu(int id = -1)
        {
            int idStudentPolozio, idPredmet, ocena;
            bool regexTest = false;
            Datum datumPolaganja = new Datum();

            bool uslov;
            do
            {
                uslov = false;
                Console.WriteLine("Unesite ID studenta koji dobija ocenu: ");
                idStudentPolozio = int.Parse(Console.ReadLine());
                if (ssluzba.GetAllStudents().FindIndex(s => s.id == idStudentPolozio) == -1)
                {
                    Console.WriteLine("Student sa unetim ID ne postoji, pokusajte ponovo. ");
                    uslov = true;
                }
                else if (ssluzba.GetAllStudentPredmet().FindIndex(p => p.StudentId == idStudentPolozio) == -1)
                {
                    Console.WriteLine("Student sa unetim ID ne slusa ni jedan predmet, pokusajte ponovo.");
                    uslov = true;
                }
                if (id == -1)
                    ssluzba.GetAllStudents().ForEach(s =>
                    {
                        if (s.id == idStudentPolozio)
                        {
                            if (s.nepolozeniIspiti.Count() == 0)
                            {
                                Console.WriteLine("Student sa unetim ID ima ocenu iz svakog predmeta, pokusajte ponovo.");
                                uslov = true;
                            }
                        }
                    });

            } while (uslov);

            do
            {
                uslov = false;
                Console.WriteLine("Unesite ID predmeta iz kog student dobija ocenu: ");
                idPredmet = int.Parse(Console.ReadLine());
                if (ssluzba.GetAllStudentPredmet().FindIndex(p => p.StudentId == idStudentPolozio && p.PredmetId == idPredmet) == -1)
                {
                    Console.WriteLine("Predmet koji ste uneli student ne slusa, pokusajte ponovo.");
                    uslov = true;
                }
                if(id==-1)
                ssluzba.GetAllStudents().ForEach(s => {
                    if (s.id == idStudentPolozio)
                    {
                        int index = s.polozeniIspiti.FindIndex(pi => pi.id == idPredmet);
                        if (index != -1)
                        {
                            Console.WriteLine("Student je vec dobio ocenu iz datog predmeta, pokusajte ponovo. ");
                            uslov = true;
                        }
                    }
                });
            } while (uslov);

            do
            {
                Console.WriteLine("Unesite ocenu koju je student dobio: ");
                ocena = int.Parse(Console.ReadLine());
                if (ocena <= 5 || ocena > 10)
                    Console.WriteLine("Nevalidan unos ocene, ocena moze imati vrednost 6-10. ");
            } while (ocena <= 5 || ocena > 10);

            string datumPolaganjaString = "";
            do
            {
                regexTest = false;
                Console.WriteLine("Unesite datum polaganja ispita dd.mm.yyyy.: ");
                datumPolaganjaString = Console.ReadLine();
                if (ssluzba.DatumRegex(datumPolaganjaString))
                {
                    regexTest = true;
                    datumPolaganja.setDatum(datumPolaganjaString);
                }
                else
                    Console.WriteLine("Datum nije validan");

            } while (regexTest == false);

            Ocena NovaOcena;
            NovaOcena = new Ocena(idStudentPolozio, idPredmet, ocena, datumPolaganja);

            if(id == -1)
                ssluzba.AddOcena(NovaOcena);
            else
            {
                NovaOcena.id = id;
                ssluzba.UpdateOcena(NovaOcena);
            }


            foreach (Student s in ssluzba._studenti)
            {
                ssluzba.izracunajProsek(s.id);
            }

            Console.WriteLine("Uspesno upisana ocenu studentu!");
        }

        private void PridruziPredmetStudentu()
        {
            bool uslov;
            int predmetID, studentID;

            do
            {
                uslov = false;
                Console.WriteLine("Unesite ID studenta koji zeli da slusa dodatan predmet: ");
                studentID = int.Parse(Console.ReadLine());
                if (ssluzba.GetAllStudents().FindIndex(s => s.id == studentID) == -1)
                {
                    Console.WriteLine("Student sa unetim ID ne postoji, pokusajte ponovo. ");
                    uslov = true;
                }
                ssluzba.GetAllStudents().ForEach(s =>
                {
                    if (s.id == studentID)
                    {
                        if (s.nepolozeniIspiti.Count() + s.polozeniIspiti.Count() == ssluzba._predmeti.Count())
                        {
                            Console.WriteLine("Student slusa ili je polozio svaki predmet koji postoji, pokusajte ponovo.");
                            uslov = true;
                        }
                    }
                });
            } while (uslov);

            do
            {
                uslov = false;
                Console.WriteLine("Unesite ID predmeta koji zelite da student slusa: ");
                predmetID = int.Parse(Console.ReadLine());
                if(ssluzba.GetAllPredmeti().FindIndex(p => p.id == predmetID) == -1)
                {
                    Console.WriteLine("Predmet sa unetim ID ne postoji, pokusajte ponovo.");
                    uslov = true;
                }

                if (ssluzba.GetAllStudentPredmet().FindIndex(p => p.StudentId == studentID && p.PredmetId == predmetID) != -1)
                {
                    Console.WriteLine("Student vec slusa ili je polozio predmet sa unetim ID, pokusajte ponovo.");
                    uslov = true;
                }

            } while (uslov);

            StudentPredmet sp = new StudentPredmet(studentID, predmetID);

            ssluzba.AddStudentPredmet(sp);

            Console.WriteLine("Uspesno ste dodali predmet studentu!");

        }
        //-----------------------------------------------------READ DEO KONZOLE-------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------
        private void StampajStudente(List<Student> studenti)
        {
            System.Console.WriteLine("Studenti: ");
            string header = $"{"ID", 4}|{"IME", 8}|{"PREZIME", 12}|{"DATUM RODJENJA", 15}|{"ADRESA",30}|{"TELEFON",12}" +
                $"|{"E-MAIL",20}|{"BR. INDEKSA", 12}|{"GOD.",7}|{"STATUS",8}|{"PROSEK",8}|{"NEPOLOZENI PREDMETI",30}|{"POLOZENI PREDMETI",30}|"; // Dodati vrstu naziva oebelzja
            System.Console.WriteLine(header);
            foreach (Student s in studenti)
            {
                System.Console.WriteLine(s.ToString());
            }
        }   

        private void StampajProfesore(List<Profesor> profesori)
        {
            System.Console.WriteLine("Profesori: ");
            string header = $"{"ID",5}|{"IME",8}|{"PREZIME",12}|{"DATUMRODJENJA",15}|{"ADRESASTANOVANJA",30}|{"TELEFON",12}|{"EMAIL",20}" +
                $"|{"BR.LICNE",12}|{"ZVANJE",20}|{"GOD.STAZA",11}|{"ID.KATEDRE",11}|{"PREDAJE",40}|";
            System.Console.WriteLine(header);
            foreach (Profesor prof in profesori)
            {
                System.Console.WriteLine(prof.ToString());
            }
        }

        private void StampajPredmete(List<Predmet> predmeti)
        { 
            System.Console.WriteLine("Predmeti: ");
            string header = $"{"ID",6}|{"SIFRA",8}|{"NAZIV",15}|{"SEMESTAR",10}|{"GOD.STUDIJA",13}|{"ESPB",6}|" +
                $"{"PROF.ID",10}|{"PREDAJE",20}|{"ID. STUDENTI POLOZILI",30}|{"ID. STUDENTI NISU POLOZILI",30}";
            System.Console.WriteLine(header);
            foreach (Predmet p in predmeti)
            {
                System.Console.WriteLine(p.ToString());
            }
        }

        private void StampajKatedre(List<Katedra> katedre)
        {
            System.Console.WriteLine("Katedre: ");
            string header = $"{"ID",10}|{"SIFRA",10}|{"NAZIV",15}|{"ID. SEFA",15}|{"SEF",30}|{"PROFESORI NA KATEDRI",50}|";
            System.Console.WriteLine(header);
            foreach (Katedra k in katedre)
            {
                System.Console.WriteLine(k.ToString());
            }
        }

        private void StampajOcene(List<Ocena> ocene)
        {
            System.Console.WriteLine("Ocene: ");
            string header = $"{"ID. OCENE",12}|{"ID. STUDENTA",15}|{"ID. PREDMETA",15}|{"OCENA",12}|{"DATUM POLAGANJA",16}|";
            System.Console.WriteLine(header);
            foreach (Ocena o in ocene)
            {
                System.Console.WriteLine(o.ToString());
            }
        }

       
        //  -------------------------------UPDATE DEO KONZOLE---------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------
        private void UpdateStudent()
        {
            int id;
            bool exists = false;
            do
            {
                Console.WriteLine("Unesite ID studenta kojeg menjate:");
                id = int.Parse(Console.ReadLine());
                if (ssluzba.GetAllStudents().FindIndex(s => s.id == id) != -1)
                {
                    exists = true;
                }
                else
                    Console.WriteLine("Student sa unetim ID-om ne postoji");
            } while (exists == false);
            DodajStudenta(id);
        }
        private void UpdateProfesor()
        {
            int id;
            bool exists = false;
            do
            {
                Console.WriteLine("Unesite ID profesora kojeg menjate:");
                id = int.Parse(Console.ReadLine());
                if (ssluzba.GetAllProfessors().FindIndex(s => s.id == id) != -1)
                {
                    exists = true;
                }
                else
                    Console.WriteLine("Profesor sa unetim ID-om ne postoji");
            } while (exists == false);
            DodajProfesora(id);
        }
        private void UpdateKatedra()
        {
            int id;
            bool exists = false;
            do
            {
                Console.WriteLine("Unesite id katedre koje menjate:");
                id = int.Parse(Console.ReadLine());
                if (ssluzba.GetAllKatedre().FindIndex(s => s.id == id) != -1)
                {
                    exists = true;
                }
                else
                    Console.WriteLine("Katedra sa unetim id-om ne postoji");
            } while (exists == false);
            DodajKatedru(id);
        }
        private void UpdateOcena()
        {
            int id;
            bool exists = false;
            do
            {
                Console.WriteLine("Unesite id ocene koju menjate:");
                id = int.Parse(Console.ReadLine());
                if (ssluzba.GetAllOcene().FindIndex(s => s.id == id) != -1)
                {
                    exists = true;
                }
                else
                    Console.WriteLine("Ocena sa unetim id-om ne postoji");
            } while (exists == false);
            DodajOcenu(id);
        }
        private void UpdatePredmet()
        {
            int id;
            bool exists = false;
            do
            {
                Console.WriteLine("Unesite id predmeta kojeg menjate:");
                id = int.Parse(Console.ReadLine());
                if (ssluzba.GetAllPredmeti().FindIndex(s => s.id == id) != -1)
                {
                    exists = true;
                }
                else
                    Console.WriteLine("Predmet sa unetim id-om ne postoji");

            } while (exists == false);
            DodajPredmet(id);
        }

        //---------------------------------------------------DELETE KONZOLA DEO---------------------------------------------
        //------------------------------------------------------------------------------------------------------------------
        private void IzbrisiStudenta()
        {
            bool provera = true;
            do
            {
                Console.WriteLine("Unesite ID studenta kog zelite da izbrisete:");
                int id = int.Parse(Console.ReadLine());
                if (ssluzba.RemoveStudent(id) == true)
                {
                    provera = false;
                }
                else
                    Console.WriteLine("Student sa unetim ID ne postoji, probajte ponovo");
            } while (provera == true);
        }
        private void IzbrisiProfesora()
        {
            bool provera = true;
            do
            {
                Console.WriteLine("Unesite id profesora kojeg zelite da izbrisete:");
                int id = int.Parse(Console.ReadLine());
                ssluzba._profesori.ForEach(p => {
                    if (p.id == id && p.predaje.Count != 0)
                    {
                        Console.WriteLine("Ne moze se obrisati profesor, jer nisu uklonjeni svi predmeti koje predaje");
                        provera = false;
                    }
                });
                if (provera == false)
                    break;
                if (ssluzba.RemoveProfesor(id) == true)
                {
                    provera = false;
                }
                else
                    Console.WriteLine("Ne postoji profesor sa unetim ID, pokusajte ponovo. ");
            } while (provera == true);
        }
        private void IzbrisiOcenu()
        {
            bool provera = true;
            do
            {
                Console.WriteLine("Unesite id ocene kojeg zelite da izbrisete:");
                int id = int.Parse(Console.ReadLine());
                if (ssluzba.RemoveOcena(id) == true)
                {
                    provera = false;
                }
                else
                    Console.WriteLine("Ocena sa unetim ID ne postoji, pokusajte ponovo. ");
            } while (provera == true);
        }
        private void IzbrisiPredmet()
        {
            bool provera = true;
            do
            {
                Console.WriteLine("Unesite id predmeta kojeg zelite da izbrisete:");
                int id = int.Parse(Console.ReadLine());
                ssluzba._predmeti.ForEach(p => { if(p.id == id && p.nisuPoloziliPredmet.Count != 0)
                    {
                        Console.WriteLine("Nije moguce izbrisati predmet jer ga nisu svi studenti polozili. ");
                        provera = false;
                    }
                });
                if (provera == false)
                    break;
                if (ssluzba.RemovePredmet(id) == true)
                {
                    provera = false;
                }
                else
                    Console.WriteLine("Ne postoji predmet sa unetim ID, pokusajte ponovo. ");
            } while (provera == true);
        }
        private void IzbrisiKatedru()
        {
            bool provera = true;
            do
            {
                provera = true;
                Console.WriteLine("Unesite id katedre kojeg zelite da izbrisete:");
                int id = int.Parse(Console.ReadLine());
                ssluzba._katedre.ForEach(k => {
                    if (k.id == id && k.profesori.Count != 0)
                    {
                        Console.WriteLine("Ne moze se obrisati katedra jer nisu uklonjeni svi profesori iz nje");
                        provera = false;
                    }
                });
                if (provera == false)
                    break;
                if (ssluzba.RemoveKatedra(id) == true)
                {
                    provera = false;
                }
                else
                    Console.WriteLine("Nepostojeca katedra, pokusajte ponovo.");
            } while (provera == true);
        }
        //----------------------------------------------POKRETANJE MENI-----------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------
        private void OdradiFunkcionalnost(string input, string input2)
        {
            switch (input)
            {
                case "1":
                    switch (input2)
                    {
                        case "1":
                            //Prikazi studente
                            StampajStudente(ssluzba.GetAllStudents());
                            break;
                        case "2":
                            // Prikazi profesore
                            StampajProfesore(ssluzba.GetAllProfessors());
                            break;
                        case "3":
                            // Prikazi predmete
                            StampajPredmete(ssluzba.GetAllPredmeti());
                            break;
                        case "4":
                            // Prikazi katedre
                            StampajKatedre(ssluzba.GetAllKatedre());
                            break;
                        case "5":
                            // Prikazi ocene
                            StampajOcene(ssluzba.GetAllOcene());
                            break;
                    }
                    break;
                case "2":
                    switch (input2)
                    {
                        case "1":
                            //Dodaj studenta
                            DodajStudenta();
                            break;


                        case "2":
                            // Dodaj profesora
                            if (ssluzba.GetAllKatedre().Count() == 0)
                                Console.WriteLine("Ne postoji katedra u kojoj bi se profesor nalazio, prvo morate dodati katedru.");
                            else
                                DodajProfesora();
                          
                            break;
                        case "3":
                            // Dodaj predmet
                            if (ssluzba.GetAllProfessors().Count() == 0)
                                Console.WriteLine("Ne postoji profesor koji bi predavali predmet, prvo morate dodati profesora,");
                            else
                                DodajPredmet();
                            break;
                        case "4":
                            // Dodaj katedru
                            DodajKatedru();
                            break;
                        case "5":
                            // Dodaj ocene
                            if (ssluzba.GetAllStudentPredmet().Count() == 0)
                                Console.WriteLine("Ne postoji student koji slusa predmet, morate prvo pridruziti predmet studentu.");
                            else
                                DodajOcenu();
                            break;
                        case "6":
                            //Pridruzi predmet studentu
                            if (ssluzba.GetAllStudents().Count() == 0)
                                Console.WriteLine("Ne postoji student kome biste dodelili predmet, morate dodati studenta ");
                            else if(ssluzba.GetAllPredmeti().Count==0)
                                Console.WriteLine("Ne postoji predmet koji bi student slusao, morate dodati predmet ");
                            else if (ssluzba.GetAllPredmeti().Count() * ssluzba.GetAllStudents().Count() == ssluzba.GetAllStudentPredmet().Count)
                                Console.WriteLine("Svaki student slusa svaki postojeci predmet");
                            else
                                PridruziPredmetStudentu();
                            break;
                    }
                    break;
                case "3":
                    switch (input2)
                    {
                        case "1":
                            //Azuriraj studenta
                            if (ssluzba._studenti != null && ssluzba._studenti.Count != 0)
                                UpdateStudent();
                            else
                                Console.WriteLine("Ne postoji lista studenata");
                            break;
                        case "2":
                            // Azuriraj profesora
                            if (ssluzba._profesori != null && ssluzba._profesori.Count != 0)
                                UpdateProfesor();
                            else
                                Console.WriteLine("Ne postoji lista profesora");
                            break;
                        case "3":
                            // Azuriraj predmet
                            if (ssluzba._predmeti != null && ssluzba._predmeti.Count != 0)
                                UpdatePredmet();
                             else
                                Console.WriteLine("Ne postoji lista predmeta");
                            break;
                        case "4":
                            // Azuriraj katedru
                            if (ssluzba._katedre != null && ssluzba._katedre.Count != 0)
                                UpdateKatedra();
                              else
                                Console.WriteLine("Ne postoji lista katedre");
                            break;
                        case "5":
                            // Azuriraj ocenu
                            if (ssluzba._ocene != null && ssluzba._ocene.Count != 0)
                                UpdateOcena();
                             else
                                Console.WriteLine("Ne postoji lista ocene");
                            break;
                    }
                    break;
                case "4":
                    switch (input2)
                    {
                        case "1":
                            //Izbrisi studenta
                            if (ssluzba._studenti != null && ssluzba._studenti.Count != 0)
                                IzbrisiStudenta();
                            else
                                Console.WriteLine("Nije moguce izbaciti studenta jer studenti ne postoje");
                            break;
                        case "2":
                            // Izbrisi profesora
                            if (ssluzba._profesori != null && ssluzba._profesori.Count != 0)
                                IzbrisiProfesora();
                            else
                                Console.WriteLine("Nije moguce izbaciti profesora jer profesori ne postoje");  
                            break;
                        case "3":
                            // Izbrisi predmet
                            if (ssluzba._predmeti != null && ssluzba._predmeti.Count != 0)
                                IzbrisiPredmet();
                            else
                                Console.WriteLine("Nije moguce izbaciti predmet jer predmeti ne postoje");
                            break;
                        case "4":
                            // Izbrisi katedru
                            if (ssluzba._katedre != null && ssluzba._katedre.Count != 0)
                                IzbrisiKatedru();
                            else
                                Console.WriteLine("Nije moguce izbaciti katedru jer katedre ne postoje");
                            break;
                        case "5":
                            // Izbrisi ocenu
                            if (ssluzba._ocene != null && ssluzba._ocene.Count != 0)
                                IzbrisiOcenu();
                            else
                                Console.WriteLine("Nije moguce izbaciti ocenu jer nesto ne postoji");
                            break;
                    }

                    break;
            }
        }

        private void DaljeMeni(string input)
        {
            switch (input)
            {
                case "1":
                    Console.WriteLine("\nKoje entitete zelite da pogledate?");
                    Console.WriteLine("1: Studente");
                    Console.WriteLine("2: Profesore");
                    Console.WriteLine("3: Predmete");
                    Console.WriteLine("4: Katedre");
                    Console.WriteLine("5: Ocene");
                    Console.WriteLine("0: Zatvori");
                    break;
                case "2":
                    Console.WriteLine("\nKoji entitet zelite da dodate?");
                    Console.WriteLine("1: Student");
                    Console.WriteLine("2: Profesor");
                    Console.WriteLine("3: Predmet");
                    Console.WriteLine("4: Katedra");
                    Console.WriteLine("5: Ocenu");
                    Console.WriteLine("6: Pridruzi predmet studentu");
                    Console.WriteLine("0: Zatvori");
                    break;
                case "3":
                    Console.WriteLine("\nKoji entitet zelite da azurirate?");
                    Console.WriteLine("1: Student");
                    Console.WriteLine("2: Profesor");
                    Console.WriteLine("3: Predmet");
                    Console.WriteLine("4: Katedra");
                    Console.WriteLine("5: Ocena");
                    Console.WriteLine("0: Zatvori");
                    break;
                case "4":
                    Console.WriteLine("\nKoji entitet zelite da izbrisete?");
                    Console.WriteLine("1: Student");
                    Console.WriteLine("2: Profesor");
                    Console.WriteLine("3: Predmet");
                    Console.WriteLine("4: Katedra");
                    Console.WriteLine("5: Ocena");
                    Console.WriteLine("0: Zatvori");
                    break;
            }
        }


        private void PrikaziMeni()
        {
            Console.WriteLine("\nIzaberite opciju: ");
            Console.WriteLine("1: Prikazi entitete");
            Console.WriteLine("2: Dodaj entitet");
            Console.WriteLine("3: Azuriraj entitet");
            Console.WriteLine("4: Ukloni entitet");
            Console.WriteLine("0: Zatvori");
        }


    }
}
