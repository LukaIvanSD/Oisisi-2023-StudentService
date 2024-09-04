using CLI.Model;
using CLI.Storage;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using CLI.Observer;

namespace CLI.DAO;

public class StudentskaSluzbaDAO
{
    public List<Student> _studenti { get; set; }
    public List<Profesor> _profesori { get; set; }
    public List<Predmet> _predmeti { get; set; }
    public List<Ocena> _ocene { get; set; }
    public List<StudentPredmet> _studentpredmet { get; set; }
    public List<Katedra> _katedre { get; set; }

    public Subject StudentiSubject;
    public Subject ProfesoriSubject;
    public Subject PredmetSubject;
    public Subject StudentPredmetSubject;
    public Subject KatedreSubject;
    public Subject OceneSubject;

    private readonly Storage<Student> _studentStorage;
    private readonly Storage<Profesor> _profesorStorage;
    private readonly Storage<Predmet> _predmetStorage;
    private readonly Storage<Ocena> _ocenaStorage;
    private readonly Storage<StudentPredmet> _studentpredmetStorage;
    private readonly Storage<Katedra> _katedraStorage;
    public StudentskaSluzbaDAO()
    {
        _studentStorage = new Storage<Student>("studenti.txt");
        _profesorStorage = new Storage<Profesor>("profesori.txt");
        _predmetStorage = new Storage<Predmet>("predmeti.txt");
        _ocenaStorage = new Storage<Ocena>("ocene.txt");
        _studentpredmetStorage = new Storage<StudentPredmet>("StudentPredmet.txt");
        _katedraStorage = new Storage<Katedra>("Katedre.txt");

        StudentiSubject = new Subject();
        ProfesoriSubject = new Subject();
        PredmetSubject = new Subject();
        StudentPredmetSubject = new Subject();
        KatedreSubject = new Subject();
        OceneSubject = new Subject();

        _studenti = _studentStorage.Load();
        _profesori = _profesorStorage.Load();
        _predmeti = _predmetStorage.Load();
        _ocene = _ocenaStorage.Load();
        _studentpredmet = _studentpredmetStorage.Load();
        _katedre = _katedraStorage.Load();

        _studenti.ForEach(s => { s.id = _studenti.IndexOf(s); });
        _profesori.ForEach(p => { p.id = _profesori.IndexOf(p); });
        _predmeti.ForEach(pr => { pr.id = _predmeti.IndexOf(pr); });
        _ocene.ForEach(o => { o.id = _ocene.IndexOf(o); });
        _katedre.ForEach(k => { k.id = _katedre.IndexOf(k); });
        //A  ZA STUDENT PREDMET MOZDA ZBOG OVOGA SAM MORAO PREKO EXITA?????? ali da nemam polje id za studentpredmet
        //INICIJALIZACIJA SVIH LISTI
        if (_katedre.Count != 0)
        {
            _katedre.ForEach(k => { k.profesori = new List<Profesor>(); });
        }
        if (_profesori.Count != 0)
        {
            _profesori.ForEach(p => { p.predaje = new List<Predmet>(); });
        }
        if (_studenti.Count != 0)
        {
            _studenti.ForEach(s => { s.nepolozeniIspiti = new List<Predmet>(); s.polozeniIspiti = new List<Predmet>(); });
        }
        if (_predmeti.Count != 0)
        {
            _predmeti.ForEach(p => { p.nisuPoloziliPredmet = new List<Student>(); p.poloziliPredmet = new List<Student>(); });
        }

        // INCIJALIZOVALI LISTU PROFESORA U KATEDRI I DODELILI SEFA OBJEKAT

        foreach (Profesor prof in _profesori)
        {

            _katedre.ForEach(k =>
            {
                if (k.sefId == prof.id)
                {
                    k.sef = prof;
                    k.profesori.Add(prof);
                }
                else if (k.id == prof.idKatedre)
                {
                    k.profesori.Add(prof);
                }
            });
        }
        //POPUNILI LISTU PREDMETA KOJE PROFESOR PREDAJE
        foreach (Profesor prof in _profesori)
        {
            _predmeti.ForEach(p =>
            {
                if (p.professorId == prof.id)
                {
                    prof.predaje.Add(p);
                    p.profesor = prof;
                }
            });
        }
        foreach (Student s in _studenti)
        {
            _studentpredmet.ForEach(sp =>
            {
                if (sp.StudentId == s.id)
                {
                    s.nepolozeniIspiti.Add(GetPredmetById(sp.PredmetId));
                }
            });
        }
        foreach (Predmet p in _predmeti)
        {
            _studentpredmet.ForEach(sp =>
            {
                if (sp.PredmetId == p.id)
                {
                    p.nisuPoloziliPredmet.Add(GetStudentById(sp.StudentId));
                }
            });
        }
        foreach (Student s in _studenti)
        {
            _ocene.ForEach(o =>
            {
                if (o.idStudentPolozio == s.id)
                {
                    s.polozeniIspiti.Add(GetPredmetById(o.idPredmet));
                    s.nepolozeniIspiti.Remove(GetPredmetById(o.idPredmet));
                }
            });
            izracunajProsek(s.id);
        }
        foreach (Predmet p in _predmeti)
        {
            _ocene.ForEach(o =>
            {
                if (o.idPredmet == p.id)
                {
                    p.poloziliPredmet.Add(GetStudentById(o.idStudentPolozio));
                    p.nisuPoloziliPredmet.Remove(GetStudentById(o.idStudentPolozio));
                }
            });
        }

    }

