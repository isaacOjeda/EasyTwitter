<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Twitter4Later.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h1>Testing Easy Twitter.</h1>
    <p>Easy twitter it's a simple wrapper of Twitter's API version 1.1</p>

    <h3>Request token test</h3>
        <p>
            <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
                Text="Request" />
        </p>
    <div id="nowLogged" runat="server"></div>
    <h4>Clic de next button to show some information about the user</h4>
        <asp:TextBox ID="txtUser" runat="server"></asp:TextBox>
        <asp:Button ID="Button2" runat="server" Text="See information" 
            onclick="Button2_Click" />
            <div id="infoUser" runat="server"></div>

    <br />
    <h4>Favorited tweets</h4>
    <br />
    <h4>Update your status</h4>
        <asp:TextBox ID="txtStatus" runat="server"></asp:TextBox>
        <asp:Button ID="Button3" runat="server" Text="Button" onclick="Button3_Click" />
    </div>
    </form>
</body>
</html>
