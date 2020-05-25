using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpravceFinanci_v2.Database;

/// <summary>
/// Aplikace pro správu financí určena pouze pro osobní užití.
/// ----------------------------------------------------------
/// Aplikace pracuje vždy pouze s daty patřící přihlášenému uživateli. 
/// Data jsou uložena na lokálním databázovém serveru, kde jsou jednotlivé prvky uloženy v konkrétních tabulkách. 
/// Díky Entity Frameworku jsou prvky z různých tabulek spojeny referencemi, díky tomu jem možné pracovat vždy pouze s daty přihlášeného uživatele, 
/// kdy reference na konkrétního uživatele je v každé podmínce definující selekci (výběr) potřebných dat.
/// Vždy při přihlášení se z databáze načte instance konkrétního uživatele, který je uchováván v kontroléru aplikace.
/// 
/// Aplikace implementuje zjednodušenou strukturu MVC architektury, kdy je aplikace rozdělena do 3 sekcí. 
/// Třídy View jsou rozděleny na pohledy psané v XAML kódu a slouží pro zobrazení dat v okenním formuláři a třídy obsluhující dané pohledy, které slouží k nastavení okenních formulářů a načtení dat určených k zobrazení.
/// Třídy Models jsou funkční třídy které uchovávají různé funkce a metody, které jsou využity pro zpracování dat, provedení různých úkonů pro zajištění správného chodu aplikace a předání dat určených k zobrazení uživateli.
/// Třídy Controllers slouží k propojení pohledů a funkčních tříd. Zprostředkovává komunikaci, předávání dat a požadavků mezi jednotlivými třídami a uchovává metody pro zobrazování oken aplikace.
/// 
/// V hlavním okně aplikace je zobrazen stručný přehled a je zde uživatelské rozhraní pro správu aplikace i pro možnost využití dalších funkcí aplikace pracujících v samostatných oknech.
/// V úvodu je otevřeno okno pro přihlášení uživatele a po úspěšném přihlášení je zobrazeno hlavní okno aplikace, které je stále otevřeno při chodu aplikace. Po zavření hlavního okna je aplikace ukončena.
/// 
/// 
/// Autor projektu: Bc. David Halas
/// Link publikovaného projektu: https://github.com/dejv147/FinanceManager_v2.0_withDatabase
/// </summary>
namespace SpravceFinanci_v2
{
   /// <summary>
   /// Třída slouží pro komunikaci s databází a zpracování dat uložených v databázi. 
   /// Pro komunikaci je využit Entity Framework, který využívá technologii LINQ to SQL a tabulkám databáze přistupuje jako ke kolekcím objektů. 
   /// Proto lze k datům v databázi přistupovat stejně jako k objektů v kolekci programu a pracovat s ní. 
   /// </summary>
   class Databaze
   {
      /// <summary>
      /// Instance kontroléru aplikace
      /// </summary>
      private SpravceFinanciController Controller;

      /// <summary>
      /// Instance databáze pro práci s daty.
      /// </summary>
      public DB_FinanceDataEntities1 DB_data { get; private set; }



      /// <summary>
      /// Konstruktor třídy pro správu databáze.
      /// </summary>
      public Databaze()
      {
         // Uložení instance kontroléru
         Controller = SpravceFinanciController.VratInstanciControlleru();

         // Vytvoření instance databáze
         DB_data = new DB_FinanceDataEntities1();
      }




      //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      /* Metody pro práci s kategoriemy */

      /// <summary>
      /// Metoda pro uložení nové kategorie do databáze (přidání nové kategorie do tabulky kategorií)
      /// </summary>
      /// <param name="Nazev">Název kategorie bez diakritiky (slouží pouze pro zpracování v programu)</param>
      /// <param name="Popis">Název kategorie s diakritikou (viditelné pro uživatele)</param>
      public void VytvorNovouKategorii(string Nazev, string Popis)
      {
         // Kontrola zda nově vytvářená kategorie v databázi již neexistuje
         if (DB_data.Categories.First(c => c.Name == Nazev && c.Description == Popis) != null)
            return;

         // Vytvoření nové kategorie
         Category newCategory = new Category();
         newCategory.Name = Nazev;
         newCategory.Description = Popis;

         // Přidání nové kategorie do tabulky v databázi
         DB_data.Categories.Add(newCategory);

         // Uložení provedených změn v databázi
         DB_data.SaveChanges();
      }