    public void SaveWhenExit()
    {
        if (_studentpredmet != null && _studentpredmet.Count() != 0)
            foreach (StudentPredmet studpred in _studentpredmet)
            {
                _studenti.ForEach(s => {
                    if (studpred.StudentId == s.id)
                    {
                        Console.WriteLine(_studenti.IndexOf(s));
                        studpred.StudentId = _studenti.IndexOf(s);
                    }
                });
                _predmeti.ForEach(p => {
                    if (studpred.PredmetId == p.id)
                    {
                        Console.WriteLine(_predmeti.IndexOf(p));
                        studpred.PredmetId = _predmeti.IndexOf(p);
                    }
                });
            }
        if (_ocene != null && _ocene.Count() != 0)
            foreach (Ocena ocena in _ocene)
            {
                _studenti.ForEach(s => {
                    if (ocena.idStudentPolozio == s.id)
                    {
                        Console.WriteLine(_studenti.IndexOf(s));
                        ocena.idStudentPolozio = _studenti.IndexOf(s);
                    }
                });
                _predmeti.ForEach(p => {
                    if (ocena.idPredmet == p.id)
                    {
                        Console.WriteLine(_predmeti.IndexOf(p));
                        ocena.idPredmet = _predmeti.IndexOf(p);
                    }
                });
            }
        _studentStorage.Save(_studenti);
        _profesorStorage.Save(_profesori);
        _predmetStorage.Save(_predmeti);
        _predmetStorage.Save(_predmeti);//OVO VALJDA ZAKOMENTARISATI
        _ocenaStorage.Save(_ocene);
        _katedraStorage.Save(_katedre);
        _studentpredmetStorage.Save(_studentpredmet);
    }

    //------------------------GENERATE ID--------------------------------
    private int GenerateStudentId()
    {
        if (_studenti.Count == 0) return 0;
        return _studenti.Max(s => s.id) + 1;
    }
    private int GenerateKatedraId()
    {
        if (_katedre.Count == 0) return 0;
        return _katedre.Max(k => k.id) + 1;
    }

    private int GenerateProfesorId()
    {
        if (_profesori.Count == 0) return 0;
        return _profesori.Max(p => p.id) + 1;
    }

    private int GeneratePredmetId()
    {
        if (_predmeti.Count == 0) return 0;
        return _predmeti.Max(p => p.id) + 1;
    }

    private int GenerateOcenaId()
    {
        if (_ocene.Count == 0) return 0;
        return _ocene.Max(o => o.id) + 1;
    }
    //-------------------------------------------------------
    //--------------------ADD FUNKCIJE-----------------------

    public void izracunajProsek(int studentId)
    {
        double zbirOcena = 0;
        int brojac = 0;

        foreach (Ocena o in _ocene)
        {
            if (o.idStudentPolozio == studentId)
            {
                zbirOcena += o.ocena;
                brojac++;
            }
        }

        _studenti.ForEach(s => { if (s.id == studentId) s.prosecnaOcena = zbirOcena / (brojac * 1.0); });
        _studentStorage.Save(_studenti);
        StudentiSubject.NotifyObservers();

    }

