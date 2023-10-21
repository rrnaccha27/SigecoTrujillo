using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIGEES.Entidades
{

   public class BeanDetallePP
    {
        private String campoVacio=" ";
        private String tipoRegistro = "2";
        private String tipoProducto = "";
        private String numeroCuentaAbono = "";
        private String razonSocialEmpleado = "";
        private String moneda = "";
        private Double importeAbonar = 0.0;
        private String tipoDocumento = "";
        private String numeroDocumento = "";
        private String tipoDocumentoPagar = "";
        private String numeroDocumentoPagar = "";
        private String tipoAbono = "";
        private String referenciaAdicional = "";
        private String flagNotaAbono = "0";
        private String flagDelivery = "0";
        private String flagValidarRuc = "";
       //------------------------------------------
       //------------------------------------------
        private String direccion = "";
        private String distrito = "";
        private String provincia = "";
        private String departamento = "";
        private String contacto = "";

        public BeanDetallePP()
        {

        }

        public BeanDetallePP(String tipoProducto, String numeroCuentaAbono, String razonSocialEmpleado, String moneda, Double importeAbonar, String tipoDocumento, String numeroDocumento, String tipoDocumentoPagar, String numeroDocumentoPagar, String tipoAbono, String referenciaAdicional, String flagValidarRuc)
        {

            
            this.tipoProducto = tipoProducto;
            this.numeroCuentaAbono = numeroCuentaAbono;
            this.razonSocialEmpleado = razonSocialEmpleado;
            this.moneda = moneda;
            this.importeAbonar = importeAbonar;
            this.tipoDocumento = tipoDocumento;
            this.numeroDocumento = numeroDocumento;
            this.tipoDocumentoPagar = tipoDocumentoPagar;
            this.numeroDocumentoPagar = numeroDocumentoPagar;
            this.tipoAbono = tipoAbono;
            this.referenciaAdicional = referenciaAdicional;
            this.flagValidarRuc = flagValidarRuc;

        }


        



        public override string ToString()
            {

                String importParsing = new utilitario().quitarPuntoDecimal(importeAbonar);

                if (numeroCuentaAbono.Length > 20)
                    numeroCuentaAbono = numeroCuentaAbono.Substring(0,20);

                if (razonSocialEmpleado.Length > 40)
                    razonSocialEmpleado = razonSocialEmpleado.Substring(0,40);

                if (numeroDocumento.Length > 12)
                    numeroDocumento = numeroDocumento.Substring(0, 12);

                if (numeroDocumentoPagar.Length > 10)
                    numeroDocumentoPagar = numeroDocumentoPagar.Substring(0, 10);

                if (referenciaAdicional.Length > 40)
                    referenciaAdicional = referenciaAdicional.Substring(0, 40);

                if (moneda.Length > 2)
                    moneda = moneda.Substring(0, 2);
                if (tipoDocumentoPagar.Length > 1)
                    tipoDocumentoPagar = tipoDocumentoPagar.Substring(0, 1);
            
            
                return
                campoVacio +
                tipoRegistro +
                tipoProducto +
                numeroCuentaAbono.PadRight(20, ' ') +
                razonSocialEmpleado.PadRight(40, ' ') +
                moneda.PadRight(2, ' ') +
                importParsing.PadLeft(15, '0') +
                tipoDocumento.PadRight(3, ' ') +
                numeroDocumento.PadRight(12, ' ') +
                tipoDocumentoPagar.PadLeft(1, 'F') +
                numeroDocumentoPagar.PadLeft(10, '9') +
                tipoAbono.PadLeft(1, '1') +
                referenciaAdicional.PadRight(40, ' ') +
                flagNotaAbono +
                flagDelivery +
                flagValidarRuc.PadRight(1, '0');
               //no son campos oblogatorios
               //-------------------------------------
               //direccion.PadRight(40, ' ') +
               //distrito.PadRight(20, ' ') +
               //provincia.PadRight(20, ' ') +
               // departamento.PadRight(20, ' ') +
               // contacto.PadRight(40, ' ');
        }


    }    
}
