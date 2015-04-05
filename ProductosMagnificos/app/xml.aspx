<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xml.aspx.cs" Inherits="ProductosMagnificos.app.xml" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
        <asp:FileUpload ID="cargador" runat="server" />
        <asp:Button ID="Cargar" runat="server" Text="Button" OnClick="Cargar_Click1" />

    </form>
</body>
</html>