    public void AddKatedra(Katedra katedra)
    {
        katedra.id = GenerateKatedraId();
        katedra.profesori = new List<Profesor>();
        _profesori.ForEach((p) =>
        {
            if (p.id == katedra.sefId)
            {
                katedra.sef = p;
                katedra.profesori.Add(p);
                foreach (Katedra k in _katedre)
                {
                    k.profesori.Remove(GetProfessorById(p.id));
                }
                p.idKatedre = katedra.id;
            }


        });

        _katedre.Add(katedra);

        _profesorStorage.Save(_profesori);
        _katedraStorage.Save(_katedre);
        ProfesoriSubject.NotifyObservers();
        KatedreSubject.NotifyObservers();

    }
    public void AddStudent(Student student)
    {
        student.id = GenerateStudentId();
        student.nepolozeniIspiti = new List<Predmet>();
        student.polozeniIspiti = new List<Predmet>();
        _studenti.Add(student);
        _studentStorage.Save(_studenti);
        StudentiSubject.NotifyObservers();

    }
    public void AddStudentPredmet(StudentPredmet sp)
    {
        _studentpredmet.Add(sp);
        _studentpredmetStorage.Save(_studentpredmet);

        _studenti.ForEach(s => { if (s.id == sp.StudentId) s.nepolozeniIspiti.Add(GetPredmetById(sp.PredmetId)); });

        _predmeti.ForEach(p => { if (p.id == sp.PredmetId) p.nisuPoloziliPredmet.Add(GetStudentById(sp.StudentId)); });
        StudentPredmetSubject.NotifyObservers();
    }
    public void AddProfesor(Profesor profesor)
    {
        profesor.id = GenerateProfesorId();
        profesor.predaje = new List<Predmet>();
        _profesori.Add(profesor);
        _katedre.ForEach(k =>
        {
            if (profesor.idKatedre == k.id)
            {
                if (k.profesori.Count() == 0)
                {
                    k.sefId = profesor.id;
                    k.sef = profesor;
                }
                k.profesori.Add(profesor);
            }
        });
        _katedraStorage.Save(_katedre);
        _profesorStorage.Save(_profesori);
        KatedreSubject.NotifyObservers();
        ProfesoriSubject.NotifyObservers();

    }
    public Profesor? ProfesorPredmet(int profId)
    {
        return _profesori.Find(p => p.id == profId);
    }
    public void AddPredmet(Predmet predmet)
    {
        predmet.id = GeneratePredmetId();
        predmet.poloziliPredmet = new List<Student>();
        predmet.nisuPoloziliPredmet = new List<Student>();
        _profesori.ForEach(p => { if (p.id == predmet.professorId) { p.predaje.Add(predmet); predmet.profesor = p; } });
        _predmeti.Add(predmet);
        _predmetStorage.Save(_predmeti);
        PredmetSubject.NotifyObservers();

    }
    public void AddOcena(Ocena ocena)
    {
        ocena.id = GenerateOcenaId();

        _studenti.ForEach(s => { if (s.id == ocena.idStudentPolozio) s.polozeniIspiti.Add(GetPredmetById(ocena.idPredmet)); });

        _predmeti.ForEach(p =>
        {
            if (p.id == ocena.idPredmet)
                p.poloziliPredmet.Add(GetStudentById(ocena.idStudentPolozio));
        });
        _studenti.ForEach(s => { if (s.id == ocena.idStudentPolozio) s.nepolozeniIspiti.Remove(GetPredmetById(ocena.idPredmet)); });
        _predmeti.ForEach(p => { if (p.id == ocena.idPredmet) p.nisuPoloziliPredmet.Remove(GetStudentById(ocena.idStudentPolozio)); });
        _ocene.Add(ocena);
        _ocenaStorage.Save(_ocene);
        OceneSubject.NotifyObservers();
    }


