using LoggerDemo.DbEntities;
using LoggerDemo.Excptions;
using LoggerDemo.Repositories;
using LoggerDemo.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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

        [HttpPost("CreateDogAndCat")]
        public IActionResult CreateDogAndCat()
        {
            var transaction = _unitOfWork.BeginTransaction();

            _dogRepository.CreateDog(new Dog() { Name = "Dog" }, transaction);
            _catRepository.CreateCat(new Cat() { Name = "Cat" }, transaction);

            _unitOfWork.Commit();

            return Ok();
        }

        [HttpPost("CreateDogAndCatWithCount")]
        public IActionResult CreateDogAndCat(int dogCount, int catCount)
        {
            var transaction = _unitOfWork.BeginTransaction();

            var dogs = new List<Dog>();
            for (int i = 0; i < dogCount; i++)
            {
                dogs.Add(new Dog() { Name = $"Dog {i + 1}" });
            }

            var cats = new List<Cat>();
            for (int j = 0; j < catCount; j++)
            {
                cats.Add(new Cat() { Name = $"Cat {j + 1}" });
            }

            _dogRepository.CreateDogs(dogs, transaction);
            _catRepository.CreateCats(cats, transaction);

            _unitOfWork.Commit();

            return Ok();
        }
    }
}
