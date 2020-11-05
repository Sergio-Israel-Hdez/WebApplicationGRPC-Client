using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplicationGRPC_Client.Models;
using Grpc.Net.Client;
using GrpcServiceRentMovie;

namespace WebApplicationGRPC_Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new UsuarioGRUD.UsuarioGRUDClient(channel);
            var reply = await client.SelectAllAsync(new Empty());
            return View(reply);
        }
        [HttpGet]
        public async Task<IActionResult>Detalle(int? id)
        {
            if (id== null)
            {
                return RedirectToAction("Index");
            }
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new UsuarioGRUD.UsuarioGRUDClient(channel);
            var reply = await client.SelectByIDAsync(new UsuarioProtoFilter() { UsuarioID = Convert.ToInt32(id) });
            return View(reply);
        }
        [HttpGet]
        public async Task<IActionResult> Modificar(int? id)
        {
            if (id==null)
            {
                return RedirectToAction("Index");
            }
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new UsuarioGRUD.UsuarioGRUDClient(channel);
            var reply = await client.SelectByIDAsync(new UsuarioProtoFilter() { UsuarioID = Convert.ToInt32(id) });
            return View(reply);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Modificar(UsuarioProto usuario)
        {
            if (!ModelState.IsValid)
            {
                return View(usuario);
            }
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new UsuarioGRUD.UsuarioGRUDClient(channel);
            await client.UpdateAsync(usuario);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult>Eliminar(int? id)
        {
            if (id==null)
            {
                return RedirectToAction("Index");
            }
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new UsuarioGRUD.UsuarioGRUDClient(channel);
            await client.DeleteAsync(new UsuarioProtoFilter()
            {
                UsuarioID = Convert.ToInt32(id)
            });
            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
