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
               books = books.Where(p=>p.BookName.ToLower().Contains(search)).ToList();
            }
            
            return View(books);
        }
        #endregion

        #region Ekleme
        //yeni kitap ekleme
        public IActionResult Create()
        {
            return View(); 
        }

        [HttpPost] //kitap kaydını veritabanına ekler
        public async Task<IActionResult>Create(Book model, IFormFile imageFile){
            
            var acceptedExtensions = new[] {".jpeg",".jpg",".png"};
            if(imageFile != null)
            {
                var extensions = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

                if(!acceptedExtensions.Contains(extensions))
                {
                    ModelState.AddModelError("","Yüklenen dosya geçerli bir resim formatı değil. Lütfen tekrar deneyin.");
                }
                else
                {
                    var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extensions}");
                    var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img",randomFileName);
                    try{
                        using(var stream = new FileStream(path, FileMode.Create)){
                            await imageFile.CopyToAsync(stream);
                        }
                        model.Image = randomFileName;
                    }
                    catch{
                        ModelState.AddModelError("","Dosya yüklenirken bir sorun yaşandı. Lütfen tekrar deneyin.");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("","Resim seçilmedi.Lütfen bir resim seçiniz.");
            }


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
                return NotFound();
            }
            var book = await _context.Books.FindAsync(id);

            if(book == null){
                return NotFound();
            }
            return View(book);
        }


        [HttpPost]
        public async Task<IActionResult> Update(int id, Book model, IFormFile? imageFile)
        {

            if (id != model.BookId)
            {
                return NotFound();
            }

            // Dosya yükleme işlemi
            if (imageFile != null)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                model.Image = fileName;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("BooksManagement");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Books.Any(b => b.BookId == model.BookId))
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
            var book = await _context.Books.FindAsync(id);
            if(book == null)
            {
                return NotFound();
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction("BooksManagement","Book");
        }
        
        #endregion



    }
}