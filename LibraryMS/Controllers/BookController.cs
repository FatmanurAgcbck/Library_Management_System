using System.Text;
using AspNetCoreGeneratedDocument;
using LibraryMS.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryMS.Controllers{
    public class BookController : Controller
    {
        private readonly Context _context;

        public BookController(Context context) //Controller'ın constructor'ı Context nesnesini Dependency Injection ile alır.
        {
            _context = context; //Context nesnesi veritabanı bağlantısı için _context değişkenine atanır.
        }

        //Asenkron olmasının nedeni, metotların çalışırken uygulamanın diğer işlemlerinin engellenmemesini sağlamaktır.


        #region Listeleme
        //Kitapları Düzenle sayfasında, veritabanındaki kitapları listeeleme
        public async Task<IActionResult> BooksManagement()
        {
             return View(await _context.Books.ToListAsync());
        }

        //Index sayfasında veritabındaki kitapların görüntülenmesini sağlar
        public async Task<IActionResult> Index(string search)
        {
            var books = await _context.Books.ToListAsync();

            //search işlemi
            if(!String.IsNullOrEmpty(search))
            { 
                //yazar adı ve kitap adına göre arama yapabilme
               books = books.Where(p=>p.BookName.ToLower().Contains(search) ||  p.Author.ToLower().Contains(search)).ToList();
               //Contains ==> alt dizede arar içeriyorsa true döndürür 
               
            }
            
            return View(books);
        }

        public async Task<IActionResult> ShowDetails(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b=>b.BookId == id); //id ye göre kitap bilgilerini getirir

            if(book == null)
            {
                return NotFound();
            }
             return View(book);
        }



        #endregion

        #region Ekleme
        //yeni kitap ekleme

        // GET: Create formunu görüntüler 
        public IActionResult Create()
        {
            return View(); 
        }

        [HttpPost] //kitap kaydını veritabanına ekler
        public async Task<IActionResult>Create(Book model, IFormFile imageFile){
            
            var acceptedExtensions = new[] {".jpeg",".jpg",".png"};
            if(imageFile != null)
            {
                var extensions = Path.GetExtension(imageFile.FileName).ToLowerInvariant(); //dosya uzantısını alır küçük harfe çevirir

                if(!acceptedExtensions.Contains(extensions)) //dosya formatı geçerli değilse hata mesajı döndürür
                {
                    ModelState.AddModelError("","Yüklenen dosya geçerli bir resim formatı değil. Lütfen tekrar deneyin.");
                }
                else
                {   //random bir dosya adı oluşturur
                    var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extensions}");
                    var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img",randomFileName); // Dosyanın kaydedileceği tam yolu oluşturur. 
                                                                                                           // `Directory.GetCurrentDirectory()` uygulamanın çalıştığı dizini alır, 
                                                                                                           // ardından "wwwroot/img" dizinini ekler ve son olarak rastgele oluşturulmuş dosya adını (randomFileName) ekler. 

                    try{
                        using(var stream = new FileStream(path, FileMode.Create)){ //dosyayı belirtilen img klasörüne kaydeder
                            await imageFile.CopyToAsync(stream);
                        }
                        model.Image = randomFileName;
                    }
                    catch{ //dosya yüklenirken hata oluştuğunda hata mesajı verir
                        ModelState.AddModelError("","Dosya yüklenirken bir sorun yaşandı. Lütfen tekrar deneyin.");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("","Resim seçilmedi.Lütfen bir resim seçiniz."); //dosya seçilmediyse
            }

            //modeli veritabanına ekler ve kaydeder
            _context.Books.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("BooksManagement");
            
        }
        #endregion

        #region Güncelleme
        //Veri güncelleme işlemleri
        //Kitabın ID'sine göre veritabanından bulunmasını sağlar ve Update View'ında görüntülenmek üzere döndürür
        public async  Task<IActionResult> Update(int? id)
        {
            if(id == null)
            {
                return NotFound(); //id boş işe 404 döner
            }
            var book = await _context.Books.FindAsync(id); //BookID'sine göre veritabanından kitap bilgisi alır

            if(book == null){ //kitap bulunamazsa 404 döner
                return NotFound();
            }
            return View(book);
        }


        [HttpPost]
        public async Task<IActionResult> Update(Book model, IFormFile? imageFile)
        {

            // Dosya yükleme işlemi
            if (imageFile != null)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}"; //random dosya adı oluşturur
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName); //resmin kaydedileceği klasör

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream); //dosya kaydedilir
                }

                model.Image = fileName; //Book modelinin Image alanına dosya adı atanır
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);  //kitap verisini günceller
                    await _context.SaveChangesAsync();
                    return RedirectToAction("BooksManagement");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Books.Any(b => b.BookId == model.BookId)) //kitap veritabanında bulunamazsa, NotFound döndürülür
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(model);
        }

        #endregion

        #region Silme
        //veri silme işlemi
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {   
            if(id == null){
                return NotFound();
            }
            var book = await _context.Books.FindAsync(id);
            if(book == null){
                return NotFound();
            }
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            var book = await _context.Books.FindAsync(id); //kitap ID'sine göre veritabanında kitap araması yapılır
   
            if(book == null)
            {
                return NotFound(); //kitap bulunamazsa 404 döner
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction("BooksManagement","Book");
        }
        
        #endregion

        
    }
}