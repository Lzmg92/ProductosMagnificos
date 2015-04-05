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
    public partial class xml : System.Web.UI.Page
    {
        SqlConnection conex = new SqlConnection();
        SqlCommand comand = new SqlCommand();
        SqlDataSource uno = new SqlDataSource();
        String _path;
        public int catego, produc, ciuda, dep, cli, pues = 0;
      
        String a = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            conex.ConnectionString = (@"Data Source = LEZS; Initial Catalog = ProductosMagnificos; Integrated security=true");
        }

        protected void Cargar_Click1(object sender, EventArgs e)
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
                                                                          codigo[cli].InnerText, nombre[cli].InnerText, apellido[cli].InnerText, nacimiento[cli].InnerText, direccion[cli].InnerText, telefono[cli].InnerText, celular[cli].InnerText, email[cli].InnerText, cdepto[cli].InnerText, limite_credito[cli].InnerText, codigo_lista[cli].InnerText ), conex);

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






                    /*////////////////////////////////////////////////////////////////////////// EMPLEADO
                    XmlNodeList empleado = ((XmlElement)nododefinicion).GetElementsByTagName("empleado");
                    foreach (XmlElement nodoempleado in empleado)
                    {
                        XmlNodeList codigo = nodoempleado.GetElementsByTagName("nit");
                        XmlNodeList nombre = nodoempleado.GetElementsByTagName("nombre");
                        XmlNodeList apellido = nodoempleado.GetElementsByTagName("apellido");
                        XmlNodeList nacimiento = nodoempleado.GetElementsByTagName("nacimiento");
                        XmlNodeList direccion = nodoempleado.GetElementsByTagName("direccion");
                        XmlNodeList telefono = nodoempleado.GetElementsByTagName("telefono");
                        XmlNodeList celular = nodoempleado.GetElementsByTagName("celular");
                        XmlNodeList email = nodoempleado.GetElementsByTagName("email");
                        XmlNodeList cdepto = nodoempleado.GetElementsByTagName("codigo_puesto");
                        XmlNodeList limite_credito = nodoempleado.GetElementsByTagName("codigo_jefe");

                    }




                    ////////////////////////////////////////////////////////////////////////////////PUESTO
                    XmlNodeList puesto = ((XmlElement)nododefinicion).GetElementsByTagName("puesto");
                    foreach (XmlElement nodopuesto in puesto)
                    {
                        XmlNodeList codigo = nodopuesto.GetElementsByTagName("codigo");
                        XmlNodeList nombre = nodopuesto.GetElementsByTagName("nombre");
                    }

                    ///////////////////////////////////////////////////////////////META
                    XmlNodeList meta = ((XmlElement)nododefinicion).GetElementsByTagName("meta");
                    foreach (XmlElement nodometa in meta)
                    {

                        XmlNodeList mnit = nodometa.GetElementsByTagName("NIT_empleado");
                        XmlNodeList mes = nodometa.GetElementsByTagName("mes_meta");


                        XmlNodeList detalle = ((XmlElement)nodometa).GetElementsByTagName("detalle");
                        foreach (XmlElement nododetalle in detalle)
                        {
                            XmlNodeList item = ((XmlElement)nododetalle).GetElementsByTagName("item");
                            foreach (XmlElement nodoitem in item)
                            {
                                XmlNodeList itemcodigo = nodoitem.GetElementsByTagName("codigo_producto");
                                XmlNodeList itemventa = nodoitem.GetElementsByTagName("meta_centa");
                            }
                        }
                    }

                    ///////////////////////////////////////////////////////////////LISTA
                    XmlNodeList lista = ((XmlElement)nododefinicion).GetElementsByTagName("lista");
                    foreach (XmlElement nodolista in lista)
                    {

                        XmlNodeList codigo = nodolista.GetElementsByTagName("codigo");
                        XmlNodeList nombre = nodolista.GetElementsByTagName("nombre");


                        XmlNodeList detalle = ((XmlElement)nodolista).GetElementsByTagName("detalle");
                        foreach (XmlElement nododetalle in detalle)
                        {
                            XmlNodeList item = ((XmlElement)nododetalle).GetElementsByTagName("item");
                            foreach (XmlElement nodoitem in item)
                            {
                                XmlNodeList itemcodigo = nodoitem.GetElementsByTagName("codigo_producto");
                                XmlNodeList itemvalor = nodoitem.GetElementsByTagName("valor");

                                // a = " " + itemcodigo[j].InnerText + "\t" + itemvalor[j].InnerText + "\t" + codigo[i].InnerText;
                                //   j++;
                            }
                        }
                        // j = 0;
                        // i++;
                    }
                    //    Label2.Text = a;
                    // i = 0;


                     * 
                     * 
                     * 
                    */

        

       

    }
}