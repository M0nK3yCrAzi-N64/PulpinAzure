using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa.Datos
{
    public class CadenaDAL
    {
        public string Cadena { get; set; }
        public CadenaDAL()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();

            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            var root = builder.Build();
            Cadena = root.GetConnectionString("cn");
        }
    }
}
