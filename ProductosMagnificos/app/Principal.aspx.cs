using System;
using System.Collections;
using System.Web.Security;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.IO;
using System.Data.Sql;
using System.Data.SqlClient;

namespace ProductosMagnificos.app
{
    public partial class Principal : System.Web.UI.Page
    {

        SqlConnection conex = new SqlConnection();
        SqlCommand comand = new SqlCommand();
        SqlDataSource uno = new SqlDataSource();
        String _path;
        public int catego, produc, ciuda, dep, cli, pues = 0;
        string categor;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            conex.ConnectionString = (@"Data Source = LEZS; Initial Catalog = ProductosMagnificos; Integrated security=true");
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            if (listcategoria.SelectedItem.Text.Equals("Operador")){categor="4";}
            else if(listcategoria.SelectedItem.Text.Equals("Supervisor")){categor="3";}
            else if(listcategoria.SelectedItem.Text.Equals("Administrador")){categor="2";}
            else if (listcategoria.SelectedItem.Text.Equals("Gerente")) { categor = "1"; }
            verificar(txtusuario.Text, txtcontraseña.Text, categor);
        }

          void verificar(String usuario, String contraseña, String categoria) {
            try
            {
                String stringusua = "";
                String stringcontra = "";
                String stringcatego = "";
                String consulta = "use ProductosMagnificos select * from Empleados where Nombre = '"+usuario+"'";
                comand = new SqlCommand(consulta, conex);
                conex.Open();

                SqlDataReader leer = comand.ExecuteReader();

                if (leer.Read() == true)
                {
                    stringusua = leer["Nombre"].ToString();
                    stringcontra = leer["Nit"].ToString();
                    stringcatego = leer["fk_codigo_puesto"].ToString();
                }

                if (stringusua == usuario && stringcontra == contraseña && stringcatego == categoria)
                {
                    if (categoria == "4")
                    {
                        Session["bienvenido"] = stringcontra;
                        Response.Redirect("MenuOperador.aspx");
                    }
                    else if (categoria == "1")
                    {
                        Session["bienvenido"] = stringusua;
                        Response.Redirect("MenuGerente.aspx");
                    }
                    else if (categoria == "2")
                    {
                        Session["bienvenido"] = stringusua;
                        Response.Redirect("MenuAdministrador.aspx");
                    }
                    else if (categoria == "3")
                    {
                        Session["bienvenido"] = stringusua;
                        Response.Redirect("MenuSupervisor.aspx");
                    }
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
            comand.Dispose();      
        }

          protected void Cargar_Click(object sender, EventArgs e)
          {
               if (cargador.PostedFile.FileName == "")
            {
                Label2.Text = "No se ha seleccionado ningun archivo";
            }
            else
            {
                String extension = Path.GetExtension(cargador.PostedFile.FileName);

                if (extension.ToLower() != ".xml")
                {
                    Label2.Text = "El tipo de archivo no es compatible";
                }
                else
                {
                    try
                    {
                        String carpeta_final = Server.MapPath(cargador.FileName);
                        cargador.PostedFile.SaveAs(carpeta_final);
                        subir(carpeta_final);

                        Label2.Text = "Archivo cargado satisfactoriamente";
                       // cargarproductos(_path);
                        cargar(_path);
                       
                    }
                    catch (Exception exp1)
                    {
                        Label2.Text = exp1.Message + "|||" + exp1.Data + " ||||" + exp1.InnerException + "|||  " + exp1.Source + "||" + exp1.StackTrace + "|||  " + exp1.TargetSite;
                    }
                                    }
                                                 }
                              }

          public void subir(String p)
          {
              _path = p;
          }

          void cargar(string datos)
          {
              XmlDocument documento = new XmlDocument();
              documento.Load(datos);
              XmlNodeList categorias = documento.GetElementsByTagName("categorias");

              ///////////////////////////////////////////////////////////////CATEGORIA

              XmlNodeList categoria = ((XmlElement)categorias[0]).GetElementsByTagName("categoria");
              foreach (XmlElement nodocategoria in categoria)
              {
                  conex.Open();
                  XmlNodeList codigo = nodocategoria.GetElementsByTagName("codigo");
                  XmlNodeList nombre = nodocategoria.GetElementsByTagName("nombre");

                  SqlCommand Comando = new SqlCommand(String.Format("Insert Categorias(Codigo, Nombre)values('{0}','{1}')", codigo[catego].InnerText, nombre[catego].InnerText), conex);
                  Comando.ExecuteNonQuery();
                  conex.Close();
              }

              ////////////////////////////////////////////////////////////// PRODUCTOS

              XmlNodeList productos = documento.GetElementsByTagName("productos");
              XmlNodeList producto = ((XmlElement)productos[0]).GetElementsByTagName("producto");
              foreach (XmlElement nodoproducto in producto)
              {
                  conex.Open();
                  XmlNodeList codigo = nodoproducto.GetElementsByTagName("codigo");
                  XmlNodeList nombre = nodoproducto.GetElementsByTagName("nombre");
                  XmlNodeList pcategoria = nodoproducto.GetElementsByTagName("categoria");

                  SqlCommand Comando = new SqlCommand(String.Format("Insert Productos(Codigo, Nombre, fk_Categoria)values('{0}','{1}','{2}')", codigo[produc].InnerText, nombre[produc].InnerText, pcategoria[produc].InnerText), conex);
                  Comando.ExecuteNonQuery();
                  conex.Close();
              }

              ////////////////////////////////////////////////////////////// CIUDADES

              XmlNodeList ciudades = documento.GetElementsByTagName("ciudades");
              XmlNodeList ciudad = ((XmlElement)ciudades[0]).GetElementsByTagName("ciudad");
              foreach (XmlElement nodociudad in ciudad)
              {
                  conex.Open();
                  XmlNodeList codigo = nodociudad.GetElementsByTagName("codigo");
                  XmlNodeList nombre = nodociudad.GetElementsByTagName("nombre");

                  SqlCommand Comando = new SqlCommand(String.Format("Insert Ciudades(Codigo, Nombre)values('{0}','{1}')", codigo[ciuda].InnerText, nombre[ciuda].InnerText), conex);
                  Comando.ExecuteNonQuery();
                  conex.Close();
              }

              ////////////////////////////////////////////////////////////// DEPTO

              XmlNodeList deptos = documento.GetElementsByTagName("deptos");
              XmlNodeList depto = ((XmlElement)deptos[0]).GetElementsByTagName("depto");
              foreach (XmlElement nododepto in depto)
              {
                  conex.Open();
                  XmlNodeList codigo = nododepto.GetElementsByTagName("codigo");
                  XmlNodeList nombre = nododepto.GetElementsByTagName("nombre");
                  XmlNodeList dciudad = nododepto.GetElementsByTagName("codigo_ciudad");

                  SqlCommand Comando = new SqlCommand(String.Format("Insert Departamento(Codigo, Nombre, fk_Ciudad)values('{0}','{1}','{2}')", codigo[dep].InnerText, nombre[dep].InnerText, dciudad[dep].InnerText), conex);

                  Comando.ExecuteNonQuery();
                  conex.Close();
              }

              ////////////////////////////////////////////////////////////// CLIENTES

              XmlNodeList clientes = documento.GetElementsByTagName("clientes");
              XmlNodeList cliente = ((XmlElement)clientes[0]).GetElementsByTagName("cliente");
              foreach (XmlElement nodocliente in cliente)
              {
                  conex.Open();
                  XmlNodeList codigo = nodocliente.GetElementsByTagName("nit");
                  XmlNodeList nombre = nodocliente.GetElementsByTagName("nombre");
                  XmlNodeList apellido = nodocliente.GetElementsByTagName("apellido");
                  XmlNodeList nacimiento = nodocliente.GetElementsByTagName("nacimiento");
                  XmlNodeList direccion = nodocliente.GetElementsByTagName("direccion");
                  XmlNodeList telefono = nodocliente.GetElementsByTagName("telefono");
                  XmlNodeList celular = nodocliente.GetElementsByTagName("celular");
                  XmlNodeList email = nodocliente.GetElementsByTagName("email");
                  XmlNodeList cdepto = nodocliente.GetElementsByTagName("depto");
                  XmlNodeList limite_credito = nodocliente.GetElementsByTagName("limite_credito");
                  XmlNodeList codigo_lista = nodocliente.GetElementsByTagName("codigo_lista");

                  SqlCommand Comando = new SqlCommand(String.Format("Insert Clientes(Nit, Nombre, Apellido, Nacimiento, Direccion, Telefono, Celular, Email, Departamento, Credito, Ciudad)values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",
                                                                    codigo[cli].InnerText, nombre[cli].InnerText, apellido[cli].InnerText, nacimiento[cli].InnerText, direccion[cli].InnerText, telefono[cli].InnerText, celular[cli].InnerText, email[cli].InnerText, cdepto[cli].InnerText, limite_credito[cli].InnerText, codigo_lista[cli].InnerText), conex);

                  Comando.ExecuteNonQuery();
                  conex.Close();
              }


              ////////////////////////////////////////////////////////////// PUESTOS

              XmlNodeList puestos = documento.GetElementsByTagName("puestos");
              XmlNodeList puesto = ((XmlElement)puestos[0]).GetElementsByTagName("puesto");
              foreach (XmlElement nodopuesto in puesto)
              {
                  conex.Open();
                  XmlNodeList codigo = nodopuesto.GetElementsByTagName("codigo");
                  XmlNodeList nombre = nodopuesto.GetElementsByTagName("nombre");

                  SqlCommand Comando = new SqlCommand(String.Format("Insert Puesto(Codigo, Nombre)values('{0}','{1}')", codigo[pues].InnerText, nombre[pues].InnerText), conex);
                  Comando.ExecuteNonQuery();
                  conex.Close();
              }


          }









    }
}