      /// <summary>
      /// Vrátí počet prvků v tabulce kategorií.
      /// </summary>
      /// <returns>Počet kategorií</returns>
      public int VratPocetKategorii()
      {
         // Zjištění celkového počtu prvků v tabulce kategorií
         int Pocet = DB_data.Categories.Count();

         // Návratová hodnota
         return Pocet;
      }

      /// <summary>
      /// Vrátí kolekci všech kategorií v tabulce kategorií.
      /// </summary>
      /// <returns>Kolekce všech kategorií</returns>
      public List<Category> VratKolekciKategorii()
      {
         // Pomocná proměnná
         List<Category> KolekceKategorii = new List<Category>();

         // Načtení všech prvků z tabulky kategorií
         foreach (Category kategorie in DB_data.Categories)
         {
            KolekceKategorii.Add(kategorie);
         }

         // Návratová hodnota
         return KolekceKategorii;
      }

      /// <summary>
      /// Vrácení celé kategorie na základě jejího ID.
      /// </summary>
      /// <param name="ID_Kategorie">ID požadované kategorie</param>
      /// <returns>Kategorie</returns>
      public Category VratKategorii(int ID_Kategorie)
      {
         // Vybrání kategorie na základě identifikace pomocí ID
         var Kategorie = from category in DB_data.Categories
                         where category.Id == ID_Kategorie
                         select category;

         return Kategorie.First();
      }





      //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      /* Metody pro práci s uživateli */

      /// <summary>
      /// Metoda pro uložení nového uživatele do databáze.
      /// </summary>
      /// <param name="Jmeno">Jméno uživatele</param>
      /// <param name="Heslo">Heslo uživatele</param>
      public void VytvorNovehoUzivatele(string Jmeno, string Heslo)
      {
         // Vytvoření nového uživatele
         User newUser = new User();
         newUser.Name = Jmeno;
         newUser.Password = Heslo;
         newUser.Note = "";
         newUser.NoteOnDisplay = false;

         // Přidání nového uživatele do tabulky v databázi
         DB_data.Users.Add(newUser);

         // Uložení provedených změn
         DB_data.SaveChanges();
      }

      /// <summary>
      /// Metoda pro editaci existujícího uživatele.
      /// </summary>
      /// <param name="ID">ID uživatele určeného k úpravě</param>
      /// <param name="Jmeno">Jméno uživatele</param>
      /// <param name="Heslo">Heslo uživatele</param>
      /// <param name="Poznamka">Text v poznámkovém bloku</param>
      /// <param name="ZobrazitPoznamkovyBlok">TRUE - poznámkový blok je určen k zobrazení, FALSE - poznámkový blok je určen ke skrytí</param>
      public void UpravUzivatele(int ID, string Jmeno, string Heslo, string Poznamka, bool ZobrazitPoznamkovyBlok)
      {
         // Nalezení instance uživatele v databázi
         User Uzivatel = DB_data.Users.First(u => u.Id == ID);

         // Provedení změn parametrů uživatele
         Uzivatel.Name = Jmeno;
         Uzivatel.Password = Heslo;
         Uzivatel.Note = Poznamka;
         Uzivatel.NoteOnDisplay = ZobrazitPoznamkovyBlok;

         // Uložení provedených změn
         DB_data.SaveChanges();
      }

      /// <summary>
      /// Metoda zkontroluje zda v databázi již neexistuje uživatel se stejným jménem a heslem.
      /// </summary>
      /// <param name="Jmeno">Jméno kontrolovaného uživatele</param>
      /// <param name="Heslo">Heslo kontrolovaného uživatele</param>
      /// <returns>TRUE - uživatel již existuje, FALSE - uživatel ještě neexistuje</returns>
      public bool KontrolaExistujicihoUzivatele(string Jmeno, string Heslo)
      {
         // Kontrola existujícího uživatele v kolekci uživatelů
         foreach (User uzivazel in DB_data.Users)
         {
            if ((uzivazel.Name == Jmeno) && (uzivazel.Password == Heslo))
               return true;
         }

         // Pokud ke shodě nedojde, vrátí se FALSE (shoda nenalezena)
         return false;
      }

