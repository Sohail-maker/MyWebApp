<%@ Page Title="" Language="C#" MasterPageFile="~/WebApp.Master" AutoEventWireup="true" CodeBehind="MysqlData.aspx.cs" Inherits="MyWebApp.WebForm2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <title>Database Update and Query</title>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnQuery">
    <div style="width:360px; background-color:#D9E1F2; margin-top:10px; padding:8px 8px 8px 8px; border-radius:5px;">
      <div class=" form-floating" style="margin-top:25px;">
          <asp:TextBox ID="txtQuery" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>
          <label for="txtQuery">Enter Item UPC </label>
          <asp:Button ID="btnQuery" runat="server" CssClass="btn-primary btn-block" Text="Search" style="margin:5px 0px 5px 0px; width:100%; vertical-align:middle; font-size:20px; border-radius:5px;" OnClick="btnQuery_Click"  />
      </div>

        <div>
            <table class=" table-hover table table-striped">
                <tr>
          <td class="auto-style1">
              <b>Name</b>
          </td>
          <td >
              <asp:Label ID="lblName" runat="server" ></asp:Label>
          </td>
            </tr>

          <tr>
              <td class="auto-style1"><b>Price</b></td>
              <td >
                  <asp:Label ID="lblPrice" runat="server" ></asp:Label>
              </td>
          </tr>

          <tr>
              <td class="auto-style1">
                  <asp:Label ID="Label7" runat="server" Text="Item Number"></asp:Label>
              </td>
              <td >
                  <asp:Label ID="lblitmNumber" runat="server"></asp:Label>
              </td>
          </tr>

          <tr>
              <td class="auto-style1">
                  <asp:Label ID="Label1" runat="server" Text="On Hand QTY"></asp:Label>
              </td>
              <td>
                  <asp:Label ID="lblOnHandQty" runat="server"></asp:Label>
              </td>
          </tr>     

          <tr>
              <td class="auto-style1">
                  <asp:Label ID="Label2" runat="server" Text="Size" Width="150px"></asp:Label>
              </td>
              <td>
                  <asp:Label ID="lblSize" runat="server"></asp:Label>
              </td>
          </tr> 
          <tr>
              <td class="auto-style1" >
                  <asp:Label ID="Label3" runat="server" Text="Attribute"></asp:Label>
              </td>
              <td >
                  <asp:Label ID="lblAttribute" runat="server"></asp:Label>
              </td>
          </tr>     

          <tr>
              <td class="auto-style1" >
                  <asp:Label ID="Label4" runat="server" Text="Last Received Date"></asp:Label>
              </td>
              <td >
                  <asp:Label ID="lblLastRcvd" runat="server"></asp:Label>
              </td>
          </tr>     

          <tr>
              <td class="auto-style1">
                  <asp:Label ID="Label8" runat="server" Text="Cost"></asp:Label>
              </td>
              <td >
                  <asp:Label ID="lblCost" runat="server"></asp:Label>
              </td>
          </tr> 
            </table>
        </div>





        <asp:Button ID="btnUpdataData" runat="server" Text="Update Data" OnClick="btnUpdataData_Click" />
        <asp:Label  ID="lblError" runat="server" style="width:350px; text-align:center; display:inline-block; color:red;" Font-Bold="true"></asp:Label>
   </div>
        </asp:Panel>

     
</asp:Content>