    public bool AddPredmetToProfessor(int idPredmeta, int idProfessor)
    {
        bool dodao = false;
        foreach (Predmet p in _predmeti)
        {
            if (p.id == idPredmeta)
            {
                p.professorId = idProfessor;
                foreach (Profesor pr in _profesori)
                {
                    if (pr.id == idProfessor)
                    {
                        p.profesor = pr;
                        pr.predaje.Add(p);
                        dodao = true;
                        break;
                    }
                }
                break;
            }
        }

        _predmetStorage.Save(_predmeti);
        PredmetSubject.NotifyObservers();
        ProfesoriSubject.NotifyObservers();
        return dodao;
    }





    public List<Student> GetStudentsForProfessor(int idProfesora)
    {
        List<Student> studenti = new List<Student>();

        foreach (Profesor p in _profesori)
        {
            if (p.id == idProfesora)
            {
                foreach (Predmet pr in p.predaje)
                {
                    foreach (Student s in pr.nisuPoloziliPredmet)
                    {
                        if (!studenti.Exists(existingStudent => existingStudent.id == s.id))
                        {
                            studenti.Add(s);
                        }

                    }
                }
            }
        }

        return studenti;

    }

    public List<Profesor> GetProfessorsForStudent(int idStudenta)
    {
        List<Profesor> profesori = new List<Profesor>();

        foreach (Student s in _studenti)
        {
            if (s.id == idStudenta)
            {
                foreach (Predmet pr in s.nepolozeniIspiti)
                {
                    foreach (Profesor p in _profesori)
                    {
                        if (p.id == pr.professorId)
                        {
                            if (!profesori.Exists(existingProfesor => existingProfesor.id == p.id))
                            {
                                profesori.Add(p);
                            }
                        }
                    }
                }
            }
        }

        return profesori;
    }

    public List<Predmet> GetPredmetiForKatedra(int idKatedre)
    {
        List<Predmet> predmeti = new List<Predmet>();

        foreach (Katedra k in _katedre)
        {
            if (k.id == idKatedre)
            {
                foreach (Profesor p in k.profesori)
                {
                    foreach (Predmet pr in p.predaje)
                    {
                        if (!predmeti.Exists(existingPredmet => existingPredmet.id == p.id))
                        {
                            predmeti.Add(pr);
                        }
                    }
                }
            }
        }

        return predmeti;
    }
    //-----------------------------------------------------------------UKOLIKO IZBACIM TIPA KATEDRU KADA IZBACIM PROFESORA DA LI TREBAM DA POZOVEM FUNKCIJU REMOVEKATEDRA/??????
    //----------------------REMOVE FUNKCIJE----------------------------