      /// <summary>
      /// Vrátí kolekci všech uživatelů v databázi.
      /// </summary>
      /// <returns>Kolekce uživatelů</returns>
      public List<User> VratKolekciUzivatelu()
      {
         // Pomocná proměnná
         List<User> KolekceUzivatelu = new List<User>();

         // Načtení všech prvků z tabulky uživatelů
         foreach (User user in DB_data.Users)
         {
            KolekceUzivatelu.Add(user);
         }

         // Návratová hodnota
         return KolekceUzivatelu;
      }




      //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      /* Metody pro vyhledávání záznamů */

      /// <summary>
      /// Metoda pro sestupné seřazení záznamů dle data.
      /// </summary>
      /// <param name="Transakce">Kolekce záznamů</param>
      /// <returns>Seřazená kolekce záznamů</returns>
      public static ObservableCollection<Transaction> SeradZaznamy(ObservableCollection<Transaction> Transakce)
      {
         // LINQ dotaz pro sestupné seřazení transakcí dle data 
         var Razeni = from transakce in Transakce
                      orderby transakce.Date descending
                      select transakce;

         // Vytvoření kolekce pro návratovou hodnotu za účelem převedení výstupu z LINQ dotazu na potřebný datový typ
         ObservableCollection<Transaction> Kolekce = new ObservableCollection<Transaction>();
         foreach (var prvek in Razeni)
         {
            Kolekce.Add((Transaction)prvek);
         }

         // Návratová hodnota vrací vybraná data jako kolekci transakcí
         return Kolekce;
      }

      /// <summary>
      /// Výběr všech záznamů patřící konkrétnímu uživateli.
      /// </summary>
      /// <param name="ID_uzivatele">ID uživatele, jehož záznamy jsou hledány</param>
      /// <returns>Kolekce záznamů</returns>
      public ObservableCollection<Transaction> VratKolekciZaznamuKonkretnihoUzivatele(int ID_uzivatele)
      {
         // Pomocná proměnná
         ObservableCollection<Transaction> KolekceZaznamu = new ObservableCollection<Transaction>();

         // Načtení všech záznamů konkrétního uživatele
         var Zaznamy = from zaznam in DB_data.Transactions
                       where (zaznam.User == ID_uzivatele)
                       orderby zaznam.Date descending
                       select zaznam;

         // Přidání načtených záznamů do pomocné kolekce
         foreach (var zaznam in Zaznamy)
         {
            KolekceZaznamu.Add((Transaction)zaznam);
         }

         // Návratová hodnota
         return KolekceZaznamu;
      }

      /// <summary>
      /// Výběr všech záznamů z aktuálního měsíce patřící konkrétnímu uživateli.
      /// </summary>
      /// <param name="ID_Uzivatele">ID uživatele, jehož záznamy se vyhledávají</param>
      /// <returns>Kolekce vyhledaných záznamů</returns>
      public ObservableCollection<Transaction> VratZaznamyAktualnihoMesice(int ID_Uzivatele)
      {
         // Pomocná proměnná
         ObservableCollection<Transaction> KolekceZaznamu = new ObservableCollection<Transaction>();

         // LINQ dotaz pro vyhledání potřebných záznamů
         var Zaznamy = from zaznam in DB_data.Transactions
                       where ((zaznam.User == ID_Uzivatele) && (zaznam.Date.Month == DateTime.Now.Month))
                       orderby zaznam.Date descending
                       select zaznam;

         // Přidání načtených záznamů do pomocné kolekce
         foreach (var zaznam in Zaznamy)
         {
            KolekceZaznamu.Add((Transaction)zaznam);
         }

         // Návratová hodnota
         return KolekceZaznamu;
      }

