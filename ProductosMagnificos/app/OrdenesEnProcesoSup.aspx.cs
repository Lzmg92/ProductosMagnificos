using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.IO;

namespace ProductosMagnificos.app
{
    public partial class OrdenesEnProcesoSup : System.Web.UI.Page
    {
         StringBuilder stringcodigotrans = new StringBuilder();
        SqlConnection conex = new SqlConnection();
        SqlCommand comando = new SqlCommand();

        String nfactura = "";

        string rtotal = "";
        string rcredito = "";
        string rnit = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            String bienvenido = (String)(Session["bienvenido"]);
            Session["bienvenido"] = bienvenido;

            conex.ConnectionString = (@"Data Source = LEZS; Initial Catalog = ProductosMagnificos; Integrated security=true");
            conex.Open();
            String com = "Select L.Codigo, L.Total, L.VigenciaInicio, L.VigenciaFinal, C.Credito, C.Nit, C.Nombre as 'Nombre Cliente', L.Estado"
                            + " From Listas L, Clientes C"
                            + " where L.fk_nit_cliente = C.Nit";




            SqlCommand comand = new SqlCommand(com, conex);
            comand.ExecuteNonQuery();

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = comand;
            DataSet ds = new DataSet();

            da.Fill(ds, "Nombre");


            GridView2.DataSource = ds;
            GridView2.DataBind();
            conex.Close();
        }

        protected void Btnbusqueda_Click(object sender, EventArgs e)
        {

            comando.Connection = conex;
            comando.CommandText = " select P.Nombre as 'Producto', I.fk_Lista as 'Codigo de lista', I.Valor as 'Valor' " +
                                  " from ItemsListas I, Productos p " +
                                  " where I.fk_Codigo_producto = P.Codigo  and " +
                                        "I.fk_Codigo_producto = '" + txtbusqueda.Text + "'	";
            conex.Open();
            comando.ExecuteNonQuery();

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = comando;
            DataSet ds = new DataSet();
            da.Fill(ds, "Codigo");
            GridView1.DataSource = ds;
            GridView1.DataBind();
            conex.Close();
        }

        protected void btnagregar_Click(object sender, EventArgs e)
        {

            GetDatos(txtorden.Text);
           
                try
                {
                    comando.Connection = conex;
                    comando.CommandText = "Update Listas set Estado = 'Aprobado' where Codigo = " + txtorden.Text;
                    conex.Open();
                    comando.ExecuteNonQuery();
                    conex.Close();
                  //   Label1.Text = "La Orden " + txtorden.Text + " se ha Aprobado";
                }
                catch
                {
                   //  Label1.Text = "La Orden " + txtorden.Text + " No se ha Aprobado";
                }


             Random random = new Random();
                string combination2 = "0123456789";
                for (int i = 0; i < 6; i++)
                {
                    stringcodigotrans.Append(combination2[random.Next(combination2.Length)]);
                    nfactura  = stringcodigotrans.ToString();        
            }


                try
                {
                    comando.Connection = conex;
                    comando.CommandText = "insert Facturas ( Codigo, fk_Lista, Fecha ) values ('" + nfactura + "' , '" + txtorden.Text + "', (CONCAT(DATEPART(DAY, SYSDATETIME()),'/',DATEPART(MONTH, SYSDATETIME()),'/',DATEPART(YEAR, SYSDATETIME()))) )";
                    conex.Open();
                    comando.ExecuteNonQuery();
                    conex.Close();
                    Label1.Text = "La Orden " + txtorden.Text + " se ha Aprobado factura no " + nfactura  ;
                }
                catch
                {
                     Label1.Text = "La Orden " + txtorden.Text + " No se ha Aprobado";
                }
        
            
        } 
        protected void btneliminar_Click(object sender, EventArgs e)
        {

            //////////////////////////////////////// factura y reporte

            try
            {
                comando.Connection = conex;
                comando.CommandText = "Update Listas set Estado = 'Cancelada' where Codigo = " + txtorden.Text;
                conex.Open();
                comando.ExecuteNonQuery();
                Label1.Text = "La Orden " + txtorden.Text + " se ha Cancelado";
            }
            catch
            {
                Label1.Text = "La Orden " + txtorden.Text + " No se ha Cancelado";
            }

        }

