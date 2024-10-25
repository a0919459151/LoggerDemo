using LoggerDemo.DbEntities;
using LoggerDemo.Repositories;
using LoggerDemo.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;

namespace LoggerDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionTestController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly DogRepository _dogRepository;
        private readonly CatRepository _catRepository;

        public TransactionTestController(
            UnitOfWork unitOfWork,
            DogRepository dogRepository,
            CatRepository catRepository)
        {
            _unitOfWork = unitOfWork;
            _dogRepository = dogRepository;
            _catRepository = catRepository;
        }


        // write 2 table log with transaction
        [HttpPost("WriteLog")]
        public IActionResult WriteLog()
        {
            var transaction = _unitOfWork.BeginTransaction();

            _dogRepository.Create(new Dog() { Name = "Dog" }, transaction);
            throw new System.Exception();
            _catRepository.Create(new Cat() { Name = "Cat" }, transaction);

            _unitOfWork.Commit();

            return Ok();
        }
    }
}
