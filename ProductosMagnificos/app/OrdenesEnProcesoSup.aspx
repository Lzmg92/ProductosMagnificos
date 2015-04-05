<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrdenesEnProcesoSup.aspx.cs" Inherits="ProductosMagnificos.app.OrdenesEnProcesoSup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>Ordenes En Proceso (Supervisor)</title>
     <link href ="Styles/Menu.css" rel="stylesheet" type ="text/css" />
</head>
<body>
    <form id="form1" runat="server">

         <br />

         <asp:Label ID="Label1" runat="server" ForeColor="White" Text="Label"></asp:Label>
    
          <asp:Label ID="lblinicio"  CssClass="titulo1" runat="server" Text="Ordenes En Proceso"></asp:Label>
 
       <menu>
        <li><a href = "MenuSupervisor.aspx">Productos y Ordenes</a></li>
        <li>Ordenes En Proceso</li>
         <li><a href = "DatosEmpleados.aspx">Datos de Empleados a Cargo</a></li>
        <li><a href = "DatosUsuario.aspx">Datos de Usuario</a></li>
        <li><a href = "Principal.aspx">Salir</a></li>
    </menu>
  
          <asp:TextBox ID="txtbusqueda" CssClass="textbox1" runat="server"></asp:TextBox>
        <asp:Button ID="Btnbusqueda" CssClass="boton1" runat="server" Text="Por Producto" OnClick="Btnbusqueda_Click" />
        <asp:Button ID="Btnbusqueda3" CssClass="botonconsulta1" runat="server" Text="Por Producto C" OnClick="Btnbusqueda3_Click" />

          <asp:GridView ID="GridView1" CssClass="tabla" runat="server" ></asp:GridView>

          <asp:GridView ID="GridView2" CssClass="tabla2" runat="server"> </asp:GridView>

              <asp:Label ID="lblsupervisor" CssClass="label1ingreso" runat="server" Text="Supervisor:"></asp:Label>
        <asp:Label ID="lblcodigosup" CssClass="label11ingreso" runat="server"  Text="Label"></asp:Label>

        <asp:Label ID="Label4" CssClass="label2ingreso" runat="server"  Text="Orden"></asp:Label>
        <asp:TextBox ID="txtorden" CssClass="textbox2ingreso" runat="server" ></asp:TextBox>

            <asp:Label ID="Label5" CssClass="label3ingreso" runat="server"  Text="Monto"></asp:Label>
        <asp:TextBox ID="txtmonto" CssClass =" textbox3ingreso" runat="server" ></asp:TextBox>

         <asp:Button ID="btnaprobar" CssClass="boton1ingreso" runat="server" Text="Aprobar" OnClick="btnagregar_Click" />
         <asp:Button ID="btncancelar" CssClass="boton2ingreso" runat="server" Text="Cancelar" OnClick="btneliminar_Click" />
      <asp:Button ID="btneliminar2" CssClass="boton3ingreso" runat="server" Text="Eliminar Abonado" OnClick="btnver_Click" />


        <asp:Button ID="btnpagar" CssClass="botonterminar" runat="server" Text="Abonar" OnClick="btnpagar_Click" />




         <asp:Button ID="btnbusqueda2" CssClass="botonconsulta2" runat="server" Text="Por Cliente" OnClick="btnbusqueda2_Click" />
        <asp:Button ID="btnreporte" CssClass="botonreporte" runat="server" Text="Generar Reporte" OnClick="btnreporte_Click"  />







        </form>

       </body>
</html>
