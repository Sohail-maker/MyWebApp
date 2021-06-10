<%@ Page Title="" Language="C#" MasterPageFile="~/WebApp.Master" AutoEventWireup="true" CodeBehind="AlterData.aspx.cs" Inherits="MyWebApp.WebForm4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div style="width:360px; background-color:#D9E1F2; margin-top:10px; padding:8px 8px 8px 8px; border-radius:5px;">
      <div class=" form-floating" style="margin-top:25px;">
          <asp:TextBox ID="txtQuery" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>
          <label for="txtQuery">Enter Item UPC </label>
          <asp:Button ID="btnQuery" runat="server" CssClass="btn-primary btn-block" Text="Search" style="margin:5px 0px 5px 0px; width:100%; vertical-align:middle; font-size:20px; border-radius:5px;" OnClick="btnQuery_Click"  />
      </div>
        <div class="input-group" style="text-align:center;">
            <Label style="margin-left:4px; margin-right:4px;" ID="lblID">ListID:</Label>
            <asp:Label ID="lblIDtxt" runat="server" Text=""></asp:Label>
        </div>
            <div class="input-group ">
                        <label for="nametxt" class="input-group-text col-3">Name</label>
                        <input runat="server" class=" form-control" type="text" value="" id="nametxt"/>
                    </div>
            <div class="input-group ">
                        <label for="pricetxt" class="input-group-text col-3">Price</label>
                        <input runat="server" class=" form-control" type="number" value="" id="pricetxt"/>
                    </div>
            <div class="input-group ">
                        <label for="sizetxt" class="input-group-text col-3">Size</label>
                        <input runat="server" class=" form-control" type="text" value="" id="sizetxt"/>
                    </div>
            <div class="input-group ">
                        <label for="attributetxt" class="input-group-text col-3">Attribute</label>
                        <input runat="server" class=" form-control" type="text" value="" id="attributetxt"/>
                    </div>
            <div>
          <asp:Button ID="btnUpdate" runat="server" CssClass="btn-primary btn-block" Text="Save Changes" style="margin:5px 0px 5px 0px; width:100%; vertical-align:middle; font-size:20px; border-radius:5px;" OnClick="btnUpdate_Click" />

      <asp:Label ID="lblError" runat="server" style="width:350px; text-align:center; display:inline-block; color:red;" Font-Bold="true"></asp:Label>

            </div>
       </div>
    </div>
</asp:Content>
