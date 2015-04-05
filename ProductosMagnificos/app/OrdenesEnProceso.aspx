<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrdenesEnProceso.aspx.cs" Inherits="ProductosMagnificos.app.OrdenesEnProceso" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ordenes En Proceso</title>
     <link href ="Styles/Menu.css" rel="stylesheet" type ="text/css" />
</head>
<body>
    <form id="form1" runat="server">
   
         <asp:Label ID="Label1" runat="server" ForeColor="White" Text="Label"></asp:Label>
          <asp:Label ID="lblinicio"  CssClass="titulo1" runat="server" Text="Ordenes En Proceso"></asp:Label>
 
    <menu>
        <li><a href = "MenuOperador.aspx">Productos y Ordenes</a></li>
        <li>Ordenes En Proceso</li>
        <li><a href = "DatosUsuario.aspx">Datos de Usuario</a></li>
        <li><a href = "Principal.aspx">Salir</a></li>

    </menu>
  
          <asp:TextBox ID="txtbusqueda" CssClass="textbox1" runat="server"></asp:TextBox>
        <asp:Button ID="Btnbusqueda" CssClass="boton1" runat="server" Text="Buscar" OnClick="Btnbusqueda_Click" />

          <asp:GridView ID="GridView1" CssClass="tabla" runat="server" ></asp:GridView>
        <asp:GridView ID="GridView2" CssClass="tabla2" runat="server" > </asp:GridView>

          <asp:Label ID="lbloperador" CssClass="label1ingreso" runat="server" Text="Operador:"></asp:Label>
        <asp:Label ID="lblcodigo" CssClass="label11ingreso" runat="server"  Text="Label"></asp:Label>

        <asp:Label ID="Label4" CssClass="label2ingreso" runat="server"  Text="Orden"></asp:Label>
        <asp:TextBox ID="txtorden" CssClass="textbox2ingreso" runat="server" ></asp:TextBox>

        <asp:Label ID="Label5" CssClass="label3ingreso" runat="server"  Text="Monto"></asp:Label>
        <asp:TextBox ID="txtmonto" CssClass =" textbox3ingreso" runat="server" ></asp:TextBox>

         <asp:Button ID="btnabonar" CssClass="boton1ingreso" runat="server" Text="Abonar" OnClick="btnabonar_Click"  />
         <asp:Button ID="btncancelar" CssClass="boton2ingreso" runat="server" Text="Cancelar" OnClick="btneliminar_Click" />
      


        <asp:Button ID="btnpagar" CssClass="botonterminar" runat="server" Text="Realizar Pago" OnClick="btnpagar_Click" />


         <asp:Button ID="btneliminar2" CssClass="boton3ingreso" runat="server" Text="Eliminar Abonado" OnClick="btneliminar2_Click" />


    </form>
</body>
</html>
