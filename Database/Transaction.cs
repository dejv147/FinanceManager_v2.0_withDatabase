/// <summary>
/// Aplikace pro spr�vu financ� ur�ena pouze pro osobn� u�it�.
/// ----------------------------------------------------------
/// Aplikace pracuje v�dy pouze s daty pat��c� p�ihl�en�mu u�ivateli. 
/// Data jsou ulo�ena na lok�ln�m datab�zov�m serveru, kde jsou jednotliv� prvky ulo�eny v konkr�tn�ch tabulk�ch. 
/// D�ky Entity Frameworku jsou prvky z r�zn�ch tabulek spojeny referencemi, d�ky tomu jem mo�n� pracovat v�dy pouze s daty p�ihl�en�ho u�ivatele, 
/// kdy reference na konkr�tn�ho u�ivatele je v ka�d� podm�nce definuj�c� selekci (v�b�r) pot�ebn�ch dat.
/// V�dy p�i p�ihl�en� se z datab�ze na�te instance konkr�tn�ho u�ivatele, kter� je uchov�v�n v kontrol�ru aplikace.
/// 
/// Aplikace implementuje zjednodu�enou strukturu MVC architektury, kdy je aplikace rozd�lena do 3 sekc�. 
/// T��dy View jsou rozd�leny na pohledy psan� v XAML k�du a slou�� pro zobrazen� dat v okenn�m formul��i a t��dy obsluhuj�c� dan� pohledy, kter� slou�� k nastaven� okenn�ch formul��� a na�ten� dat ur�en�ch k zobrazen�.
/// T��dy Models jsou funk�n� t��dy kter� uchov�vaj� r�zn� funkce a metody, kter� jsou vyu�ity pro zpracov�n� dat, proveden� r�zn�ch �kon� pro zaji�t�n� spr�vn�ho chodu aplikace a p�ed�n� dat ur�en�ch k zobrazen� u�ivateli.
/// T��dy Controllers slou�� k propojen� pohled� a funk�n�ch t��d. Zprost�edkov�v� komunikaci, p�ed�v�n� dat a po�adavk� mezi jednotliv�mi t��dami a uchov�v� metody pro zobrazov�n� oken aplikace.
/// 
/// V hlavn�m okn� aplikace je zobrazen stru�n� p�ehled a je zde u�ivatelsk� rozhran� pro spr�vu aplikace i pro mo�nost vyu�it� dal��ch funkc� aplikace pracuj�c�ch v samostatn�ch oknech.
/// V �vodu je otev�eno okno pro p�ihl�en� u�ivatele a po �sp�n�m p�ihl�en� je zobrazeno hlavn� okno aplikace, kter� je st�le otev�eno p�i chodu aplikace. Po zav�en� hlavn�ho okna je aplikace ukon�ena.
/// 
/// 
/// Autor projektu: Bc. David Halas
/// Link publikovan�ho projektu: https://github.com/dejv147/FinanceManager_v2.0_withDatabase
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
