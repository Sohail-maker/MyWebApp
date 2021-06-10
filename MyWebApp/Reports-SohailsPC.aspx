<%@ Page Title="" Language="C#" MasterPageFile="~/WebApp.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="MyWebApp.WebForm3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <title>Accountant Report</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="width:360px; background-color:#D9E1F2; margin-top:10px; padding:8px 8px 8px 8px; border-radius:5px;">
        <asp:Panel runat="server" ID="reportPanel" >
            <div>
                <select id="selectReport" runat="server" class="form-select">
                    <option selected>Select a report</option>
                    <option value="CustomerBalanceSummary">Customers Balance</option>
                    <option value="VendorBalanceSummary">Vendors Balance</option>
                    <option value="ProfitAndLossStandard">Profit And Loss</option>
                    <option value="BalanceSheetSummary">Balance Sheet</option>
                </select>
                <div style="margin-top:4px;">
                    <div class="input-group ">
                        <label for="dateFrom" class="input-group-text col-2">From</label>
                        <input runat="server" class=" form-control" type="date" value="" id="dateFrom"/>
                    </div>

                    <div class="input-group ">
                        <label for="dateTo" class="input-group-text col-2">To</label>
                        <input runat="server" class=" form-control" type="date" value="" id="dateTo"/>
                    </div>
                </div>
                <div style="margin-top:4px; margin-bottom:4px;">
                    <asp:Button ID="btnGetReport" runat="server" CssClass=" btn-primary" Text="Get Report" style="width:100%; vertical-align:middle; font-size:20px; border-radius:5px;" OnClick="btnGetReport_Click"/>
                </div>

                <div>
                    <table runat="server" id="tblReport">
                        <thead>
                        </thead>

                    </table>
                </div>
                <asp:GridView ID="reportView" runat="server" AutoGenerateColumns="true" CssClass=" table table-striped" >
                   
                </asp:GridView>
                        
           </div>

        </asp:Panel>
    </div>

</asp:Content>
