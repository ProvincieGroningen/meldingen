<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MeldingenOpKaart.aspx.cs"
    Inherits="Meldingen.MeldingenOpKaart" %>

<%@ Register Src="webcontrols/Kaart.ascx" TagName="Kaart" TagPrefix="uc1" %>
<%@ Register Src="webcontrols/MeldingDetails.ascx" TagName="MeldingDetails" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Meldingen op de kaart</title>
    <link href="Site.css" rel="stylesheet" type="text/css" />
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scriptManager" runat="server">
        </asp:ScriptManager>
        <div id="filterDiv">
            <span class="filterLabel" title="Het veld 'Onderwerp' bevat">Onderwerp</span>
            <input id="FilterOnderwerp" type="text" onchange="ajaxWijzigFilter();" onkeypress="ajaxWijzigFilter();" title="Het veld 'Onderwerp' bevat" />
            <span class="filterLabel">Status</span>
            <select id="FilterStatus" onchange="ajaxWijzigFilter();">
                <option value="">Alles</option>
                <option value="Open" selected="selected">Open</option>
                <option value="Gesloten">Gesloten</option>
            </select>
            <span class="filterLabel" title="Het veld 'Afzender' bevat">Afzender</span>
            <input id="FilterAfzender" type="text" onchange="ajaxWijzigFilter();" onkeypress="ajaxWijzigFilter();" title="Het veld 'Afzender' bevat" />
            <span class="filterLabel" title="Het veld 'Opmerking' bevat">Opmerking</span>
            <input id="FilterOpmerking" type="text" onchange="ajaxWijzigFilter();" onkeypress="ajaxWijzigFilter();" title="Het veld 'Opmerking' bevat" />
            <span class="filterLabel" title="Het veld 'Toegewezen aan' bevat">Toegewezen aan</span>
            <input id="FilterToegewezenAan" type="text" onchange="ajaxWijzigFilter();" onkeypress="ajaxWijzigFilter();" title="Het veld 'Toegewezen aan' bevat" />
            <a id="ResetFilter" href="#" onclick="resetFilter(); return false;">reset filter</a>
            <div id="filterStatusDiv"></div>
            <div id="beheerDiv"><a href="Beheer.aspx">beheer</a></div>
        </div>
        <asp:UpdatePanel ID="updatePanelFilter" runat="server">
            <ContentTemplate>
                <div style="display: none">
                    <asp:TextBox ID="TextBoxIdMelding" runat="server" />
                    <asp:Button ID="ButtonMeldingSelected" runat="server" OnClick="Melding_Selected" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <uc1:Kaart ID="Kaart1" runat="server" style="float: left" />
        <div id="detailsDiv">
            <asp:UpdatePanel ID="updatePanelDetails" runat="server">
                <ContentTemplate>
                    <uc2:MeldingDetails ID="meldingDetails" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
