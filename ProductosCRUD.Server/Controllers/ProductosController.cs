using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductosCRUD.Server.Models;

namespace ProductosCRUD.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {

        private readonly ProductosCRUDContext _context;

        public ProductosController(ProductosCRUDContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("crearProducto")]
        public async Task<IActionResult>CreateProduct(Producto producto)
        {
            //guardar el producto en la base de datos
            await _context.Productos.AddAsync(producto);
            await _context.SaveChangesAsync();

            //devolver un mensaje de exito
            return Ok();
        }

        [HttpGet]
        [Route("listaProducto")]
        public async Task<ActionResult<IEnumerable<Producto>>>GetProducts()
        {
            //Obten la lista de productos de la base de datos
            var productos = await _context.Productos.ToListAsync();

            //devuelve una lista de productos
            return Ok(productos);
        }

        [HttpGet]
        [Route("verProducto")]
        public async Task<IActionResult>GetProduct(int id)
        {
            //obtener el producto de la base de datos
            Producto producto = await _context.Productos.FindAsync(id);
            
            //devolver el producto
            if(producto == null)
            {
                return NotFound();
            }

            return Ok(producto);
        }

        [HttpPut]
        [Route("editarProducto")]
        public async Task<IActionResult> UpdateProduct(int id, Producto producto)
        {
            //Actualizar el producto en la base de datos
            var productoExistente = await _context.Productos.FindAsync(id);
            productoExistente!.Nombre = producto.Nombre;
            productoExistente.Descripcion = producto.Descripcion;
            productoExistente.Precio = producto.Precio;

            await _context.SaveChangesAsync();

            //devolver un mensaje de exito
            return Ok();
        }

        [HttpDelete]
        [Route("eliminarProducto")]
        public async Task<IActionResult>DeleteProduct(int id)
        {
            //Eliminar producto de la base de datos
            var productoBorrado = await _context.Productos.FindAsync(id);
            _context.Productos.Remove(productoBorrado!);

            await _context.SaveChangesAsync();

            //Devolver un mensaje de exito
            return Ok();
        }

    }
}
