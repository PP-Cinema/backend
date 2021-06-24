using System.Threading.Tasks;
using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services
{
    public class HallService : IHallService
    {
        private readonly IHallRepository hallRepository;
        
        public HallService(IHallRepository hallRepository)
        {
            this.hallRepository = hallRepository;
        }
        
        public async Task<IActionResult> GetAllAsync()
        {
            return new JsonResult(await hallRepository.GetAllAsync()) {StatusCode = 200};
        }
    }
}