      /// <summary>
      /// Výběr všech záznamů spadajících do kategorie příjmů, nebo výdajů.
      /// </summary>
      /// <param name="ID_Uzivatele">ID uživatele, jehož záznamy se vyhledávají</param>
      /// <param name="PrijmyNeboVydaje">TRUE - Vrátí všechny příjmy, FALSE - Vrátí všechny výdaje</param>
      /// <returns>Kolekce vybraných záznamů</returns>
      public ObservableCollection<Transaction> VratVsechnyPrijmyNeboVydaje(int ID_Uzivatele, bool PrijmyNeboVydaje)
      {
         // LINQ dotaz pro výběr všech záznamů splňující potřebné podmínky
         var Prijmy = from zaznam in DB_data.Transactions
                      where ((zaznam.User == ID_Uzivatele) && (zaznam.Income == PrijmyNeboVydaje))
                      orderby zaznam.Date descending
                      select zaznam;

         // Vytvoření kolekce pro návratovou hodnotu za účelem převedení výstupu z LINQ dotazu na potřebný datový typ
         ObservableCollection<Transaction> Kolekce = new ObservableCollection<Transaction>();
         foreach (var prvek in Prijmy)
         {
            Kolekce.Add((Transaction)prvek);
         }

         // Návratová hodnota vrací vybraná data jako kolekci záznamů
         return Kolekce;
      }

      /// <summary>
      /// Výběr všech záznamů s odpovídajícím názvem.
      /// </summary>
      /// <param name="ID_Uzivatele">ID uživatele, jehož záznamy se vyhledávají</param>
      /// <param name="Nazev"></param>
      /// <returns>Kolekce vybraných záznamů</returns>
      public ObservableCollection<Transaction> VratZaznamyDleNazvu(int ID_Uzivatele, string Nazev)
      {
         // LINQ dotaz pro výběr všech záznamů splňující potřebné podmínky
         var ZaznamyDleNazvu = from zaznam in DB_data.Transactions
                               where ((zaznam.User == ID_Uzivatele) && (zaznam.Name == Nazev))
                               select zaznam;

         // Vytvoření kolekce pro návratovou hodnotu za účelem převedení výstupu z LINQ dotazu na potřebný datový typ
         ObservableCollection<Transaction> Kolekce = new ObservableCollection<Transaction>();
         foreach (var prvek in ZaznamyDleNazvu)
         {
            Kolekce.Add((Transaction)prvek);
         }

         // Návratová hodnota vrací vybraná data jako kolekci záznamů
         return Kolekce;
      }

      /// <summary>
      /// Výběr všech záznamů spadající do požadované kategorie.
      /// </summary>
      /// <param name="ID_Uzivatele">ID uživatele, jehož záznamy se vyhledávají</param>
      /// <param name="ID_Kategorie">ID požadované kategorie</param>
      /// <returns>Kolekce vybraných záznamů</returns>
      public ObservableCollection<Transaction> VratZaznamyDleKategorie(int ID_Uzivatele, int ID_Kategorie)
      {
         // LINQ dotaz pro výběr všech záznamů splňující potřebné podmínky
         var ZaznamyDleKategorie = from zaznam in DB_data.Transactions
                                   where ((zaznam.User == ID_Uzivatele) && (zaznam.Category == ID_Kategorie))
                                   select zaznam;

         // Vytvoření kolekce pro návratovou hodnotu za účelem převedení výstupu z LINQ dotazu na potřebný datový typ
         ObservableCollection<Transaction> Kolekce = new ObservableCollection<Transaction>();
         foreach (var prvek in ZaznamyDleKategorie)
         {
            Kolekce.Add((Transaction)prvek);
         }

         // Návratová hodnota vrací vybraná data jako kolekci záznamů
         return Kolekce;
      }

