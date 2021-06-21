using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.DTO;
using Backend.Entities;
using Backend.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services
{
    public class ArticleService : IArticleService
    {
        private readonly DataContext context;
        private readonly IArticleRepository articleRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ArticleService(DataContext context, IArticleRepository articleRepository, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.articleRepository = articleRepository;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> CreateAsync(string title, string articleAbstract, IFormFile thumbnailFile, IFormFile file,
            HttpRequest request)
        {
            if (file == null) return new JsonResult(new ExceptionDto {Message = "File not found"}) {StatusCode = 422};

            var extension = Path.GetExtension(file.FileName);

            if (string.IsNullOrEmpty(extension) || extension != ".pdf")
                return new JsonResult(new ExceptionDto {Message = "Wrong file extension"}) {StatusCode = 422};
            
            if (thumbnailFile == null) return new JsonResult(new ExceptionDto {Message = "Thumbnail file not found"}) {StatusCode = 422};
            
            var thumbnailExtension = Path.GetExtension(thumbnailFile.FileName);
            if (string.IsNullOrEmpty(thumbnailExtension) || !(thumbnailExtension == ".png" || thumbnailExtension == ".jpg"))
                return new JsonResult(new ExceptionDto {Message = "Wrong thumbnail file extension"}) {StatusCode = 422};

            var fileName = Path.GetRandomFileName();
            var path = Path.Combine(webHostEnvironment.WebRootPath, "articles", fileName + extension);
            
            var thumbnailFileName = Path.GetRandomFileName();
            var thumbnailPath = Path.Combine(webHostEnvironment.WebRootPath, "articles", thumbnailFileName + thumbnailExtension);

            await using var fileStream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(fileStream);
            
            await using var thumbnailFileStream = new FileStream(thumbnailPath, FileMode.Create);
            await thumbnailFile.CopyToAsync(thumbnailFileStream);

            var article = new Article
            {
                Abstract = articleAbstract,
                Date = DateTime.Now,
                ThumbnailFilePath = request.Scheme + "://" + request.Host + "/articles/" + thumbnailFileName +
                                    thumbnailExtension,
                FilePath = request.Scheme + "://" + request.Host + "/articles/" + fileName +
                           extension,
                Title = title
            };

            return new JsonResult(await articleRepository.AddAsync(article)) {StatusCode = 201};
        }

        public async Task<IActionResult> GetAsync(int id)
        {
            var existingArticle = await articleRepository.GetAsync(id);
            return existingArticle == null
                ? new JsonResult(new ExceptionDto {Message = "No article was found"}) {StatusCode = 422}
                : new JsonResult(existingArticle) {StatusCode = 200};
        }

        public async Task<IActionResult> GetAllAsync()
        {
            return new JsonResult(await articleRepository.GetAllAsync()) {StatusCode = 200};
        }

        public async Task<IActionResult> GetPageAsync(int page, int itemsPerPage)
        {
            var articles = (await articleRepository.GetAllAsync()).ToList();
            var articlesCount = articles.Count;
            var startIndex = page * itemsPerPage;
            var itemsOnPage = itemsPerPage;
            if (articlesCount - startIndex < itemsPerPage)
            {
                itemsOnPage = articlesCount - startIndex;
            }

            if (startIndex < 0 || itemsOnPage <= 0)
                return new JsonResult(new ExceptionDto {Message = "Page don't exist"}) {StatusCode = 422};
            
            var articlesOnPage = articles.GetRange(startIndex, itemsOnPage);
            
            return new JsonResult(articlesOnPage) {StatusCode = 200};
        }

        public async Task<IActionResult> GetPageCountAsync(int itemsPerPage)
        {
            if (itemsPerPage <= 0)
            {
                return new JsonResult(new ExceptionDto {Message = "Incorrect page count"}) {StatusCode = 422};
            }
            var articlesCount = (await articleRepository.GetAllAsync()).Count();
            var pagesCount = articlesCount / itemsPerPage + (articlesCount%itemsPerPage == 0 ? 0 : 1);
            
            return new JsonResult(pagesCount) {StatusCode = 200};
        }

        public async Task<IActionResult> DeleteAsync(int id)
        {
            var existingArticle = await articleRepository.GetAsync(id);
            if (existingArticle == null)
                return new JsonResult(new ExceptionDto {Message = "Movie with given id does not exist"})
                {
                    StatusCode = 422
                };

            context.Database?.BeginTransactionAsync();
            
            var result = await articleRepository.DeleteAsync(existingArticle.Id);
            var fileName = existingArticle.FilePath.Split('/').Last();
            var filePath = Path.Combine(webHostEnvironment.WebRootPath, "articles", fileName);
            if (File.Exists(filePath))
                File.Delete(filePath);
            
            var thumbnailFileName = existingArticle.ThumbnailFilePath.Split('/').Last();
            var thumbnailFilePath = Path.Combine(webHostEnvironment.WebRootPath, "articles", thumbnailFileName);
            if (File.Exists(thumbnailFilePath))
                File.Delete(thumbnailFilePath);
            
            context.Database?.CommitTransactionAsync();

            return new JsonResult(result) {StatusCode = 200};
        }
    }
}