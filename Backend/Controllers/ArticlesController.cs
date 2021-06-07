using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.DTO;
using Backend.Entities;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController: ControllerBase
    {
        private readonly IArticleService articleService;

        public ArticlesController(IArticleService articleService)
        {
            this.articleService = articleService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        [ProducesResponseType(typeof(Article), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateArticle([FromForm] ArticleDto articleDto)
        {
            return await articleService.CreateAsync(articleDto.Title, articleDto.Abstract, articleDto.File, Request);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Article), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetArticle(int id)
        {
            return await articleService.Get(id);
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Article>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetArticle()
        {
            return await articleService.GetAll();
        }
    }
}