using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

using SIGEES.DataAcces;
using SIGEES.Entidades;


namespace SIGEES.BusinessLogic
{
    public partial class TemplateCorreoBL : GenericBL<TemplateCorreoBL>
    {
        public List<template_correo_dto> ListarParametroa(int codigo_template)
        {
            return TemplateCorreoSelDA.Instance.ListarParametroa(codigo_template);
        }

    }
}