      /// <summary>
      /// Výběr všech záznamů s datem v požadovaném období.
      /// </summary>
      /// <param name="ID_Uzivatele">ID uživatele, jehož záznamy se vyhledávají</param>
      /// <param name="PocatecniDatum">Spodní hranice pro vyhledání</param>
      /// <param name="KoncoveDatum">Horní hranice pro vyhledání</param>
      /// <returns>Kolekce vybraných záznamů</returns>
      public ObservableCollection<Transaction> VratZaznamyVCasovemRozmezi(int ID_Uzivatele, DateTime PocatecniDatum, DateTime KoncoveDatum)
      {
         // LINQ dotaz pro výběr všech záznamů splňující potřebné podmínky
         var ZaznamyDleCasu = from zaznam in DB_data.Transactions
                              where ((zaznam.User == ID_Uzivatele) && (zaznam.Date >= PocatecniDatum) && (zaznam.Date <= KoncoveDatum))
                              select zaznam;

         // Vytvoření kolekce pro návratovou hodnotu za účelem převedení výstupu z LINQ dotazu na potřebný datový typ
         ObservableCollection<Transaction> Kolekce = new ObservableCollection<Transaction>();
         foreach (var prvek in ZaznamyDleCasu)
         {
            Kolekce.Add((Transaction)prvek);
         }

         // Návratová hodnota vrací vybraná data jako kolekci záznamů
         return Kolekce;
      }

      /// <summary>
      /// Výběr všech záznamů s hodnotou v požadovaném intervalu.
      /// </summary>
      /// <param name="ID_Uzivatele">ID uživatele, jehož záznamy se vyhledávají</param>
      /// <param name="MIN">Minimální hodnota pro vyhledání</param>
      /// <param name="MAX">Maximální hodnota pro vyhledání</param>
      /// <returns>Kolekce vybraných záznamů</returns>
      public ObservableCollection<Transaction> VratZaznamyVRozmeziHodnot(int ID_Uzivatele, double MIN, double MAX)
      {
         // LINQ dotaz pro výběr všech záznamů splňující potřebné podmínky
         var ZaznamyDleHodnoty = from zaznam in DB_data.Transactions
                                 where ((zaznam.User == ID_Uzivatele) && (zaznam.Price >= (decimal)MIN) && (zaznam.Price <= (decimal)MAX))
                                 select zaznam;

         // Vytvoření kolekce pro návratovou hodnotu za účelem převedení výstupu z LINQ dotazu na potřebný datový typ
         ObservableCollection<Transaction> Kolekce = new ObservableCollection<Transaction>();
         foreach (var prvek in ZaznamyDleHodnoty)
         {
            Kolekce.Add((Transaction)prvek);
         }

         // Návratová hodnota vrací vybraná data jako kolekci záznamů
         return Kolekce;
      }

      /// <summary>
      /// Výběr všech záznamů s počtem položek v požadovaném intervalu.
      /// </summary>
      /// <param name="ID_Uzivatele">ID uživatele, jehož záznamy se vyhledávají</param>
      /// <param name="MIN">Minimální počet položek</param>
      /// <param name="MAX">Maximální počet položek</param>
      /// <returns>Kolekce vybraných záznamů</returns>
      public ObservableCollection<Transaction> VratZaznamyDlePoctuPolozek(int ID_Uzivatele, int MIN, int MAX)
      {
         // LINQ dotaz pro výběr všech záznamů splňující potřebné podmínky
         var ZaznamyDlePolozek = from zaznam in DB_data.Transactions
                                 where ((zaznam.User == ID_Uzivatele) && (VratPocetPolozekZaznamu(zaznam.Id) >= MIN) && (VratPocetPolozekZaznamu(zaznam.Id) <= MAX))
                                 select zaznam;

         // Vytvoření kolekce pro návratovou hodnotu za účelem převedení výstupu z LINQ dotazu na potřebný datový typ
         ObservableCollection<Transaction> Kolekce = new ObservableCollection<Transaction>();
         foreach (var prvek in ZaznamyDlePolozek)
         {
            Kolekce.Add((Transaction)prvek);
         }

         // Návratová hodnota vrací vybraná data jako kolekci záznamů
         return Kolekce;
      }



      //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      /* Metody pro práci se záznamy */

