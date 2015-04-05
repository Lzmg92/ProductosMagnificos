using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ProductosMagnificos.app
{
    public partial class MenuSupervisor : System.Web.UI.Page
    {
        SqlConnection conex = new SqlConnection();
        SqlCommand comando = new SqlCommand();
        SqlCommand comando2 = new SqlCommand();
        StringBuilder stringcodigotrans = new StringBuilder();

        double suma;
        protected void Page_Load(object sender, EventArgs e)
        {
            conex.ConnectionString = (@"Data Source = LEZS; Initial Catalog = ProductosMagnificos; Integrated security=true");
            conex.Open();
            String com = " Select Productos.Codigo, Productos.Nombre,  C.Nombre as Categoría"
                           + "  From Categorias C, Productos "
                            + " where C.Codigo = Productos.fk_Categoria";
            SqlCommand comand = new SqlCommand(com, conex);
            comand.ExecuteNonQuery();

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = comand;
            DataSet ds = new DataSet();

            da.Fill(ds, "Nombre");


            GridView1.DataSource = ds;
            GridView1.DataBind();
            conex.Close();

            if (!IsPostBack)
            {
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
                                     + " values ('" + codigoproducto + "','" + txtproducto.Text + "','" + txtvalor.Text + "','" + lblcodigo.Text + "') "
                                     + " Insert Metas (Codigo, Valor, Mes, Nit_empleado) values"
                                     + " ('" + codigoproducto + "','" + txtvalor.Text + "',(CONCAT(DATEPART(MONTH, SYSDATETIME()),'/',DATEPART(YEAR, SYSDATETIME()))) ,'" + lblcodigovendedor.Text + "')";
                conex.Open();
                comando.ExecuteNonQuery();

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
                comando.CommandText = "insert Listas (Codigo, Nombre , fk_nit_cliente) values('" + lblcodigo.Text + "','Lista de " + txtcliente.Text + "','" + txtcliente.Text + "')";

                conex.Open();
                comando.ExecuteNonQuery();

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
                comando.CommandText = "delete from ItemsListas where Codigo=" + txtproducto.Text;
                conex.Open();
                comando.ExecuteNonQuery();

            }
            catch
            {
                Label1.Text = "El producto " + txtproducto.Text + " No Pudo Eliminarse";
            }
        }

        protected void btnver_Click(object sender, EventArgs e)
        {
            try
            {

                conex.ConnectionString = (@"Data Source = LEZS; Initial Catalog = ProductosMagnificos; Integrated security=true");
                conex.Open();


                String com = " Select Productos.Nombre , C.Nombre as Categoría , I.Valor"
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
            catch { }

        }

        protected void btnterminar_Click(object sender, EventArgs e)
        {

            try
            {
                comando.Connection = conex;
                comando.CommandText = "Update Listas set Total = '" + Session["sumas"] + "' where Codigo = " + lblcodigo.Text;
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

    }
}