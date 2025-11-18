<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Datwise WebForms - Default</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Hello from Datwise</h2>
            <asp:Button runat="server" ID="btnGet" Text="Call API" OnClick="btnGet_Click" />
            <asp:Literal runat="server" ID="litResult" />
        </div>
    </form>
</body>
</html>