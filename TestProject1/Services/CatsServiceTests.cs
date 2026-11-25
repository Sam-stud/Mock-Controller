using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mock.depart.Controllers;
using mock.depart.Models;
using mock.depart.Services;
using mock.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1.Services
{
    [TestClass]
    public sealed class CatsServiceTests
    {
        [TestMethod]
        public async Task Delete_CatNotFound()
        {
            Mock<CatsService> serviceMock = new Mock<CatsService>();
            Mock<CatsController> controller = new Mock<CatsController>(serviceMock.Object) { CallBase = true };

            serviceMock.Setup(s => s.Get(It.IsAny<int>())).Returns(value: null);

            controller.Setup(c => c.UserId).Returns("1");

            var actionResult = controller.Object.DeleteCat(0);

            var result = actionResult.Result as NotFoundResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Delete_WrongOwner()
        {
            Mock<CatsService> serviceMock = new Mock<CatsService>();
            Mock<CatsController> controller = new Mock<CatsController>(serviceMock.Object) { CallBase = true };

            CatOwner co = new CatOwner()
            {
                Id = "11111"
            };
            Cat c = new Cat()
            {
                Id = 1,
                Name = "Tite Dali",
                CatOwner = co,
                CuteLevel = Cuteness.Amazing
            };

            serviceMock.Setup(s => s.Get(It.IsAny<int>())).Returns(c);
            controller.Setup(c => c.UserId).Returns("1");

            var actionResult = controller.Object.DeleteCat(0);

            var result = actionResult.Result as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Cat is not yours", result.Value);
        }
        [TestMethod]
        public async Task Delete_TooCute()
        {
            Mock<CatsService> serviceMock = new Mock<CatsService>();
            Mock<CatsController> controller = new Mock<CatsController>(serviceMock.Object) { CallBase = true };

            CatOwner co = new CatOwner()
            {
                Id = "11111"
            };
            Cat c = new Cat()
            {
                Id = 1,
                Name = "Tite Dali",
                CatOwner = co,
                CuteLevel = Cuteness.Amazing
            };

            serviceMock.Setup(s => s.Get(It.IsAny<int>())).Returns(c);
            controller.Setup(c => c.UserId).Returns("11111");

            var actionResult = controller.Object.DeleteCat(0);

            var result = actionResult.Result as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Cat is not yours", result.Value);
        }
        [TestMethod]
        public async Task Delete_Ok()
        {
            Mock<CatsService> serviceMock = new Mock<CatsService>();
            Mock<CatsController> controller = new Mock<CatsController>(serviceMock.Object) { CallBase = true };

            CatOwner co = new CatOwner()
            {
                Id = "11111"
            };
            Cat c = new Cat()
            {
                Id = 1,
                Name = "Tite Dali",
                CatOwner = co,
                CuteLevel = Cuteness.BarelyOk
            };

            serviceMock.Setup(s => s.Get(It.IsAny<int>())).Returns(c);
            serviceMock.Setup(s => s.Delete(It.IsAny<int>())).Returns(c);
            controller.Setup(c => c.UserId).Returns("11111");

            var actionResult = controller.Object.DeleteCat(0);

            var result = actionResult.Result as OkObjectResult;

            Assert.IsNotNull(result);

            Cat? catresult = (Cat?)result!.Value;
            Assert.AreEqual(c.Id, catresult!.Id);
        }
        [TestMethod]
        public void Delete_Code18()
        {
            Mock<CatsService> serviceMock = new Mock<CatsService>();
            Mock<CatsController> controller = new Mock<CatsController>(serviceMock.Object) { CallBase = true };

            CatOwner co = new CatOwner()
            {
                Id = "11111"
            };
            Cat c = new Cat()
            {
                Id = 1,
                Name = "Chat 1",
                CatOwner = co,
                CuteLevel = Cuteness.BarelyOk
            };

            serviceMock.Setup(s => s.Get(It.IsAny<int>())).Throws(new Code18Exception());
            controller.Setup(c => c.UserId).Returns("11111");

            var actionResult = controller.Object.DeleteCat(0);

            var result = actionResult.Result as UnauthorizedResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ChiffreChanceux_Ok()
        {
            Mock<CatsService> serviceMock = new Mock<CatsService>();
            Mock<CatsController> controller = new Mock<CatsController>(serviceMock.Object) { CallBase = true };

            serviceMock.SetupSequence(s => s.ChiffreChanceux()).Returns(33).Returns(42).Returns(67);

            var actionResult1 = controller.Object.ChiffreChanceux();
            var result1 = actionResult1.Result as OkObjectResult;
            Assert.IsNotNull(result1);
            Assert.AreEqual(33, result1!.Value);

            var actionResult2 = controller.Object.ChiffreChanceux();
            var result2 = actionResult2.Result as OkObjectResult;
            Assert.IsNotNull(result2);
            Assert.AreEqual(42, result2!.Value);

            var actionResult3 = controller.Object.ChiffreChanceux();
            var result3 = actionResult3.Result as OkObjectResult;
            Assert.IsNotNull(result3);
            Assert.AreEqual(67, result3!.Value);
        }
    }
}