        protected void btnver_Click(object sender, EventArgs e)
        {

            try
            {

                String montop = (String)(Session["montop"]);
                String totalp = (String)(Session["totalp"]);
                String clientep = (String)(Session["clientep"]);

                String consulta = "use ProductosMagnificos select * from Listas where Codigo = '" + txtorden.Text + "'";
                comando = new SqlCommand(consulta, conex);
                conex.Open();

                SqlDataReader leer = comando.ExecuteReader();

                if (leer.Read() == true)
                {
                    Session["montop"] = leer["Nombre"].ToString();
                    Session["totalp"] = leer["Total"].ToString();
                    Session["clientep"] = leer["fk_nit_cliente"].ToString();
                }

                conex.Close();

                comando.Connection = conex;
                comando.CommandText = "Update Listas set Estado = 'Cancelado' where Codigo = " + txtorden.Text;
                conex.Open();
                comando.ExecuteNonQuery();
                conex.Close();


                string html = "<html><head><H1><center>Nota de Credito</center></H1></head><body>" +
                              "<br> Nit del cliente: <b>" + Session["clientep"] + "</b>" +
                              "<br> Monto pagado previamente: <b>" + Session["montop"] + "</b>" +
                              "<br><br><br>Total a de la lista : <b>Q." + Session["totalp"] + "</b>. <br><br>" +
                              "<br> ----------------------------------------------------------------------------------------------------- <b> <br><br>" +
                              "<head><H1><center>Factura</center></H1></head>" +
                              "<br> Nit del cliente: <b>" + Session["clientep"] + "</b>" +
                              "<br> Monto pagado previamente: Q.<b>" + Session["montop"] + "</b>" +
                              "</body></html>";

                Document nvopdf = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                PdfWriter.GetInstance(nvopdf, Response.OutputStream);
                nvopdf.Open();

                foreach (IElement E in HTMLWorker.ParseToList(new StringReader(html), new StyleSheet()))

                    nvopdf.Add(E);

                nvopdf.Close();

                Response.ContentType = "aplication/pdf";
                Response.AppendHeader("content-disposition", "attachment;filename=Nota de Credito.pdf");
                Response.Write(nvopdf);
                Response.Flush();
                Response.End();

                //   Label1.Text = "La Orden " + txtorden.Text + " se ha Aprobado";
            }
            catch
            {
                //  Label1.Text = "La Orden " + txtorden.Text + " No se ha Aprobado";
            }

        }



        void GetDatos(String orden)
        {
            
            try
            {
                String stringorden = "";
                double totalpr = 0;
                double creditopr = 0;
            
                String consulta = "use ProductosMagnificos Select L.Codigo, L.Total, C.Credito, C.Nit"
                            + " From Listas L, Clientes C"
                            + " where L.fk_nit_cliente = C.Nit";
                comando = new SqlCommand(consulta, conex);
                conex.Open();

                SqlDataReader leer = comando.ExecuteReader();

                if (leer.Read() == true)
                {
                    stringorden = leer["Codigo"].ToString();

                    rtotal = leer["Total"].ToString();

                    String Total = (String)(Session["Total"]);
                    Session["Total"] = rtotal;

                    rcredito = leer["Credito"].ToString();

                    String Credito = (String)(Session["Credito"]);
                    Session["Credito"] = rcredito;

                    rnit = leer["Nit"].ToString();


                    String Nit = (String)(Session["Nit"]);
                    Session["Nit"] = rnit;

                    
                }

                if (stringorden == orden)
                {

                    totalpr = Convert.ToDouble(Session["Total"]);
                    creditopr = Convert.ToDouble(Session["Credito"]);

                    double nuevocredito = creditopr - totalpr;

                    String NCredito = (String)(Session["NCredito"]);
                    Session["NCredito"] = Convert.ToString(nuevocredito);
                  //  Label1.Text = Convert.ToString(nuevocredito);

                }

                else
                {
                    Label1.Text = "verifique sus datos";
                }
            }
            catch (SqlException)
            {
                Label1.Text = "verifique conexion";
            }
            conex.Close();
            comando.Dispose();
        }

        protected void btnpagar_Click(object sender, EventArgs e)
        {
            String orden = txtorden.Text;
            String monto = txtmonto.Text;
            String total = "";

            String consulta = "use ProductosMagnificos select * from Listas where Codigo = '" + orden + "'";
            comando = new SqlCommand(consulta, conex);
            conex.Open();

            SqlDataReader leer = comando.ExecuteReader();

            if (leer.Read() == true)
            {
                total = leer["Total"].ToString();
            }

            conex.Close();

            Double ntot = 0;

            Double tot = 0;
            Double txttot = 0;

            try
            {
                txttot = Convert.ToDouble(monto);
                tot = Convert.ToDouble(total);

                ntot = tot - txttot;

                comando.Connection = conex;
                comando.CommandText = "Update Listas set Total = " + ntot.ToString() + " where Codigo = " + orden +
                                                      " Update Listas set Nombre = " + txtmonto.Text + " where Codigo = " + orden;
                conex.Open();
                comando.ExecuteNonQuery();

                Label1.Text = "La Orden " + txtorden.Text + " se ha abonado";
            }
            catch
            {
                Label1.Text = "La Orden " + txtorden.Text + " No se ha abonado";
            }   
            

        }

