using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibraryMS.Entity
{
    public class Book
    {

        [Display(Name = "ID")]
        [Key]
        public int BookId {get; set;}

        [Display(Name = "Fotoğraf")]
        public string? Image { get; set; }

        public required string ISBN { get; set; }

        [Display(Name = "Kitap Adı")]
        public  required string BookName { get; set; }

        [Display(Name = "Yazar")]
        public required string Author {get; set;}

        [Display(Name = "Yayınevi")]
        public string? Publisher { get; set; }

        [Display(Name = "Tür")]
        public string? Genre { get; set; }

        [Display(Name = "Sayfa Sayısı")]
        public int Pages { get; set; }

        [Display(Name = "Basım Yılı")]
        public int? PublicationYear { get; set; }

        [Display(Name = "Dil")]
        public string? Language { get; set; }

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Display(Name = "Durum")]
        public bool IsAvailable { get; set; }

    }
}