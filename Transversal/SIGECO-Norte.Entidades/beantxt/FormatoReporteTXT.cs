using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SIGEES.Entidades
{
    public class FormatoReporteTXT
    {

        public void generaFormatoPlanillaHaberes(string ruta, System.Text.Encoding encoding, List<BeanDetalleHA> listaFormato, BeanCabeceraCargoPlanillaHPD beanCabeceraCargoPlanillaHPD)
        {
            //ruta=ruta+nombre.txt
            if (!verificaExistenciaArchivo(ruta))//si no existe el archivo creamos
            {
                StreamWriter fileWriter = new StreamWriter(ruta, true, encoding);                

                fileWriter.WriteLine(beanCabeceraCargoPlanillaHPD.ToString());//cabecera del archivo

                foreach (BeanDetalleHA beanRRHH in listaFormato)
                {
                    
                    fileWriter.WriteLine(beanRRHH.ToString());
                }

                fileWriter.Close();

            }
            else//si existe solo agregamos
            {

                StreamWriter WriteReportFile = File.AppendText(ruta);
                foreach (BeanDetalleHA beanRRHH in listaFormato)
                {
                    //strRegistro = beanRRHH.ToString();
                    WriteReportFile.WriteLine(beanRRHH.ToString());
                }
                WriteReportFile.Close();
              
            }

        }


        public void generaFormatoPlanillaProveedor(string ruta, System.Text.Encoding encoding, List<BeanDetallePP> listaFormato, BeanCabeceraCargoPlanillaHPD beanCabeceraCargoPlanillaHPD)
        {
            
            if (!verificaExistenciaArchivo(ruta))//si no existe el archivo creamos
            {
                StreamWriter fileWriter = new StreamWriter(ruta, true, encoding);
                fileWriter.WriteLine(beanCabeceraCargoPlanillaHPD.ToString());//cabecera del archivo

                int lineasMax = listaFormato.Count();
                int lineas = 1;

                foreach (BeanDetallePP beanRRHH in listaFormato)
                {
                    //strRegistro = beanRRHH.ToString();
                    if (lineas == lineasMax)
                    {
                        fileWriter.Write(beanRRHH.ToString());
                    }
                    else
                    {
                        fileWriter.WriteLine(beanRRHH.ToString());
                    }
                    lineas++;                    
                }

                fileWriter.Close();

            }
            else//si existe solo agregamos
            {

                StreamWriter WriteReportFile = File.AppendText(ruta);
                foreach (BeanDetallePP beanRRHH in listaFormato)
                {
                    
                    WriteReportFile.WriteLine(beanRRHH.ToString());
                }
                WriteReportFile.Close();
               

            }

        }
        //fin metodo

        public void lecturaArchivo(String ruta, System.Text.Encoding encoding)
        {

            using (StreamReader lector = new StreamReader(ruta, encoding, true))
            {
                while (lector.Peek() > -1)
                {
                    string linea = lector.ReadLine();
                    if (!String.IsNullOrEmpty(linea))
                    {
                        Console.WriteLine(linea);
                    }
                }

                lector.Close();
            }

        }

        public Boolean verificaExistenciaArchivo(String ruta)
        {

            if (File.Exists(ruta))
                return true;
            else
                return false;

        }

        public void generarArchivoContabilidad(string ruta, System.Text.Encoding encoding, List<txt_contabilidad_planilla_dto> listado)
        {
            StreamWriter fileWriter;

            if (!verificaExistenciaArchivo(ruta))//si no existe el archivo creamos
            {
                fileWriter = new StreamWriter(ruta, true, encoding);
            }
            else//si existe solo agregamos
            {
                fileWriter = File.AppendText(ruta);
            }

            foreach (txt_contabilidad_planilla_dto data in listado)
            {
                fileWriter.WriteLine(data.ToString());
            }

            fileWriter.Close();
        }

    }
}

