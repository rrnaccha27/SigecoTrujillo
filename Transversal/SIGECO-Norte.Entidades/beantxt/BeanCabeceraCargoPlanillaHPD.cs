using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIGEES.Entidades
{
    public class BeanCabeceraCargoPlanillaHPD
    {

        public BeanCabeceraCargoPlanillaHPD(String tipoPagoMasivo, String tipoProducto, String numeroCuenta, String moneda, Double importeCargar, String fechaProcesar, String referencia, String checksum, String totalregistroAbonar, String subTipoPagoMasivo, String identificadorDividendio, String indicadorNotacargo)
        {
            this.tipoPagoMasivo = tipoPagoMasivo;
            this.tipoProducto = tipoProducto;
            this.numeroCuenta = numeroCuenta;
            this.moneda = moneda;
            this.importeCargar = importeCargar;
            this.fechaProcesar = fechaProcesar;
            this.referencia = referencia;
            this.checksum = checksum;
            this.totalregistroAbonar = totalregistroAbonar;
            this.subTipoPagoMasivo = subTipoPagoMasivo;
            this.identificadorDividendio = identificadorDividendio;
            this.indicadorNotacargo = indicadorNotacargo;
        }

        private String planillaNuevo = "#";//constante
        private String tiporegistro = "1";//constante
        private String tipoPagoMasivo = "";
        private String tipoProducto = "";
        private String numeroCuenta = "";
        private String moneda = "";
        private Double importeCargar = 0.00;
        private String fechaProcesar = "";
        private String referencia = "";
        private String checksum = "";
        private String totalregistroAbonar = "";
        private String subTipoPagoMasivo = "";
        private String identificadorDividendio = "";
        private String indicadorNotacargo = "";

        

        public override string ToString()
        {
            //564556656
            String importParsing = new utilitario().quitarPuntoDecimal(importeCargar);
            if (string.IsNullOrWhiteSpace(numeroCuenta))
            {
                throw new Exception("Falta configurar nro de la cuenta del personal.");
            }
            if (string.IsNullOrWhiteSpace(moneda))
            {
                throw new Exception("Falta configurar tipo de moneda de la cuenta del personal.");
            }
            if (numeroCuenta.Length > 20)
                numeroCuenta = numeroCuenta.Substring(0, 20);

            if (fechaProcesar.Length > 8)
                fechaProcesar = fechaProcesar.Substring(0, 8);

            if (identificadorDividendio.Length > 15)
                identificadorDividendio = identificadorDividendio.Substring(0, 15);

            if (referencia.Length > 20)
                referencia = referencia.Substring(0, 20);

            if (moneda.Length > 2)
                moneda = moneda.Substring(0, 2);

            return
            planillaNuevo +
            tiporegistro +
            tipoPagoMasivo.PadLeft(1, ' ') +
            tipoProducto.PadLeft(1, ' ') +
            numeroCuenta.PadRight(20, ' ') +
            moneda.PadRight(2, ' ') +
            importParsing.PadLeft(15, '0') +
            fechaProcesar.PadRight(8, ' ') +
            referencia.PadRight(20, ' ') +
            checksum.PadLeft(15, '*') +//por verificar
            totalregistroAbonar.PadLeft(6, '0') +
            subTipoPagoMasivo +
            identificadorDividendio.PadRight(15, ' ') +
            indicadorNotacargo.PadLeft(1, '0');
        }


    }
}
