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
    public partial class OrdenesEnProceso : System.Web.UI.Page
    {
        SqlConnection conex = new SqlConnection();
        SqlCommand comando = new SqlCommand();
        protected void Page_Load(object sender, EventArgs e)
        {
            String bienvenido = (String)(Session["bienvenido"]);
            Session["bienvenido"] = bienvenido;

            conex.ConnectionString = (@"Data Source = LEZS; Initial Catalog = ProductosMagnificos; Integrated security=true");
            conex.Open();
            String com = "Select L.Codigo, L.Total, L.VigenciaInicio, L.VigenciaFinal, C.Credito, C.Nit, C.Nombre as 'Nombre Cliente', L.Estado"
                            +" From Listas L, Clientes C"
                            +" where L.fk_nit_cliente = C.Nit";

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
            String str =  "Select L.Codigo as Codigo, L.Total, L.VigenciaInicio, L.VigenciaFinal, C.Credito, C.Nit, C.Nombre as 'Nombre Cliente', L.Estado"
                            +" From Listas L, Clientes C"
                            +" where L.fk_nit_cliente = C.Nit and (C.Nombre like '%' + @search +'%')";
            SqlCommand comando = new SqlCommand(str, conex);
            comando.Parameters.Add("@search", SqlDbType.NVarChar).Value = txtbusqueda.Text;

            conex.Open();
            comando.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = comando;
            DataSet ds = new DataSet();
            da.Fill(ds, "Codigo");
            GridView2.DataSource = ds;
            GridView2.DataBind();
            conex.Close();

        }

        protected void btnabonar_Click(object sender, EventArgs e)
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
                comando.CommandText = "Update Listas set Total = " + ntot.ToString() + " where Codigo = " + orden+
                                                      " Update Listas set Nombre = "+ txtmonto.Text+ " where Codigo = " + orden ;
                conex.Open();
                comando.ExecuteNonQuery();

                Label1.Text = "La Orden " + txtorden.Text + " se ha abonado";
            }
            catch
            {
                Label1.Text = "La Orden " + txtorden.Text + " No se ha abonado";
            }   
            


        }

        protected void btneliminar_Click(object sender, EventArgs e)
        {
            try
            {
                comando.Connection = conex;
                comando.CommandText = "Update Listas set Estado = 'Cancelado' where Codigo = " + txtorden.Text;
                conex.Open();
                comando.ExecuteNonQuery();
                //   Label1.Text = "La Orden " + txtorden.Text + " se ha Aprobado";
            }
            catch
            {
                //  Label1.Text = "La Orden " + txtorden.Text + " No se ha Aprobado";
            }


        }

  
        protected void btnpagar_Click(object sender, EventArgs e)
        {

        }

        protected void btneliminar2_Click(object sender, EventArgs e)
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

    }
}