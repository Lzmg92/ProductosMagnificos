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
    public partial class MenuOperador : System.Web.UI.Page
    {
        SqlConnection conex = new SqlConnection();
        SqlCommand comando = new SqlCommand();
        SqlCommand comand = new SqlCommand();
        SqlCommand comando2 = new SqlCommand();
        StringBuilder stringcodigotrans = new StringBuilder();
    
        double suma;
    
    
        
        protected void Page_Load(object sender, EventArgs e)
        {

            conex.ConnectionString = (@"Data Source = LEZS; Initial Catalog = ProductosMagnificos; Integrated security=true");
            conex.Open();
            String com = " Select Productos.Codigo, Productos.Nombre as 'Nombre Del Producto',  C.Nombre as Categoria"
                           + "  From Categorias C, Productos "
                            + " where C.Codigo = Productos.fk_Categoria" ;
            SqlCommand comand = new SqlCommand(com, conex);
            comand.ExecuteNonQuery();

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = comand;
            DataSet ds = new DataSet();

            da.Fill(ds, "Nombre");

            
            GridView1.DataSource = ds;
            GridView1.DataBind();
            conex.Close();

            if (!IsPostBack) {
                Random random = new Random();
                string combination2 = "0123456789";
                for (int i = 0; i < 6; i++)
                {
                    stringcodigotrans.Append(combination2[random.Next(combination2.Length)]);
                    lblcodigo.Text = stringcodigotrans.ToString();
                }
            }

            String bienvenido = (String)(Session["bienvenido"]);
            Session["bienvenido"] = bienvenido;
            lblcodigovendedor.Text = bienvenido;
        }

        void agregar()
        {
            string codigoproducto = "";
            Random random = new Random();
            string combination2 = "0123456789";
            for (int i = 0; i < 6; i++)
            {
                stringcodigotrans.Append(combination2[random.Next(combination2.Length)]);
                codigoproducto = stringcodigotrans.ToString();
            }

           try
            {
                comando.Connection = conex;
                comando.CommandText = "Insert ItemsListas(Codigo, fk_Codigo_producto, Valor, fk_Lista)" 
                                     +" values ('"+codigoproducto + "','" + txtproducto.Text + "','" + txtvalor.Text + "','" + lblcodigo.Text + "') "
                                     + " Insert Metas (Codigo, Valor, Mes, Nit_empleado) values"
                                     +" ('"+codigoproducto+"','"+txtvalor.Text+"',(CONCAT(DATEPART(MONTH, SYSDATETIME()),'/',DATEPART(YEAR, SYSDATETIME()))) ,'"+lblcodigovendedor.Text+"')";
                conex.Open();
                comando.ExecuteNonQuery();
                conex.Close();

                conex.ConnectionString = (@"Data Source = LEZS; Initial Catalog = ProductosMagnificos; Integrated security=true");
                conex.Open();


                String com = " Select Productos.Nombre , C.Nombre as Categora , I.Valor"
                             + "  From ItemsListas I, Productos, Categorias C  "
                              + " where Productos.Codigo =I.fk_Codigo_producto and Productos.fk_Categoria = C.Codigo  and fk_Lista =" + lblcodigo.Text;
                SqlCommand comand = new SqlCommand(com, conex);
                comand.ExecuteNonQuery();

                SqlDataAdapter da = new SqlDataAdapter();


                da.SelectCommand = comand;
                DataSet ds = new DataSet();

                da.Fill(ds, "Nombre");
                GridView2.DataSource = ds;
                GridView2.DataBind();
                conex.Close();

               //////////////////////////////////////////////////////////////////////////////////////////////////////////

                String consulta = "use ProductosMagnificos select * from Moneda where Simbolo = '$'";
                comando = new SqlCommand(consulta, conex);
                conex.Open();

                double camb = 0;

                SqlDataReader leer = comando.ExecuteReader();

                if (leer.Read() == true)
                {
                    camb = Convert.ToDouble(leer["Cambio"]);
                }

                conex.Close();


                String consulta2 = "use ProductosMagnificos select * from Moneda where Simbolo = 'E'";
                comando = new SqlCommand(consulta2, conex);
                conex.Open();

                double camb2 = 0;

                SqlDataReader leer2 = comando.ExecuteReader();

                if (leer2.Read() == true)
                {
                    camb2 = Convert.ToDouble(leer2["Cambio"]);
                }

                conex.Close();

               /////////////////////////////////////////////////////////////////////////////////////////////////////////

                double totales = 0;
                double totales2 = 0; 

                suma = GridView2.Rows.Cast<GridViewRow>().Sum(x => Convert.ToDouble(x.Cells[2].Text));

                totales = suma * camb;
                totales2 = suma * camb2;

                lbltotal.Text = "El total es: <br />Q." + Convert.ToString(suma) + "<br> $." + totales.ToString() + "<br> E." + totales2.ToString();



                String sumas = (String)(Session["sumas"]);
                Session["sumas"] = Convert.ToString(suma);

           
        }
        catch
          {
        
          Label1.Text = "El producto " + txtproducto.Text + " No Pudo Agregarse";
          }
       }

      
    

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i <= GridView1.Rows.Count; i++)
            {
          
            }
        }


        protected void GridView2_RowCreated(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i <= GridView1.Rows.Count; i++)
            {
            }
                    }



        protected void btncomenzar_Click(object sender, EventArgs e)
        {


           try
          {
                comando.Connection = conex;
                comando.CommandText = "insert Listas (Codigo, Nombre , fk_nit_cliente) values('" + lblcodigo.Text + "','Lista de " + txtcliente.Text +"','"+txtcliente.Text+"')";
              
                conex.Open();
                comando.ExecuteNonQuery();

                conex.Close();

                String cnit = (String)(Session["cnit"]);
                Session["cnit"] = txtcliente.Text;

               
                
       
            Label1.Text = "Transaccion " + lblcodigo.Text + " Iniciada Exitosamente";
          }
          catch
           {
              Label1.Text = "Transaccion " + lblcodigo.Text + " No Pudo Iniciarse";
           }
      

           
        }

        protected void btnagregar_Click(object sender, EventArgs e)
        {
            agregar();
            txtproducto.Text = "";
            txtvalor.Text = "";
        }

        protected void Btnbusqueda_Click(object sender, EventArgs e)
        {
            String str = "select * from Productos where (Nombre like '%' + @search +'%')";
            SqlCommand comando = new SqlCommand(str, conex);
            comando.Parameters.Add("@search", SqlDbType.NVarChar).Value = txtbusqueda.Text;

            conex.Open();
            comando.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = comando;
            DataSet ds = new DataSet();
            da.Fill(ds, "Nombre");
            GridView1.DataSource = ds;
            GridView1.DataBind();
            conex.Close();

        }

        protected void btneliminar_Click(object sender, EventArgs e)
        {
            try
           {
                comando.Connection = conex;               
                comando.CommandText = "delete from ItemsListas where Codigo = '" + lblcodigo.Text +"'  and fk_Codigo_producto = '"+ txtproducto.Text +"'" ;
                conex.Open();
                comando.ExecuteNonQuery();
                conex.Close();


                conex.ConnectionString = (@"Data Source = LEZS; Initial Catalog = ProductosMagnificos; Integrated security=true");
                conex.Open();


                String com = " Select Productos.Nombre , C.Nombre as Categora , I.Valor"
                             + "  From ItemsListas I, Productos, Categorias C  "
                              + " where Productos.Codigo =I.fk_Codigo_producto and Productos.fk_Categoria = C.Codigo  and fk_Lista =" + lblcodigo.Text;
                SqlCommand comand = new SqlCommand(com, conex);
                comand.ExecuteNonQuery();

                SqlDataAdapter da = new SqlDataAdapter();


                da.SelectCommand = comand;
                DataSet ds = new DataSet();

                da.Fill(ds, "Nombre");
                GridView2.DataSource = ds;
                GridView2.DataBind();
                conex.Close();



                suma = GridView2.Rows.Cast<GridViewRow>().Sum(x => Convert.ToDouble(x.Cells[2].Text));
                lbltotal.Text = "El total es: <br />Q." + Convert.ToString(suma);


                String sumas = (String)(Session["sumas"]);
                Session["sumas"] = Convert.ToString(suma);


            }
            catch
           {
            Label1.Text = "El producto " + txtproducto.Text + " No Pudo Eliminarse";
          } 
        }

       
        protected void btnterminar_Click(object sender, EventArgs e)
        {
  
            try
            {
                comando.Connection = conex;
                comando.CommandText = "Update Listas set Total = '" + Session["sumas"] + "' where Codigo = " + lblcodigo.Text+
                                       "Update Listas set VigenciaInicio = (CONCAT(DATEPART(DAY, SYSDATETIME()),'/',DATEPART(MONTH, SYSDATETIME()),'/',DATEPART(YEAR, SYSDATETIME())))   where Codigo = " + lblcodigo.Text +
                                       "Update Listas set VigenciaFinal = (CONCAT(DATEPART(DAY, SYSDATETIME()),'/12/',DATEPART(YEAR, SYSDATETIME())))   where Codigo = " + lblcodigo.Text;
              ;
                conex.Open();
                comando.ExecuteNonQuery();
                Label1.Text = "La Orden " + lblcodigo.Text + " se ha guardado";

            
            }
            catch
            {
                Label1.Text = "La Orden " + lblcodigo.Text + " No se ha guardado";
            }



            Random random = new Random();
            string combination2 = "0123456789";
            for (int i = 0; i < 6; i++)
            {
                stringcodigotrans.Append(combination2[random.Next(combination2.Length)]);
                lblcodigo.Text = stringcodigotrans.ToString();
            }

            lbltotal.Text = "El total es: <br />Q.";
            txtcliente.Text = "";

        }

        protected void btnreporte_Click(object sender, EventArgs e)
        {

            String cnombre = (String)(Session["cnombre"]);
            String capellido = (String)(Session["capellido"]);
            String cdireccion = (String)(Session["cdireccion"]);
            String ctelefono = (String)(Session["ctelefono"]);



            //comando.Connection = conex;
            String consulta = "use ProductosMagnificos select * from Clientes where Nit = '" + Session["cnit"] + "'";
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



            string html = "<html><head><H1><center>Reporte de Lista</center></H1></head><body>" +
                          "<br> Nombre del cliente: <b>" + Session["cnombre"] + " " + Session["capellido"] +"</b>"+
                          "<br> Nit del cliente: <b>" + Session["cnit"] + "</b>" +
                          "<br> Telefono del cliente: <b>" + Session["ctelefono"] + "</b>" +
                          "<br> Direccion del cliente: <b>" + Session["cdireccion"] + "</b>" +
                          "<br><br><br>Total a pagar: <b>Q." + Session["sumas"] + "</b>. <br><br>" +
                          "</body></html>";

            PdfPTable tabla = new PdfPTable(GridView2.HeaderRow.Cells.Count);
                 
            /////////////// para las celdas de encabezado
                foreach (TableCell celda in GridView2.HeaderRow.Cells)
                {
                    PdfPCell pdfcelda = new PdfPCell(new Phrase(celda.Text));
                    tabla.AddCell(pdfcelda);
                }
            ///////////////// para el resto de las celdas
            foreach (GridViewRow fila in GridView2.Rows)
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
            Response.AppendHeader("content-disposition", "attachment;filename=Lista.pdf");
            Response.Write(nvopdf);
            Response.Flush();
            Response.End();
        }

    }
}