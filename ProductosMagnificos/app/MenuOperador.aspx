<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuOperador.aspx.cs" Inherits="ProductosMagnificos.app.MenuOperador" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Menú Operador</title>
     <link href ="Styles/Menu.css" rel="stylesheet" type ="text/css" />
</head>
<body>
    <form id="form1" runat="server">

          <asp:Label ID="lblinicio"  CssClass="titulo1" runat="server" Text="Menú Operador"></asp:Label>
 
    <menu>
        <li>Productos y Ordenes</li>
        <li><a href = "OrdenesEnProceso.aspx">Ordenes En Proceso</a></li>
        <li><a href = "DatosUsuario.aspx">Datos de Usuario</a></li>
        <li><a href = "Principal.aspx">Salir</a></li>

    </menu>
  
        <asp:Label ID="Label1" runat="server" ForeColor="White" Text="Label"></asp:Label>
        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>


        <asp:TextBox ID="txtbusqueda" CssClass="textbox1" runat="server"></asp:TextBox>
        <asp:Button ID="Btnbusqueda" CssClass="boton1" runat="server" Text="Buscar" OnClick="Btnbusqueda_Click" />


           <asp:Label ID="lblvendedor" CssClass="label1ingreso" runat="server" Text="Vendedor:"></asp:Label>
        <asp:Label ID="lblcodigovendedor" CssClass="label11ingreso" runat="server"  Text="Label"></asp:Label>

        <asp:Label ID="Label4" CssClass="label2ingreso" runat="server"  Text="Cliente"></asp:Label>
        <asp:TextBox ID="txtcliente" CssClass="textbox2ingreso" runat="server" ></asp:TextBox>

        <asp:Label ID="Label5" CssClass="label3ingreso" runat="server"  Text="Producto"></asp:Label>
        <asp:TextBox ID="txtproducto" CssClass =" textbox3ingreso" runat="server" ></asp:TextBox>

        <asp:Label ID="Label45" runat="server" CssClass="label4ingreso" Text="Valor"></asp:Label>
        <asp:TextBox ID="txtvalor" CssClass="textbox4ingreso" runat="server"  ></asp:TextBox>


        <asp:GridView ID="GridView2" CssClass="tabla2" runat="server" OnRowcreated = "GridView2_RowCreated"> </asp:GridView>

        <asp:GridView ID="GridView1" CssClass="tabla" runat="server" OnRowcreated = "GridView1_RowCreated"></asp:GridView>

      
        
          <asp:Button ID="btnagregar" CssClass="boton1ingreso" runat="server" Text="Agregar" OnClick="btnagregar_Click" />
        <asp:Button ID="btneliminar" CssClass="boton2ingreso" runat="server" Text="Eliminar" OnClick="btneliminar_Click" />

       
          <asp:Label ID="Label6" CssClass="label01ingreso" runat="server" Text="Transacción:"></asp:Label>
        <asp:Label ID="lblcodigo" CssClass="label011ingreso" runat="server" Text="Label"></asp:Label>

        
          <asp:Button ID="btncomenzar" CssClass="botoncomenzar" runat="server" Text="Iniciar Transacción" OnClick="btncomenzar_Click" />
        <asp:Button ID="btnterminar" CssClass="botonterminar" runat="server" Text="Cerrar Orden" OnClick="btnterminar_Click" />
        <asp:Label ID="lbltotal" CssClass="labeltotal" runat="server" Text="El total es: "></asp:Label>
           
          <asp:Button ID="btnreporte" CssClass="botonreporte" runat="server" Text="Crear Reporte" OnClick="btnreporte_Click" />
        
        
        
    </form>
</body>
</html>
