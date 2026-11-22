
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using DSW1_T1_VilchezMendozaLlaneza.Models;

namespace DSW1_T1_VilchezMendozaLlaneza.Controllers
{
    public class LibroController : Controller
    {
        private readonly IConfiguration _configuration;

        // CONSTRUCTOR
        public LibroController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // 1. MÉTODO PARA LISTAR LIBROS
        public IEnumerable<Libro> ListarLibros()
        {
            List<Libro> libros = new List<Libro>();

            string conexion = _configuration
                .GetConnectionString("cn")!;

            using (SqlConnection cn = new SqlConnection(conexion))
            {
                using (SqlCommand cmd = new SqlCommand(
                    "usp_ListarLibros", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Libro libro = new Libro
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                NombreLibro = dr["NombreLibro"].ToString()!,
                                Autor = dr["Autor"].ToString()!,
                                NombreEditorial = dr["NombreEditorial"].ToString()!,
                                ISDN = dr["ISDN"].ToString()!
                            };

                            libros.Add(libro);
                        }
                    }
                }
            }

            return libros;
        }

        // 2. MOSTRAR EL LISTADO

        public IActionResult Index()
        {
            ViewBag.Mensaje = TempData["Mensaje"];
            ViewBag.Error = TempData["Error"];

            return View(ListarLibros());
        }


        // 3. MOSTRAR EL FORMULARIO PARA CREAR
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        // 4. REGISTRAR UN LIBRO EN SQL SERVER
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Libro libro)
        {
            if (!ModelState.IsValid)
            {
                return View(libro);
            }

            try
            {
                string conexion = _configuration
                    .GetConnectionString("cn")!;

                using (SqlConnection cn = new SqlConnection(conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(
                        "usp_RegistrarLibro", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@NombreLibro",
                            SqlDbType.VarChar, 120).Value = libro.NombreLibro;

                        cmd.Parameters.Add("@Autor",
                            SqlDbType.VarChar, 100).Value = libro.Autor;

                        cmd.Parameters.Add("@NombreEditorial",
                            SqlDbType.VarChar, 100).Value = libro.NombreEditorial;

                        cmd.Parameters.Add("@ISDN",
                            SqlDbType.VarChar, 20).Value = libro.ISDN;

                        cn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                TempData["Mensaje"] = "Libro registrado correctamente";

                return RedirectToAction("Index");
            }
            catch (SqlException)
            {
                ModelState.AddModelError("",
                    "No se pudo registrar el libro. Verifique la conexión y los datos.");

                return View(libro);
            }
        }


        // 5. MOSTRAR EL FORMULARIO PARA EDITAR
        [HttpGet]
        public IActionResult Editar(int id)
        {
            Libro? libro = ListarLibros()
                .FirstOrDefault(x => x.Id == id);

            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // 6. ACTUALIZAR LOS DATOS DEL LIBRO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Libro libro)
        {
            if (!ModelState.IsValid)
            {
                return View(libro);
            }

            try
            {
                string conexion = _configuration
                    .GetConnectionString("cn")!;

                using (SqlConnection cn = new SqlConnection(conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(
                        "usp_ActualizarLibro", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@Id",
                            SqlDbType.Int).Value = libro.Id;

                        cmd.Parameters.Add("@NombreLibro",
                            SqlDbType.VarChar, 120).Value = libro.NombreLibro;

                        cmd.Parameters.Add("@Autor",
                            SqlDbType.VarChar, 100).Value = libro.Autor;

                        cmd.Parameters.Add("@NombreEditorial",
                            SqlDbType.VarChar, 100).Value = libro.NombreEditorial;

                        cmd.Parameters.Add("@ISDN",
                            SqlDbType.VarChar, 20).Value = libro.ISDN;

                        cn.Open();

                        int filas = cmd.ExecuteNonQuery();

                        if (filas == 0)
                        {
                            ModelState.AddModelError("",
                                "El libro no existe o no pudo actualizarse.");

                            return View(libro);
                        }
                    }
                }

                TempData["Mensaje"] = "Libro actualizado correctamente";

                return RedirectToAction("Index");
            }
            catch (SqlException)
            {
                ModelState.AddModelError("",
                    "Ocurrió un error al actualizar el libro.");

                return View(libro);
            }
        }


        // 7. ELIMINAR UN LIBRO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(int id)
        {
            try
            {
                string conexion = _configuration
                    .GetConnectionString("cn")!;

                using (SqlConnection cn = new SqlConnection(conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(
                        "usp_EliminarLibro", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@Id",
                            SqlDbType.Int).Value = id;

                        cn.Open();

                        int filas = cmd.ExecuteNonQuery();

                        if (filas == 0)
                        {
                            TempData["Error"] =
                                "No se encontró el libro que desea eliminar.";
                        }
                        else
                        {
                            TempData["Mensaje"] =
                                "Libro eliminado correctamente.";
                        }
                    }
                }
            }
            catch (SqlException)
            {
                TempData["Error"] =
                    "No se pudo eliminar el libro. Puede tener registros relacionados.";
            }

            return RedirectToAction("Index");
        }

        // 8. LISTAR LIBROS POR AUTOR
        public IEnumerable<Libro> ListarLibrosPorAutor(string autor)
        {
            List<Libro> libros = new List<Libro>();

            string conexion = _configuration
                .GetConnectionString("cn")!;

            using (SqlConnection cn = new SqlConnection(conexion))
            {
                using (SqlCommand cmd = new SqlCommand(
                    "usp_ListarLibrosPorAutor", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Autor",
                        SqlDbType.VarChar, 100).Value = autor;

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Libro libro = new Libro
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                NombreLibro = dr["NombreLibro"].ToString()!,
                                Autor = dr["Autor"].ToString()!,
                                NombreEditorial = dr["NombreEditorial"].ToString()!,
                                ISDN = dr["ISDN"].ToString()!
                            };

                            libros.Add(libro);
                        }
                    }
                }
            }

            return libros;
        }

        // 9. MOSTRAR LA BÚSQUEDA POR AUTOR
        [HttpGet]
        public IActionResult BuscarPorAutor(string? autor)
        {
            IEnumerable<Libro> libros;

            if (string.IsNullOrWhiteSpace(autor))
            {
                libros = ListarLibros();
            }
            else
            {
                libros = ListarLibrosPorAutor(autor.Trim());
            }

            ViewBag.Autor = autor;

            return View(libros);
        }


    }
}
