﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dominio
{
    public class Articulo
    {
        public int Id {  get; set; }
        public string Nombre { get; set; }
        [DisplayName("Descripción")] 
        public string Descripcion { get; set; }
        public int IdMarca {  get; set; }
        [DisplayName("Categoría")]
        public int IdCategoria {  get; set; }
        public string ImagenUrl {  get; set; }
        public decimal Precio {  get; set; }
        public string Codigo { get; set; }
        public Marcas Marca { get; set; }
        public Categorias Categoria { get; set; }
       






    }
}