      /// <summary>
      /// Metoda pro uložení nového záznamu do databáze.
      /// </summary>
      /// <param name="ID_Uzivatele">ID uživatele, kterému patří nový záznam</param>
      /// <param name="Nazev">Název záznamu</param>
      /// <param name="Cena">Hodnota záznamu</param>
      /// <param name="Poznamka">Textová poznámka</param>
      /// <param name="ID_Kategorie">ID kategorie, do které záznam spadá</param>
      /// <param name="PrijemNeboVydaj">TRUE = příjem, FALSE = výdaj</param>
      /// <param name="Datum">Datum záznamu</param>
      public Transaction VytvorNovyZaznam(int ID_Uzivatele, string Nazev, decimal Cena, string Poznamka, int ID_Kategorie, bool PrijemNeboVydaj, DateTime Datum)
      {
         // Vytvoření nového záznamu
         Transaction newTransaction = new Transaction
         {
            User = ID_Uzivatele,
            Category = ID_Kategorie,
            Name = Nazev,
            Price = Cena,
            Note = Poznamka,
            Income = PrijemNeboVydaj,
            Date = Datum,
            Creation_Date = DateTime.Now
         };

         // Přidání nového záznamu do tabulky v databázi
         DB_data.Transactions.Add(newTransaction);

         // Uložení provedených změn
         DB_data.SaveChanges();

         // Vrácení instance nově vytvořeného záznamu v databázi
         return newTransaction;
      }

      /// <summary>
      /// Uložení předaného záznamu do databáze jako nový záznam.
      /// </summary>
      /// <param name="NovyZaznam">Instance nového záznamu</param>
      public void VytvorNovyZaznam(Transaction NovyZaznam)
      {
         // Přidání nového záznamu do tabulky v databázi
         DB_data.Transactions.Add(NovyZaznam);

         // Přidání položek nového záznamu do tabulky v databázi
         foreach (Item polozka in NovyZaznam.Items)
         {
            VytvorNovouPolozku(NovyZaznam.Id, polozka.Name, polozka.Price, polozka.Note, polozka.Category);
         }

         // Uložení provedených změn
         DB_data.SaveChanges();
      }

      /// <summary>
      /// Metoda pro editaci existujícího záznamu
      /// </summary>
      /// <param name="ID">ID záznamu určeného k úpravě</param>
      /// <param name="Nazev">Název záznamu</param>
      /// <param name="Cena">Hodnota záznamu</param>
      /// <param name="Poznamka">Textová poznámka</param>
      /// <param name="ID_Kategorie">ID kategorie, do které záznam spadá</param>
      /// <param name="PrijemNeboVydaj">TRUE = příjem, FALSE = výdaj</param>
      /// <param name="Datum">Datum záznamu</param>
      public void UpravZaznam(int ID, string Nazev, decimal Cena, string Poznamka, int ID_Kategorie, bool PrijemNeboVydaj, DateTime Datum)
      {
         // Nalezení instance záznamu v databázi
         Transaction Zaznam = DB_data.Transactions.First(z => z.Id == ID);

         // Provedení změn parametrů záznamu
         Zaznam.Name = Nazev;
         Zaznam.Price = Cena;
         Zaznam.Note = Poznamka;
         Zaznam.Category = ID_Kategorie;
         Zaznam.Income = PrijemNeboVydaj;
         Zaznam.Date = Datum;

         // Uložení provedených změn
         DB_data.SaveChanges();
      }

      /// <summary>
      /// Metoda pro smazání konkrétního záznamu z databáze.
      /// </summary>
      /// <param name="zaznam">Záznam určen ke smazání</param>
      public void OdstranZaznam(Transaction zaznam)
      {
         // Smazání zvoleného záznamu z databáze
         DB_data.Transactions.Remove(zaznam);

         // Smazání všech položek patřící danému záznamu
         SmazVsechnyPolozky(zaznam.Id);

         // Uložení provedených změn
         DB_data.SaveChanges();
      }

      /// <summary>
      /// Metoda pro smazání všech záznamů konkrétního uživatele včetně vymazání všech položek patřících daným záznamům.
      /// </summary>
      /// <param name="ID_Uzivatele">ID uživatele, jehož záznamy budou smazány</param>
      public void SmazVsechnyZaznamy(int ID_Uzivatele)
      {
         // Postupné procházení všech záznamů v databázi
         foreach (Transaction zaznam in DB_data.Transactions)
         {
            // Kontrola zda záznam patří zvolenému uživateli
            if (zaznam.User == ID_Uzivatele)
            {
               // Smazání všech položek patřící danému záznamu
               SmazVsechnyPolozky(zaznam.Id);

               // Smazání daného záznamu
               DB_data.Transactions.Remove(zaznam);
            }
         }

         // Uložení provedených změn
         DB_data.SaveChanges();
      }




