using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductosCRUD.Client.Models;
using System.Net.Http;
using System.Text;

namespace ProductosCRUD.Client.Controllers
{
    public class ProductosController : Controller
    {
        private readonly HttpClient _httpClient;
        public ProductosController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7073/api");
        }
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("/api/Productos/listaProducto");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var productos = JsonConvert.DeserializeObject<IEnumerable<Producto>>(content);
                return View("Index", productos);
            }

            // Manejar el caso en que la solicitud HTTP no fue exitosa.
            return View(new List<Producto>()); // Puedes mostrar una vista vacía o un mensaje de error.
        }

        public async Task<IActionResult> Details(int id)
        {

            var response = await _httpClient.GetAsync($"/api/Productos/verProducto?id={id}");


            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var producto = JsonConvert.DeserializeObject<Producto>(content);

                // Devuelve la vista de edición con los detalles del producto.
                return View(producto);
            }
            else
            {
                // Manejar el caso de error al obtener los detalles del producto.
                return RedirectToAction("Details"); // Redirige a la página de lista de productos u otra acción apropiada.
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(producto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Productos/crearProducto", content);

                if (response.IsSuccessStatusCode)
                {
                    // Manejar el caso de creación exitosa.
                    return RedirectToAction("Index");
                }
                else
                {
                    // Manejar el caso de error en la solicitud POST, por ejemplo, mostrando un mensaje de error.
                    ModelState.AddModelError(string.Empty, "Error al crear el producto.");
                }
            }
            return View(producto);
        }

        public async Task<IActionResult> Edit(int id)
        {

            var response = await _httpClient.GetAsync($"/api/Productos/verProducto?id={id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var producto = JsonConvert.DeserializeObject<Producto>(content);

                // Devuelve la vista de edición con los detalles del producto.
                return View(producto);
            }
            else
            {
                // Manejar el caso de error al obtener los detalles del producto.
                return RedirectToAction("Details"); // Redirige a la página de lista de productos u otra acción apropiada.
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Producto producto)
        {
            if (ModelState.IsValid)
            {
                

                var json = JsonConvert.SerializeObject(producto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"/api/Productos/editarProducto?id={id}", content);

                if (response.IsSuccessStatusCode)
                {
                    // Manejar el caso de actualización exitosa, por ejemplo, redirigiendo a la página de detalles del producto.
                    return RedirectToAction("Index", new { id });
                }
                else
                {
                    // Manejar el caso de error en la solicitud PUT o POST, por ejemplo, mostrando un mensaje de error.
                    ModelState.AddModelError(string.Empty, "Error al actualizar el producto.");
                }
            }

            // Si hay errores de validación, vuelve a mostrar el formulario de edición con los errores.
            return View(producto);
        }
        
        public async Task<IActionResult> Delete(int id)
        {            
            var response = await _httpClient.DeleteAsync($"/api/Productos/eliminarProducto?id={id}");

            if (response.IsSuccessStatusCode)
            {
                // Maneja el caso de eliminación exitosa, por ejemplo, redirigiendo a la página de lista de productos.
                return RedirectToAction("Index");
            }
            else
            {
                // Maneja el caso de error en la solicitud DELETE, por ejemplo, mostrando un mensaje de error.
                TempData["Error"] = "Error al eliminar el producto.";
                return RedirectToAction("Index");
            }
        }


    }
}
