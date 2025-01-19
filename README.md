# LIBRARY MANAGEMENT SYSTEM

Bu projenin amacı, kütüphane yönetim süreçlerini dijitalleştirerek kullanıcı dostu bir sistem oluşturmaktır.
<br>
⁛ Admin tarafı: Kitap ekleme, güncelleme, silme ve kullanıcı işlemlerini yönetme gibi işlevleri içerir.
<br>
⁛ Kullanıcı tarafı: Kullanıcıların kütüphanede mevcut olan kitapları görüntüleyebileceği bir platform sağlamaktır.
  
------------------------------------------------------------------------------------------------------

## Projemde Kullandığım Tool ve Frameworkler

⁛Projemde backend kısmı için .NET Core MVC kullandım.
<br>
⁛Veritabanı olarak, hem hafif hem de kullanımı kolay olduğu için SQLite (Version 9.0.0) tercih ettim. 
<br>
⁛Frontend kısmında ise HTML, CSS ve Bootstrap5 kullanarak kullanıcı dostu ve responsive bir arayüz tasarladım. 
<br>
⁛Geliştirme sürecinde Visual Studio Code’u kullandım çünkü kod yazarken bana esneklik ve hız sağladı. 
<br>
⁛Bu teknolojilerle, hem performanslı hem de kullanıcıların kolayca anlayıp kullanabileceği bir sistem geliştirmeyi amaçladım.
<br>
Microsoft.EntityFrameworkCore.Sqlite --version 9.0.0
<br>
Microsoft.EntityFrameworkCore.Design --version 9.0.0


------------------------------------------------------------------------------------------------------------

## Kod Yapısı

⁛Projemde .NET Core MVC yapısını kullanarak kitap yönetimi ve görüntülenmesi için bir sistem geliştirdim. Bu sistem, kullanıcıların kitapları listelemesini, detaylarını görüntülemesini, yeni kitap eklemesini,
mevcut kitapları güncellemesini ve silmesini sağlıyor. Bu işlemleri gerçekleştirebilmek için Controller, Model ve View yapısına dayalı bir düzen kurdum.
<br>
⁛BookController sınıfında, kitaplarla ilgili işlemleri yöneten metotlar bulunuyor. Dependency Injection yöntemi ile veritabanı bağlantısı sağlandı ve Entity Framework Core kullanılarak veritabanı işlemleri gerçekleştirildi. 
Veritabanı işlemlerinin asenkron yapılması, uygulamanın performansını artırarak kullanıcıların bekleme süresini kısalttı. Kullanıcılar, arama yaparak kitapları yazar adı ya da kitap adına göre filtreleyebiliyor.
<br>
⁛Frontend kısmında ise Razor şablon motorunu kullanarak dinamik web sayfaları oluşturdum. Razor şablonları sayesinde @Model gibi işaretlemelerle, Controller’dan gelen verileri HTML içinde dinamik bir şekilde gösterdim. 
Bu sayede, kullanıcıya özelleştirilmiş ve veritabanından gelen içerikleri gösterebildim.


---------------------------------------------------------

![1](https://github.com/user-attachments/assets/81798df4-28d0-4a8e-8ab6-510715fac24f)
![3](https://github.com/user-attachments/assets/96999e20-ab52-485e-aca5-c63973d76768)
![2](https://github.com/user-attachments/assets/5ac669b5-c6b5-4d16-8b81-5e94413bbd85)

