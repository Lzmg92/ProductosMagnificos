<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Principal.aspx.cs" Inherits="ProductosMagnificos.app.Principal" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>Inicio</title>
    <link href ="Styles/StylePrincipal.css" rel="stylesheet" type ="text/css" />
</head>
<body>
    <form id="form1" runat="server">

        
        <asp:Label ID="lblinicio"  CssClass="titulo1" runat="server" Text="INICIO"></asp:Label>
         <asp:Label ID="lblsesion"  CssClass="titulo2" runat="server" Text="Iniciar Sesión"></asp:Label>
       
 
    <menu>
        <li>Iniciar Sesión</li>
        <li><a href = "NuevoCliente.aspx">Nuevo Cliente</a></li>
        <li><a href = "AccCrear.aspx">temp</a></li>
    </menu>
  
        <asp:Label ID="Labelusuario"  CssClass="item11" runat="server" Text="Usuario"></asp:Label>            
                 <asp:TextBox ID="txtusuario" CssClass="textbox11" runat="server"></asp:TextBox> 

            <asp:Label ID="lblcategoria"  CssClass="item1" runat="server" Text="Categoría"></asp:Label>            
        <asp:DropDownList ID="listcategoria" CssClass="textbox1" runat="server">
              <asp:ListItem>Operador</asp:ListItem>
             <asp:ListItem>Gerente</asp:ListItem>
             <asp:ListItem>Supervisor</asp:ListItem>
              <asp:ListItem>Administrador</asp:ListItem>
         </asp:DropDownList>

            <asp:Label ID="lblcontraseña"  CssClass="item2" runat="server" Text="Contraseña"></asp:Label>            
                 <asp:TextBox ID="txtcontraseña" CssClass="textbox2" runat="server"></asp:TextBox>

   
              <asp:Button ID="btnaceptar" CssClass="boton1" runat="server" OnClick="Button1_Click1" Text="Aceptar" />   
        <asp:Button ID="Cargar" CssClass="boton2" runat="server" Text="Cargar XML" OnClick="Cargar_Click" />
   

        <asp:Label ID="Label1" runat="server" ForeColor="White" Text="Label"></asp:Label>
        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>

             <asp:FileUpload ID="cargador" CssClass="textbox21" runat="server" />


    </form>
</body>
</html>
