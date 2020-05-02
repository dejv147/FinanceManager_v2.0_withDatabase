/// <summary>
/// Aplikace pro správu financí urèena pouze pro osobní užití.
/// ----------------------------------------------------------
/// Aplikace pracuje vždy pouze s daty patøící pøihlášenému uživateli. 
/// Data jsou uložena na lokálním databázovém serveru, kde jsou jednotlivé prvky uloženy v konkrétních tabulkách. 
/// Díky Entity Frameworku jsou prvky z rùzných tabulek spojeny referencemi, díky tomu jem možné pracovat vždy pouze s daty pøihlášeného uživatele, 
/// kdy reference na konkrétního uživatele je v každé podmínce definující selekci (výbìr) potøebných dat.
/// Vždy pøi pøihlášení se z databáze naète instance konkrétního uživatele, který je uchováván v kontroléru aplikace.
/// 
/// Aplikace implementuje zjednodušenou strukturu MVC architektury, kdy je aplikace rozdìlena do 3 sekcí. 
/// Tøídy View jsou rozdìleny na pohledy psané v XAML kódu a slouží pro zobrazení dat v okenním formuláøi a tøídy obsluhující dané pohledy, které slouží k nastavení okenních formuláøù a naètení dat urèených k zobrazení.
/// Tøídy Models jsou funkèní tøídy které uchovávají rùzné funkce a metody, které jsou využity pro zpracování dat, provedení rùzných úkonù pro zajištìní správného chodu aplikace a pøedání dat urèených k zobrazení uživateli.
/// Tøídy Controllers slouží k propojení pohledù a funkèních tøíd. Zprostøedkovává komunikaci, pøedávání dat a požadavkù mezi jednotlivými tøídami a uchovává metody pro zobrazování oken aplikace.
/// 
/// V hlavním oknì aplikace je zobrazen struèný pøehled a je zde uživatelské rozhraní pro správu aplikace i pro možnost využití dalších funkcí aplikace pracujících v samostatných oknech.
/// V úvodu je otevøeno okno pro pøihlášení uživatele a po úspìšném pøihlášení je zobrazeno hlavní okno aplikace, které je stále otevøeno pøi chodu aplikace. Po zavøení hlavního okna je aplikace ukonèena.
/// 
/// 
/// Autor projektu: Bc. David Halas
/// Link publikovaného projektu: https://github.com/dejv147/FinanceManager_v2.0_withDatabase
/// </summary>
namespace SpravceFinanci_v2.Database
{
    using System;
    using System.Collections.Generic;
   using System.ComponentModel.DataAnnotations;
   using System.ComponentModel.DataAnnotations.Schema;
   using System.Data.Entity.Spatial;

   [Table("Transactions")]
   public partial class Transaction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Transaction()
        {
            this.Items = new HashSet<Item>();
        }
    
        public int Id { get; set; }

      [Required]
      [StringLength(300)]
      public string Name { get; set; }

        public decimal Price { get; set; }

      [StringLength(10000)]
      public string Note { get; set; }

        public int Category { get; set; }

        public bool Income { get; set; }

        public System.DateTime Date { get; set; }

        public System.DateTime Creation_Date { get; set; }

        public int User { get; set; }
    
        public virtual Category Category1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Item> Items { get; set; }
        public virtual User User1 { get; set; }
    }
}