    //kad se izbaci katedra profesoru se dodeljuje daje na katedri sa ID-em -1??
    //STA URADITI SA IDKATEDRE KOD PROFESORA JER SE ONA ISPISUJE
    public bool RemoveKatedra(int id)
    {
        Katedra katedra = _katedre.Find(k => k.id == id);
        if (katedra != null)
        {
            _katedre.Remove(katedra);
            _katedraStorage.Save(_katedre);
            KatedreSubject.NotifyObservers();
            return true;
        }
        return false;
    }
    //kada se izbaci student brisem i njegovu ocenu iz ocena.txt i brisem iz studentpredmet red sa tim studentom
    public bool RemoveStudent(int id)
    {
        Student student = GetStudentById(id);
        if (student == null)
            return false;

        if (_ocene != null && _ocene.Count != 0)
        {
            _ocene.RemoveAll(o => o.idStudentPolozio == id);
            _ocenaStorage.Save(_ocene);
            OceneSubject.NotifyObservers();
        }
        if (_predmeti != null && _predmeti.Count != 0)
        {
            _predmeti.ForEach(p =>
            {
                if (p.poloziliPredmet != null && p.poloziliPredmet.Count != 0)
                {
                    p.poloziliPredmet.Remove(student);
                }
                else if (p.nisuPoloziliPredmet != null && p.nisuPoloziliPredmet.Count != 0)
                {
                    p.nisuPoloziliPredmet.Remove(student);
                }
            });
        }
        if (_studentpredmet != null && _studentpredmet.Count != 0)
        {
            _studentpredmet.RemoveAll(sp => sp.StudentId == id);
            _studentpredmetStorage.Save(_studentpredmet);//OVO NE RADI!!!!!!!
            StudentPredmetSubject.NotifyObservers();
        }
        _studenti.Remove(student);
        _studentStorage.Save(_studenti);
        StudentiSubject.NotifyObservers();
        return true;
    }
    // KAD SE IZBRISE PROFESOR ID PREDMETA NA KOJEM JE PREDAVAO IDE NA -1 A PROFESOR NA NULL ukoliko je bio sef katedre brisemo katedru
    // PROVERITI DA LI OVO RADI
    //DA LI ZA SVAKI REMOVE MORAM DA PITAM DA LI JE TA LISTA PRAZNA!!!!!!!!!
    public bool RemoveProfesor(int id)
    {
        Profesor profesor = _profesori.Find(f => f.id == id);
        if (profesor != null)
        {
            _katedre.ForEach(k =>
            {
                if (k.id == profesor.idKatedre)
                {
                    k.profesori.Remove(profesor);
                }
                if (k.sefId == id)
                {
                    k.sefId = -1;
                    k.sef = null;
                }
            });
            _profesori.Remove(profesor);
            _katedraStorage.Save(_katedre);
            _profesorStorage.Save(_profesori);
            KatedreSubject.NotifyObservers();
            ProfesoriSubject.NotifyObservers();
            return true;
        }
        return false;
    }
    public bool RemovePredmet(int id)
    {
        Predmet predmet = _predmeti.Find(pred => pred.id == id);
        if (predmet == null)
            return false;

        if (_studenti != null && _studenti.Count != 0)
            _studenti.ForEach(s => { if (s.polozeniIspiti.Count != 0) s.polozeniIspiti.Remove(predmet); });

        _profesori.ForEach(p => { if (p.predaje.Count != 0) p.predaje.Remove(predmet); });
        if (_studentpredmet != null && _studentpredmet.Count != 0)
        {
            _studentpredmet.RemoveAll(sp => sp.PredmetId == id);//NE RADIIII!!!!!
            _studentpredmetStorage.Save(_studentpredmet);
            StudentPredmetSubject.NotifyObservers();
        }
        if (_ocene != null && _ocene.Count != 0)
        {
            _ocene.RemoveAll(o => o.idPredmet == id);
            _ocenaStorage.Save(_ocene);
            OceneSubject.NotifyObservers();
        }
        _predmeti.Remove(predmet);
        _predmetStorage.Save(_predmeti);
        PredmetSubject.NotifyObservers();

        return true;
    }
    public bool RemoveOcena(int id)
    {
        Ocena ocena = GetOcenaById(id);
        if (ocena == null)
        {
            return false;
        }

        if (_studenti.Count != 0 && _predmeti.Count != 0)
        {
            Predmet predmet = GetPredmetById(ocena.idPredmet);
            _studenti.ForEach(s =>
            {
                //Debug.WriteLine("STUDENT ID PROVERA: " + s.id);
                if (s.id == ocena.idStudentPolozio)
                    if (s.polozeniIspiti != null && s.polozeniIspiti.Count != 0)
                    {
                        s.polozeniIspiti.Remove(predmet);
                        izracunajProsek(s.id);
                        if (s.nepolozeniIspiti == null)
                            s.nepolozeniIspiti = new List<Predmet>();
                        s.nepolozeniIspiti.Add(predmet);
                    }
            });
        }
        if (_predmeti.Count != 0 && _studenti.Count != 0)
        {
            Student student = GetStudentById(ocena.idStudentPolozio);
            _predmeti.ForEach(p =>
                {
                    if (p.id == ocena.idPredmet)
                        if (p.poloziliPredmet != null && p.poloziliPredmet.Count != 0)
                        {
                            p.poloziliPredmet.Remove(student);
                            if (p.nisuPoloziliPredmet == null)
                                p.nisuPoloziliPredmet = new List<Student>();
                            p.nisuPoloziliPredmet.Add(student);
                        }
                });
        }
        _ocene.Remove(ocena);
        _ocenaStorage.Save(_ocene);
        OceneSubject.NotifyObservers();
        return true;
    }


