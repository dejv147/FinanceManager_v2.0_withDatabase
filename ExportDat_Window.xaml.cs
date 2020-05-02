using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Xml.Serialization;

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
   /// Třída obsahující logickou vrstvu pro vytvoření a ovládání okenního formuláře ExportDat_Window.xaml
   /// Třída slouži ke správě okna pro možnost exportovat záznamy do souboru.
   /// </summary>
   public partial class ExportDat_Window : Window
   {
      /// <summary>
      /// Instance kontroléru aplikace
      /// </summary>
      private SpravceFinanciController Controller;


      /// <summary>
      /// Konstruktor třídy pro správu okenního formuláře sloužící pro export dat.
      /// </summary>
      public ExportDat_Window()
      {
         // Inicializace okenního formuláře
         InitializeComponent();

         // Načtení instance kontroléru
         Controller = SpravceFinanciController.VratInstanciControlleru();

         // Nastavení barvy pozadí
         Background = Controller.BarvaPozadi;

         // Úvodní nastavení okna
         UvodniNastaveni();
      }


      /// <summary>
      /// Nastavení okna do režimu pro export dat včetně vytvoření potřebných tlačítek.
      /// </summary>
      private void UvodniNastaveni()
      {
         Title = "Export dat";
         Icon = new BitmapImage(new Uri(Path.Combine(Validace.VratCestuSlozkyAplikace(), "Icons\\Disketa.png")));
         Height = 600;
         Width = 450;

         // Nastavení tlačítka pro Uložení vybraných záznamů do souboru
         UlozitButton.Content = "Exportovat data";
         UlozitButton.Width = 200;
         UlozitButton.Height = 50;
         UlozitButton.FontSize = 24;
         UlozitButton.Margin = new Thickness(200, 0, 20, 20);
         UlozitButton.VerticalAlignment = VerticalAlignment.Bottom;
         UlozitButton.HorizontalAlignment = HorizontalAlignment.Left;
         UlozitButton.Foreground = Brushes.DarkGreen;
         UlozitButton.Background = Brushes.Orange;

         // Nastavení viditelnosti tlačítka pro export dat a přidání události pro možnost ragovat na kliknutí na tlačítko
         UlozitButton.Visibility = Visibility.Visible;
         UlozitButton.Click += UlozitButton_Click;

         // Nastavení tlačítka pro vyhledání záznamů k exportu
         VyhedatButton.Content = "Vyhledat";
         VyhedatButton.Width = 150;
         VyhedatButton.Height = 50;
         VyhedatButton.FontSize = 24;
         VyhedatButton.Margin = new Thickness(20, 0, 20, 20);
         VyhedatButton.VerticalAlignment = VerticalAlignment.Bottom;
         VyhedatButton.HorizontalAlignment = HorizontalAlignment.Left;
         VyhedatButton.Foreground = Brushes.DarkGreen;
         VyhedatButton.Background = Brushes.CadetBlue;

         // Nastavení viditelnosti tlačítka pro vyhledání dat a přidání události pro možnost ragovat na kliknutí na tlačítko
         VyhedatButton.Visibility = Visibility.Visible;
         VyhedatButton.Click += VyhedatButton_Click;

         // Nastavení plátna pro vykreslení seznamu vybraných záznamů určených k exportu
         SeznamZaznamuProExportCanvas.Width = 300;
         SeznamZaznamuProExportCanvas.Height = 450;
         SeznamZaznamuProExportCanvas.Margin = new Thickness(20, 20, 0, 0);
         SeznamZaznamuProExportCanvas.VerticalAlignment = VerticalAlignment.Top;
         SeznamZaznamuProExportCanvas.HorizontalAlignment = HorizontalAlignment.Left;
         SeznamZaznamuProExportCanvas.Visibility = Visibility.Visible;
         SeznamZaznamuProExportCanvas.Background = Controller.BarvaPozadi;
      }


      /// <summary>
      /// Obsluha tlačítka vyhledání požadovaných záznamů určených k exportu do souboru.
      /// </summary>
      /// <param name="sender">Zvolené tlačítko</param>
      /// <param name="e">Vyvolaná událost</param>
      private void VyhedatButton_Click(object sender, RoutedEventArgs e)
      {
         try
         {
            // Otevření okna pro možnost vyhledat konkrétní záznamy
            Controller.OtevriOknoVyhledat();

            // Opětovné vytvoření správce grafické reprezentace záznamů po zavření okna Vyhledat
            Controller.VytvorSpravuGrafickychZaznamu(SeznamZaznamuProExportCanvas, null);

            // Aktualizace vykreslení vybraných záznamů
            Controller.VykresliSeznamZaznamu();
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
         }
      }

      /// <summary>
      /// Obsluha tlačítka pro provedení exportu dat do souboru.
      /// </summary>
      /// <param name="sender">Zvolené tlačítko</param>
      /// <param name="e">Vyvolaná událost</param>
      private void UlozitButton_Click(object sender, RoutedEventArgs e)
      {
         // Export vybraných záznamů do souboru
         Controller.ExportujZaznamy();

         // Zavření okna
         Close();
      }

   }
}
