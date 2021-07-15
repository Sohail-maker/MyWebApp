<%@ Page Title="" Language="C#" MasterPageFile="~/WebApp.Master" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="MyWebApp.WebForm5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <style type="text/css">
        .auto-style3 {
            font-size: 11pt;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="width:360px; background-color:#D9E1F2; margin-top:10px; padding:8px 8px 8px 8px; border-radius:5px;">
        <div style="margin:20px 0px;">
            <asp:Label Font-Size="20" ID="lblusername" runat="server" Text="Please Login to Edit Your Account"></asp:Label>
            
        </div>
        
        <div runat="server" id="divPassedit">
            <input type="text"  runat="server" class="form-control" id="oldPass" placeholder="Enter Old Password" />
            <input type="text"  runat="server" class="form-control" id="newPass"  placeholder="Enter New Password" />
            <asp:Button id="btnSavePass" runat="server" Text="Search" Cssclass="btn-primary btn-block " style="margin:5px 0px; width:100%; vertical-align:middle; font-size:20px; border-radius:5px;" OnClick="btnSavePass_Click1"/>

        </div>
        <div runat="server" id="divAddAccount" style="margin:20px 0px;">
            <input type="text"  runat="server" class="form-control" id="accountName" placeholder="Enter Full Name"/>
            <input type="text" runat="server" class="form-control" id="userName" placeholder="Enter User Name"/>
            <input type="password" runat="server" class="form-control" id="userPassword" placeholder="Enter New Password"/>
            <asp:Button id="btnAddAccount" runat="server" Text="Add Account" CssClass="btn-primary btn-block bluebtn" style="margin:5px 0px; width:100%; vertical-align:middle; font-size:20px; border-radius:5px;" OnClick="btnAddAccount_Click1"/>
        </div>
    </div>
    <script>
        
    </script>
</asp:Content>
