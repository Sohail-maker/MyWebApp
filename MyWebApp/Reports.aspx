<%@ Page Title="" Language="C#" MasterPageFile="~/WebApp.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="MyWebApp.WebForm3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <title>Accountant Report</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div style="background-color:#D9E1F2; margin-top:10px; padding:8px 8px 8px 8px; border-radius:5px;">
        <asp:Panel runat="server" ID="reportPanel" >
            <div>
            <div style="width:360px;" class="container-fluid">
                <select id="selectReport" runat="server" class="form-select" onchange="showDiv('divCustomer', this, 'MiscellaneousAR')">
                    <option selected>Select a report</option>
                    <option value="CustomerBalanceSummary">Customers Balance</option>
                    <option value="VendorBalanceSummary">Vendors Balance</option>
                    <option value="ProfitAndLossStandard">Profit And Loss</option>
                    <option value="BalanceSheetSummary">Balance Sheet</option>
                    <option value="MiscellaneousAR">Customers Detail</option>
                </select>
                <div id="divCustomer" style="display:none">
                    <select id="selectCustomer" runat="server" class="form-select">
                    <option selected value="Miscellaneous">Miscellaneous</option>
                    <option value="WAIS KHOJANDI">Haji Wais Khojandi</option>
                    <option value="660 HAJI ABDULLAH">Haji Abdullah Wahedi</option>
                    <option value="Ghafori, Haji Basir">Haji Basir Ghafoori</option>
                    <option value="OSMANI MUSLIM">Haji Bashir Osmani</option>
                    <option value="HAJI HASHIM">Haji Hashim</option>
                    <option value="Wolayat, Khalid">Khallid Wolayat</option>
                    <option value="DAI">DAI</option>
                    <option value="DAI, SECOND ACCOUNT">DAI Second Account</option>
                    <option value="GIZ">GIZ</option>
                    <option value="Iran Consulate">Iran Consulate</option>
                    
                </select>
                </div>
                <div style="margin-top:4px;">
                    <select id="selectDateMacro" runat="server" class="form-select" onchange="showDiv('divDate', this, 'custom')">
                    <option selected>Select Date Period</option>
                    <option value="All">All</option>
                    <option value="ThisMonth">This Month</option>
                    <option value="ThisYear">This Year</option>
                    <option value="LastMonth">Last Month</option>
                    <option value="LastYear">Last Year</option>
                    <option value="custom">CustomDate</option>
                </select>
                    <div id="divDate" style="display:none;" >
                    <div class="input-group " >
                        <label for="dateFrom" class="input-group-text col-2">From</label>
                        <input runat="server" class=" form-control" type="date" value="" id="dateFrom"/>
                    </div>

                    <div class="input-group ">
                        <label for="dateTo" class="input-group-text col-2">To</label>
                        <input runat="server" class=" form-control" type="date" value="" id="dateTo"/>
                    </div>
                    </div>

                </div>
                <div style="margin-top:4px; margin-bottom:4px;">
                    <asp:Button ID="btnGetReport" runat="server" CssClass=" btn-primary" Text="Get Report" style="width:100%; vertical-align:middle; font-size:20px; border-radius:5px;" OnClick="btnGetReport_Click"/>
                </div>
                </div>
                <div style="margin-top:4px; margin-left:4px;" id="divTotal" runat="server" visible="false">
                   <b> <Label style="margin-left:4px; margin-right:4px;">Total</Label>
                    <asp:Label ID="lblTotal" runat="server" Text=""></asp:Label>
                   </b>
                </div>

                <div>
                <asp:GridView ID="reportView" runat="server" AutoGenerateColumns="true" CssClass=" table table-striped" >
                   
                </asp:GridView>
                </div>        
           </div>

        </asp:Panel>
    </div>
    
    <script type="text/javascript">
        function showDiv(divId, element, value) {
            document.getElementById(divId).style.display = element.value == value ? 'block' : 'none';
        }
    </script>

</asp:Content>
