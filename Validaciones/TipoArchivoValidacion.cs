﻿using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Validaciones
{
    public class TipoArchivoValidacion : ValidationAttribute
    {
        private readonly string[] tiposValidos;

        public TipoArchivoValidacion(string[]TiposValidos)
        {
            tiposValidos = TiposValidos;
        }
        public TipoArchivoValidacion(GrupoArchivos grupoArchivos)
        {
            if(grupoArchivos == GrupoArchivos.Imagen)
            {
                tiposValidos = new string[] {"image/jpeg","image/png","image/gif" };
            }
            
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;
            if (formFile == null)
            {
                return ValidationResult.Success;
            }
            if (!tiposValidos.Contains(formFile.ContentType))
            {
                return new ValidationResult($"El tipo de archivo debe ser {string.Join(", ",tiposValidos)} ");
            }
            return ValidationResult.Success;
        }
    }
}
