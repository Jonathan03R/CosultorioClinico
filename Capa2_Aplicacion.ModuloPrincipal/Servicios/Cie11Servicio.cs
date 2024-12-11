using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa4_Persistencia.SqlServer.ModuloBase;
using Newtonsoft.Json;

namespace Capa2_Aplicacion.ModuloPrincipal.Servicios
{
    public class Cie11Servicio
    {
        private readonly Cie11ApiClient cie11ApiClient;

        public Cie11Servicio()
        {
            cie11ApiClient = new Cie11ApiClient(); // Instancia el cliente de la API en la capa de persistencia
        }

        // Lógica de la aplicación para buscar términos en CIE-11
        public async Task<List<Cie11Resultado>> BuscarTermino(string termino)
        {
            try
            {
                // Llama a la capa de persistencia para realizar la búsqueda
                var jsonResponse = await cie11ApiClient.BuscarTerminoAsync(termino);

                // Formatea la respuesta JSON en una lista de resultados
                var resultado = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                List<Cie11Resultado> listaResultados = new List<Cie11Resultado>();

                foreach (var entity in resultado.destinationEntities)
                {
                    listaResultados.Add(new Cie11Resultado
                    {
                        Codigo = entity.theCode,
                        Titulo = entity.title
                    });
                }

                return listaResultados; // Devuelve los resultados procesados
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la capa de aplicación al buscar en CIE-11: {ex.Message}");
            }
        }
    }

    // Clase para encapsular los datos del resultado
    public class Cie11Resultado
    {
        public string Codigo { get; set; }
        public string Titulo { get; set; }
    }
}