    public bool RemoveStudentFromPredmet(int studentId, int predmetId)
    {
        bool obrisao = false;

        foreach (Predmet p in _predmeti)
        {
            if (p.id == predmetId)
            {
                foreach (Student s in p.nisuPoloziliPredmet)
                {
                    if (s.id == studentId)
                    {
                        p.nisuPoloziliPredmet.Remove(s);
                        s.nepolozeniIspiti.Remove(p);
                        foreach (StudentPredmet sp in _studentpredmet)
                        {
                            if (sp.PredmetId == predmetId && sp.StudentId == studentId)
                            {
                                _studentpredmet.Remove(sp);
                                obrisao = true;
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }
        _studentpredmetStorage.Save(_studentpredmet);
        StudentPredmetSubject.NotifyObservers();
        PredmetSubject.NotifyObservers();
        StudentiSubject.NotifyObservers();

        return obrisao;
    }


    public bool RemovePredmetFromProfessor(int idProf, int idPredmet)
    {
        bool skinuo = false;

        foreach (Profesor p in _profesori)
        {
            if (p.id == idProf)
            {
                p.predaje.Remove(GetPredmetById(idPredmet));
                foreach (Predmet pr in _predmeti)
                {
                    if (pr.id == idPredmet)
                    {
                        pr.professorId = -1;
                        pr.profesor = null;
                        skinuo = true;
                        break;
                    }
                }
                break;
            }
        }

        _predmetStorage.Save(_predmeti);
        PredmetSubject.NotifyObservers();
        ProfesoriSubject.NotifyObservers();
        return skinuo;
    }
    //-----------------------------------------------------
    //-------------------GET FUNKCIJE----------------------
    public List<Student> GetAllStudents()
    {
        return _studenti;
    }
    public List<Profesor> GetAllProfessors()
    {
        return _profesori;
    }
    public List<Katedra> GetAllKatedre()
    {
        return _katedre;
    }
    public List<Predmet> GetAllPredmeti()
    {
        return _predmeti;
    }
    public List<Ocena> GetAllOcene()
    {
        return _ocene;
    }
    public List<StudentPredmet> GetAllStudentPredmet()
    {
        return _studentpredmet;
    }
    public Katedra? GetKatedraById(int id)
    {
        return _katedre.Find(k => k.id == id);
    }
    public Student? GetStudentById(int id)
    {
        return _studenti.Find(s => s.id == id);
    }
    public Profesor? GetProfessorById(int id)
    {
        return _profesori.Find(p => p.id == id);
    }
    public Predmet? GetPredmetById(int id)
    {
        return _predmeti.Find(p => p.id == id);
    }
    private Ocena? GetOcenaById(int id)
    {
        return _ocene.Find(o => o.id == id);
    }
    //----------------------------------------------------
    //------------------UPDATE FUNKCIJE------------------- DA LI POZVATI FUNKCIJE REMOVE I FUNKCIJE ADD PRILIKOM UPDATE-a
    public void UpdateStudent(Student student)//RADI VALJDA
    {
        _studenti.ForEach(s =>
        {
            if (s.id == student.id)
            {
                Console.WriteLine(s.ime);
                s.ime = student.ime;
                s.prezime = student.prezime;
                s.emailAdresa = student.emailAdresa;
                s.adresaStanovanja = student.adresaStanovanja;
                s.brojIndeksa = student.brojIndeksa;
                s.datumRodjena = student.datumRodjena;
                s.trenutnaGodinaStudija = student.trenutnaGodinaStudija;
                s.kontaktTelefon = student.kontaktTelefon;
                s.status = student.status;
            }
        });
        if (_predmeti != null && _predmeti.Count != 0)
        {
            _predmeti.ForEach(p =>
            {
                p.poloziliPredmet.ForEach(pred =>
                {
                    if (pred.id == student.id)
                    {
                        pred = student;

                    }
                });
                p.nisuPoloziliPredmet.ForEach(predmet =>
                {
                    if (predmet.id == student.id)
                    {
                        predmet = student;
                    }
                });
            });
            _predmetStorage.Save(_predmeti);
            PredmetSubject.NotifyObservers();
        }
        _studentStorage.Save(_studenti);
        StudentiSubject.NotifyObservers();
    }
    public void UpdateKatedra(Katedra katedra)
    {
        //NE ZNAM DA LI JE DOBRO ILI FALI JOS NEKI USLOV
        if (_profesori != null && _profesori.Count != 0)
        {
            _profesori.ForEach(p =>
            {
                if (p.id == katedra.sefId)
                {
                    katedra.sef = p;
                    //  if(_katedre.ForEach(kk=>kk.profesori.Find(kkp=>kkp.idKatedre=katedra.id))==null)
                    // katedra.profesori.Add(p);//mozda ovu liniju izbaciti jer je vec u toj listi

                }
                else
                    katedra.sef = null;

            });
            _profesorStorage.Save(_profesori);
            ProfesoriSubject.NotifyObservers();
        }
        _katedre.ForEach(k =>
        {
            if (k.id == katedra.id)
            {
                k.sifra = katedra.sifra;
                k.naziv = katedra.naziv;
                if (k.sefId != katedra.sefId)
                {
                    k.sefId = katedra.sefId;
                    k.sef = katedra.sef;
                    //if (k.profesori.Find(p => p == katedra.sef) == null)//ovu i sledecu liniju mozda treba izbaciti jer se svakako valjda nalazi u toj listi
                    //  k.profesori.Add(katedra.sef);
                }
            }
        });
        _katedraStorage.Save(_katedre);
        KatedreSubject.NotifyObservers();

    }
    public void UpdateProfessor(Profesor profesor)//RADI VALJDA
    {
        Profesor profesorstari = GetProfessorById(profesor.id);


        if (_predmeti != null && _predmeti.Count != 0)
        {
            _predmeti.ForEach(pred =>
            {
                if (pred.professorId == profesor.id)
                {
                    pred.profesor = profesor;// _profesori.Find(p => p.id == pred.professorId);
                }
            });
            _predmetStorage.Save(_predmeti);
            PredmetSubject.NotifyObservers();
        }

        if (_katedre != null && _katedre.Count != 0)
        {
            _katedre.ForEach(k =>
            {
                if (k.sefId == profesor.id && k.id != profesor.idKatedre)
                {
                    k.sefId = -1;
                    k.sef = null;
                }
                if (k.id == profesor.idKatedre)
                {
                    if (k.profesori != null && k.profesori.Count != 0)
                    {
                        if (k.profesori.Find(profesori => profesori.id == profesor.id) == null)
                            k.profesori.Add(profesor);
                    }
                    else
                    {
                        k.sefId = profesor.id;
                        k.sef = profesor;
                        k.profesori.Add(profesor);
                    }
                }
                else if (k.profesori != null && k.profesori.Count != 0)
                    k.profesori.RemoveAll(prof => prof.id == profesor.id);


            });
            _katedraStorage.Save(_katedre);
            KatedreSubject.NotifyObservers();
        }
        _profesori.ForEach(p =>
        {
            if (p.id == profesor.id)
            {
                p.ime = profesor.ime;
                p.prezime = profesor.prezime;
                p.datumRodjenja = profesor.datumRodjenja;
                p.adresaStanovanja = profesor.adresaStanovanja;
                p.kontaktTelefon = profesor.kontaktTelefon;
                p.emailAdresa = profesor.emailAdresa;
                p.brojLicneKarte = profesor.brojLicneKarte;
                p.zvanje = profesor.zvanje;
                p.godineStaza = profesor.godineStaza;
                p.idKatedre = profesor.idKatedre;
            }
        });
        _profesorStorage.Save(_profesori);
        ProfesoriSubject.NotifyObservers();

    }
    public bool UpdatePredmet(Predmet predmet)
    {
        Profesor profa = null;
        _profesori.ForEach(prof =>
        {
            if (predmet.professorId == prof.id)
                profa = prof;

        });
        _predmeti.ForEach(p =>
        {
            if (predmet.id == p.id)
            {
                p.espb = predmet.espb;
                p.professorId = predmet.professorId;
                p.sifra = predmet.sifra;
                p.semestar = predmet.semestar;
                p.naziv = predmet.naziv;
                p.godinaStudija = predmet.godinaStudija;
                p.profesor = profa;

            }
        });

        if (_studenti != null && _studenti.Count != 0)
        {
            _studenti.ForEach(s =>
            {
                if (s.polozeniIspiti != null && s.polozeniIspiti.Count != 0)
                {
                    s.polozeniIspiti.ForEach(pol =>
                    {
                        if (pol.id == predmet.id)
                        {
                            pol = predmet;
                        }
                    });
                }

                if (s.nepolozeniIspiti != null && s.nepolozeniIspiti.Count != 0)
                {
                    s.nepolozeniIspiti.ForEach(pol =>
                    {
                        if (pol.id == predmet.id)
                        {
                            pol = predmet;
                        }
                    });

                }
            });
            if (_profesori != null && _profesori.Count != 0)
            {
                _profesori.ForEach(prof =>
                {
                    prof.predaje.RemoveAll(p => p.id == predmet.id);
                    if (prof == profa)
                        prof.predaje.Add(predmet);

                });
            }

            _studentStorage.Save(_studenti);
            StudentiSubject.NotifyObservers();
        }
        _predmetStorage.Save(_predmeti);
        PredmetSubject.NotifyObservers();
        return true;
    }
    public bool UpdateOcena(Ocena ocena)
    {//NISU DODATE OSTALE VEZE
        Predmet predmetnovi = GetPredmetById(ocena.idPredmet);
        Student studentnovi = GetStudentById(ocena.idStudentPolozio);
        Ocena staraocena = GetOcenaById(ocena.id);

        _ocene.ForEach(o =>
        {
            if (o.id == ocena.id)
            {
                o.idStudentPolozio = ocena.idStudentPolozio;
                o.idPredmet = ocena.idPredmet;
                o.ocena = ocena.ocena;
                o.datumPolaganja = ocena.datumPolaganja;
            }
        });
        _predmeti.ForEach(p =>
        {
            if (p.id == staraocena.idPredmet)
            {
                p.poloziliPredmet.RemoveAll(pred => pred == GetStudentById(staraocena.idStudentPolozio));
            }
            if (p.id == ocena.idPredmet)
            {
                p.nisuPoloziliPredmet.Remove(studentnovi);
                p.poloziliPredmet.Add(studentnovi);
            }
        });
        izracunajProsek(studentnovi.id);
        _ocenaStorage.Save(_ocene);
        OceneSubject.NotifyObservers();
        return true;
    }
    //-------------REGEX FUNKCIJE------------------------------
    //---------------------------------------------------------
    public bool DatumRegex(string datum)
    {
        string pattern = @"^(0?[1-9]|[1-2][0-9]|3[0-1])\.(0?[1-9]|1[0-2])\.(19[2-9][0-9]|20([0-1][0-9]|2[0-3]))\.$";
        bool isMatch = Regex.IsMatch(datum, pattern);
        return isMatch;
    }
    public bool AdresaRegex(string adresa)
    { 
        string pattern = @"^[0-9.ČĆŠĐŽćčšđžA-Za-z\s]+ \d{1,3} [ČĆŠĐŽćčšđžA-Za-z\s]+,[ČĆŠĐŽćčšđžA-Za-z\s]+$";
            bool isMatch = Regex.IsMatch(adresa, pattern);
            return isMatch;
        }
        public bool IndexRegex(string index)
        {//RA125/2021
            string pattern = @"^[A-Z]{2} \d{1,3}\/(19[2-9][0-9]|20([0-1][0-9]|2[0-3]))$";
            bool isMatch = Regex.IsMatch(index, pattern);
            return isMatch;
        }
        //----------------------------------------------------------

        /*


            public List<Student> GetAllStudents(int page, int pageSize, string sortCriteria, SortDirection sortDirection)
            {
                IEnumerable<Student> students = _students;

                // Sortiranje studenata ako je sortCriteria naveden.
                if (!string.IsNullOrEmpty(sortCriteria))
                {
                    switch (sortCriteria)
                    {
                        case "Id":
                            students = students.OrderBy(s => s.Id);
                            break;
                        case "Name":
                            students = students.OrderBy(s => s.Name);
                            break;
                        case "LastName":
                            students = students.OrderBy(s => s.LastName);
                            break;
                            // Dodajte druge kriterijume za sortiranje po potrebi.
                    }
                }

                // Promena redosleda ako je potrebno.
                if (sortDirection == SortDirection.Descending)
                    students = students.Reverse();

                // Paginacija.
                students = students.Skip((page - 1) * pageSize).Take(pageSize);

                return students.ToList();
            }

            // Slične metode za profesore, predmete i ocene.*/
    }