        protected void btnbusqueda2_Click(object sender, EventArgs e)
        {

            comando.Connection = conex;
            comando.CommandText = " 	select A.Producto , A.Valor, L.Codigo as lista " +
                                " from ( " +
                                    " select P.Nombre as 'Producto', I.fk_Lista as 'Codigo', I.Valor as 'Valor' " +
                                " from ItemsListas I, Productos p " +
                                " where I.fk_Codigo_producto = P.Codigo ) A , Listas L " +

                                " where A.Codigo  =  L.Codigo and " +
                                " L.fk_nit_cliente = '" + txtbusqueda.Text + "'  ";
            conex.Open();
            comando.ExecuteNonQuery();

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = comando;
            DataSet ds = new DataSet();
            da.Fill(ds, "Codigo");
            GridView1.DataSource = ds;
            GridView1.DataBind();
            conex.Close();




            String cnombre = (String)(Session["cnombre"]);
            String capellido = (String)(Session["capellido"]);
            String cdireccion = (String)(Session["cdireccion"]);
            String ctelefono = (String)(Session["ctelefono"]);



            //comando.Connection = conex;
            String consulta = "use ProductosMagnificos select * from Clientes where Nit = '" + txtbusqueda.Text + "'";
            comando = new SqlCommand(consulta, conex);
            conex.Open();

            SqlDataReader leer = comando.ExecuteReader();

            if (leer.Read() == true)
            {
                Session["cnombre"] = leer["Nombre"].ToString();
                Session["capellido"] = leer["Apellido"].ToString();
                Session["cdireccion"] = leer["Direccion"].ToString();
                Session["ctelefono"] = leer["Telefono"].ToString();

            }

            conex.Close();



            string html = "<html><head><H1><center>Reporte Cliente</center></H1></head><body>" +
                          "<br> Nombre del cliente: <b>" + Session["cnombre"] + " " + Session["capellido"] +"</b>"+
                          "<br> Nit del cliente: <b>" + txtbusqueda.Text + "</b>" +
                          "<br> Telefono del cliente: <b>" + Session["ctelefono"] + "</b>" +
                          "<br> Direccion del cliente: <b>" + Session["cdireccion"] + "</b></b>. <br><br>"+
                          "</body></html>";

            PdfPTable tabla = new PdfPTable(GridView1.HeaderRow.Cells.Count);
                 
            /////////////// para las celdas de encabezado
                foreach (TableCell celda in GridView1.HeaderRow.Cells)
                {
                    PdfPCell pdfcelda = new PdfPCell(new Phrase(celda.Text));
                    tabla.AddCell(pdfcelda);
                }
            ///////////////// para el resto de las celdas
            foreach (GridViewRow fila in GridView1.Rows)
            {
                foreach (TableCell celda in fila.Cells)
                {
                    PdfPCell pdfcelda = new PdfPCell(new Phrase(celda.Text));
                    tabla.AddCell(pdfcelda);
                }
            }

            ///////////////// escribe el documento
            Document nvopdf = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
            PdfWriter.GetInstance(nvopdf, Response.OutputStream);
            nvopdf.Open();

            foreach (IElement E in HTMLWorker.ParseToList(new StringReader(html), new StyleSheet()))
                
            nvopdf.Add(E);

            nvopdf.Add(tabla);
            nvopdf.Close();

            Response.ContentType = "aplication/pdf";
            Response.AppendHeader("content-disposition", "attachment;filename=Reporte.pdf");
            Response.Write(nvopdf);
            Response.Flush();
            Response.End();
        
        }

        protected void Btnbusqueda3_Click(object sender, EventArgs e)
        {
            comando.Connection = conex;
            comando.CommandText = "	select P.Nombre as 'Producto', I.fk_Lista as 'Codigo de lista', I.Valor as 'Valor' " +
                                  " from ItemsListas I, Productos p " +
                                  " where I.fk_Codigo_producto = P.Codigo  and " +
                                        "P.fk_Categoria = '" + txtbusqueda.Text + "'  ";
            conex.Open();
            comando.ExecuteNonQuery();

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = comando;
            DataSet ds = new DataSet();
            da.Fill(ds, "Codigo");
            GridView1.DataSource = ds;
            GridView1.DataBind();
            conex.Close();
        }

        protected void btnreporte_Click(object sender, EventArgs e)
        {
           

            string html = "<html><head><H1><center>Reporte</center></H1></head><body> <br><br>" +
                          "</body></html>";

            PdfPTable tabla = new PdfPTable(GridView1.HeaderRow.Cells.Count);

            /////////////// para las celdas de encabezado
            foreach (TableCell celda in GridView1.HeaderRow.Cells)
            {
                PdfPCell pdfcelda = new PdfPCell(new Phrase(celda.Text));
                tabla.AddCell(pdfcelda);
            }
            ///////////////// para el resto de las celdas
            foreach (GridViewRow fila in GridView1.Rows)
            {
                foreach (TableCell celda in fila.Cells)
                {
                    PdfPCell pdfcelda = new PdfPCell(new Phrase(celda.Text));
                    tabla.AddCell(pdfcelda);
                }
            }

            ///////////////// escribe el documento
            Document nvopdf = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
            PdfWriter.GetInstance(nvopdf, Response.OutputStream);
            nvopdf.Open();

            foreach (IElement E in HTMLWorker.ParseToList(new StringReader(html), new StyleSheet()))

                nvopdf.Add(E);

            nvopdf.Add(tabla);
            nvopdf.Close();

            Response.ContentType = "aplication/pdf";
            Response.AppendHeader("content-disposition", "attachment;filename=Reporte.pdf");
            Response.Write(nvopdf);
            Response.Flush();
            Response.End();
        }



    }
}