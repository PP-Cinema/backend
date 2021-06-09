using System;
using System.IO;
using System.Threading.Tasks;
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
        private readonly IArticleRepository articleRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ArticleService(IArticleRepository articleRepository, IWebHostEnvironment webHostEnvironment)
        {
            this.articleRepository = articleRepository;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> CreateAsync(string title, string articleAbstract, IFormFile file,
            HttpRequest request)
        {
            if (file == null) return new JsonResult(new ExceptionDto {Message = "File not found"}) {StatusCode = 422};
            ;

            var extension = Path.GetExtension(file.FileName);

            if (string.IsNullOrEmpty(extension) || extension != ".pdf")
                return new JsonResult(new ExceptionDto {Message = "Wrong file extension"}) {StatusCode = 422};

            var fileName = Path.GetRandomFileName();
            var path = Path.Combine(webHostEnvironment.WebRootPath, "articles", fileName + extension);

            await using var fileStream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(fileStream);

            var article = new Article
            {
                Abstract = articleAbstract,
                Date = DateTime.Now,
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
    }
}