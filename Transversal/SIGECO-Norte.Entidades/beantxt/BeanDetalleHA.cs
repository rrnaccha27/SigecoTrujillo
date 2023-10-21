using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIGEES.Entidades
{
    public class BeanDetalleHA
    {
        private String campoVacio = " ";
        private String tipoRegistro = "2";
        private String tipoProducto = "";
        private String numeroCuentaAbono = "";
        private String nombreEmpleado = "";
        private String moneda = "";
        private Double importeAbonar = 0.0;
        private String referenciaAdicional = "";
        private String flagNotaAbono = "0";
        private String tipoDocumento = "";
        private String numeroDocumento = "";
        private String flagsValidarIDC = "";


        public BeanDetalleHA(
            String tipoProducto, 
            String numeroCuentaAbono, 
            String nombreEmpleado, 
            String moneda,
            Double importeAbonar, 
            String referenciaAdicional, 
            String tipoDocumento,
            String numeroDocumento,             
            String flagsValidarIDC
            )
        {            
            this.tipoProducto = tipoProducto;
            this.numeroCuentaAbono = numeroCuentaAbono;
            this.nombreEmpleado = nombreEmpleado;
            this.moneda = moneda;
            this.importeAbonar = importeAbonar;
            this.tipoDocumento = tipoDocumento;
            this.numeroDocumento = numeroDocumento;
            this.flagsValidarIDC = flagsValidarIDC;

        }




        public override string ToString()
        {
            String importParsing = new utilitario().quitarPuntoDecimal(importeAbonar);


            if (numeroCuentaAbono.Length > 20)
                numeroCuentaAbono = numeroCuentaAbono.Substring(0, 20);

            if (nombreEmpleado.Length > 40)
                nombreEmpleado = nombreEmpleado.Substring(0, 40);

            if (numeroDocumento.Length > 12)
                numeroDocumento = numeroDocumento.Substring(0, 12);

            if (referenciaAdicional.Length > 40)
                referenciaAdicional = referenciaAdicional.Substring(0, 40);

            if (moneda.Length > 2)
                moneda = moneda.Substring(0, 2);

            if (tipoDocumento.Length > 3)
                tipoDocumento = tipoDocumento.Substring(0, 3);

            if (numeroDocumento.Length > 12)
                numeroDocumento = numeroDocumento.Substring(0, 12);

            return
            campoVacio +
            tipoRegistro +
            tipoProducto +
            numeroCuentaAbono.PadRight(20, ' ') +
            nombreEmpleado.PadRight(40, ' ') +
            moneda.PadRight(2, ' ') +
            importParsing.PadLeft(15, '0') +
            referenciaAdicional.PadRight(40, ' ') +
            flagNotaAbono +
            tipoDocumento.PadRight(3, ' ') +
            numeroDocumento.PadRight(12, ' ') +
            flagsValidarIDC.PadRight(1, '0');
                       
        }

    }
}