      //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      /* Metody pro práci s položkami */

      /// <summary>
      /// Metoda pro uložení nové položky do databáze.
      /// </summary>
      /// <param name="ID_Zaznamu">ID záznamu, ke kterému patří nová položka</param>
      /// <param name="Nazev">Název položky</param>
      /// <param name="Cena">Hodnota položky</param>
      /// <param name="Poznamka">Textová poznámka</param>
      /// <param name="ID_Kategorie">ID kategorie, do které položka spadá</param>
      public void VytvorNovouPolozku(int ID_Zaznamu, string Nazev, decimal Cena, string Poznamka, int ID_Kategorie)
      {
         // Vytvoření nové položky
         Item newItem = new Item
         {
            Name = Nazev,
            Note = Poznamka,
            Price = Cena,
            Transaction = ID_Zaznamu,
            Category = ID_Kategorie
         };

         // Přidání nové položky do tabulky v databázi
         DB_data.Items.Add(newItem);

         // Uložení provedených změn
         DB_data.SaveChanges();
      }

      /// <summary>
      /// Metoda pro smazání konkrétní položky z databáze.
      /// </summary>
      /// <param name="polozka">Položka určena ke smazání</param>
      public void OdstranPolozku(Item polozka)
      {
         // Smazání konkrétní položky z databáze
         DB_data.Items.Remove(polozka);

         // Uložení provedených změn
         DB_data.SaveChanges();
      }

      /// <summary>
      /// Metoda pro vyhledání všech položek patřící konkrétnímu záznamu, identifikovanému na základě jeho ID.
      /// </summary>
      /// <param name="ID_Zaznamu">ID záznamu, kterému patří vyhledávané položky</param>
      /// <returns>Kolekce položek patřící konrétnímu záznamu</returns>
      public ObservableCollection<Item> VratVsechnyPolozkyZaznamu(int ID_Zaznamu)
      {
         // LINQ dotaz pro výběr všech položek patřící konkrétnímu záznamu identifikovatelnému pomocí jeho ID
         var items = from item in DB_data.Items
                     where (item.Transaction == ID_Zaznamu)
                     select item;

         // Pomocné proměnné
         ObservableCollection<Item> KolekcePolozek = new ObservableCollection<Item>();

         // Převední vybraných položek na kolekci položek v interním formátu
         foreach (var item in items)
         {
            KolekcePolozek.Add((Item)item);
         }

         // Návratová hodnota
         return KolekcePolozek;
      }

      /// <summary>
      /// Metoda pro získání počtu položek patřící konkrétnímu záznamu.
      /// </summary>
      /// <param name="ID_Zaznamu">ID záznamu, kterému patří vyhledávané položky</param>
      /// <returns>Počet položek požadovaného záznamu</returns>
      public int VratPocetPolozekZaznamu(int ID_Zaznamu)
      {
         // LINQ dotaz pro výběr všech položek patřící konkrétnímu záznamu identifikovatelnému pomocí jeho ID
         var items = from item in DB_data.Items
                     where (item.Transaction == ID_Zaznamu)
                     select item.Id;

         // Návratová hodnota udávající počet prvků v získané kolekci
         return items.Count();
      }

      /// <summary>
      /// Smazání všech položek patřící konkrétnímu záznamu.
      /// </summary>
      /// <param name="ID_Zazanamu">ID záznamu, jehož položky budou smazány</param>
      public void SmazVsechnyPolozky(int ID_Zazanamu)
      {
         // Postupné procházení všech položek v databázi
         foreach (Item polozka in DB_data.Items)
         {
            // Kontrola zda položka patří zvolenému záznamu
            if (polozka.Transaction == ID_Zazanamu)
            {
               // Smazání dané položky
               DB_data.Items.Remove(polozka);
            }
         }

         // Uložení provedených změn
         DB_data.SaveChanges();
      }



   }
}
