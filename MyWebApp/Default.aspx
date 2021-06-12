<%@ Page Language="C#" MasterPageFile="~/WebApp.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MyWebApp.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    
 
    <title>Khojandi Supermarket</title>
    
    <style type="text/css">
        
        .auto-style1 {
            width: 119px;
        }
        tr:nth-child(even) {
            background-color:#f5f5f5;
        }
        
        .auto-style2 {
            width: 25%;
            height: 49px;
        }
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div style="width:360px; background-color:#D9E1F2; margin-top:10px; padding:8px 8px 8px 8px; border-radius:5px;">


        <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSearch">
      <div style="margin:auto;">

          <div class=" form-floating" style="margin-top:25px" >
             <asp:TextBox ID="txtUPC" runat="server" CssClass="form-control" style="width:100%;"></asp:TextBox>
              <label for="txtUPC" class="">Enter Item Info</label>
             <asp:Button id="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" Cssclass="btn-primary btn-block" style="margin:5px 0px 5px 0px; width:100%; vertical-align:middle; font-size:20px; border-radius:5px;"/>
           </div>
         

      <div>
      <table border="2" class="table table-striped table-hover">
         

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

      <asp:Label ID="lblError" runat="server" style="width:350px; text-align:center; display:inline-block; color:red;" Font-Bold="true"></asp:Label>
          <asp:Button ID="btnUpdateData" runat="server" Text="Update Data" OnClick="btnUpdateData_Click" Visible="false"/>

          <div runat="server" id="printDiv" Visible="false" class="row">
              <div style="width:70%" >
              <asp:Button ID="btnPrint" runat="server" Text="Print Tag" Cssclass="btn-Block btn-primary " style="width:100%;vertical-align:middle; font-size:20px; border-radius:4px;" OnClick="btnPrint_Click"/>
                </div>
              <div style="width:25%" >
                  
               <input runat="server" id="txtCopies" class="form-control" type="Number" min="1" max="1000" value="1" style="width:100%;" />
          </div>
          </div>
          
      

</div>
            </asp:Panel>

      <div style="text-align:center;">
        <p class="form-control" style=" background-color:#f5f5f5;" >Sohail &copy;2021 all rights reserved</p>

    </div>
        </div>
</asp:Content>
