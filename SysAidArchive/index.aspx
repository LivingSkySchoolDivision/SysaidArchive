<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="SysAidArchive.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        body {
            font-family: Arial;
        }
        
        td {
            font-size: 8pt;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 800px; margin-left: auto; margin-right: auto;">
        <h1>LSKYSD SysAid Archive</h1>
        <p>Our SysAid server no longer functions, but you can search it's database of tickets here.</p>
        <p>Split multiple search terms with a comma ','. Multiple words with just spaces count as a single search term.</p>
        <p>
            <asp:TextBox ID="txtSearchTerms" runat="server" style="width: 500px; margin: 5px;"></asp:TextBox><asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_OnClick" />
        </p>
        <asp:Label ID="lblResults" runat="server" Text=""></asp:Label>
        <asp:Table ID="tblResults" runat="server" CellPadding="4" Width="100%"></asp:Table>
    </div>
        
    </form>
</body>
</html>
